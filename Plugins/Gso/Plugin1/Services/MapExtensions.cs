using System.Reflection;
using Microsoft.JSInterop;

namespace Lis.Gso.Plugin1;

public class MapExtensions {
    private static MapExtensions _instance = null;
    private static bool _initialized = false;
    private static IJSRuntime _jSRuntime;

    private MapExtensions() { }

    public static MapExtensions Instance {
        get {
            if (_instance == null) {
                _instance = new MapExtensions();
            }
            return _instance;
        }
    }

    public async Task InitializeAsync(IJSRuntime jSRuntime) {
        if (!_initialized) {
            _jSRuntime = jSRuntime;
            var assembly = Assembly.GetAssembly(typeof(MapExtensions));
            await _jSRuntime.InvokeVoidAsync("eval", GetEmbeddedJSInteropCode(assembly, "Lis.Gso.Plugin1.wwwroot.map.js"));

            _initialized = true;
        }
    }

    public async Task ZoomIn() {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.zoomIn");
    }

    public async Task ZoomOut() {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.zoomOut");
    }

    public async Task LoadGeoJson() {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.loadGeoJson");
    }

    public async Task InitializeDrawingTools<T>(DotNetObjectReference<T> objRef) where T : class {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.initializeDrawingTools", objRef);
    }

    public async Task SetDrawingTool(string tool) { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.setDrawingTool", tool);
    }

    public async Task CopySelectedShape() { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.copySelectedShape");
    }

    public async Task PasteSelectedShape() { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.pasteSelectedShape");
    }

    public async Task DeleteSelectedShape() { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.deleteSelectedShape");
    }

    public async Task SetColor(string color) { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.setColor", color);
    }

    private static string GetEmbeddedJSInteropCode(Assembly assembly, string path) {
        using var stream = assembly.GetManifestResourceStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
