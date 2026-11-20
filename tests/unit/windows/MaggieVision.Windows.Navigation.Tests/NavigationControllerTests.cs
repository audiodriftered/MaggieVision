using MaggieVision.Windows.Navigation;

namespace MaggieVision.Windows.Navigation.Tests;

public sealed class NavigationControllerTests
{
    [Fact]
    public void StartsExpandedOnVideoHomeWithOnlyVideoDestinations()
    {
        var navigation = new NavigationController();

        Assert.Equal(MediaModule.Video, navigation.State.Module);
        Assert.Equal("video.home", navigation.State.SelectedDestinationId);
        Assert.False(navigation.State.IsPaneCollapsed);
        Assert.Equal(
            ["Home", "Films", "TV Programmes", "Personal Videos", "Folders", "Collections", "Watchlist", "Favourites", "Search"],
            navigation.Destinations.Select(destination => destination.Label));
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == "music.artists");
    }

    [Fact]
    public void SwitchingModulesShowsItsRoutesAndRestoresItsLastSelectedRoute()
    {
        var navigation = new NavigationController();
        Assert.True(navigation.NavigateTo("video.films"));

        Assert.True(navigation.SwitchModule(MediaModule.Music));
        Assert.Equal("music.home", navigation.State.SelectedDestinationId);
        Assert.Contains(navigation.Destinations, destination => destination.Id == "music.now-playing");
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == "video.films");

        Assert.True(navigation.NavigateTo("music.artists"));
        Assert.True(navigation.SwitchModule(MediaModule.Video));
        Assert.Equal("video.films", navigation.State.SelectedDestinationId);
        Assert.True(navigation.SwitchModule(MediaModule.Music));
        Assert.Equal("music.artists", navigation.State.SelectedDestinationId);
    }

    [Fact]
    public void VideoAndMusicFavouritesRemainDistinct()
    {
        var navigation = new NavigationController();
        Assert.Contains(navigation.Destinations, destination => destination.Id == "video.favourites");

        navigation.SwitchModule(MediaModule.Music);

        Assert.Contains(navigation.Destinations, destination => destination.Id == "music.favourites");
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == "video.favourites");
    }

    [Fact]
    public void SettingsSelectionIsSeparateAndDoesNotReplaceTheCurrentRoute()
    {
        var navigation = new NavigationController();
        Assert.True(navigation.NavigateTo("video.folders"));

        navigation.SelectSettings();

        Assert.True(navigation.State.IsSettingsSelected);
        Assert.Equal("video.folders", navigation.State.SelectedDestinationId);
        Assert.True(navigation.NavigateTo("video.films"));
        Assert.False(navigation.State.IsSettingsSelected);
    }

    [Fact]
    public void CollapsingAndReopeningPanePreservesTheCurrentRoute()
    {
        var navigation = new NavigationController();
        Assert.True(navigation.NavigateTo("video.films"));

        navigation.TogglePane();
        Assert.True(navigation.State.IsPaneCollapsed);
        Assert.Equal("video.films", navigation.State.SelectedDestinationId);

        navigation.TogglePane();
        Assert.False(navigation.State.IsPaneCollapsed);
        Assert.Equal("video.films", navigation.State.SelectedDestinationId);
    }

    [Fact]
    public void CustomDestinationCanInterleaveBetweenFilmsAndTvInTheUnifiedList()
    {
        var navigation = new NavigationController();
        var documentaries = new NavigationDestination(
            "video.documentaries",
            "Documentaries",
            "Video",
            DestinationKind.SavedFilter,
            "filter-documentaries",
            MediaModule.Video);

        Assert.True(navigation.AddCustomDestination(documentaries, insertionIndex: 2));
        Assert.Equal(
            ["Home", "Films", "Documentaries", "TV Programmes", "Personal Videos"],
            navigation.Destinations.Take(5).Select(destination => destination.Label));

        Assert.True(navigation.NavigateTo("video.documentaries"));
        navigation.SwitchModule(MediaModule.Music);
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == documentaries.Id);
        navigation.SwitchModule(MediaModule.Video);
        Assert.Equal("video.documentaries", navigation.State.SelectedDestinationId);
    }

    [Fact]
    public void CustomDestinationRejectsInvalidOrDuplicateEntriesAndIndexes()
    {
        var navigation = new NavigationController();
        var valid = new NavigationDestination(
            "video.documentaries",
            "Documentaries",
            "Video",
            DestinationKind.SavedFilter,
            "filter-documentaries",
            MediaModule.Video);

        Assert.True(navigation.AddCustomDestination(valid));
        Assert.False(navigation.AddCustomDestination(valid));
        Assert.False(navigation.AddCustomDestination(valid with { Id = "video.invalid", TargetId = null }));
        Assert.False(navigation.AddCustomDestination(valid with { Id = "video.outside", Module = MediaModule.Music }));
        Assert.False(navigation.AddCustomDestination(valid with { Id = "video.bad-index" }, insertionIndex: -1));
        Assert.False(navigation.AddCustomDestination(valid with { Id = "video.bad-index" }, insertionIndex: 999));
    }
}
