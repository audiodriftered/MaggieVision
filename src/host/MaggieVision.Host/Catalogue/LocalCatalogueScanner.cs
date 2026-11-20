using System.Collections.ObjectModel;

namespace MaggieVision.Host.Catalogue;

/// <summary>Enumerates one caller-supplied local directory tree without opening file contents or writing to it.</summary>
public sealed class LocalCatalogueScanner
{
    private readonly ICatalogueFileSystem _fileSystem;

    /// <summary>Creates a scanner backed by the operating-system filesystem.</summary>
    public LocalCatalogueScanner() : this(new PhysicalCatalogueFileSystem())
    {
    }

    internal LocalCatalogueScanner(ICatalogueFileSystem fileSystem)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    /// <summary>Performs one synchronous scan of the explicit root and returns an in-memory snapshot.</summary>
    /// <param name="rootPath">A fully qualified local directory path. Relative and network roots are rejected.</param>
    /// <param name="cancellationToken">Cancellation checked between directory and entry operations.</param>
    /// <exception cref="CatalogueRootException">The root is invalid, unavailable, not a directory, or a reparse point.</exception>
    /// <exception cref="OperationCanceledException">The caller cancels before the scan completes; no snapshot is returned.</exception>
    public CatalogueSnapshot Scan(string? rootPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var fullRoot = NormalizeRoot(rootPath);
        var rootAttributes = ReadRootAttributes(fullRoot);

        if ((rootAttributes & FileAttributes.Directory) == 0)
        {
            throw new CatalogueRootException(CatalogueRootError.NotDirectory, "The selected root is not a directory.");
        }

        if ((rootAttributes & FileAttributes.ReparsePoint) != 0)
        {
            throw new CatalogueRootException(CatalogueRootError.ReparsePointNotAllowed, "The selected root is a reparse point.");
        }

        var entries = new List<CatalogueEntry>();
        var issues = new List<CatalogueIssue>();
        var directories = new Stack<DirectoryInfo>();
        directories.Push(new DirectoryInfo(fullRoot));

        while (directories.TryPop(out var directory))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!TryConfirmDirectoryIsTraversable(directory, fullRoot, issues))
            {
                continue;
            }

            var childDirectories = new List<DirectoryInfo>();
            try
            {
                foreach (var child in _fileSystem.EnumerateChildren(directory.FullName))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    AddChild(child, fullRoot, entries, issues, childDirectories);
                }
            }
            catch (UnauthorizedAccessException)
            {
                issues.Add(new CatalogueIssue(GetSafeRelativePath(fullRoot, directory.FullName), CatalogueIssueCode.DirectoryAccessDenied));
            }
            catch (IOException)
            {
                issues.Add(new CatalogueIssue(GetSafeRelativePath(fullRoot, directory.FullName), CatalogueIssueCode.DirectoryReadFailed));
            }

            cancellationToken.ThrowIfCancellationRequested();
            childDirectories.Sort(static (left, right) => StringComparer.OrdinalIgnoreCase.Compare(left.FullName, right.FullName));
            for (var index = childDirectories.Count - 1; index >= 0; index--)
            {
                directories.Push(childDirectories[index]);
            }
        }

        cancellationToken.ThrowIfCancellationRequested();
        entries.Sort(static (left, right) => StringComparer.OrdinalIgnoreCase.Compare(left.RelativePath, right.RelativePath));
        issues.Sort(static (left, right) =>
        {
            var pathOrder = StringComparer.OrdinalIgnoreCase.Compare(left.RelativePath, right.RelativePath);
            return pathOrder != 0 ? pathOrder : left.Code.CompareTo(right.Code);
        });

        return new CatalogueSnapshot(fullRoot, entries, issues);
    }

    private string NormalizeRoot(string? rootPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath) || !Path.IsPathFullyQualified(rootPath))
        {
            throw new CatalogueRootException(CatalogueRootError.InvalidPath, "Supply a fully qualified local directory path.");
        }

        string fullRoot;
        try
        {
            fullRoot = Path.GetFullPath(rootPath);
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or NotSupportedException)
        {
            throw new CatalogueRootException(CatalogueRootError.InvalidPath, "The selected root path is invalid.");
        }

        if (OperatingSystem.IsWindows() && fullRoot.StartsWith("\\\\", StringComparison.Ordinal))
        {
            throw new CatalogueRootException(CatalogueRootError.NetworkRootNotAllowed, "Network and device roots are outside this local catalogue slice.");
        }

        return fullRoot;
    }

    private FileAttributes ReadRootAttributes(string fullRoot)
    {
        try
        {
            return _fileSystem.GetAttributes(fullRoot);
        }
        catch (FileNotFoundException)
        {
            throw new CatalogueRootException(CatalogueRootError.NotFound, "The selected root does not exist.");
        }
        catch (DirectoryNotFoundException)
        {
            throw new CatalogueRootException(CatalogueRootError.NotFound, "The selected root does not exist.");
        }
        catch (UnauthorizedAccessException)
        {
            throw new CatalogueRootException(CatalogueRootError.AccessDenied, "The selected root is not accessible.");
        }
        catch (IOException)
        {
            throw new CatalogueRootException(CatalogueRootError.Unavailable, "The selected root could not be inspected.");
        }
    }

    private bool TryConfirmDirectoryIsTraversable(
        DirectoryInfo directory,
        string fullRoot,
        ICollection<CatalogueIssue> issues)
    {
        var relativePath = GetSafeRelativePath(fullRoot, directory.FullName);
        try
        {
            var attributes = _fileSystem.GetAttributes(directory.FullName);
            if ((attributes & FileAttributes.ReparsePoint) != 0)
            {
                issues.Add(new CatalogueIssue(relativePath, CatalogueIssueCode.ReparsePointNotTraversed));
                return false;
            }

            if ((attributes & FileAttributes.Directory) == 0)
            {
                issues.Add(new CatalogueIssue(relativePath, CatalogueIssueCode.DirectoryReadFailed));
                return false;
            }

            return true;
        }
        catch (UnauthorizedAccessException)
        {
            issues.Add(new CatalogueIssue(relativePath, CatalogueIssueCode.DirectoryAccessDenied));
            return false;
        }
        catch (IOException)
        {
            issues.Add(new CatalogueIssue(relativePath, CatalogueIssueCode.DirectoryReadFailed));
            return false;
        }
    }

    private void AddChild(
        FileSystemInfo child,
        string fullRoot,
        ICollection<CatalogueEntry> entries,
        ICollection<CatalogueIssue> issues,
        ICollection<DirectoryInfo> childDirectories)
    {
        string relativePath;
        try
        {
            relativePath = _fileSystem.GetRelativePath(fullRoot, child.FullName);
            if (!IsContainedRelativePath(relativePath))
            {
                issues.Add(new CatalogueIssue(null, CatalogueIssueCode.OutsideRootPathRejected));
                return;
            }

            var attributes = _fileSystem.GetAttributes(child.FullName);
            var kind = (attributes & FileAttributes.Directory) != 0
                ? CatalogueEntryKind.Directory
                : CatalogueEntryKind.File;
            var isReparsePoint = (attributes & FileAttributes.ReparsePoint) != 0;

            entries.Add(new CatalogueEntry(
                NormalizeRelativePath(relativePath),
                kind,
                isReparsePoint ? CatalogueEntryState.SkippedReparsePoint : CatalogueEntryState.Enumerated));

            if (kind == CatalogueEntryKind.Directory && !isReparsePoint)
            {
                childDirectories.Add(new DirectoryInfo(child.FullName));
            }
            else if (isReparsePoint)
            {
                issues.Add(new CatalogueIssue(NormalizeRelativePath(relativePath), CatalogueIssueCode.ReparsePointNotTraversed));
            }
        }
        catch (UnauthorizedAccessException)
        {
            issues.Add(new CatalogueIssue(GetSafeRelativePath(fullRoot, child.FullName), CatalogueIssueCode.EntryMetadataAccessDenied));
        }
        catch (IOException)
        {
            issues.Add(new CatalogueIssue(GetSafeRelativePath(fullRoot, child.FullName), CatalogueIssueCode.EntryMetadataUnavailable));
        }
        catch (ArgumentException)
        {
            issues.Add(new CatalogueIssue(null, CatalogueIssueCode.InvalidEntryPath));
        }
        catch (NotSupportedException)
        {
            issues.Add(new CatalogueIssue(null, CatalogueIssueCode.InvalidEntryPath));
        }
    }

    private bool IsContainedRelativePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath) || Path.IsPathRooted(relativePath) || relativePath == ".")
        {
            return false;
        }

        var parentPrefix = $"..{Path.DirectorySeparatorChar}";
        var alternateParentPrefix = $"..{Path.AltDirectorySeparatorChar}";
        return relativePath != ".."
            && !relativePath.StartsWith(parentPrefix, StringComparison.Ordinal)
            && !relativePath.StartsWith(alternateParentPrefix, StringComparison.Ordinal);
    }

    private string? GetSafeRelativePath(string fullRoot, string path)
    {
        try
        {
            var relativePath = _fileSystem.GetRelativePath(fullRoot, path);
            return IsContainedRelativePath(relativePath) ? NormalizeRelativePath(relativePath) : null;
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or NotSupportedException)
        {
            return null;
        }
    }

    private static string NormalizeRelativePath(string relativePath) =>
        relativePath.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
}

/// <summary>One in-memory view of an explicit root. Entries mean metadata was listed, not that file contents were opened.</summary>
public sealed class CatalogueSnapshot
{
    internal CatalogueSnapshot(string rootPath, IEnumerable<CatalogueEntry> entries, IEnumerable<CatalogueIssue> issues)
    {
        RootPath = rootPath;
        Entries = new ReadOnlyCollection<CatalogueEntry>(entries.ToArray());
        Issues = new ReadOnlyCollection<CatalogueIssue>(issues.ToArray());
    }

    /// <summary>Gets the normalized explicit root for this invocation.</summary>
    public string RootPath { get; }

    /// <summary>Gets listed files and folders using root-relative paths with forward slashes.</summary>
    public IReadOnlyList<CatalogueEntry> Entries { get; }

    /// <summary>Gets non-fatal access or path issues encountered while listing the root.</summary>
    public IReadOnlyList<CatalogueIssue> Issues { get; }
}

/// <summary>A single file or folder listed under the selected root.</summary>
public sealed record CatalogueEntry(string RelativePath, CatalogueEntryKind Kind, CatalogueEntryState State);

/// <summary>A non-fatal issue encountered while enumerating part of the selected root.</summary>
public sealed record CatalogueIssue(string? RelativePath, CatalogueIssueCode Code);

/// <summary>The kind of an enumerated filesystem entry.</summary>
public enum CatalogueEntryKind
{
    File,
    Directory
}

/// <summary>Whether an entry was listed or intentionally not traversed.</summary>
public enum CatalogueEntryState
{
    Enumerated,
    SkippedReparsePoint
}

/// <summary>Stable, non-sensitive issue categories returned in a snapshot.</summary>
public enum CatalogueIssueCode
{
    DirectoryAccessDenied,
    DirectoryReadFailed,
    EntryMetadataAccessDenied,
    EntryMetadataUnavailable,
    InvalidEntryPath,
    OutsideRootPathRejected,
    ReparsePointNotTraversed
}

/// <summary>Why an explicit catalogue root could not be accepted.</summary>
public enum CatalogueRootError
{
    InvalidPath,
    NetworkRootNotAllowed,
    NotFound,
    AccessDenied,
    Unavailable,
    NotDirectory,
    ReparsePointNotAllowed
}

/// <summary>Raised when an explicitly selected root cannot be safely scanned.</summary>
public sealed class CatalogueRootException(CatalogueRootError error, string message) : IOException(message)
{
    /// <summary>Gets the machine-readable rejection reason.</summary>
    public CatalogueRootError Error { get; } = error;
}

internal interface ICatalogueFileSystem
{
    FileAttributes GetAttributes(string path);

    IEnumerable<FileSystemInfo> EnumerateChildren(string directoryPath);

    string GetRelativePath(string rootPath, string path);
}

internal sealed class PhysicalCatalogueFileSystem : ICatalogueFileSystem
{
    public FileAttributes GetAttributes(string path) => File.GetAttributes(path);

    public IEnumerable<FileSystemInfo> EnumerateChildren(string directoryPath) =>
        new DirectoryInfo(directoryPath).EnumerateFileSystemInfos();

    public string GetRelativePath(string rootPath, string path) => Path.GetRelativePath(rootPath, path);
}
