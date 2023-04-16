using MupiModel.Dtos.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatioManager.Services
{
    public interface ISyncParametersService
    {
        public Task<List<ResultSyncEntitiesParams>> GetSyncEntities();

    }
}
