using MaggieVision.Windows.Video;

namespace MaggieVision.Windows.Video.Tests;

public sealed class DemoTvCatalogueTests
{
    [Fact]
    public void FindReturnsKnownFictionalProgrammeAndUnknownIsNull()
    {
        Assert.Equal("Brightwater", DemoTvCatalogue.Find("brightwater")?.Title);
        Assert.Null(DemoTvCatalogue.Find("missing"));
    }

    [Fact]
    public void SeasonLookupAndEpisodeCountAreDeterministic()
    {
        var show = DemoTvCatalogue.Find("brightwater");
        Assert.Equal(18, show?.EpisodeCount);
        Assert.Equal(3, show?.Seasons.Count);
        Assert.Equal("First Impressions", DemoTvCatalogue.FindSeason("brightwater", 1)?.Episodes[0].Title);
        Assert.Null(DemoTvCatalogue.FindSeason("brightwater", 9));
    }
}
