using CreatioManager.Models.Batch;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace CreatioManager.HttpClients
{
    public class BatchClient
    {
        private readonly HttpClient _client;


        const string batchOdataUrl = "0/odata/$batch";
        public BatchClient(HttpClient client)
        {
            _client = client;
        }


        public async Task<BatchResult> RequestBatch(IEnumerable<BatchRequest> requests, int rpb=100)
        {
            var result = new BatchResult();
            for (int i = 0; i < requests.Count(); i = i + rpb)
            {
                var batchRequest = requests.Skip(i).Take(rpb);

                var batchResults = await ExecuteBatch(batchRequest);

                result.Responses.AddRange(batchResults.Responses);

            }

            return result;
        }


        private async Task<BatchResult> ExecuteBatch(IEnumerable<BatchRequest> requests)
        {
            var result = new BatchResult();

            var msg = new HttpRequestMessage(HttpMethod.Post, batchOdataUrl);

            var requestBody = new BatchRequests
            {
                Requests = requests,
            };


            var body = JsonConvert.SerializeObject(requestBody,
                            Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            msg.Content = new StringContent(body, Encoding.UTF8);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.SendAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BatchResult>(responseStr);
            }

            return result ?? new BatchResult();
        }
    }
}
