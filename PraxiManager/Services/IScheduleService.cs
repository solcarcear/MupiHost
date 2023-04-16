using MupiModel.Dtos.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraxiManager.Services
{
    public interface IScheduleService
    {
        public Task<List<ResultSyncEntitiesParams>> GetSyncEntities();
    }
}
