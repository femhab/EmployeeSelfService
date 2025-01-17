﻿using AutoMapper;
using Business.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class PayslipService: IPayslipService
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IEmployeeService _employeeService;
        private readonly PayrollContext _payrollContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PayslipService(IEmployeeService employeeService, PayrollContext payrollContext, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:PRServerConnection"]);
            _employeeService = employeeService;
            _payrollContext = payrollContext;
            _mapper = mapper;
        }

        public async Task<PayslipResponseModel> GeneratePayslipData(Guid employeeId, DateTime payPeriod)
        {
            PayslipResponseModel payslipResponseModel = new PayslipResponseModel();
            var employee = await _employeeService.GetById(employeeId);
            using (_sqlConnection)
            {
                var sql = $"select * from PREmpMst where emp_no='{employee.Emp_No}';";
                SqlCommand query = new SqlCommand(sql, _sqlConnection);
                PREmpMst resource = new PREmpMst();

                _sqlConnection.Open();

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PREmpMst requester = new PREmpMst()
                        {
                            prz_entityCode = reader["prz_entityCode"].ToString(),
                            prz_paygrpCode = reader["prz_paygrpCode"].ToString(),
                            emp_no = reader["emp_no"].ToString(),
                            prz_payfrqCode = reader["prz_payfrqCode"].ToString()
                        };
                        resource = requester;
                    }
                }
                try
                {
                    var userBasic = _payrollContext.Set<BasicReportModel>().FromSql($"dbo.RPT_GETEMPBASICESS @emp_no = { employee.Emp_No}, @pay_period = {payPeriod}").FirstOrDefault();

                    payslipResponseModel.BasicReport = _mapper.Map<BasicReportModel>(userBasic);

                    var userEarn = _payrollContext.Set<EarningReportModel>().FromSql($"dbo.RPT_GETEMPEARNESS  @emp_no = {employee.Emp_No}, @pay_period = {payPeriod}").ToList();
                    var earningItem = new List<EarningReportModel>();
                    foreach(var item in userEarn)
                    {
                        var earnModel = _mapper.Map<EarningReportModel>(item);
                        earningItem.Add(earnModel);
                    }
                    payslipResponseModel.EarningReport = earningItem;

                    var userDed = _payrollContext.Set<DeductionReportModel>().FromSql($"dbo.RPT_GETEMPDEDESS @emp_no = {employee.Emp_No}, @pay_period = {payPeriod}").ToList();
                    var deductionItem = new List<DeductionReportModel>();
                    foreach (var item in userDed)
                    {
                        var dedModel = _mapper.Map<DeductionReportModel>(item);
                        deductionItem.Add(dedModel);
                    }
                    payslipResponseModel.DeductionReport = deductionItem;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                _sqlConnection.Close();

                return payslipResponseModel;
            }
        }

        public async Task<List<PayrollCalender>> PayrollCalender()
        {
            var sql = $"select * from prp_calend";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<prp_calend> resource = new List<prp_calend>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    prp_calend requester = new prp_calend()
                    {
                        prz_entityCode = reader["prz_entityCode"].ToString(),
                        prz_paygrpCode = reader["prz_paygrpCode"].ToString(),
                        pay_period = Convert.ToDateTime(reader["pay_period"]).ToString("dd/MM/yyyy"),
                        prz_taxYrCode = reader["prz_taxYrCode"].ToString()
                    };
                    resource.Add(requester);
                }
            }
            List<PayrollCalender> calenders = new List<PayrollCalender>();
            foreach(var item in resource)
            {
                var calender = new PayrollCalender()
                {
                    PayPeriod = DateTime.ParseExact(item.pay_period, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    PayrollEntityCode = item.prz_entityCode,
                    PayrollGroupCode = item.prz_paygrpCode,
                    TaxYearCode = item.prz_taxYrCode
                };
                calenders.Add(calender);
            }
            _sqlConnection.Close();
            return calenders;
        }
    }
}
