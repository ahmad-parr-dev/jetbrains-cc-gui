namespace CcGui.Desktop.Models;

/// <summary>One row in the repository / workspace file explorer.</summary>
public sealed class WorkspaceFile
{
    public string Name { get; init; } = string.Empty;

    public string LastCommit { get; init; } = string.Empty;

    public string UpdatedAgo { get; init; } = string.Empty;

    public bool IsDirectory { get; init; }
}
