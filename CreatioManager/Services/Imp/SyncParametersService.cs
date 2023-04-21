using CreatioManager.HttpClients;
using CreatioManager.Models;
using CreatioManager.Models.Batch;
using CreatioManager.Models.Result;
using MupiModel.Dtos.Results;
using Newtonsoft.Json;

namespace CreatioManager.Services.Imp
{

    public class SyncParametersService : ISyncParametersService
    {
        private readonly BatchClient _batchClient;

        public SyncParametersService(BatchClient batchClient)
        {
            _batchClient = batchClient;
        }

        public async Task<List<ResultSyncEntitiesParams>> GetSyncEntities()
        {
       

            var batchRequest = await ShapeRequestGetParams();

            var requestResponse = await _batchClient.RequestBatch(batchRequest);

            var result = JsonConvert.DeserializeObject<ResultSyncParams>(requestResponse.Responses[0].Body.ToString());


            return result.Value.Select(x=>new ResultSyncEntitiesParams
            {
                Entity=x.Name,
                Frequency=x.UsrFrecuencia,
                From=x.UsrDateFrom,
                To=x.UsrDateTo,
                Active=x.UsrActivo,
                IsFirstLoad=x.UsrIsFirstLoad
            }).ToList();
        }


        private async Task<IEnumerable<BatchRequest>> ShapeRequestGetParams()
        {
            var result = new List<BatchRequest>();

            var request = new BatchRequest
            {
                Id = $"R1",
                Method = BatchRequest.GET,
                Url = UsrMupiSyncParameters.urlGetActiveParams,
                Headers = new BatchHeaders(),
                Body = new object()
            };
            result.Add(request);

            return result;

        }
    }
}
