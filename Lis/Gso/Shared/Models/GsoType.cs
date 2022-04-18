namespace Lis.Gso.Shared;

public record GsoType(
    int Id,
    string Code,
    string Description,
    string Color,
    string IconName,
    IEnumerable<GsoSubType> SubTypes
);

public record GsoSubType(
    int Id,
    string Code,
    string Description
);