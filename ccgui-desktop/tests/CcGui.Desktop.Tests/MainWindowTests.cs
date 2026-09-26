using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CcGui.Desktop.ViewModels;
using CcGui.Desktop.Views;
using Xunit;

[assembly: AvaloniaTestApplication(typeof(CcGui.Desktop.Tests.TestApp))]

namespace CcGui.Desktop.Tests;

public class TestApp : Avalonia.Application
{
    public override void Initialize()
    {
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<TestApp>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}

public class MainWindowTests
{
    [AvaloniaFact]
    public async Task MainWindow_Renders_And_SendPrompt_Works()
    {
        var vm = new MainViewModel();
        var window = new MainWindow
        {
            DataContext = vm,
            Width = 1440,
            Height = 900
        };
        window.Show();

        // Seed data from the view model.
        Assert.Equal(8, vm.Files.Count);
        Assert.Equal(2, vm.Messages.Count);
        Assert.Equal("Subagent", vm.SelectedAgentMode);

        // Drive the full send flow: user message + simulated agent reply.
        vm.InputPrompt = "deploy the mesh";
        await vm.SendPromptCommand.ExecuteAsync(null);

        Assert.Equal(string.Empty, vm.InputPrompt);
        Assert.Equal(4, vm.Messages.Count);
        Assert.Equal("User", vm.Messages[^2].Sender);
        Assert.Equal("deploy the mesh", vm.Messages[^2].Content);
        Assert.Equal("Agent", vm.Messages[^1].Sender);
        Assert.NotNull(vm.Messages[^1].ExecutionTime);
        Assert.Contains("Subagent", vm.Messages[^1].Content);

        // Render the real window to a PNG — proves the XAML loads and lays out.
        await Task.Delay(300);
        using var bitmap = new RenderTargetBitmap(new PixelSize(1440, 900), new Vector(96, 96));
        bitmap.Render(window);
        var shotPath = Path.Combine(Path.GetTempPath(), "ccgui-mainwindow.png");
        bitmap.Save(shotPath);

        window.Close();

        Assert.True(File.Exists(shotPath));
    }

    [AvaloniaFact]
    public void SendPrompt_Ignores_Blank_Input()
    {
        var vm = new MainViewModel();
        var before = vm.Messages.Count;

        vm.InputPrompt = "   ";
        vm.SendPromptCommand.Execute(null);

        Assert.Equal(before, vm.Messages.Count);
    }

    [AvaloniaFact]
    public void AgentMode_Switches()
    {
        var vm = new MainViewModel();

        vm.SelectedAgentMode = "Edits";

        Assert.Equal("Edits", vm.SelectedAgentMode);
        Assert.Contains("Edits", vm.AgentModes);
    }
}
