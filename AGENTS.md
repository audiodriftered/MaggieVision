# MaggieVision — rules for coding agents

Read the Product Bible and latest UI addendum before changing design or code. Product decisions marked AGREED are not implemented code. Only claim a feature verified with repeatable build/test and actual-device evidence.

- WP-00 is an inventory/recovery phase, **not** permission for a blanket rewrite.
- Absolutely no writes to source media or selected source-library folders on **any** platform. No source rename, moves, deletions, tags or sidecars.
- Scan roots and rescan only on explicit user action.
- Windows UI must be a **real native desktop app**; never substitute a browser, WebView or localhost page.
- Device discovery does not grant access; enforce device, profile and library permissions on the host.
- Local/LAN play must not require a cloud session, manual IP address or pairing codes.
- Do not require ADB, CLI tools or Xbox Developer Mode of consumers.
- Preserve legacy prototype archives untouched and import source with provenance to `prototypes/` first. Do not commit personal media, secrets, signing keys or diagnostic logs.
- First commercial release: Music and Video; RSS Podcasts, MaggieReads, Pictures are deferred release gates although their designs/prototypes may continue.
- Any AI coding change must report commit SHA, changed paths, build commands, test results, device evidence and remaining unknowns.
