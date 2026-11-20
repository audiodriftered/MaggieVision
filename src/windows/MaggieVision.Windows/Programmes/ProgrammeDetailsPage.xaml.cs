using MaggieVision.Windows.Video;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MaggieVision_Windows.Video.Programmes;

/// <summary>Native details surface for one synthetic TV programme.</summary>
public sealed partial class ProgrammeDetailsPage : Page
{
    private readonly DemoTvProgramme _programme;
    private double _seasonCardWidth = 320;
    private bool _isReady;

    public ProgrammeDetailsPage(DemoTvProgramme programme)
    {
        _programme = programme ?? throw new ArgumentNullException(nameof(programme));
        InitializeComponent();
        Backdrop.Source = Artwork(programme.ArtworkFile);
        ProgrammeTitle.Text = programme.Title;
        Metadata.Text = $"{programme.YearRange}  ·  {programme.Seasons.Count} season{(programme.Seasons.Count == 1 ? "" : "s")}  ·  {programme.Genre}";
        Synopsis.Text = programme.Synopsis;
        AutomationProperties.SetName(Backdrop, $"Synthetic backdrop for {programme.Title}");
        _isReady = true;
        PopulateSeasons();
    }

    public event EventHandler? BackRequested;
    public event EventHandler<int>? SeasonSelected;
    public DemoTvProgramme Programme => _programme;

    private Button CreateSeasonCard(DemoTvSeason season)
    {
        var button = new Button
        {
            Width = _seasonCardWidth,
            Height = _seasonCardWidth * 0.55,
            Content = new Grid
            {
                Children =
                {
                    new Image { Source = Artwork(season.Episodes[0].ArtworkFile), Stretch = Microsoft.UI.Xaml.Media.Stretch.UniformToFill },
                    new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Padding = new Thickness(12),
                        Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(190, 0, 0, 0)),
                        Children =
                        {
                            new TextBlock { Text = season.Label, FontSize = 18, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold },
                            new TextBlock { Text = $"{season.Episodes.Count} Episodes" }
                        }
                    }
                }
            },
            Tag = season.Number
        };
        AutomationProperties.SetName(button, $"Open {season.Label}, {season.Episodes.Count} episodes");
        button.Click += OnSeasonClick;
        return button;
    }

    private static BitmapImage Artwork(string fileName) => new(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", fileName)));
    private void OnBackClick(object sender, RoutedEventArgs args) => BackRequested?.Invoke(this, EventArgs.Empty);
    private void OnSeasonClick(object sender, RoutedEventArgs args)
    {
        if ((sender as Button)?.Tag is int number) SeasonSelected?.Invoke(this, number);
    }

    private void OnUnavailableActionClick(object sender, RoutedEventArgs args)
    {
        ActionMessage.Title = "Not available in this build";
        ActionMessage.Message = "Playback and profile-specific watchlist/favourite state require the future library and playback integrations.";
        ActionMessage.IsOpen = true;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args)
    {
        if (!_isReady || args.NewSize.Width <= 0) return;
        var nextWidth = Math.Max(260, (args.NewSize.Width - ContentRoot.Padding.Left - ContentRoot.Padding.Right - 2 * 14) / 3);
        if (Math.Abs(nextWidth - _seasonCardWidth) < 1) return;
        _seasonCardWidth = nextWidth;
        PopulateSeasons();
    }

    private void PopulateSeasons()
    {
        SeasonCards.Children.Clear();
        foreach (var season in _programme.Seasons) SeasonCards.Children.Add(CreateSeasonCard(season));
    }
}
