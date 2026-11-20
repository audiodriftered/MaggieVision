using MaggieVision.Windows.Music;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace MaggieVision_Windows.Music;

public enum DemoRepeatMode { Off, One, All }

/// <summary>One native Windows MediaPlayer and queue shared by browsing, Now Playing and the mini-player.</summary>
public sealed class MusicPlaybackSession
{
    private static readonly Lazy<MusicPlaybackSession> Shared = new(() => new MusicPlaybackSession());
    private readonly MediaPlayer _player = new();
    private readonly HashSet<string> _favourites = new(StringComparer.Ordinal);
    private readonly Dictionary<string, int> _ratings = new(StringComparer.Ordinal);
    private bool _sourceLoaded;

    private MusicPlaybackSession()
    {
        _player.MediaEnded += OnMediaEnded;
        _player.MediaFailed += OnMediaFailed;
        Queue = new DemoPlaybackQueue();
        Queue.PlayNow(DemoMusicCatalogue.RecentlyPlayed);
    }

    public static MusicPlaybackSession Current => Shared.Value;
    public DemoPlaybackQueue Queue { get; }
    public DemoMusicTrack? Track => Queue.Current;
    public bool IsPlaying => _player.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;
    public bool IsShuffleEnabled { get; private set; }
    public DemoRepeatMode RepeatMode { get; private set; }
    public double Volume
    {
        get => _player.Volume;
        set => _player.Volume = Math.Clamp(value, 0, 1);
    }
    public TimeSpan Position => _player.PlaybackSession.Position;
    public TimeSpan Duration => _player.PlaybackSession.NaturalDuration > TimeSpan.Zero
        ? _player.PlaybackSession.NaturalDuration
        : TimeSpan.FromSeconds(20);
    public MediaPlayer Player => _player;
    public string? PlaybackError { get; private set; }
    public event EventHandler? StateChanged;

    public bool IsFavourite(DemoMusicTrack track) => _favourites.Contains(track.Id);
    public int RatingFor(DemoMusicTrack track) => _ratings.GetValueOrDefault(track.Id);

    public void SetRating(DemoMusicTrack track, int rating)
    {
        ArgumentNullException.ThrowIfNull(track);
        if (rating is < 0 or > 5) throw new ArgumentOutOfRangeException(nameof(rating));
        _ratings[track.Id] = rating;
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleFavourite(DemoMusicTrack track)
    {
        if (!_favourites.Add(track.Id)) _favourites.Remove(track.Id);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleShuffle()
    {
        IsShuffleEnabled = !IsShuffleEnabled;
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void CycleRepeatMode()
    {
        RepeatMode = RepeatMode switch
        {
            DemoRepeatMode.Off => DemoRepeatMode.All,
            DemoRepeatMode.All => DemoRepeatMode.One,
            _ => DemoRepeatMode.Off
        };
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void PlayNow(IReadOnlyList<DemoMusicTrack> tracks, int selectedIndex = 0)
    {
        Queue.PlayNow(tracks, selectedIndex);
        StartCurrent();
    }

    public void PlayNext(DemoMusicTrack track)
    {
        Queue.PlayNext(track);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddToQueue(DemoMusicTrack track)
    {
        Queue.Add(track);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TogglePlayback()
    {
        if (Track is null)
        {
            PlayNow(DemoMusicCatalogue.AllTracks, 0);
            return;
        }
        if (!_sourceLoaded)
        {
            StartCurrent();
            return;
        }
        if (IsPlaying) _player.Pause(); else _player.Play();
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Next()
    {
        if (Queue.MoveNext(RepeatMode == DemoRepeatMode.All, IsShuffleEnabled) is not null) StartCurrent();
    }

    public void Previous()
    {
        if (_player.PlaybackSession.Position.TotalSeconds > 3)
        {
            _player.PlaybackSession.Position = TimeSpan.Zero;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        else if (Queue.MovePrevious() is not null) StartCurrent();
    }

    public void Seek(TimeSpan position)
    {
        _player.PlaybackSession.Position = position;
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    private async void StartCurrent()
    {
        if (Track is null) return;
        var samplePath = Path.Combine(AppContext.BaseDirectory, "Assets", "demo-music-synthetic.wav");
        try
        {
            var file = await StorageFile.GetFileFromPathAsync(samplePath);
            _player.Source = MediaSource.CreateFromStorageFile(file);
            _sourceLoaded = true;
            PlaybackError = null;
            _player.Play();
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception error)
        {
            PlaybackError = $"Demo audio could not be opened: {error.Message}";
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnMediaEnded(MediaPlayer sender, object args)
    {
        if (RepeatMode == DemoRepeatMode.One)
        {
            _player.PlaybackSession.Position = TimeSpan.Zero;
            _player.Play();
            StateChanged?.Invoke(this, EventArgs.Empty);
            return;
        }
        Next();
    }

    private void OnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
    {
        _sourceLoaded = false;
        PlaybackError = $"Demo audio playback failed: {args.ErrorMessage}";
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}
