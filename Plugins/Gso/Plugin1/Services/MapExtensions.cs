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
            _instance ??= new MapExtensions();
            return _instance;
        }
    }

    public static async Task InitializeAsync(IJSRuntime jSRuntime) {
        if (!_initialized) {
            _jSRuntime = jSRuntime;
            var assembly = Assembly.GetAssembly(typeof(MapExtensions));
            await _jSRuntime.InvokeVoidAsync("eval", GetEmbeddedJSInteropCode(assembly, "Lis.Gso.Plugin1.wwwroot.map.js"));

            _initialized = true;
        }
    }

    public static async Task ZoomIn() {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.zoomIn");
    }

    public static async Task ZoomOut() {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.zoomOut");
    }

    public static async Task LoadGeoJson() {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.loadGeoJson");
    }

    public static async Task InitializeDrawingTools<T>(DotNetObjectReference<T> objRef) where T : class {
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.initializeDrawingTools", objRef);
    }

    public static async Task SetDrawingTool(string tool) { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.setDrawingTool", tool);
    }

    public static async Task CopySelectedShape() { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.copySelectedShape");
    }

    public static async Task PasteSelectedShape() { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.pasteSelectedShape");
    }

    public static async Task DeleteSelectedShape() { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.deleteSelectedShape");
    }

    public static async Task SetColor(string color) { 
        await _jSRuntime.InvokeVoidAsync("plugin1MapExtensions.setColor", color);
    }

    private static string GetEmbeddedJSInteropCode(Assembly assembly, string path) {
        using var stream = assembly.GetManifestResourceStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
