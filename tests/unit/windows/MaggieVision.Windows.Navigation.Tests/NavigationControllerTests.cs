using MaggieVision.Windows.Navigation;
namespace MaggieVision.Windows.Navigation.Tests;

public sealed class NavigationControllerTests
{
    [Fact] public void StartsOnVideoHomeWithVideoDestinations()
    {
        var navigation = new NavigationController();
        Assert.Equal(MediaModule.Video, navigation.State.Module);
        Assert.Equal("video.home", navigation.State.SelectedDestinationId);
        Assert.Contains(navigation.Destinations, destination => destination.Id == "video.films");
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == "music.artists");
    }
    [Fact] public void SwitchingModuleChangesListAndSelectsItsHome()
    {
        var navigation = new NavigationController();
        Assert.True(navigation.SwitchModule(MediaModule.Music));
        Assert.Equal("music.home", navigation.State.SelectedDestinationId);
        Assert.Contains(navigation.Destinations, destination => destination.Id == "music.now-playing");
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == "video.films");
    }
    [Fact] public void ModuleFavouritesAreDistinct()
    {
        var navigation = new NavigationController();
        Assert.Contains(navigation.Destinations, destination => destination.Id == "video.favourites");
        navigation.SwitchModule(MediaModule.Music);
        Assert.Contains(navigation.Destinations, destination => destination.Id == "music.favourites");
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == "video.favourites");
    }
    [Fact] public void NavigationAndSettingsUpdateSelectionExplicitly()
    {
        var navigation = new NavigationController();
        Assert.True(navigation.NavigateTo("video.folders"));
        Assert.Equal("video.folders", navigation.State.SelectedDestinationId);
        navigation.SelectSettings();
        Assert.True(navigation.State.IsSettingsSelected);
        Assert.True(navigation.NavigateTo("video.films"));
        Assert.False(navigation.State.IsSettingsSelected);
    }
    [Fact] public void PaneCollapseDoesNotChangeSelection()
    {
        var navigation = new NavigationController();
        navigation.NavigateTo("video.films");
        navigation.TogglePane();
        Assert.True(navigation.State.IsPaneCollapsed);
        Assert.Equal("video.films", navigation.State.SelectedDestinationId);
    }
    [Fact] public void CustomDestinationsShareTheFlatListAndRequireUniqueTypedTargets()
    {
        var navigation = new NavigationController();
        var custom = new NavigationDestination("video.documentaries", "Documentaries", "Video", DestinationKind.SavedFilter, "filter-42", MediaModule.Video);
        Assert.True(navigation.AddCustomDestination(custom));
        Assert.Equal("video.films", navigation.Destinations[1].Id);
        Assert.Equal(custom, navigation.Destinations[^1]);
        Assert.False(navigation.AddCustomDestination(custom));
        Assert.False(navigation.AddCustomDestination(custom with { Id = "video.invalid", TargetId = null }));
        navigation.SwitchModule(MediaModule.Music);
        Assert.DoesNotContain(navigation.Destinations, destination => destination.Id == custom.Id);
    }
}
