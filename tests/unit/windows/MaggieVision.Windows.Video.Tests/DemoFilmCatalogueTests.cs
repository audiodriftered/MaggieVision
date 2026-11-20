using MaggieVision.Windows.Video;

namespace MaggieVision.Windows.Video.Tests;

public sealed class DemoFilmCatalogueTests
{
    [Fact]
    public void SearchMatchesTitleWithoutConsideringCaseOrSurroundingWhitespace()
    {
        var results = DemoFilmCatalogue.Search("  NORTHERN ", "All Genres", FilmSortOrder.Title);

        Assert.Equal(["northern-range"], results.Select(film => film.Id));
    }

    [Fact]
    public void GenreFilterAndTitleSortAreAppliedTogether()
    {
        var results = DemoFilmCatalogue.Search(null, "drama", FilmSortOrder.Title);

        Assert.Equal(["garden-of-stone", "riverside-summer"], results.Select(film => film.Id));
    }

    [Fact]
    public void RecentlyAddedSortReturnsNewestFirstAndUsesTitleForTies()
    {
        var results = DemoFilmCatalogue.Search(null, null, FilmSortOrder.RecentlyAdded);

        Assert.Equal("coastal-echoes", results[0].Id);
        Assert.Equal("silver-district", results[1].Id);
        Assert.Equal("northern-range", results[2].Id);
    }

    [Fact]
    public void YearSortOrdersYearDescendingAndThenTitle()
    {
        var results = DemoFilmCatalogue.Search(null, null, FilmSortOrder.Year);

        Assert.Equal("northern-range", results[0].Id);
        Assert.Equal("map-of-tomorrow", results[1].Id);
        Assert.Equal("midnight-valley", results[2].Id);
        Assert.Equal("orbit-seven", results[3].Id);
    }

    [Fact]
    public void FindReturnsOnlyExistingDemoRecords()
    {
        Assert.Equal("The Last Signal", DemoFilmCatalogue.Find("last-signal")?.Title);
        Assert.Null(DemoFilmCatalogue.Find("missing"));
    }
}
