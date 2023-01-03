using System.Reflection;
using Microsoft.JSInterop;

namespace Lis.Gso.Plugin3;

public class JsInvoker {
    private static JsInvoker _instance = null;
    private static bool _initialized = false;
    private static IJSInProcessRuntime _jSRuntime;

    private JsInvoker() { }

    public static JsInvoker Instance {
        get {
            _instance ??= new JsInvoker();
            return _instance;
        }
    }

    public void InitializeAsync(IServiceProvider sp) {
        if (!_initialized) {
            _jSRuntime = (IJSInProcessRuntime)sp.GetService(typeof(IJSRuntime));
            var assembly = Assembly.GetAssembly(typeof(JsInvoker));
            _jSRuntime.InvokeVoid("eval", GetEmbeddedJSInteropCode(assembly, "Lis.Gso.Plugin3.wwwroot.plugin.js"));
            _initialized = true;
        }
    }

    public void DownloadFile(string fileName, string contentType, byte[] content) {
         _jSRuntime.InvokeVoid("plugin3JsInvoker.downloadFile", fileName, contentType, content);
    }

    private string GetEmbeddedJSInteropCode(Assembly assembly, string path) {
        using var stream = assembly.GetManifestResourceStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
