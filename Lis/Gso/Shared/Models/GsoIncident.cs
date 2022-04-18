namespace Lis.Gso.Shared;

public record GsoIncident(
    int Id,
    string Reference,
    string Address,
    double Lat,
    double Lng,
    GsoDictionaryItem Status,
    GsoDictionaryItem Type,
    GsoDictionaryItem SubType,
    int Priority,
    bool Internal,
    string InitialImageUrl,
    bool CanEdit,
    string AssignedToUserInitials,
    string AssignedToUserName,
    DateTime LastUpdateDate,
    DateTime ReportDate
);

public record GsoDictionaryItem(
    int Id, 
    string Label
);

