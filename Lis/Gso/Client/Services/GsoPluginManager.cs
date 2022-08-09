namespace Lis.Gso.Client.Services;

public class GsoPluginManager : IGsoPluginManager {
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GsoWelcomeManager> _logger;

    public GsoPluginManager(
        IServiceProvider serviceProvider,
        HttpClient httpClient,
        ILogger<GsoWelcomeManager> logger
     ) {
        _httpClient = httpClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public IList<IGsoPlugin> AvailablePlugins { get; private set; } = new List<IGsoPlugin>();

    public IList<Assembly> AvailablePluginsAssemblies { get; private set; } = new List<Assembly>();

    public async Task Initialize() {
        try {
            var availablePlugins = await _httpClient.GetFromJsonAsync<string[]>("api/plugins");
            foreach (var plugin in availablePlugins) {
                try {
                    _logger.LogInformation("Loading plugin {plugin}", plugin);
                    var bytes = await _httpClient.GetStreamAsync($"/plugins/{plugin}.dll");
                    var assembly = AssemblyLoadContext.Default.LoadFromStream(bytes);
                    var type = assembly.GetType($"{plugin}.Plugin");
                    var pluginInstance = (IGsoPlugin)Activator.CreateInstance(type, _serviceProvider);
                    AvailablePlugins.Add(pluginInstance);
                    AvailablePluginsAssemblies.Add(assembly);
                } catch (Exception ex) {
                    _logger.LogError("Error loading plugin '{plugin}'. Details: {error}", plugin, ex.Message);
                }
            }
        } catch (Exception ex) {
            _logger.LogError("Error getting list of available plugins. Details: {error}", ex.Message);
        }
    }
}