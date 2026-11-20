namespace MaggieVision.Windows.Music;

/// <summary>Reads short windows of the exact PCM source used by the demo player and derives spectrum bars.</summary>
public static class DemoAudioSpectrum
{
    private static readonly double[] BandFrequencies = [55, 80, 110, 165, 220, 330, 440, 660, 880, 1320, 1760, 2640];
    private const int WindowSize = 1024;

    public static float[] Analyze(ReadOnlySpan<short> pcm, int sampleRate)
    {
        if (sampleRate <= 0) throw new ArgumentOutOfRangeException(nameof(sampleRate));
        if (pcm.Length < WindowSize) return new float[BandFrequencies.Length];

        var results = new float[BandFrequencies.Length];
        var mean = pcm[..WindowSize].ToArray().Average(value => (double)value);
        for (var band = 0; band < BandFrequencies.Length; band++)
        {
            var real = 0d;
            var imaginary = 0d;
            var frequency = BandFrequencies[band];
            for (var index = 0; index < WindowSize; index++)
            {
                var window = 0.5 - 0.5 * Math.Cos(2 * Math.PI * index / (WindowSize - 1));
                var phase = 2 * Math.PI * frequency * index / sampleRate;
                var sample = (pcm[index] - mean) / 32768d * window;
                real += sample * Math.Cos(phase);
                imaginary -= sample * Math.Sin(phase);
            }

            var magnitude = Math.Sqrt(real * real + imaginary * imaginary) * 2 / WindowSize;
            results[band] = (float)Math.Clamp(magnitude / 0.12, 0, 1);
        }
        return results;
    }
}
