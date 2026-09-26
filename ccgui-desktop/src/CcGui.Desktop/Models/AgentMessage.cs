using System;

namespace CcGui.Desktop.Models;

/// <summary>A single entry in the agent message stream.</summary>
public sealed class AgentMessage
{
    /// <summary>"User" or "Agent".</summary>
    public string Sender { get; init; } = "User";

    public string Content { get; init; } = string.Empty;

    public DateTime Timestamp { get; init; } = DateTime.Now;

    /// <summary>e.g. "This reply took 0:08". Null when not applicable.</summary>
    public string? ExecutionTime { get; init; }

    public string TimestampLabel => Timestamp.ToString("HH:mm");

    public bool IsUser => Sender == "User";
}
