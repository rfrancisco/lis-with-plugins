namespace Lis.Gso.Shared;

public record GsoStatus(
    int Id,
    string Code,
    string Description,
    string Color,
    string ClusterIcon,
    string IconName,
    bool IsValidation,
    bool IsExecution,
    bool ShowMobile,
    bool StartVisible
);
