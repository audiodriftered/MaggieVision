# MaggieVision navigation model

## Purpose

This `net10.0` library defines the shell's module-scoped destinations and explicit navigation state without referencing WinUI, the host, filesystems, or media. `NavigationController` is owned by the Windows shell and is safe to unit test independently.

## State and transitions

- `MediaModule` is currently Video or Music.
- `NavigationDestination` carries a stable route ID, visible label, icon key, typed `DestinationKind`, optional target ID, and optional module scope. Built-in destinations have no target ID; a custom destination must name its module and target.
- `NavigationState` records the active module, selected route, whether the pane is collapsed, whether Settings is selected, and the in-memory custom destinations.
- Switching modules selects that module's Home route, clears Settings selection and presents only that module's built-ins and custom destinations.
- Settings is selected separately from the module route. Pane open/close changes do not alter the selected destination.
- Custom shortcuts are appended to the same flat list as built-ins. The model rejects built-in kinds, duplicate/blank IDs, blank labels or targets, invalid kinds, and shortcuts scoped to a different module.

## Persistence and boundaries

This is an in-memory UI model only. It does not create playlists or collections, authorize access, resolve targets, persist preferences, inspect folders, open media, or change source files. A future persistence layer must preserve the distinction between shortcut and target and must use host-side authorization when connecting routes to data.

## Build and tests

From `src/windows`, use the commands in `MaggieVision.Windows/README.md`. Tests are in `tests/unit/windows/MaggieVision.Windows.Navigation.Tests`; they cover module route lists, module-scoped Favourites and custom destinations, route/settings selection, and pane collapse behavior.
