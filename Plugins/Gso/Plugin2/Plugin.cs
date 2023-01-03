using MudBlazor;
using Lis.Gso.Plugin2.Components;
using Lis.Gso.Shared;
using System.Text.Json;

namespace Lis.Gso.Plugin2;

public class Plugin : GsoPluginBase {
    public Plugin(IServiceProvider sp) : base(sp) {
        HeaderActions = new List<GsoAction>() {
            new ("Show Modal", Icons.Material.Filled.Umbrella, () => ShowModal(), null, Color.Tertiary),
            new ("Show Settings Page", Icons.Material.Filled.Settings, () => ShowSettings(), null, Color.Tertiary)
        };
        IncidentsMapActions = new List<GsoAction>() {
            new ("Show CarTrack", "", null, typeof(MapCartrack))
        };
        IncidentsListActions = new List<GsoAction<GsoIncidentsListActionContext>>() {
            new ("Print", Icons.Material.Filled.ImportExport, (data) => PrintList(data), null, Color.Tertiary)
        }; 
        IncidentsListItemActions = new List<GsoAction<GsoIncidentsListItemActionContext>>() {
            new ("Copy", Icons.Material.Filled.Edit, (data) => PrintListItem(data), null, Color.Tertiary),
            new ("Paste", Icons.Material.Filled.Delete, (data) => PrintListItem(data), null, Color.Tertiary),
        }; 
    }

    private Task ShowModal() {
        var dialogSvc = (IDialogService)_sp.GetService(typeof(IDialogService));
        var dialog = dialogSvc.Show<Modal>("Modal");
        return dialog.Result;
    }

    private void ShowSettings() {
        var gsoNavManager = (IGsoNavigationManager)_sp.GetService(typeof(IGsoNavigationManager));
        gsoNavManager.NavigateTo("/settings");
    }

    private void PrintList(GsoIncidentsListActionContext data) {
        Console.WriteLine(JsonSerializer.Serialize(data.Incidents));
    }
    
    private void PrintListItem(GsoIncidentsListItemActionContext data) {
        Console.WriteLine(JsonSerializer.Serialize(data.Incident));
    }
}
