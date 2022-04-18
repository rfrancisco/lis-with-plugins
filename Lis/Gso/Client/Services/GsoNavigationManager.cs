namespace Lis.Gso.Client.Services;

public class GsoNavigationManager : IGsoNavigationManager {
    private readonly NavigationManager _navManager;
    private readonly IGsoPluginManager _gsoPluginManager;

    public GsoNavigationManager(NavigationManager navManager, IGsoPluginManager gsoPluginManager) {
        _navManager = navManager;
        _gsoPluginManager = gsoPluginManager;
        _navManager.LocationChanged += OnLocationChanged;
    }

    public event Action NavigationContextChanged;

    public Type BottomDrawerComponent { get; private set; }

    public Type PrimaryDrawerComponent { get; private set; }

    public Type SecondaryDrawerComponent { get; private set; }

    public void Initialize() {
        var uri = _navManager.ToAbsoluteUri(_navManager.Uri);
        var primaryActivity = uri.AbsolutePath.Split("/")[1].ToLower();
        var secondaryActivity = QueryHelpers.ParseQuery(uri.Query).GetValueOrDefault("panel", "").FirstOrDefault().ToLower();

        // Check if there's a plugin that wants to handle the current uri
        var handledByPlugin = _gsoPluginManager.AvailablePlugins.Any(x => x.HandleUri(uri));
        if (handledByPlugin)
            return;

        BottomDrawerComponent = GetBottomDrawerComponent(uri);
        PrimaryDrawerComponent = GetPrimaryDrawerComponent(uri);
        SecondaryDrawerComponent = GetSecondaryDrawerComponent(uri);
        NavigationContextChanged?.Invoke();
    }

    public void NavigateTo(string url) {
        _navManager.NavigateTo(url);
    }

    public void ShowSecondaryActivity(string activity) {
        _navManager.NavigateTo(_navManager.GetUriWithQueryParameter("panel", activity));
    }

    public void HideSecondaryActivity() {
        _navManager.NavigateTo(new Uri(_navManager.Uri).AbsolutePath);
    }

    public void ToggleSecondaryActivity(string activity) {
        if (_navManager.Uri.Contains($"panel={activity}")) {
            HideSecondaryActivity();
        } else {
            ShowSecondaryActivity(activity);
        }
    }

    public void ShowPrimaryActivity(string activity, int? id = null) { 
        if (id.HasValue)
            _navManager.NavigateTo($"{activity}/{id.Value}" + new Uri(_navManager.Uri).Query);
        else
            _navManager.NavigateTo($"{activity}" + new Uri(_navManager.Uri).Query);
    }

    public void HidePrimaryActivity() {
        _navManager.NavigateTo(new Uri(_navManager.Uri).Query);
    }

    public void TogglePrimaryActivity(string activity, int? id = null) {
        if (_navManager.Uri.Contains($"/{activity}")) {
            HidePrimaryActivity();
        } else {
            ShowPrimaryActivity(activity, id);
        }
    }

    public void SetDrawers(Type primaryDrawerComponent = null, Type secondaryDrawerComponent = null, Type bottomDrawerComponent = null) {
        PrimaryDrawerComponent = primaryDrawerComponent;
        SecondaryDrawerComponent = secondaryDrawerComponent;
        BottomDrawerComponent = bottomDrawerComponent;
        NavigationContextChanged?.Invoke();
    }

    public Type GetBottomDrawerComponent(Uri uri) {
        var primaryActivity = uri.AbsolutePath.Split("/")[1].ToLower();

        return primaryActivity switch {
            "edit" => typeof(LisGsoIncidentsHistory),
            _ => null
        };
    }

    public Type GetPrimaryDrawerComponent(Uri uri) {
        var primaryActivity = uri.AbsolutePath.Split("/")[1].ToLower();

        return primaryActivity switch {
            "create" => typeof(LisGsoIncidentsCreate),
            "edit" => typeof(LisGsoIncidentsEdit),
            "details" => typeof(LisGsoIncidentsDetails),
            _ => null
        };
    }

    public Type GetSecondaryDrawerComponent(Uri uri) {
        var secondaryActivity = QueryHelpers.ParseQuery(uri.Query).GetValueOrDefault("panel", "").FirstOrDefault().ToLower();
        return secondaryActivity switch {
            "list" => typeof(LisGsoIncidentsList),
            "report" => typeof(LisGsoIncidentsReport),
            _ => null
        };
    }

    #region Auxiliar methods

    private void OnLocationChanged(object sender, LocationChangedEventArgs e) {
        Initialize();
        NavigationContextChanged?.Invoke();
    }

    #endregion
}