namespace Lis.Gso.Client.Services;

public class GsoWelcomeManager : IGsoWelcomeManager {
    private readonly IDialogService _dialogService;
    private readonly ILocalStorageService _localStorage;
    private readonly IGsoNavigationManager _gsoNavigationMan;
    private readonly ILogger<GsoWelcomeManager> _logger;

    public GsoWelcomeManager(
        IDialogService dialogService,
        ILocalStorageService localStorage,
        IGsoNavigationManager gsoNavigationMan,
        ILogger<GsoWelcomeManager> logger
    ) {
        _dialogService = dialogService;
        _localStorage = localStorage;
        _gsoNavigationMan = gsoNavigationMan;
        _logger = logger;
    }

    public async void Show() {
        var showWelcome = (await _localStorage.GetItemAsStringAsync("welcome")) != "dont-show";

        if (showWelcome) {
            var options = new DialogOptions() { DisableBackdropClick = true, CloseOnEscapeKey = false };
            var dialog = _dialogService.Show<LisGsoWelcome>("Welcome", options);
            var result = await dialog.Result;
            // Present the modal and return a tupple with the button that was clicked (Item1), and a
            // boolean (Item2) indicating if the user wants to hide the welcome window in the future
            var data = (Tuple<string, bool>)result.Data;

            // Save option not to show the welcome screen again
            if (data.Item2)
                await _localStorage.SetItemAsStringAsync("welcome", "dont-show");

            // Show the create form if the user clicked on the 'report' button
            if (data.Item1 == "report")
                _gsoNavigationMan.ShowPrimaryActivity("create");
        } else {
            _logger.LogInformation("User preference disabled the Welcome dialog");
        }
    }
}