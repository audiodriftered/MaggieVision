using System.Buffers.Binary;
using MaggieVision.Windows.Music;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace MaggieVision_Windows.Music;

/// <summary>Immersive native player. All controls bind to the same MusicPlaybackSession as the mini-player.</summary>
public sealed partial class MusicNowPlayingPage : Page
{
    private static readonly Lazy<short[]> PcmSamples = new(LoadPcmSamples);
    private static readonly int[] BandHeights = [10, 14, 20, 26, 33, 42, 36, 28, 22, 16, 12, 8];
    private readonly MusicPlaybackSession _session = MusicPlaybackSession.Current;
    private bool _updating;
    private bool _ratingReady;
    private int? _selectedQueueIndex;
    private string? _lastTrackId;
    private readonly List<Rectangle> _bars = [];

    public MusicNowPlayingPage()
    {
        InitializeComponent();
        _session.StateChanged += OnSessionChanged;
        CreateVisualizerBars();
        Loaded += (_, _) =>
        {
            _ratingReady = true;
            RefreshPlaybackState();
        };
        RefreshPlaybackState();
    }

    private void CreateVisualizerBars()
    {
        foreach (var maximumHeight in BandHeights)
        {
            var bar = new Rectangle
            {
                Width = 9,
                Height = maximumHeight,
                RadiusX = 4,
                RadiusY = 4,
                VerticalAlignment = VerticalAlignment.Center,
                Fill = new Microsoft.UI.Xaml.Media.SolidColorBrush(Color.FromArgb(220, 90, 190, 255))
            };
            _bars.Add(bar);
            Visualizer.Children.Add(bar);
        }
    }

    public void RefreshPlaybackState()
    {
        var track = _session.Track;
        if (track is null) return;
        _updating = true;
        if (_lastTrackId != track.Id)
        {
            var image = new BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", track.ArtworkFile)));
            Backdrop.Source = image;
            _lastTrackId = track.Id;
        }
        TrackTitle.Text = track.Title;
        TrackSubtitle.Text = $"{track.Artist}  ·  {track.Album}";
        Remaining.Text = FormatTime(_session.Duration);
        Elapsed.Text = FormatTime(_session.Position);
        Progress.Maximum = Math.Max(1, _session.Duration.TotalSeconds);
        Progress.Value = Math.Clamp(_session.Position.TotalSeconds, Progress.Minimum, Progress.Maximum);
        PlayPauseGlyph.Glyph = _session.IsPlaying ? "\uE769" : "\uE768";
        FavouriteGlyph.Glyph = _session.IsFavourite(track) ? "\uEB52" : "\uEB51";
        TrackRating.Value = _session.RatingFor(track);
        AutomationProperties.SetName(ShuffleButton, _session.IsShuffleEnabled ? "Shuffle on" : "Toggle shuffle");
        AutomationProperties.SetName(RepeatButton, $"Repeat mode: {_session.RepeatMode}");
        RepeatGlyph.Glyph = _session.RepeatMode == DemoRepeatMode.Off ? "\uE8EE" : "\uE8ED";
        PlaybackStatus.Text = _session.PlaybackError ?? "Spectrum is calculated from the currently playing synthetic PCM sample.";
        var spectrum = new float[BandHeights.Length];
        var start = (int)(_session.Position.TotalSeconds * 44100);
        if (_session.IsPlaying && start >= 0 && start + 1024 <= PcmSamples.Value.Length)
            spectrum = DemoAudioSpectrum.Analyze(PcmSamples.Value.AsSpan(start, 1024), 44100);
        for (var index = 0; index < _bars.Count; index++)
        {
            _bars[index].Height = Math.Max(3, BandHeights[index] * spectrum[index]);
        }
        _updating = false;
    }

    private void OnSessionChanged(object? sender, EventArgs args) => DispatcherQueue.TryEnqueue(RefreshPlaybackState);
    private void OnPlayPauseClick(object sender, RoutedEventArgs args) => _session.TogglePlayback();
    private void OnPreviousClick(object sender, RoutedEventArgs args) => _session.Previous();
    private void OnNextClick(object sender, RoutedEventArgs args) => _session.Next();
    private void OnShuffleClick(object sender, RoutedEventArgs args) => _session.ToggleShuffle();
    private void OnRepeatClick(object sender, RoutedEventArgs args) => _session.CycleRepeatMode();
    private void OnFavouriteClick(object sender, RoutedEventArgs args) { if (_session.Track is { } track) _session.ToggleFavourite(track); }
    private void OnRatingChanged(RatingControl sender, object args)
    {
        if (_ratingReady && !_updating && _session.Track is { } track) _session.SetRating(track, (int)sender.Value);
    }

    private void OnProgressChanged(object sender, RangeBaseValueChangedEventArgs args)
    {
        if (!_updating) _session.Seek(TimeSpan.FromSeconds(args.NewValue));
    }

    private void OnOverlayClick(object sender, RoutedEventArgs args)
    {
        if ((sender as Button)?.Tag is not string overlay) return;
        OverlayHost.Visibility = Visibility.Visible;
        OverlayTitle.Text = overlay switch { "queue" => "Up Next", "equaliser" => "Equaliser", "lyrics" => "Lyrics", _ => "Track details" };
        QueueList.Visibility = overlay == "queue" ? Visibility.Visible : Visibility.Collapsed;
        var buttons = ((Grid)OverlayHost.Child).Children.OfType<StackPanel>().Last();
        buttons.Visibility = overlay == "queue" ? Visibility.Visible : Visibility.Collapsed;
        if (overlay == "queue") RefreshQueue();
        else
        {
            QueueList.ItemsSource = overlay switch
            {
                "equaliser" => new[] { "Output: default Windows device", "Equaliser controls become available with the profile audio engine." },
                "lyrics" => new[] { "No local .lrc file is attached to this synthetic track." },
                _ => new[] { $"{_session.Track?.Title} · {_session.Track?.Artist} · {_session.Track?.Album}", "20 second generated PCM demonstration recording · 44.1 kHz · 16-bit mono" }
            };
        }
    }

    private void RefreshQueue()
    {
        QueueList.ItemsSource = null;
        QueueList.ItemsSource = _session.Queue.Tracks.Select((track, index) => $"{(index == _session.Queue.CurrentIndex ? "▶ " : "")} {track.Title} — {track.Artist}").ToArray();
    }

    private void OnQueueSelectionChanged(object sender, SelectionChangedEventArgs args) => _selectedQueueIndex = QueueList.SelectedIndex >= 0 ? QueueList.SelectedIndex : null;
    private void OnMoveQueueUp(object sender, RoutedEventArgs args) => MoveQueueSelection(-1);
    private void OnMoveQueueDown(object sender, RoutedEventArgs args) => MoveQueueSelection(1);
    private void OnCloseOverlayClick(object sender, RoutedEventArgs args) => OverlayHost.Visibility = Visibility.Collapsed;

    public void OpenQueue()
    {
        OverlayHost.Visibility = Visibility.Visible;
        OverlayTitle.Text = "Up Next";
        QueueList.Visibility = Visibility.Visible;
        ((Grid)OverlayHost.Child).Children.OfType<StackPanel>().Last().Visibility = Visibility.Visible;
        RefreshQueue();
    }

    private void MoveQueueSelection(int offset)
    {
        if (_selectedQueueIndex is not { } index || !_session.Queue.Move(index, index + offset)) return;
        RefreshQueue();
        QueueList.SelectedIndex = index + offset;
    }

    private static short[] LoadPcmSamples()
    {
        var path = System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", "demo-music-synthetic.wav");
        var bytes = File.ReadAllBytes(path);
        var dataOffset = 44;
        var sampleCount = Math.Max(0, (bytes.Length - dataOffset) / sizeof(short));
        var samples = new short[sampleCount];
        for (var index = 0; index < samples.Length; index++) samples[index] = BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(dataOffset + index * 2, 2));
        return samples;
    }

    private static string FormatTime(TimeSpan time) => $"{(int)time.TotalMinutes}:{time.Seconds:00}";
}
