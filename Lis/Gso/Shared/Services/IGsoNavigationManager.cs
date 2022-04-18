namespace Lis.Gso.Shared;

public interface IGsoNavigationManager {
    event Action NavigationContextChanged;
    Type BottomDrawerComponent { get; }
    Type PrimaryDrawerComponent { get; }
    Type SecondaryDrawerComponent { get; }
    void Initialize();
    void NavigateTo(string url);
    void ShowSecondaryActivity(string activity);
    void HideSecondaryActivity();
    void ToggleSecondaryActivity(string activity);
    void ShowPrimaryActivity(string activity, int? id = null);
    void HidePrimaryActivity();
    void TogglePrimaryActivity(string activity, int? id = null);
    void SetDrawers(Type primaryDrawerComponent = null, Type secondaryDrawerComponent = null, Type bottomDrawerComponent = null);
    Type GetBottomDrawerComponent(Uri uri);
    Type GetPrimaryDrawerComponent(Uri uri);
    Type GetSecondaryDrawerComponent(Uri uri);
}
