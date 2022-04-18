namespace Lis.Gso.Client.Services.Api;

public class GsoStatusService {
    private readonly HttpClient _httpClient;

    public GsoStatusService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public Task<GsoStatus[]> GetMany() {
        return _httpClient.GetFromJsonAsync<GsoStatus[]>("data/statuses.json");
    }
}
