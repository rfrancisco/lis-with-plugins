using MudBlazor;
using Lis.Gso.Plugin1.Components;
using Lis.Gso.Shared;

namespace Lis.Gso.Plugin1;

public class Plugin : GsoPluginBase {

    public Plugin(IServiceProvider sp) : base(sp) {
        HeaderActions = new List<GsoAction>() {
            //new ("Show modal", Icons.Material.Filled.WebAsset, () => ShowModal(), null, Color.Secondary),
            new ("SHow toast", Icons.Material.Filled.NotificationAdd, () => ShowSnackbar(), null, Color.Secondary),
            new ("Interact with navigation service", Icons.Material.Filled.Edit, () => ToggleEdit(), null, Color.Secondary),
        };
        IncidentsMapActions = new List<GsoAction>() {
            //new ("Zomm in", "", null, typeof(MapZoomIn)),
            //new ("Zoom out", "", null, typeof(MapZoomOut)),
            new ("Load GeoJson", "", null, typeof(MapGeoJson)),
            new ("Drawing Tools", "", null, typeof(MapDrawingTools))
        };
    }

    private Task ShowModal() {
        var dialogSvc = (IDialogService)_sp.GetService(typeof(IDialogService));
        var dialog = dialogSvc.Show<Modal>("Modal");
        return dialog.Result;
    }

    private void ShowSnackbar() {
        var snackbar = (ISnackbar)_sp.GetService(typeof(ISnackbar));
        snackbar.Add("This is a notification from Plugin1", Severity.Normal);
    }

    private void ToggleEdit() {
        var gsoNavManager = (IGsoNavigationManager)_sp.GetService(typeof(IGsoNavigationManager));
        gsoNavManager.TogglePrimaryActivity("edit", 97234);
    }
}
