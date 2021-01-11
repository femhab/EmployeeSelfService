using AutoMapper;
using Business.Interfaces;
using Business.Providers.JWT;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthService: IAuthService
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IEmployeeService _employeeService;
        private readonly IUserRoleService _userRoleService;
        private readonly IAuthTokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, IEmployeeService employeeService, IUserRoleService userRoleService, IAuthTokenProvider tokenProvider, IConfiguration configuration, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _employeeService = employeeService;
            _userRoleService = userRoleService;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<(bool status, string message, string token, string refreshToken)> Register(string email, string password, string lastName, string firstName, string userName, string empNo, List<Guid> roleId, string createdBy = null)
        {
            //check for existing user
            var checkuser = (email != null) ? await _userManager.FindByEmailAsync(email) : await _userManager.FindByNameAsync(empNo);
            if (checkuser != null)
            {
                return (false, "Employee already exist, try another", null, null);
            }

            var user = new AppIdentityUser { Email = email, UserName = empNo, LastName = lastName, FirstName = firstName};

            //Create user
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                //save the domain user into the user table                  
                var employeeModel = new Employee { FirstName = firstName, LastName = lastName, UserName = userName, EmailAddress = email, Emp_No = empNo, Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = createdBy };             
               
                var userdata = await _employeeService.Create(employeeModel);
                if (userdata.Status)
                {
                    //use my queue package to send email
                    //create userrole
                    foreach (var item in roleId)
                    {
                        var employeeRole = new UserRole() { EmployeeId = employeeModel.Id, Emp_No = empNo, RoleId = item, Id = Guid.NewGuid(), CreatedDate = DateTime.Now, CreatedBy = createdBy };
                        await _userRoleService.Create(employeeRole);
                    }
                    

                    return (true, "Your registration was successful!", null, null);
                }
                return (false, "Unable to complete the registration. Please try again", null, null);
            }

            return (false, "Registration failed. Please try again!", null, null);
        }

        public async Task<(bool status, string message, string token, string refreshToken)> Login(string email, string password)
        {
            var appUser = await _userManager.FindByNameAsync(email) ?? await _userManager.FindByEmailAsync(email);
            if (appUser != null)
            {
                var signin = await _signInManager.PasswordSignInAsync(email, password, false, false);
                if (signin.Succeeded)
                {
                    var userdata = await _employeeService.GetByEmployerIdOrEmail(email);
                    if (userdata != null)
                    {
                        // TODO: Track all login history.                      
                        var refreshToken = _tokenProvider.GenerateRefreshToken();
                        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshExpireDays"]));

                        var tokenModel = _mapper.Map<AuthTokenModel>(userdata);

                        ////add claims
                        var token = _tokenProvider.GenerateJwtToken(tokenModel, out List<Claim> claims);

                        //check ifuser claim exist before
                        var userClaims = await _userManager.GetClaimsAsync(appUser);
                        if (userClaims.Count < 1)
                        {
                            await _userManager.AddClaimsAsync(appUser, claims);
                        }

                        return (true, "You have successfully logged in", token, refreshToken);
                    }
                }
                return (false, "Email or password not correct, check and try again.", null, null);
            }
            return (false, "User not found, check and try again.", null, null);
        }

        public async Task<string> Logout()
        {
            await _signInManager.SignOutAsync();
            return "Logout Successful";
        }
    }
}
