namespace MaggieVision.Windows.Navigation;

public enum MediaModule { Video, Music }
public enum DestinationKind { BuiltIn, SavedFilter, PinnedFolder, Playlist, Collection }

/// <summary>A menu shortcut to a page or a user-owned view target.</summary>
public sealed record NavigationDestination(
    string Id,
    string Label,
    string Symbol,
    DestinationKind Kind,
    string? TargetId = null,
    MediaModule? Module = null);

/// <summary>Explicit, in-memory navigation state for the Windows shell.</summary>
public sealed record NavigationState(
    MediaModule Module,
    string SelectedDestinationId,
    bool IsPaneCollapsed,
    bool IsSettingsSelected,
    IReadOnlyList<NavigationDestination> Destinations);

/// <summary>Pure navigation transitions; it does not access media or persist preferences.</summary>
public sealed class NavigationController
{
    private readonly Dictionary<MediaModule, List<NavigationDestination>> _destinations = new()
    {
        [MediaModule.Video] = BuiltIns.For(MediaModule.Video).ToList(),
        [MediaModule.Music] = BuiltIns.For(MediaModule.Music).ToList()
    };

    private readonly Dictionary<MediaModule, string> _selectedDestinationIds = new()
    {
        [MediaModule.Video] = "video.home",
        [MediaModule.Music] = "music.home"
    };

    public NavigationController() => State = CreateState(MediaModule.Video, "video.home", false, false);

    public NavigationState State { get; private set; }
    public IReadOnlyList<NavigationDestination> Destinations => State.Destinations;

    public bool SwitchModule(MediaModule module)
    {
        if (!Enum.IsDefined(module)) return false;
        State = CreateState(module, _selectedDestinationIds[module], State.IsPaneCollapsed, false);
        return true;
    }

    public bool NavigateTo(string destinationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationId);
        if (!Destinations.Any(item => string.Equals(item.Id, destinationId, StringComparison.OrdinalIgnoreCase)))
            return false;

        _selectedDestinationIds[State.Module] = destinationId;
        State = State with { SelectedDestinationId = destinationId, IsSettingsSelected = false };
        return true;
    }

    public void TogglePane() => State = State with { IsPaneCollapsed = !State.IsPaneCollapsed };

    public void SetPaneCollapsed(bool collapsed) => State = State with { IsPaneCollapsed = collapsed };

    public void SelectSettings() => State = State with { IsSettingsSelected = true };

    /// <summary>
    /// Adds a validated, in-memory shortcut at a precise position in the module's single flat list.
    /// The default remains append for callers that do not choose an insertion point.
    /// </summary>
    public bool AddCustomDestination(NavigationDestination destination, int? insertionIndex = null)
    {
        ArgumentNullException.ThrowIfNull(destination);
        var destinations = _destinations[State.Module];
        if (string.IsNullOrWhiteSpace(destination.Id) ||
            string.IsNullOrWhiteSpace(destination.Label) ||
            string.IsNullOrWhiteSpace(destination.TargetId) ||
            destination.Module != State.Module ||
            !Enum.IsDefined(destination.Kind) ||
            destination.Kind == DestinationKind.BuiltIn ||
            destinations.Any(item => string.Equals(item.Id, destination.Id, StringComparison.OrdinalIgnoreCase)) ||
            (insertionIndex is not null && (insertionIndex < 0 || insertionIndex > destinations.Count)))
        {
            return false;
        }

        destinations.Insert(insertionIndex ?? destinations.Count, destination);
        State = State with { Destinations = destinations.ToArray() };
        return true;
    }

    private NavigationState CreateState(MediaModule module, string selectedId, bool isPaneCollapsed, bool isSettingsSelected) =>
        new(module, selectedId, isPaneCollapsed, isSettingsSelected, _destinations[module].ToArray());

    private static class BuiltIns
    {
        private static readonly NavigationDestination[] Video =
        [
            new("video.home", "Home", "Home", DestinationKind.BuiltIn),
            new("video.films", "Films", "Video", DestinationKind.BuiltIn),
            new("video.tv", "TV Programmes", "TV", DestinationKind.BuiltIn),
            new("video.personal", "Personal Videos", "Library", DestinationKind.BuiltIn),
            new("video.folders", "Folders", "Folder", DestinationKind.BuiltIn),
            new("video.collections", "Collections", "Library", DestinationKind.BuiltIn),
            new("video.watchlist", "Watchlist", "Favorite", DestinationKind.BuiltIn),
            new("video.favourites", "Favourites", "Like", DestinationKind.BuiltIn),
            new("video.search", "Search", "Find", DestinationKind.BuiltIn)
        ];

        private static readonly NavigationDestination[] Music =
        [
            new("music.home", "Home", "Home", DestinationKind.BuiltIn),
            new("music.artists", "Artists", "Contact", DestinationKind.BuiltIn),
            new("music.albums", "Albums", "Library", DestinationKind.BuiltIn),
            new("music.tracks", "Tracks", "MusicInfo", DestinationKind.BuiltIn),
            new("music.playlists", "Playlists", "PlaylistMusic", DestinationKind.BuiltIn),
            new("music.genres", "Genres", "Library", DestinationKind.BuiltIn),
            new("music.years", "Years", "Calendar", DestinationKind.BuiltIn),
            new("music.smart-playlists", "Smart Playlists", "Filter", DestinationKind.BuiltIn),
            new("music.folders", "Folders", "Folder", DestinationKind.BuiltIn),
            new("music.now-playing", "Now Playing", "Play", DestinationKind.BuiltIn),
            new("music.favourites", "Favourites", "Like", DestinationKind.BuiltIn)
        ];

        public static NavigationDestination[] For(MediaModule module) => module switch
        {
            MediaModule.Video => Video,
            MediaModule.Music => Music,
            _ => throw new ArgumentOutOfRangeException(nameof(module))
        };
    }
}
