namespace PocPerfAsyncAndThread
{
    public interface IHttpClientAdapter
    {
        public Task<string> GetStringAsync(string url);
    }
}
