using System.Reflection;
using Microsoft.JSInterop;

namespace Lis.Gso.Plugin2;

public class MapExtensions {
    private static MapExtensions _instance = null;
    private static bool _initialized = false;
    private static IJSRuntime _jSRuntime;

    private MapExtensions() { }

    public static MapExtensions Instance {
        get {
            _instance ??= new MapExtensions();
            return _instance;
        }
    }

    public async Task InitializeAsync(IJSRuntime jSRuntime) {
        if (!_initialized) {
            _jSRuntime = jSRuntime;
            var assembly = Assembly.GetAssembly(typeof(MapExtensions));
            await _jSRuntime.InvokeVoidAsync("eval", GetEmbeddedJSInteropCode(assembly, "Lis.Gso.Plugin2.wwwroot.map.js"));

            _initialized = true;
        }
    }

    public async Task GenerateRandomMarker() {
        await _jSRuntime.InvokeVoidAsync("plugin2MapExtensions.generateRandomMarker");
    }

    private string GetEmbeddedJSInteropCode(Assembly assembly, string path) {
        using var stream = assembly.GetManifestResourceStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
