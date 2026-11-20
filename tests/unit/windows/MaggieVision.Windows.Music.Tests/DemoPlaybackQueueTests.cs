using MaggieVision.Windows.Music;

namespace MaggieVision.Windows.Music.Tests;

public sealed class DemoPlaybackQueueTests
{
    private static DemoMusicTrack Track(string id) => new(id, id, "Test Artist", "Test Album", "0:20", "demo-video-00.png");

    [Fact]
    public void PlayNextAndAddShareQueueAndAdvanceCurrentTrack()
    {
        var queue = new DemoPlaybackQueue();
        queue.PlayNow(Track("first"));
        queue.Add(Track("last"));
        queue.PlayNext(Track("next"));

        Assert.Equal(["first", "next", "last"], queue.Tracks.Select(track => track.Id));
        Assert.Equal("next", queue.MoveNext()?.Id);
    }

    [Fact]
    public void ReorderingTracksKeepsTheCurrentTrackIdentity()
    {
        var queue = new DemoPlaybackQueue();
        queue.PlayNow(Track("first"));
        queue.Add(Track("second"));
        queue.Add(Track("third"));
        queue.MoveNext();

        Assert.True(queue.Move(2, 0));
        Assert.Equal("second", queue.Current?.Id);
        Assert.Equal(["third", "first", "second"], queue.Tracks.Select(track => track.Id));
    }

    [Fact]
    public void RepeatAllWrapsToTheFirstTrack()
    {
        var queue = new DemoPlaybackQueue();
        queue.PlayNow([Track("first"), Track("second")]);
        queue.MoveNext();

        Assert.Equal("first", queue.MoveNext(repeatAll: true)?.Id);
        Assert.Equal("second", queue.MoveNext()?.Id);
        Assert.Null(queue.MoveNext());
    }

    [Fact]
    public void ShuffleSelectsAnotherTrackWithoutChangingTheQueue()
    {
        var queue = new DemoPlaybackQueue();
        queue.PlayNow([Track("first"), Track("second"), Track("third")]);

        var selected = queue.MoveNext(shuffle: true, random: new Random(42));

        Assert.NotEqual("first", selected?.Id);
        Assert.Equal(["first", "second", "third"], queue.Tracks.Select(track => track.Id));
    }

    [Fact]
    public void CatalogueProvidesSyntheticArtistAndAlbumBrowseData()
    {
        Assert.Equal(4, DemoMusicCatalogue.AllArtists.Count);
        Assert.NotEmpty(DemoMusicCatalogue.RecentlyPlayed);
        Assert.Contains(DemoMusicCatalogue.SearchArtists("window"), artist => artist.Id == "north-window");
        Assert.Empty(DemoMusicCatalogue.SearchArtists("missing"));
    }

    [Fact]
    public void SpectrumAnalysisRespondsToTheFrequencyInThePcmWindow()
    {
        const int rate = 44100;
        var pcm = Enumerable.Range(0, 1024)
            .Select(index => (short)(Math.Sin(2 * Math.PI * 440 * index / rate) * 9000))
            .ToArray();

        var bands = DemoAudioSpectrum.Analyze(pcm, rate);

        Assert.Equal(12, bands.Length);
        Assert.True(bands[6] > bands[0]);
        Assert.True(bands[6] > 0.4f);
    }

    [Fact]
    public void SilentPcmProducesNoSpectrumEnergy()
    {
        Assert.All(DemoAudioSpectrum.Analyze(new short[1024], 44100), value => Assert.Equal(0f, value));
    }
}
