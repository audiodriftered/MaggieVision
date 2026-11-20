using MaggieVision.Windows.Video;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MaggieVision_Windows.Video.Films;

/// <summary>Full-content-area film detail page, without fabricated catalogue or playback claims.</summary>
public sealed partial class FilmDetailsPage : Page
{
    public FilmDetailsPage(DemoFilm film)
    {
        ArgumentNullException.ThrowIfNull(film);
        InitializeComponent();
        Backdrop.Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", film.ArtworkFile)));
        FilmTitle.Text = film.Title;
        FilmMetadata.Text = film.Metadata;
        FilmSynopsis.Text = film.Synopsis;
        AutomationProperties.SetName(Backdrop, $"Synthetic demonstration backdrop for {film.Title}");
    }

    public event EventHandler? BackRequested;

    private void OnBackClick(object sender, RoutedEventArgs args) => BackRequested?.Invoke(this, EventArgs.Empty);

    private void OnUnavailableActionClick(object sender, RoutedEventArgs args)
    {
        ActionMessage.Title = "Not available in this build";
        ActionMessage.Message = "Playback, trailers and profile-specific watchlist or favourite actions need the library and playback integrations.";
        ActionMessage.IsOpen = true;
    }
}
