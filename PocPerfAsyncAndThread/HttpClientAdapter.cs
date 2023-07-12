namespace PocPerfAsyncAndThread
{
    public class HttpClientAdapter : IHttpClientAdapter
    {
        HttpClient _httpClient;

        public HttpClientAdapter()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetStringAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }
    }
}
