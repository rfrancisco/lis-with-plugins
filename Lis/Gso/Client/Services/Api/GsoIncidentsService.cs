namespace Lis.Gso.Client.Services.Api;

public class GsoIncidentsService {
    private readonly HttpClient _httpClient;

    public GsoIncidentsService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    public Task<GsoIncident[]> GetMany() {
        return _httpClient.GetFromJsonAsync<GsoIncident[]>("data/incidents.json");
    }
}
