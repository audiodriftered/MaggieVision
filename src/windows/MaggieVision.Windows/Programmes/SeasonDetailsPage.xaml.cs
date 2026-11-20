using MaggieVision.Windows.Video;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MaggieVision_Windows.Video.Programmes;

/// <summary>Native episode list for one synthetic TV season.</summary>
public sealed partial class SeasonDetailsPage : Page
{
    private readonly DemoTvProgramme _programme;
    private readonly DemoTvSeason _season;
    private double _episodeCardWidth = 290;
    private bool _isReady;

    public SeasonDetailsPage(DemoTvProgramme programme, DemoTvSeason season)
    {
        _programme = programme ?? throw new ArgumentNullException(nameof(programme));
        _season = season ?? throw new ArgumentNullException(nameof(season));
        InitializeComponent();
        Backdrop.Source = Artwork(season.Episodes[0].ArtworkFile);
        BackButton.Content = $"‹ {_programme.Title}";
        SeasonTitle.Text = season.Label;
        SeasonMetadata.Text = $"{season.Year}  ·  {season.Episodes.Count} Episodes  ·  {season.Genre}";
        _isReady = true;
        PopulateEpisodes();
    }

    public event EventHandler? BackRequested;

    private Button CreateEpisodeCard(DemoTvEpisode episode)
    {
        var button = new Button
        {
            Width = _episodeCardWidth,
            Height = _episodeCardWidth * 0.82,
            Margin = new Thickness(0, 0, 12, 12),
            Padding = new Thickness(0),
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Spacing = 5,
                Children =
                {
                    new Image { Source = Artwork(episode.ArtworkFile), Stretch = Stretch.UniformToFill, Height = 150 },
                    new TextBlock { Text = $"{episode.Number}. {episode.Title}", FontSize = 16, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, TextTrimming = TextTrimming.CharacterEllipsis, Margin = new Thickness(10, 2, 10, 0) },
                    new TextBlock { Text = episode.Runtime, FontSize = 13, Margin = new Thickness(10, 0, 10, 0) },
                    new TextBlock { Text = episode.Synopsis, FontSize = 12, TextWrapping = TextWrapping.Wrap, MaxLines = 2, TextTrimming = TextTrimming.CharacterEllipsis, Margin = new Thickness(10, 0, 10, 8) }
                }
            }
        };
        AutomationProperties.SetName(button, $"Episode {episode.Number}: {episode.Title}, {episode.Runtime}");
        button.Click += OnUnavailableActionClick;
        return button;
    }

    private static BitmapImage Artwork(string fileName) => new(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", fileName)));
    private void OnBackClick(object sender, RoutedEventArgs args) => BackRequested?.Invoke(this, EventArgs.Empty);

    private void OnSizeChanged(object sender, SizeChangedEventArgs args)
    {
        if (!_isReady || args.NewSize.Width <= 0) return;
        var nextWidth = Math.Max(250, (args.NewSize.Width - ContentRoot.Padding.Left - ContentRoot.Padding.Right - 3 * 12) / 4);
        if (Math.Abs(nextWidth - _episodeCardWidth) < 1) return;
        _episodeCardWidth = nextWidth;
        PopulateEpisodes();
    }

    private void PopulateEpisodes()
    {
        EpisodeList.Items.Clear();
        foreach (var episode in _season.Episodes) EpisodeList.Items.Add(CreateEpisodeCard(episode));
    }

    private void OnUnavailableActionClick(object sender, RoutedEventArgs args)
    {
        ActionMessage.Title = "Playback is not implemented";
        ActionMessage.Message = "These are synthetic episode records. No video file was opened.";
        ActionMessage.IsOpen = true;
    }
}
