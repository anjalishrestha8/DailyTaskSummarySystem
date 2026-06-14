namespace ClientMVC.Services
{
    public interface IApiService
    {
        public Task<TResponse?> GetAsync<TResponse>(string uri);
        public Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data);
        public Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest data);
        public Task<TResponse?> DeleteAsync<TResponse>(string uri);
    }
}
