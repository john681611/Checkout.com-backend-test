
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Payment.CKOBankClient.Models;
using Payment.Utils;

namespace Payment.CKOBankClient;
public class RestClient
{
    private string APIKey { get; init; }
    private string BaseURL { get; init; }
    private HttpClient Client { get; init; }

    public RestClient(HttpClient? client = null)
    {
        BaseURL = EnvUtils.GetRequiredEnvironmentVariable("CKO_BANK_URL");
        APIKey = EnvUtils.GetRequiredEnvironmentVariable("CKO_BANK_API_KEY");

        Client = client ?? new HttpClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", APIKey);
    }


    public async Task<CKOPaymentResponse> MakePayment(CKOPaymentRequest paymentRequest)
    {
        return await CallAPI<CKOPaymentResponse, CKOPaymentRequest>("Payment", paymentRequest);
    }

    private async Task<T> CallAPI<T, V>(string path, V data)
    {
        var URI = new Uri($"{BaseURL}/{path}");
        var messageContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await Client.PostAsync(URI, messageContent);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseString))
            throw new NullReferenceException($"API call response is empty ({URI.AbsoluteUri})");

        var responseContent = JsonConvert.DeserializeObject<T>(responseString);
        if (responseContent is null)
            throw new NullReferenceException($"Parsed API response is empty ({URI.AbsoluteUri})");

        return responseContent;
    }
}
