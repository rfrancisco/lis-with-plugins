namespace Lis.Gso.Shared;

public interface IGsoHeaderManager { 
    event Action HeaderChanged;
    string Title { get; set; }
    string LogoImageUrl { get; set; }
    IList<GsoAction> Actions { get; set; }
    void Initialize(string title, string logoImageUrl, IEnumerable<GsoAction> actions);
    void AddAction(GsoAction action);
    void AddActions(IEnumerable<GsoAction> actions);
    void RemoveAction(GsoAction action);
}
