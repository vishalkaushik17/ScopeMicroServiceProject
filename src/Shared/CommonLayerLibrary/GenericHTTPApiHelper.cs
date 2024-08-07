using GenericFunction.ResultObject;
using System.Net.Http.Json;
using System.Text;

namespace GenericFunction;

public static class GenericHttpApiHelper
{
    // In my case this is https://localhost:44366/
    //private static readonly string apiBasicUri = ConfigurationManager.MailConfiguration["EmailService:Url"];

    public static async Task<MailResponse> Post<T>(string apiBasicUri, string url, T contentValue)
    {
        MailResponse? response = new MailResponse();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBasicUri);
            var content = new StringContent(contentValue.ToJson(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);
            if (result.IsSuccessStatusCode)
            {
                response = await result.Content.ReadFromJsonAsync<MailResponse>();

                //result.EnsureSuccessStatusCode();
                //response.Message = result.ReasonPhrase;
                response.IsSuccess = true;
                response.StatusCode = result.StatusCode;

            }
            else
            {
                response.Message = result.ReasonPhrase;
                response.IsSuccess = true;
                response.StatusCode = result.StatusCode;
            }

            return response;
        }
    }

    public static async Task Put<T>(string apiBasicUri, string url, T stringValue)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBasicUri);
            var content = new StringContent(stringValue.ToJson(), Encoding.UTF8, "application/json");
            var result = await client.PutAsync(url, content);
            result.EnsureSuccessStatusCode();
        }
    }

    public static async Task<T> Get<T>(string apiBasicUri, string url)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBasicUri);
            var result = await client.GetAsync(url);
            result.EnsureSuccessStatusCode();
            string resultContentString = await result.Content.ReadAsStringAsync();
            T resultContent = resultContentString.FromJsonToObject<T>();
            return resultContent;
        }
    }

    public static async Task Delete(string apiBasicUri, string url)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(apiBasicUri);
            var result = await client.DeleteAsync(url);
            result.EnsureSuccessStatusCode();
        }
    }
}