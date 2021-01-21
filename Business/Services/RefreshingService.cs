using Business.Interfaces;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RefreshingService: IRefreshingService
    {
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ILGAService _lgaService;
        private readonly ILocationService _locationService;
        private readonly ICourtesyService _courtesyService;
        private readonly IMaritalStatusService _maritalStatusService;
        private readonly IEmployeeTitleService _employeeTitleService;
        private readonly IAvailabilityStatusService _availabilityStatusService;
        private readonly ITrainingService _trainingService;
        private readonly IDivisionService _divisionService;
        private readonly IDepartmentService _departmentService;
        private readonly ISectionService _sectionService;
        private readonly IGradeLevelService _gradeLevelService;
        private readonly ITrainingNominationService _trainingNominationService;
        private readonly IAppraisalCategoryItemService _appraisalCategoryItemService;
        private readonly IAppraisalCategoryService _appraisalCategoryService;
        private readonly IEducationalGradeService _educationalGradeService;
        private readonly IEducationalLevelService _educationalLevelService;
        private readonly IEducationalQualificationService _educationalQualificationService;
        private readonly IEmployeeService _employeeService;

        public RefreshingService(ICountryService countryService, IStateService stateService, ILGAService lgaService, ISectionService sectionService, ILocationService locationService, ICourtesyService courtesyService, IMaritalStatusService maritalStatusService, IEmployeeTitleService employeeTitleService, IAvailabilityStatusService availabilityStatusService, ITrainingService trainingService, IDepartmentService departmentService, IGradeLevelService gradeLevelService, ITrainingNominationService trainingNominationService, IAppraisalCategoryService appraisalCategoryService, IAppraisalCategoryItemService appraisalCategoryItemService, IDivisionService divisionService, IEducationalGradeService educationalGradeService, IEducationalLevelService educationalLevelService, IEducationalQualificationService educationalQualificationService, IEmployeeService employeeService)
        {
            _countryService = countryService;
            _stateService = stateService;
            _lgaService = lgaService;
            _sectionService = sectionService;
            _locationService = locationService;
            _courtesyService = courtesyService;
            _maritalStatusService = maritalStatusService;
            _employeeTitleService = employeeTitleService;
            _availabilityStatusService = availabilityStatusService;
            _trainingService = trainingService;
            _trainingNominationService = trainingNominationService;
            _departmentService = departmentService;
            _gradeLevelService = gradeLevelService;
            _appraisalCategoryService = appraisalCategoryService;
            _appraisalCategoryItemService = appraisalCategoryItemService;
            _divisionService = divisionService;
            _educationalGradeService = educationalGradeService;
            _educationalLevelService = educationalLevelService;
            _educationalQualificationService = educationalQualificationService;
            _employeeService = employeeService;
        }

        public async Task Refresh()
        {
            try 
            {
                await _countryService.Refresh();
                await _stateService.Refresh();
                await _lgaService.Refresh();
                await _locationService.Refresh();
                await _availabilityStatusService.Refresh();
                await _employeeTitleService.Refresh();
                await _courtesyService.Refresh();
                await _maritalStatusService.Refresh();
                await _trainingService.RefreshTopics();
                await _trainingNominationService.Refresh();
                await _gradeLevelService.Refresh();
                await _divisionService.Refresh();
                await _departmentService.Refresh();
                await _sectionService.Refresh();
                await _appraisalCategoryService.Refresh();
                await _appraisalCategoryItemService.Refresh();
                await _educationalGradeService.Refresh();
                await _educationalLevelService.Refresh();
                await _educationalQualificationService.Refresh();
                await _employeeService.RefreshEmployeeDetail();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
