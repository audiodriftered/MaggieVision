# MaggieVision

**Your media. Your network. Your rules.**

Private development monorepo. Currently at WP-00 **baseline and prototype recovery**. No production source or build has been verified in this repository yet.

MaggieVision is a local-first personal media application. The first commercial release is scoped around MaggieMusic and MaggieVideo. Future modules (MaggieReads, MaggiePictures, online podcast subscriptions) remain separately planned and must not block Music/Video 1.0.

## Start here

- [Product Bible v2.1 — 19 November 2026](docs/product/MaggieVision_Product_Bible_v2.1_2026-11-19.md) — canonical working draft; awaiting formal owner signoff.
- [UI & Navigation Addendum — 20 November 2026](docs/decisions/MaggieVision_UI_Addendum_2026-11-20.md) — latest agreed UI direction.
- [WP-00 execution brief](docs/engineering/MaggieVision_WP00_Execution_Brief_2026-11-20.md) — prototype recovery and verified build baseline.
- [Prototype recovery register](docs/engineering/Prototype_Recovery_Register.md).

### Repository layout

`src/` — production source after review. `prototypes/` — recovered and preserved source trees. `api/` — versioned host/client contracts. `tests/` — real runnable tests. `docs/` — decisions, engineering, test evidence. `scripts/` — safe inventory and validation tools. `recovery/incoming/` — **locally ignored**, untrusted downloaded archive staging.

**Important:** All existing source libraries are strictly read-only; library scans are explicitly user-initiated. Windows application must be native, not WebView. Discovery never authorises unknown devices. No manual IP, pairing code, ADB or Xbox Developer Mode in consumer onboarding.

Do not commit test media, local libraries, credentials, signing keys, source ZIP archives, binary installers, narration model weights, or unredacted diagnostics.

### CI

The initial Action validates **documentation only**; it does **not** compile MaggieVision. Build/test CI will be added after actual source recovery and toolchain verification.
