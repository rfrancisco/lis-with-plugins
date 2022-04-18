var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(c => c.SnackbarConfiguration.PreventDuplicates = false);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IGsoNavigationManager, GsoNavigationManager>();
builder.Services.AddScoped<IGsoHeaderManager, GsoHeaderManager>();
builder.Services.AddScoped<IGsoMapManager, GsoMapManager>();
builder.Services.AddScoped<IGsoPluginManager, GsoPluginManager>();
builder.Services.AddScoped<IGsoWelcomeManager, GsoWelcomeManager>();
builder.Services.AddScoped<GsoStatusService>();
builder.Services.AddScoped<GsoTypeService>();
builder.Services.AddScoped<GsoIncidentsService>();

var host = builder.Build();
var pluginManager = (IGsoPluginManager)host.Services.GetService(typeof(IGsoPluginManager));
await pluginManager.Initialize();
await host.RunAsync();
