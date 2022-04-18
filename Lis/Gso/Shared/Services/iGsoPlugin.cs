namespace Lis.Gso.Shared;

public interface IGsoPlugin {
    IEnumerable<GsoAction> HeaderActions { get; }
    IEnumerable<GsoAction> IncidentsMapActions { get; }
    IEnumerable<GsoAction<GsoIncidentsListActionContext>> IncidentsListActions { get; }
    IEnumerable<GsoAction<GsoIncidentsListItemActionContext>> IncidentsListItemActions { get; }
    IEnumerable<GsoExtension> IncidentCreateExtensions { get; }
    IEnumerable<GsoExtension> IncidentEditExtensions { get; }
    IEnumerable<GsoExtension> IncidentDetailsExtensions { get; }
    bool HandleUri(Uri uri);
}

public abstract class GsoPluginBase : IGsoPlugin {
    protected IServiceProvider _sp;    
    public IEnumerable<GsoAction> HeaderActions { get; protected set; } = new List<GsoAction>();
    public IEnumerable<GsoAction> IncidentsMapActions { get; protected set; } = new List<GsoAction>();
    public IEnumerable<GsoAction<GsoIncidentsListActionContext>> IncidentsListActions { get; protected set; } = new List<GsoAction<GsoIncidentsListActionContext>>();
    public IEnumerable<GsoAction<GsoIncidentsListItemActionContext>> IncidentsListItemActions { get; protected set; } = new List<GsoAction<GsoIncidentsListItemActionContext>>();
    public IEnumerable<GsoExtension> IncidentCreateExtensions { get; protected set; } = new List<GsoExtension>();
    public IEnumerable<GsoExtension> IncidentEditExtensions { get; protected set; } = new List<GsoExtension>();
    public IEnumerable<GsoExtension> IncidentDetailsExtensions { get; protected set; } = new List<GsoExtension>();
    public GsoPluginBase(IServiceProvider sp) => _sp = sp;
    public virtual bool HandleUri(Uri uri) => false;
}
