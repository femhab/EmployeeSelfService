using Business.Interfaces;
using Coravel.Invocable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Helper.Schedular
{
    public class RefreshHelper: IInvocable
    {
        private readonly IRefreshingService _refreshingService;

        public RefreshHelper(IRefreshingService refreshingService)
        {
            _refreshingService = refreshingService;
        }

        public async Task Invoke()
        {
            try
            {
                await _refreshingService.Refresh();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
