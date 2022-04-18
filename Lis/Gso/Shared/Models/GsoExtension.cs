namespace Lis.Gso.Shared;

public record GsoExtension(
    Type ComponentType,
    ExtentionPlacement Placement,
    Action BeforeSave = null,
    Action AfterSave = null
);

public enum ExtentionPlacement { 
    Top,
    Bottom
}
