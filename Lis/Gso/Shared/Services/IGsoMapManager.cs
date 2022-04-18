namespace Lis.Gso.Shared;

public interface IGsoMapManager {
    Task Initialize(GsoMapOptions mapOptions);
    ValueTask SetNextAvailableMapTypeId();
    ValueTask ShowUserCurrentLocation();
    ValueTask ZoomIn();
    ValueTask ZoomOut();
    ValueTask ShowIncidents(IEnumerable<GsoIncident> incidents);
}
