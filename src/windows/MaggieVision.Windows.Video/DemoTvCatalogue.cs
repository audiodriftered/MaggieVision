namespace MaggieVision.Windows.Video;

/// <summary>Fictional episode used to exercise TV navigation without opening a media file.</summary>
public sealed record DemoTvEpisode(int Number, string Title, string Runtime, string Synopsis, string ArtworkFile);

/// <summary>Fictional season and its deterministic demonstration episodes.</summary>
public sealed record DemoTvSeason(int Number, int Year, string Genre, IReadOnlyList<DemoTvEpisode> Episodes)
{
    public string Label => $"Season {Number}";
}

/// <summary>A synthetic television programme for the native Video screens.</summary>
public sealed record DemoTvProgramme(
    string Id,
    string Title,
    int FirstYear,
    int LastYear,
    string Genre,
    string Synopsis,
    string ArtworkFile,
    IReadOnlyList<DemoTvSeason> Seasons)
{
    public int EpisodeCount => Seasons.Sum(season => season.Episodes.Count);
    public string YearRange => FirstYear == LastYear ? FirstYear.ToString() : $"{FirstYear} – {LastYear}";
}

/// <summary>Immutable fictional TV fixtures. This class performs no library or filesystem access.</summary>
public static class DemoTvCatalogue
{
    private static readonly DemoTvEpisode[] FirstSeasonEpisodes =
    [
        new(1, "First Impressions", "29 min", "A new arrival turns an ordinary morning into an unexpected challenge.", "demo-video-00.png"),
        new(2, "A Different View", "31 min", "A promising idea takes a detour when the team sees the problem differently.", "demo-video-01.png"),
        new(3, "The Long Way", "28 min", "A shortcut creates more questions than it answers.", "demo-video-02.png"),
        new(4, "Small Hours", "30 min", "A late shift brings two neighbours together.", "demo-video-03.png"),
        new(5, "The Invitation", "29 min", "An unexpected invitation tests a carefully made plan.", "demo-video-04.png"),
        new(6, "Home Again", "32 min", "The season ends with a choice about what comes next.", "demo-video-05.png")
    ];

    private static readonly DemoTvProgramme[] Programmes =
    [
        new("brightwater", "Brightwater", 2022, 2026, "Drama",
            "The people of a coastal town learn to live with a changing shoreline and one another.", "demo-video-02.png",
            [new(1, 2022, "Drama", FirstSeasonEpisodes), new(2, 2024, "Drama", FirstSeasonEpisodes), new(3, 2026, "Drama", FirstSeasonEpisodes)]),
        new("copper-street", "Copper Street", 2021, 2025, "Comedy",
            "A community workshop finds improbable ways to keep its doors open.", "demo-video-03.png",
            [new(1, 2021, "Comedy", FirstSeasonEpisodes)]),
        new("northline", "Northline", 2023, 2026, "Mystery",
            "A mountain rail crew follows a signal that should not be on the line.", "demo-video-04.png",
            [new(1, 2023, "Mystery", FirstSeasonEpisodes), new(2, 2025, "Mystery", FirstSeasonEpisodes)]),
        new("the-night-shift", "The Night Shift", 2024, 2026, "Thriller",
            "A hospital's overnight team confronts a pattern no one can explain.", "demo-video-05.png",
            [new(1, 2024, "Thriller", FirstSeasonEpisodes)])
    ];

    public static IReadOnlyList<DemoTvProgramme> All => Programmes;

    public static DemoTvProgramme? Find(string id) => Programmes.FirstOrDefault(show => show.Id == id);

    public static DemoTvSeason? FindSeason(string programmeId, int seasonNumber) =>
        Find(programmeId)?.Seasons.FirstOrDefault(season => season.Number == seasonNumber);
}
