namespace Lis.Gso.Client.Services;

public class GsoMapManager : IGsoMapManager {
    private DotNetObjectReference<GsoMapManager> _objRef;
    private IJSObjectReference _module;
    private readonly IJSRuntime _jsRuntime;

    public GsoMapManager(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    public async Task Initialize(GsoMapOptions mapOptions) {
        _objRef = DotNetObjectReference.Create(this);
        _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/map.js");
        await _module.InvokeVoidAsync("initMap", mapOptions, _objRef);
    }

    public ValueTask SetNextAvailableMapTypeId() {
        return _module.InvokeVoidAsync("setNextAvailableMapTypeId");
    }

    public ValueTask ShowUserCurrentLocation() {
        return _module.InvokeVoidAsync("showUserCurrentLocation");
    }

    public ValueTask ZoomIn() {
        return _module.InvokeVoidAsync("zoomIn");
    }

    public ValueTask ZoomOut() {
        return _module.InvokeVoidAsync("zoomOut");
    }

    public ValueTask ShowIncidents(IEnumerable<GsoIncident> incidents) {
        return _module.InvokeVoidAsync("showIncidents", incidents);
    }
}