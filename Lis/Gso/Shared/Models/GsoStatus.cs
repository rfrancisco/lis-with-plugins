namespace Lis.Gso.Shared;

public record GsoStatus(
    int Id,
    string Code,
    string Description,
    string Color,
    string ClusterIcon,
    string IconName,
    bool isValidation,
    bool isExecution,
    bool ShowMobile,
    bool startVisible
);
