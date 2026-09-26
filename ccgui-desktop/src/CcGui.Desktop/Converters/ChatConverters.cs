using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media;

namespace CcGui.Desktop.Converters;

/// <summary>User messages align right, agent messages align left.</summary>
public sealed class SenderToAlignmentConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() == "User" ? HorizontalAlignment.Right : HorizontalAlignment.Left;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => BindingOperations.DoNothing;
}

/// <summary>Chat bubble fill: blue for the user, dark slate for the agent.</summary>
public sealed class SenderToBubbleBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush UserBrush = new(Color.Parse("#0972D3"));
    private static readonly SolidColorBrush AgentBrush = new(Color.Parse("#232F3E"));

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() == "User" ? UserBrush : AgentBrush;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => BindingOperations.DoNothing;
}

/// <summary>True when the bound value is not null (for IsVisible bindings).</summary>
public sealed class NullToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => BindingOperations.DoNothing;
}

/// <summary>Connection dot: green when connected, red otherwise.</summary>
public sealed class BoolToConnectionBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush ConnectedBrush = new(Color.Parse("#22C55E"));
    private static readonly SolidColorBrush DisconnectedBrush = new(Color.Parse("#EF4444"));

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? ConnectedBrush : DisconnectedBrush;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => BindingOperations.DoNothing;
}
