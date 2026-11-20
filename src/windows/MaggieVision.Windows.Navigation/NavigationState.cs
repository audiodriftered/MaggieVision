namespace MaggieVision.Windows.Navigation;

public enum MediaModule { Video, Music }
public enum DestinationKind { BuiltIn, SavedFilter, PinnedFolder, Playlist, Collection }

/// <summary>A menu shortcut to a page or a user-owned view target.</summary>
public sealed record NavigationDestination(string Id, string Label, string Symbol, DestinationKind Kind, string? TargetId = null, MediaModule? Module = null);

/// <summary>Explicit, in-memory navigation state for the Windows shell.</summary>
public sealed record NavigationState(MediaModule Module, string SelectedDestinationId, bool IsPaneCollapsed, bool IsSettingsSelected, IReadOnlyList<NavigationDestination> CustomDestinations);

/// <summary>Pure navigation transitions; it does not access media or persist preferences.</summary>
public sealed class NavigationController
{
    public NavigationController() => State = new NavigationState(MediaModule.Video, "video.home", false, false, Array.Empty<NavigationDestination>());
    public NavigationState State { get; private set; }
    public IReadOnlyList<NavigationDestination> Destinations => BuiltIns.For(State.Module).Concat(State.CustomDestinations.Where(item => item.Module == State.Module)).ToArray();

    public bool SwitchModule(MediaModule module)
    {
        if (!Enum.IsDefined(module)) return false;
        State = State with { Module = module, SelectedDestinationId = BuiltIns.For(module)[0].Id, IsSettingsSelected = false };
        return true;
    }

    public bool NavigateTo(string destinationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(destinationId);
        if (!Destinations.Any(item => item.Id == destinationId)) return false;
        State = State with { SelectedDestinationId = destinationId, IsSettingsSelected = false };
        return true;
    }

    public void TogglePane() => State = State with { IsPaneCollapsed = !State.IsPaneCollapsed };
    public void SetPaneCollapsed(bool collapsed) => State = State with { IsPaneCollapsed = collapsed };
    public void SelectSettings() => State = State with { IsSettingsSelected = true };

    /// <summary>Adds a typed, in-memory shortcut alongside built-in destinations.</summary>
    public bool AddCustomDestination(NavigationDestination destination)
    {
        ArgumentNullException.ThrowIfNull(destination);
        if (string.IsNullOrWhiteSpace(destination.Id) || string.IsNullOrWhiteSpace(destination.Label) ||
            string.IsNullOrWhiteSpace(destination.TargetId) || destination.Module != State.Module ||
            !Enum.IsDefined(destination.Kind) || destination.Kind == DestinationKind.BuiltIn ||
            Destinations.Any(item => item.Id == destination.Id)) return false;
        State = State with { CustomDestinations = State.CustomDestinations.Append(destination).ToArray() };
        return true;
    }

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
