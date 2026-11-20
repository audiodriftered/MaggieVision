# Prototype source recovery — candidate inventory

**All entries below are references from previous development discussions, not verified archives or working builds.** Record exact archive name, source conversation/local path, SHA-256, manifest, dependency versions, actual compile command/result and evidence before importing source.

| Component | Previously reported source package / candidate | WP-00 status |
|---|---|---|
| Windows mainline | `MaggieVision-Mainline-Source-v0.11.0-alpha1.zip` | Locate, hash, verify |
| Windows consolidation | `MaggieVision-Consolidation-v0.21-Source.zip` | Locate, hash, verify |
| Windows music/resume | `MaggieVision-v0.21.1-Music-Repair-Source.zip`; `MaggieVision-v0.21.2-Resume-Cleanup-Source.zip` | Locate, compare lineage, verify |
| Android tablet | `MaggieVision-Tablet-v0.21.3-AndroidStudio-Source.zip`; `MaggieVision-Tablet-v0.21.4-Redesign-Source.zip` | Locate, hash, verify |
| Android phone | `MaggieVision-Phone-v0.21.3-AndroidStudio-Source.zip` | Locate, hash, verify |
| Fire TV | Earlier Media3 Android source and installed APK | Locate original project and verify build/device correspondence |
| Windows UI lab | `MaggieVision-UI-Prototype-v0.1-Windows.zip` | Locate, establish native/experimental status |
| Books narration lab | MaggieBooks / MaggieNarrator / VoxCPM2 experiments | Locate projects, keep model binaries out of Git |
| Apple and Xbox | Earlier exploratory builds or plans | Verify actual source exists; Xbox retail is not proven |

## Rules

Place downloaded ZIPs in **ignored** `recovery/incoming/`. Never overwrite originals. Inventory before extracting. Inspect archive entries for path traversal, executables, secrets and copyrighted media. Import reviewed source into `prototypes/` on a branch, then document reusable components and verified tests before promoting to `src/`.

No claims of implemented production code until recovered, built and tested.
