using Lis.Gso.Shared;
using Microsoft.AspNetCore.Components;

namespace Lis.Gso.Client.Shared.Components;

public class LisGsoBaseActivity : ComponentBase {

    [Inject]
    public IGsoNavigationManager GsoNavigation { get; set; }

    [Inject]
    public IGsoPluginManager GsoPlugins { get; set; }

    [CascadingParameter]
    public int Id { get; set; }
}
