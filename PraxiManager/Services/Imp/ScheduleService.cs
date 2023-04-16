using CreatioManager.Services;
using MupiModel.Dtos.Results;

namespace PraxiManager.Services.Imp
{
    public class ScheduleService : IScheduleService
    {
        private readonly ISyncParametersService _SyncParametersService;

        public ScheduleService(ISyncParametersService syncParametersService)
        {
            _SyncParametersService = syncParametersService;
        }

        public async Task<List<ResultSyncEntitiesParams>> GetSyncEntities()
        {
            var result = await _SyncParametersService.GetSyncEntities();           

            return result;
        }
    }
}
