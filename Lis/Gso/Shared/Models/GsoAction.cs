using MudBlazor;

namespace Lis.Gso.Shared;

public record GsoAction(
    string Label,
    string Icon,
    Action OnClick,
    Type ComponentType = null,
    Color Color = Color.Inherit
);

public record GsoAction<T>(
    string Label,
    string Icon,
    Action<T> OnClick,
    Type ComponentType = null,
    Color Color = Color.Inherit
);
