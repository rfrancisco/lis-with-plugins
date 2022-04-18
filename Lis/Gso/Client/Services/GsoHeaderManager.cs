namespace Lis.Gso.Client.Services;

public class GsoHeaderManager : IGsoHeaderManager {
    private string _title;
    private string _logoImageUrl;
    private IList<GsoAction> _actions = new List<GsoAction>();

    public event Action HeaderChanged;

    public string Title {
        get => _title;
        set { _title = value; HeaderChanged?.Invoke(); }
    }

    public string LogoImageUrl {
        get => _logoImageUrl;
        set { _logoImageUrl = value; HeaderChanged?.Invoke(); }
    }

    public IList<GsoAction> Actions {
        get => _actions;
        set { _actions = value; HeaderChanged?.Invoke(); }
    }

    public void Initialize(string title, string logoImageUrl = "", IEnumerable<GsoAction> actions = null) {
        _title = title;
        _logoImageUrl = logoImageUrl;
        _actions = actions?.ToList();
        HeaderChanged?.Invoke();
    }

    public void AddAction(GsoAction action) {
        _actions.Add(action);
        HeaderChanged?.Invoke();
    }

    public void AddActions(IEnumerable<GsoAction> actions) {
        foreach (var action in actions) {
            _actions.Add(action);
        }
        HeaderChanged?.Invoke();
    }

    public void RemoveAction(GsoAction action) {
        _actions.Remove(action);
        HeaderChanged?.Invoke();
    }
}