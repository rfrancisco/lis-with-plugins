namespace Lis.Gso.Client.Services.Api;

public class GsoTypeService {
    private readonly HttpClient _httpClient;

    public GsoTypeService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public Task<GsoType[]> GetMany() {
        return _httpClient.GetFromJsonAsync<GsoType[]>("data/types.json");
    }
}
