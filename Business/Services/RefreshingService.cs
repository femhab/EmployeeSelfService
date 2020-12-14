using Business.Interfaces;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RefreshingService: IRefreshingService
    {
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ILGAService _lgaService;
        private readonly ISectionService _sectionService;
        private readonly ILocationService _locationService;
        private readonly ICourtesyService _courtesyService;
        private readonly IMaritalStatusService _maritalStatusService;
        private readonly IEmployeeTitleService _employeeTitleService;
        private readonly IAvailabilityStatusService _availabilityStatusService;

        public RefreshingService(ICountryService countryService, IStateService stateService, ILGAService lgaService, ISectionService sectionService, ILocationService locationService, ICourtesyService courtesyService, IMaritalStatusService maritalStatusService, IEmployeeTitleService employeeTitleService, IAvailabilityStatusService availabilityStatusService)
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
        }

        public async Task Refresh()
        {
            await _countryService.Refresh();
            await _stateService.Refresh();
            await _lgaService.Refresh();
            await _locationService.Refresh();
            await _sectionService.Refresh();
            await _availabilityStatusService.Refresh();
            await _employeeTitleService.Refresh();
            await _courtesyService.Refresh();
            await _maritalStatusService.Refresh();
        }
    }
}
