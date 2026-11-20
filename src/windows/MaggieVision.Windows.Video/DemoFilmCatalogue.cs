namespace MaggieVision.Windows.Video;

/// <summary>One fictional record used only to exercise native Video browsing before library integration.</summary>
public sealed record DemoFilm(
    string Id,
    string Title,
    int Year,
    string Genre,
    string Runtime,
    string ArtworkFile,
    string Synopsis,
    bool IsResumeCandidate = false,
    double? ResumeProgress = null,
    bool IsFeatured = false)
{
    public string Metadata => $"{Year}  ·  {Genre}  ·  {Runtime}";
}

/// <summary>
/// Deterministic, fictional Video data for presentation testing. It never reads a library or media file.
/// </summary>
public static class DemoFilmCatalogue
{
    private static readonly DemoFilm[] Films =
    [
        new("amber-horizon", "The Amber Horizon", 2024, "Adventure", "2 h 9 min", "demo-featured-amber-horizon.png",
            "A map, a mystery and a final journey across a forgotten land bring two estranged siblings back together.", IsFeatured: true),
        new("coastal-echoes", "Coastal Echoes", 2023, "Mystery", "1 h 48 min", "demo-video-00.png",
            "A remote lighthouse keeps an unexpected record of the people who pass its shore.", true, 0.58),
        new("silver-district", "Silver District", 2022, "Thriller", "1 h 56 min", "demo-video-01.png",
            "A night-shift courier discovers that a city block is missing from every public map.", true, 0.63),
        new("northern-range", "The Northern Range", 2025, "Adventure", "1 h 44 min", "demo-video-02.png",
            "Two climbers follow a weather station signal into a mountain range no one has crossed.", true, 0.26),
        new("garden-of-stone", "Garden of Stone", 2021, "Drama", "2 h 2 min", "demo-video-03.png",
            "The restoration of an old aqueduct reconnects a forest community with its history.", true, 0.49),
        new("orbit-seven", "Orbit Seven", 2024, "Science Fiction", "2 h 5 min", "demo-video-04.png",
            "An engineer on a quiet orbital station receives a message that appears to come from tomorrow.", true, 0.12),
        new("midnight-valley", "Midnight Valley", 2024, "Thriller", "1 h 54 min", "demo-video-05.png",
            "A family returns to a mountain cabin and finds its old radio transmitting from an empty frequency."),
        new("riverside-summer", "Riverside Summer", 2024, "Drama", "1 h 46 min", "demo-video-06.png",
            "Two friends meet again at the river where their lives first took different directions."),
        new("last-signal", "The Last Signal", 2023, "Science Fiction", "2 h 1 min", "demo-video-07.png",
            "A remote observatory receives a signal that changes the team's understanding of the night sky."),
        new("map-of-tomorrow", "Map of Tomorrow", 2024, "Science Fiction", "1 h 56 min", "demo-video-08.png",
            "A cartographer redraws the city after an unexplained transit line appears overnight."),
        new("hidden-harbour", "Hidden Harbour", 2023, "Mystery", "1 h 42 min", "demo-video-09.png",
            "A harbour pilot investigates the return of a boat that disappeared decades earlier.")
    ];

    public static IReadOnlyList<DemoFilm> All => Films;

    public static DemoFilm? Find(string id) => Films.FirstOrDefault(film => film.Id == id);

    public static IReadOnlyList<DemoFilm> Search(string? query, string? genre, FilmSortOrder sortOrder)
    {
        IEnumerable<DemoFilm> results = Films.Where(film => !film.IsFeatured);

        if (!string.IsNullOrWhiteSpace(query))
        {
            results = results.Where(film => film.Title.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(genre) && !string.Equals(genre, "All Genres", StringComparison.OrdinalIgnoreCase))
        {
            results = results.Where(film => string.Equals(film.Genre, genre, StringComparison.OrdinalIgnoreCase));
        }

        return sortOrder switch
        {
            FilmSortOrder.Title => results.OrderBy(film => film.Title, StringComparer.OrdinalIgnoreCase).ToArray(),
            FilmSortOrder.Year => results.OrderByDescending(film => film.Year).ThenBy(film => film.Title, StringComparer.OrdinalIgnoreCase).ToArray(),
            _ => results.ToArray()
        };
    }
}

public enum FilmSortOrder { RecentlyAdded, Title, Year }
