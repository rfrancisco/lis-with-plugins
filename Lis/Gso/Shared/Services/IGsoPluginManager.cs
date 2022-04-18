using System.Reflection;

namespace Lis.Gso.Shared;

public interface IGsoPluginManager {
    IList<IGsoPlugin> AvailablePlugins { get; }
    IList<Assembly> AvailablePluginsAssemblies { get; }
    Task Initialize();
}