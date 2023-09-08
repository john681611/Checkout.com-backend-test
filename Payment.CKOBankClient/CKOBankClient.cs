namespace Payment.CKOBankClient;

using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace Payment.CKOBankClient.Client
{
    public class CKOBankClient
    {
        private string APIKey { get; init; }
        private string BaseURL { get; init; }
        private HttpClient Client { get; init; }

        public WeatherService(HttpClient? client = null)
        {
            BaseURL = Utils.GetRequiredEnvironmentVariable("CKO_BANK_URL");
            APIKey = Utils.GetRequiredEnvironmentVariable("CKO_BANK_API_KEY");

            Client = client ?? new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }


        private async Task<CKOPaymentResponse> MakePayment(PaymentRequest paymentRequest)
        {
            return await CallAPI<CKOPaymentResponse, CKOPaymentRequest>("/payment", paymentRequest);
        }

        private async Task<T> CallAPI<T, V>(string path, V data)
        {
            var URI = new Uri($"{BaseURL}/{path}{queryString}");
            HttpResponseMessage response = await Client.PostAsync(URI, data);
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
}
