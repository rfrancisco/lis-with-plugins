using MudBlazor;
using Lis.Gso.Plugin3.Components;
using Lis.Gso.Shared;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;

namespace Lis.Gso.Plugin3;

public class Plugin : GsoPluginBase {
    public Plugin(IServiceProvider sp) : base(sp) {
        HeaderActions = new List<GsoAction>() {
            new ("Show custom content in bottom drawer", Icons.Filled.PictureInPicture, () => ShowControlInDrawer(), null, Color.Warning)
        };
        IncidentsListActions = new List<GsoAction<GsoIncidentsListActionContext>>() {
            new ("Export to csv", Icons.Filled.ImportExport, (data) => ExportList(data), null, Color.Warning)
        }; 
        IncidentsListItemActions = new List<GsoAction<GsoIncidentsListItemActionContext>>() {
            new ("Edit", Icons.Filled.Edit, (_) => {}, null, Color.Warning),
            new ("Delete", Icons.Filled.Delete, (_) => {}, null, Color.Warning),
        };
        IncidentEditExtensions = new List<GsoExtension>() { 
            new(typeof(EditExtensionTop), ExtentionPlacement.Top, () => Console.WriteLine("Edit form: Plugin 3 extension - before save"), () => Console.WriteLine("Edit form: Plugin 3 extension - after save")),
            new(typeof(EditExtensionBottom), ExtentionPlacement.Bottom, () => Console.WriteLine("Edit form: Plugin 3 extension - before save"), () => Console.WriteLine("Edit form: Plugin 3 extension - after save")),
        };
    }

    private void ShowControlInDrawer() {
        var gsoNavManager = (IGsoNavigationManager)_sp.GetService(typeof(IGsoNavigationManager));
        gsoNavManager.SetDrawers(typeof(CustomContent));
    }

    public override bool HandleUri(Uri uri) {
        var primaryActivity = uri.AbsolutePath.Split("/")[1].ToLower();

        if (primaryActivity == "create")
        {
            var gsoNavManager = (IGsoNavigationManager)_sp.GetService(typeof(IGsoNavigationManager));
            
            gsoNavManager.SetDrawers(typeof(CustomCreate), gsoNavManager.GetSecondaryDrawerComponent(uri), gsoNavManager.GetBottomDrawerComponent(uri));
            return true;
        }

        return false;
    }

    private void ExportList(GsoIncidentsListActionContext data) {
        var sb = new StringBuilder();
        foreach (GsoIncident incident in data.Incidents) {
            sb.AppendLine($"{incident.Id},{incident.Reference},{incident.Lat},{incident.Lng},{incident.Status.Label},{incident.Type.Label},{incident.Address}");
        }
        byte[] file = Encoding.UTF8.GetBytes(sb.ToString());
        JsInvoker.Instance.InitializeAsync(_sp);
        JsInvoker.Instance.DownloadFile("incidents.csv", "text/csv", file);
    }
}
