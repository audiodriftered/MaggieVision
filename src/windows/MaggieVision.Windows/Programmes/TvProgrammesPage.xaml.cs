using MaggieVision.Windows.Video;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace MaggieVision_Windows.Video.Programmes;

/// <summary>Native TV landing page populated exclusively by fictional demonstration programmes.</summary>
public sealed partial class TvProgrammesPage : Page
{
    private bool _isReady;
    private bool _isBrowseMode;
    private double _cardWidth = 300;

    public TvProgrammesPage()
    {
        InitializeComponent();
        _isReady = true;
        PopulateRows();
        RefreshResults();
    }

    public event EventHandler<DemoTvProgramme>? ProgrammeSelected;

    private Button CreateCard(DemoTvProgramme show, string? status = null, bool wide = false)
    {
        var width = _cardWidth;
        var content = new Grid();
        content.Children.Add(new Image
        {
            Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", show.ArtworkFile))),
            Stretch = Stretch.UniformToFill
        });
        content.Children.Add(new Rectangle
        {
            Height = 64,
            VerticalAlignment = VerticalAlignment.Bottom,
            Fill = new LinearGradientBrush
            {
                StartPoint = new Windows.Foundation.Point(0.5, 0),
                EndPoint = new Windows.Foundation.Point(0.5, 1),
                GradientStops =
                {
                    new GradientStop { Color = Windows.UI.Color.FromArgb(0, 0, 0, 0), Offset = 0 },
                    new GradientStop { Color = Windows.UI.Color.FromArgb(235, 0, 0, 0), Offset = 1 }
                }
            }
        });
        var labels = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, Spacing = 2, Padding = new Thickness(10, 0, 10, 10) };
        labels.Children.Add(new TextBlock { Text = show.Title, FontSize = 16, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, TextTrimming = TextTrimming.CharacterEllipsis });
        labels.Children.Add(new TextBlock { Text = status ?? $"{show.Seasons.Count} season{(show.Seasons.Count == 1 ? "" : "s")}", FontSize = 13, TextTrimming = TextTrimming.CharacterEllipsis });
        content.Children.Add(labels);

        var button = new Button
        {
            Width = width,
            Height = width * 9d / 16d,
            Padding = new Thickness(0),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = content,
            Tag = show
        };
        AutomationProperties.SetName(button, $"Open TV programme details for {show.Title}");
        button.Click += OnProgrammeClick;
        return button;
    }

    private void OnProgrammeClick(object sender, RoutedEventArgs args)
    {
        if ((sender as Button)?.Tag is DemoTvProgramme show) ProgrammeSelected?.Invoke(this, show);
    }

    private void OnViewClick(object sender, RoutedEventArgs args)
    {
        _isBrowseMode = (sender as RadioButton)?.Tag?.ToString() == "browse";
        RefreshResults();
    }

    private void OnSearchClick(object sender, RoutedEventArgs args)
    {
        SearchBox.Visibility = SearchBox.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        if (SearchBox.Visibility == Visibility.Collapsed) SearchBox.Text = string.Empty;
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs args)
    {
        if (_isReady) RefreshResults();
    }

    private void RefreshResults()
    {
        if (!_isReady) return;
        var query = SearchBox.Text.Trim();
        var matches = DemoTvCatalogue.All.Where(show => string.IsNullOrEmpty(query) || show.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToArray();
        ContinueSection.Visibility = _isBrowseMode || !string.IsNullOrEmpty(query) ? Visibility.Collapsed : Visibility.Visible;
        ResumeSection.Visibility = _isBrowseMode || !string.IsNullOrEmpty(query) ? Visibility.Collapsed : Visibility.Visible;
        RecentCards.Children.Clear();
        foreach (var show in matches.OrderBy(show => show.Title, StringComparer.OrdinalIgnoreCase)) RecentCards.Children.Add(CreateCard(show));
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args)
    {
        if (!_isReady || args.NewSize.Width <= 0) return;
        var nextWidth = Math.Max(240, (args.NewSize.Width - ContentRoot.Padding.Left - ContentRoot.Padding.Right - 3 * 12) / 4);
        if (Math.Abs(nextWidth - _cardWidth) < 1) return;
        _cardWidth = nextWidth;
        PopulateRows();
        RefreshResults();
    }

    private void PopulateRows()
    {
        var shows = DemoTvCatalogue.All;
        LibraryCount.Text = $"{shows.Count} demonstration programmes";
        ContinueCards.Children.Clear();
        ContinueCards.Children.Add(CreateCard(shows[0], "S2 · E4 · First Impressions", true));
        ContinueCards.Children.Add(CreateCard(shows[1], "S1 · E2 · A Different View", true));
        ContinueCards.Children.Add(CreateCard(shows[2], "S2 · E1 · The Long Way", true));
        ContinueCards.Children.Add(CreateCard(shows[3], "S1 · E3 · Small Hours", true));
        ResumeCards.Children.Clear();
        ResumeCards.Children.Add(CreateCard(shows[0], "S1 · E5 · The Invitation", true));
        ResumeCards.Children.Add(CreateCard(shows[2], "S1 · E2 · A Different View", true));
    }

}
