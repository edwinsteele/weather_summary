using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace weather_summary
{
    class Program
    {

        private static readonly HttpClient Client = new HttpClient();
        private static readonly string TEMPERATURE_SUMMARY_URL = "http://www.bom.gov.au/fwo/IDN60801/IDN60801.94763.json";

        static async Task<float> GetApparentTemperature()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, TEMPERATURE_SUMMARY_URL);
            // BOM doesn't give JSON data without gzip
            //requestMessage.Headers.Add("Accept-Encoding", "gzip");
            //requestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            var responseBody = await Client.SendAsync(requestMessage);
            var responseString = await responseBody.Content.ReadAsStringAsync();
            dynamic responseJson = JsonConvert.DeserializeObject(responseString);
            float apparentTemp = responseJson.observations.data[0].apparent_t;
            return apparentTemp;
        }
        

        static async Task Main(string[] args)
        {
            try
            {
                float apparentTemp = await GetApparentTemperature();
                Console.WriteLine($"{apparentTemp} °C");
                
            }  
            catch(HttpRequestException e)
            {
               Console.WriteLine("\nException Caught!");    
               Console.WriteLine("Message :{0} ",e.Message);
            }

            // Need to call dispose on the HttpClient object
            // when done using it, so the app doesn't leak resources
            Client.Dispose();
        }
    }
}
