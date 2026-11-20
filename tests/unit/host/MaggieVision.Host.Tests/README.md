# Host Unit Tests

Run the .NET 10 solution from `src/host` using the locked restore, Release build and test commands in [the host README](../../../src/host/README.md).

`HostCompositionTests` builds the real host in-process and verifies the WP-00 worker starts and stops through cancellation. `LocalCatalogueScannerTests` exercise the opt-in explicit-root catalogue using temporary synthetic fixtures only; no personal media library is used. They verify nested relative paths, exclusion of an unselected sibling, source-byte and creation/write/access-time stability, unchanged directory contents, reparse-point non-traversal, invalid roots, deterministic access errors, cancellation, and release of filesystem handles.

The catalogue tests are implementation-owned unit tests. They are not independent black-box conformance tests and do not pass QA-001. SCM installation, before-login access, device trust, real-library behavior and production readiness remain unverified.
