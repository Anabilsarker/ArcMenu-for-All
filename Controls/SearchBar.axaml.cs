using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace ArcMenu_for_All;

public partial class SearchBar : UserControl
{
    // Text Property
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<SearchBar, string>(
            nameof(Text),
            defaultValue: string.Empty,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    // Placeholder Property
    public static readonly StyledProperty<string> PlaceholderProperty =
        AvaloniaProperty.Register<SearchBar, string>(
            nameof(Placeholder),
            defaultValue: "Enter text...");

    // Leading Icon Property
    public static readonly StyledProperty<object> LeadingIconProperty =
        AvaloniaProperty.Register<SearchBar, object>(nameof(LeadingIcon));

    // Trailing Icon Property
    public static readonly StyledProperty<object> TrailingIconProperty =
        AvaloniaProperty.Register<SearchBar, object>(nameof(TrailingIcon));

    // IsReadOnly Property
    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(IsReadOnly));

    // MaxLength Property
    public static readonly StyledProperty<int> MaxLengthProperty =
        AvaloniaProperty.Register<SearchBar, int>(
            nameof(MaxLength),
            defaultValue: 0);

    // AcceptsReturn Property
    public static readonly StyledProperty<bool> AcceptsReturnProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(AcceptsReturn));

    // TextWrapping Property
    public static readonly StyledProperty<TextWrapping> TextWrappingProperty =
        AvaloniaProperty.Register<SearchBar, TextWrapping>(
            nameof(TextWrapping),
            defaultValue: TextWrapping.NoWrap);

    // Glass Tint Property (for customization)
    public static readonly StyledProperty<IBrush> GlassTintProperty =
        AvaloniaProperty.Register<SearchBar, IBrush>(
            nameof(GlassTint),
            defaultValue: new SolidColorBrush(Colors.White, 0.5));

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public object LeadingIcon
    {
        get => GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public object TrailingIcon
    {
        get => GetValue(TrailingIconProperty);
        set => SetValue(TrailingIconProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public int MaxLength
    {
        get => GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public bool AcceptsReturn
    {
        get => GetValue(AcceptsReturnProperty);
        set => SetValue(AcceptsReturnProperty, value);
    }

    public TextWrapping TextWrapping
    {
        get => GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    public IBrush GlassTint
    {
        get => GetValue(GlassTintProperty);
        set => SetValue(GlassTintProperty, value);
    }

    public SearchBar()
    {
        InitializeComponent();
        //inputField
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        // Handle custom property changes if needed
        if (change.Property == GlassTintProperty)
        {
            // Update glass tint dynamically
            UpdateGlassTint();
        }
    }

    private void UpdateGlassTint()
    {
        // You can implement dynamic tint updates here if needed
        // This would involve finding and updating the template elements
    }
}