using System.Diagnostics;
using MaggieVision.Host.Catalogue;

namespace MaggieVision.Host.Tests;

public sealed class LocalCatalogueScannerTests
{
    [Fact]
    public void Scan_lists_nested_files_and_folders_only_below_the_explicit_root()
    {
        using var fixture = new TemporaryDirectory();
        var selectedRoot = Directory.CreateDirectory(Path.Combine(fixture.Path, "selected")).FullName;
        var nestedFolder = Directory.CreateDirectory(Path.Combine(selectedRoot, "Albums", "Live Set")).FullName;
        File.WriteAllBytes(Path.Combine(nestedFolder, "track.flac"), [1, 2, 3, 4]);

        var unselected = Directory.CreateDirectory(Path.Combine(fixture.Path, "unselected")).FullName;
        File.WriteAllText(Path.Combine(unselected, "private.txt"), "not selected");

        var snapshot = new LocalCatalogueScanner().Scan(selectedRoot);

        Assert.Equal(selectedRoot, snapshot.RootPath);
        Assert.Equal(
            ["Albums", "Albums/Live Set", "Albums/Live Set/track.flac"],
            snapshot.Entries.Select(entry => entry.RelativePath));
        Assert.Equal(CatalogueEntryKind.Directory, snapshot.Entries[0].Kind);
        Assert.Equal(CatalogueEntryKind.File, snapshot.Entries[^1].Kind);
        Assert.All(snapshot.Entries, entry => Assert.Equal(CatalogueEntryState.Enumerated, entry.State));
        Assert.Empty(snapshot.Issues);
    }

    [Fact]
    public void Scan_does_not_change_source_bytes_timestamps_or_directory_tree_and_releases_handles()
    {
        using var fixture = new TemporaryDirectory();
        var root = Directory.CreateDirectory(Path.Combine(fixture.Path, "library")).FullName;
        var nested = Directory.CreateDirectory(Path.Combine(root, "Albums", "Disc 1")).FullName;
        var sourcePath = Path.Combine(nested, "track.bin");
        var originalBytes = new byte[] { 0, 7, 19, 255 };
        File.WriteAllBytes(sourcePath, originalBytes);
        var expectedWriteTime = new DateTime(2020, 1, 2, 3, 4, 6, DateTimeKind.Utc);
        var expectedCreationTime = new DateTime(2020, 1, 2, 3, 4, 8, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(sourcePath, expectedWriteTime);
        File.SetCreationTimeUtc(sourcePath, expectedCreationTime);

        var treeBefore = CaptureTree(root);
        var bytesBefore = File.ReadAllBytes(sourcePath);
        var writeTimeBefore = File.GetLastWriteTimeUtc(sourcePath);
        var creationTimeBefore = File.GetCreationTimeUtc(sourcePath);
        var accessTimeBefore = File.GetLastAccessTimeUtc(sourcePath);

        var snapshot = new LocalCatalogueScanner().Scan(root);

        var writeTimeAfterScan = File.GetLastWriteTimeUtc(sourcePath);
        var creationTimeAfterScan = File.GetCreationTimeUtc(sourcePath);
        var accessTimeAfterScan = File.GetLastAccessTimeUtc(sourcePath);
        var bytesAfter = File.ReadAllBytes(sourcePath);
        var treeAfter = CaptureTree(root);

        Assert.Equal(bytesBefore, bytesAfter);
        Assert.Equal(writeTimeBefore, writeTimeAfterScan);
        Assert.Equal(creationTimeBefore, creationTimeAfterScan);
        Assert.Equal(accessTimeBefore, accessTimeAfterScan);
        Assert.Equal(treeBefore, treeAfter);
        Assert.Contains(snapshot.Entries, entry => entry.RelativePath == "Albums/Disc 1/track.bin");

        var renamedRoot = Path.Combine(fixture.Path, "renamed-library");
        Directory.Move(root, renamedRoot);
        Directory.Delete(renamedRoot, recursive: true);
    }

    [Fact]
    public void Scan_lists_reparse_point_but_never_traverses_its_target()
    {
        using var fixture = new TemporaryDirectory();
        var root = Directory.CreateDirectory(Path.Combine(fixture.Path, "selected")).FullName;
        var outsideRoot = Directory.CreateDirectory(Path.Combine(fixture.Path, "outside")).FullName;
        var outsideFile = Path.Combine(outsideRoot, "not-in-catalogue.txt");
        File.WriteAllText(outsideFile, "outside root");
        var junction = Path.Combine(root, "external-folder");
        CreateDirectoryJunction(junction, outsideRoot);

        try
        {
            Assert.True((File.GetAttributes(junction) & FileAttributes.ReparsePoint) != 0);

            var snapshot = new LocalCatalogueScanner().Scan(root);

            var linkEntry = Assert.Single(snapshot.Entries, entry => entry.RelativePath == "external-folder");
            Assert.Equal(CatalogueEntryKind.Directory, linkEntry.Kind);
            Assert.Equal(CatalogueEntryState.SkippedReparsePoint, linkEntry.State);
            Assert.DoesNotContain(snapshot.Entries, entry => entry.RelativePath.StartsWith("external-folder/", StringComparison.Ordinal));
            Assert.Contains(snapshot.Issues, issue => issue.RelativePath == "external-folder" && issue.Code == CatalogueIssueCode.ReparsePointNotTraversed);
            Assert.Equal("outside root", File.ReadAllText(outsideFile));
        }
        finally
        {
            Directory.Delete(junction);
        }
    }

    [Fact]
    public void Scan_reports_a_denied_subdirectory_without_losing_entries_from_accessible_siblings()
    {
        using var fixture = new TemporaryDirectory();
        var root = Directory.CreateDirectory(Path.Combine(fixture.Path, "library")).FullName;
        File.WriteAllText(Path.Combine(root, "visible.txt"), "synthetic");
        var deniedDirectory = Directory.CreateDirectory(Path.Combine(root, "denied")).FullName;
        File.WriteAllText(Path.Combine(deniedDirectory, "hidden.txt"), "synthetic");

        var fileSystem = new DenyDirectoryFileSystem(new PhysicalCatalogueFileSystem(), deniedDirectory);
        var snapshot = new LocalCatalogueScanner(fileSystem).Scan(root);

        Assert.Contains(snapshot.Entries, entry => entry.RelativePath == "visible.txt");
        Assert.Contains(snapshot.Issues, issue => issue.RelativePath == "denied" && issue.Code == CatalogueIssueCode.DirectoryAccessDenied);
        Assert.DoesNotContain(snapshot.Entries, entry => entry.RelativePath == "denied/hidden.txt");
    }

    [Fact]
    public void Scan_reports_an_inaccessible_root_without_disclosing_its_absolute_path()
    {
        using var fixture = new TemporaryDirectory();
        var root = Directory.CreateDirectory(Path.Combine(fixture.Path, "library")).FullName;
        var fileSystem = new DenyRootAttributesFileSystem(new PhysicalCatalogueFileSystem(), root);

        var exception = Assert.Throws<CatalogueRootException>(() => new LocalCatalogueScanner(fileSystem).Scan(root));

        Assert.Equal(CatalogueRootError.AccessDenied, exception.Error);
        Assert.DoesNotContain(root, exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Scan_rejects_invalid_missing_file_and_reparse_roots_and_honours_pre_cancellation()
    {
        using var fixture = new TemporaryDirectory();
        var scanner = new LocalCatalogueScanner();

        var relativeRoot = Assert.Throws<CatalogueRootException>(() => scanner.Scan("relative-root"));
        Assert.Equal(CatalogueRootError.InvalidPath, relativeRoot.Error);

        if (OperatingSystem.IsWindows())
        {
            var networkRoot = Assert.Throws<CatalogueRootException>(() => scanner.Scan(@"\\server\share"));
            Assert.Equal(CatalogueRootError.NetworkRootNotAllowed, networkRoot.Error);
        }

        var missingRoot = Assert.Throws<CatalogueRootException>(() => scanner.Scan(Path.Combine(fixture.Path, "missing")));
        Assert.Equal(CatalogueRootError.NotFound, missingRoot.Error);

        var filePath = Path.Combine(fixture.Path, "not-a-directory.txt");
        File.WriteAllText(filePath, "synthetic");
        var fileRoot = Assert.Throws<CatalogueRootException>(() => scanner.Scan(filePath));
        Assert.Equal(CatalogueRootError.NotDirectory, fileRoot.Error);

        var root = Directory.CreateDirectory(Path.Combine(fixture.Path, "library")).FullName;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Throws<OperationCanceledException>(() => scanner.Scan(root, cancellation.Token));

        var reparseRoot = Path.Combine(fixture.Path, "root-link");
        CreateDirectoryJunction(reparseRoot, root);
        try
        {
            var linkRoot = Assert.Throws<CatalogueRootException>(() => scanner.Scan(reparseRoot));
            Assert.Equal(CatalogueRootError.ReparsePointNotAllowed, linkRoot.Error);
        }
        finally
        {
            Directory.Delete(reparseRoot);
        }
    }

    [Fact]
    public void Scan_returns_no_partial_snapshot_when_cancelled_during_enumeration()
    {
        using var fixture = new TemporaryDirectory();
        var root = Directory.CreateDirectory(Path.Combine(fixture.Path, "library")).FullName;
        using var cancellation = new CancellationTokenSource();
        var fileSystem = new CancelDuringEnumerationFileSystem(new PhysicalCatalogueFileSystem(), cancellation);

        Assert.Throws<OperationCanceledException>(() =>
            new LocalCatalogueScanner(fileSystem).Scan(root, cancellation.Token));
        Assert.True(fileSystem.EnumerationStarted);
    }

    private static string[] CaptureTree(string root) =>
        Directory.EnumerateFileSystemEntries(root, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(root, path).Replace(Path.DirectorySeparatorChar, '/'))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static void CreateDirectoryJunction(string linkPath, string targetPath)
    {
        var command = new ProcessStartInfo
        {
            FileName = Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true
        };
        command.ArgumentList.Add("/c");
        command.ArgumentList.Add("mklink");
        command.ArgumentList.Add("/J");
        command.ArgumentList.Add(linkPath);
        command.ArgumentList.Add(targetPath);

        using var process = Process.Start(command) ?? throw new InvalidOperationException("Could not start cmd.exe to create the synthetic junction.");
        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();
        process.WaitForExit();
        Assert.True(process.ExitCode == 0, $"Could not create synthetic directory junction. {standardOutput}{standardError}");
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        public TemporaryDirectory()
        {
            Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "MaggieVision-Catalogue-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public string Path { get; }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }

    private sealed class DenyDirectoryFileSystem(ICatalogueFileSystem inner, string deniedDirectory) : ICatalogueFileSystem
    {
        public FileAttributes GetAttributes(string path) => inner.GetAttributes(path);

        public IEnumerable<FileSystemInfo> EnumerateChildren(string directoryPath)
        {
            if (PathsEqual(directoryPath, deniedDirectory))
            {
                throw new UnauthorizedAccessException("Synthetic access denial.");
            }

            return inner.EnumerateChildren(directoryPath);
        }

        public string GetRelativePath(string rootPath, string path) => inner.GetRelativePath(rootPath, path);
    }

    private sealed class DenyRootAttributesFileSystem(ICatalogueFileSystem inner, string deniedRoot) : ICatalogueFileSystem
    {
        public FileAttributes GetAttributes(string path)
        {
            if (PathsEqual(path, deniedRoot))
            {
                throw new UnauthorizedAccessException("Synthetic root access denial.");
            }

            return inner.GetAttributes(path);
        }

        public IEnumerable<FileSystemInfo> EnumerateChildren(string directoryPath) => inner.EnumerateChildren(directoryPath);

        public string GetRelativePath(string rootPath, string path) => inner.GetRelativePath(rootPath, path);
    }

    private sealed class CancelDuringEnumerationFileSystem(
        ICatalogueFileSystem inner,
        CancellationTokenSource cancellation) : ICatalogueFileSystem
    {
        public bool EnumerationStarted { get; private set; }

        public FileAttributes GetAttributes(string path) => inner.GetAttributes(path);

        public IEnumerable<FileSystemInfo> EnumerateChildren(string directoryPath)
        {
            return EnumerateAndCancel(directoryPath);
        }

        public string GetRelativePath(string rootPath, string path) => inner.GetRelativePath(rootPath, path);

        private IEnumerable<FileSystemInfo> EnumerateAndCancel(string directoryPath)
        {
            EnumerationStarted = true;
            cancellation.Cancel();
            yield return new FileInfo(Path.Combine(directoryPath, "synthetic-entry.txt"));
        }
    }

    private static bool PathsEqual(string left, string right) =>
        string.Equals(Path.GetFullPath(left), Path.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
}
