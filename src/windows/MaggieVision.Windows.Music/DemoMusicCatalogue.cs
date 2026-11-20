namespace MaggieVision.Windows.Music;

/// <summary>A fictional album track backed by the bundled synthetic demo recording.</summary>
public sealed record DemoMusicTrack(string Id, string Title, string Artist, string Album, string Duration, string ArtworkFile);

/// <summary>A synthetic album summary for Music Home.</summary>
public sealed record DemoMusicAlbum(string Title, string Artist, int Year, IReadOnlyList<DemoMusicTrack> Tracks, string ArtworkFile);

/// <summary>An immutable fictional artist fixture.</summary>
public sealed record DemoMusicArtist(string Id, string Name, string Genre, IReadOnlyList<DemoMusicAlbum> Albums)
{
    public int TrackCount => Albums.Sum(album => album.Tracks.Count);
}

/// <summary>Deterministic demonstration music. No filesystem scan or media-library access occurs.</summary>
public static class DemoMusicCatalogue
{
    private static DemoMusicTrack Track(string id, string title, string artist, string album, string art) =>
        new(id, title, artist, album, "0:20", art);

    private static readonly DemoMusicArtist[] Artists =
    [
        new("north-window", "North Window", "Ambient", [
            new("Quiet Signals", "North Window", 2025, [Track("windowlight", "Windowlight", "North Window", "Quiet Signals", "demo-video-00.png"), Track("soft-arrival", "Soft Arrival", "North Window", "Quiet Signals", "demo-video-03.png")], "demo-video-00.png"),
            new("Open Water", "North Window", 2024, [Track("blue-hour", "Blue Hour", "North Window", "Open Water", "demo-video-02.png")], "demo-video-02.png")]),
        new("paper-planes", "Paper Planes", "Indie", [
            new("Small Hours", "Paper Planes", 2025, [Track("small-hours-song", "Small Hours", "Paper Planes", "Small Hours", "demo-video-05.png"), Track("stay-awake", "Stay Awake", "Paper Planes", "Small Hours", "demo-video-06.png")], "demo-video-05.png")]),
        new("amber-coast", "Amber Coast", "Electronic", [
            new("Low Tide", "Amber Coast", 2023, [Track("low-tide", "Low Tide", "Amber Coast", "Low Tide", "demo-video-07.png"), Track("shoreline", "Shoreline", "Amber Coast", "Low Tide", "demo-video-08.png")], "demo-video-07.png")]),
        new("juniper-park", "Juniper Park", "Acoustic", [
            new("Side Streets", "Juniper Park", 2024, [Track("side-streets", "Side Streets", "Juniper Park", "Side Streets", "demo-video-09.png"), Track("morning-train", "Morning Train", "Juniper Park", "Side Streets", "demo-video-10.png")], "demo-video-09.png")])
    ];

    public static IReadOnlyList<DemoMusicArtist> AllArtists => Artists;
    public static IReadOnlyList<DemoMusicAlbum> AllAlbums => Artists.SelectMany(artist => artist.Albums).ToArray();
    public static IReadOnlyList<DemoMusicTrack> AllTracks => AllAlbums.SelectMany(album => album.Tracks).ToArray();
    public static IReadOnlyList<DemoMusicTrack> RecentlyPlayed => [AllTracks[0], AllTracks[2], AllTracks[4], AllTracks[6]];
    public static IReadOnlyList<DemoMusicAlbum> RecentlyAdded => AllAlbums.OrderByDescending(album => album.Year).ThenBy(album => album.Title, StringComparer.OrdinalIgnoreCase).ToArray();
    public static IReadOnlyList<DemoMusicAlbum> FavouriteAlbums => [AllAlbums[0], AllAlbums[2], AllAlbums[3]];
    public static IReadOnlyList<DemoMusicTrack> FindArtistTracks(string artistId) =>
        Artists.FirstOrDefault(artist => artist.Id == artistId)?.Albums.SelectMany(album => album.Tracks).ToArray() ?? [];
    public static IReadOnlyList<DemoMusicArtist> SearchArtists(string query) =>
        string.IsNullOrWhiteSpace(query)
            ? Artists
            : Artists.Where(artist => artist.Name.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase)).ToArray();
}

/// <summary>One shared in-memory queue with simple play-now/next/add/reorder semantics.</summary>
public sealed class DemoPlaybackQueue
{
    private readonly List<DemoMusicTrack> _tracks = [];
    public IReadOnlyList<DemoMusicTrack> Tracks => _tracks;
    public int CurrentIndex { get; private set; } = -1;
    public DemoMusicTrack? Current => CurrentIndex >= 0 && CurrentIndex < _tracks.Count ? _tracks[CurrentIndex] : null;

    public void PlayNow(DemoMusicTrack track)
    {
        ArgumentNullException.ThrowIfNull(track);
        _tracks.Clear();
        _tracks.Add(track);
        CurrentIndex = 0;
    }

    public void PlayNow(IReadOnlyList<DemoMusicTrack> tracks, int currentIndex = 0)
    {
        ArgumentNullException.ThrowIfNull(tracks);
        if (tracks.Count == 0 || currentIndex < 0 || currentIndex >= tracks.Count)
            throw new ArgumentOutOfRangeException(nameof(currentIndex), "A non-empty queue and valid current index are required.");
        _tracks.Clear();
        _tracks.AddRange(tracks);
        CurrentIndex = currentIndex;
    }

    public void Add(DemoMusicTrack track)
    {
        ArgumentNullException.ThrowIfNull(track);
        _tracks.Add(track);
        if (CurrentIndex < 0) CurrentIndex = 0;
    }

    public void PlayNext(DemoMusicTrack track)
    {
        ArgumentNullException.ThrowIfNull(track);
        var index = Math.Clamp(CurrentIndex + 1, 0, _tracks.Count);
        _tracks.Insert(index, track);
    }

    public DemoMusicTrack? MoveNext(bool repeatAll = false, bool shuffle = false, Random? random = null)
    {
        if (_tracks.Count == 0) return null;
        if (shuffle && _tracks.Count > 1)
        {
            random ??= Random.Shared;
            var next = random.Next(_tracks.Count - 1);
            if (next >= CurrentIndex) next++;
            CurrentIndex = next;
            return Current;
        }
        if (CurrentIndex + 1 >= _tracks.Count)
        {
            if (!repeatAll) return null;
            CurrentIndex = 0;
            return Current;
        }
        CurrentIndex++;
        return Current;
    }

    public DemoMusicTrack? MovePrevious()
    {
        if (CurrentIndex <= 0) return null;
        CurrentIndex--;
        return Current;
    }

    public bool Move(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _tracks.Count || toIndex < 0 || toIndex >= _tracks.Count) return false;
        var track = _tracks[fromIndex];
        _tracks.RemoveAt(fromIndex);
        _tracks.Insert(toIndex, track);
        if (fromIndex == CurrentIndex) CurrentIndex = toIndex;
        else if (fromIndex < CurrentIndex && toIndex >= CurrentIndex) CurrentIndex--;
        else if (fromIndex > CurrentIndex && toIndex <= CurrentIndex) CurrentIndex++;
        return true;
    }
}
