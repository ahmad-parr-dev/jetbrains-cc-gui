using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CcGui.Desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CcGui.Desktop.ViewModels;

public sealed partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _selectedAgentMode = "Subagent";

    [ObservableProperty]
    private string _inputPrompt = string.Empty;

    [ObservableProperty]
    private string _environmentProfile = "AWS: default@us-west-1";

    [ObservableProperty]
    private string _branch = "main";

    [ObservableProperty]
    private bool _isConnected = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProblemSummary))]
    private int _errorCount = 1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProblemSummary))]
    private int _warningCount = 0;

    public string ProblemSummary => $"{ErrorCount} error(s) · {WarningCount} warning(s)";

    public ObservableCollection<AgentMessage> Messages { get; } = new();

    public ObservableCollection<WorkspaceFile> Files { get; } = new();

    public string[] AgentModes { get; } = ["Tasks", "Subagent", "Edits"];

    public string[] Branches { get; } = ["main", "develop", "feature/cc-gui-desktop"];

    public string[] EnvironmentProfiles { get; } =
        ["AWS: default@us-west-1", "AWS: prod@us-east-1", "Local"];

    public MainViewModel()
    {
        Files.Add(new WorkspaceFile { Name = ".agents/skills", LastCommit = "chore(skills): add vercel-react-best-practices skill package", UpdatedAgo = "5 months ago", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = ".github", LastCommit = "ci: guard main against PRs targeting wrong base branch", UpdatedAgo = "last week", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = "ai-bridge", LastCommit = "fix(pnpm): preserve CLI PATH priority when spawning pi binaries", UpdatedAgo = "2 days ago", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = "docs", LastCommit = "docs: record the DSH question follow-up fixes", UpdatedAgo = "last week", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = "src", LastCommit = "feat(models): add Claude Opus 3.5 and GPT-4o / Sonnet drop", UpdatedAgo = "2 days ago", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = "test", LastCommit = "docs: translate comments and JSDoc to English across files", UpdatedAgo = "6 months ago", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = "webview", LastCommit = "chore(release): v0.5.7", UpdatedAgo = "2 days ago", IsDirectory = true });
        Files.Add(new WorkspaceFile { Name = "README.md", LastCommit = "docs: update branding to reflect multi-engine support", UpdatedAgo = "3 weeks ago", IsDirectory = false });

        var now = DateTime.Now;
        Messages.Add(new AgentMessage
        {
            Sender = "User",
            Content = "HELLO",
            Timestamp = now.AddMinutes(-2)
        });
        Messages.Add(new AgentMessage
        {
            Sender = "Agent",
            Content = "Sovereign agent online. Encapsulated local execution enabled — prompts never leave this machine.",
            Timestamp = now.AddMinutes(-2).AddSeconds(8),
            ExecutionTime = "This reply took 0:08"
        });
    }

    [RelayCommand]
    private async Task SendPromptAsync()
    {
        var text = InputPrompt.Trim();
        if (text.Length == 0)
            return;

        Messages.Add(new AgentMessage
        {
            Sender = "User",
            Content = text,
            Timestamp = DateTime.Now
        });
        InputPrompt = string.Empty;

        var sw = Stopwatch.StartNew();
        await Task.Delay(700);
        sw.Stop();

        Messages.Add(new AgentMessage
        {
            Sender = "Agent",
            Content = $"[{SelectedAgentMode}] Executed locally in encapsulated mode: {text}",
            Timestamp = DateTime.Now,
            ExecutionTime = $"This reply took 0:{Math.Max(1, (int)sw.Elapsed.TotalSeconds):00}"
        });
    }
}
