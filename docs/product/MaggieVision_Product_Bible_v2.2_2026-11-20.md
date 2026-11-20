# MAGGIEVISION
## Comprehensive Product Bible

**Reconciled engineering edition v2.2 — 20 November 2026**  
**Product status:** approved directions and scoped requirements; **not** a statement that MaggieVision application v1.0 is built or commercially ready.  
**Working brand:** MaggieVision.  
**Positioning:** Your media. Your network. Your rules.

> **The promise.** A polished, low-friction, local-first player and personal media ecosystem for music, films and television. It should be easy for a nontechnical customer to install, use on their own device, connect securely to their Windows-hosted library, and optionally access that library away from home. The user's media remain theirs.

**Document authority.** This edition audits and supersedes the v2.0 consolidation and integrates later explicit decisions through 19 November 2026. It also retains relevant requirements from the MaggieVision Product Bible v1.1 dated 14 November 2026. Where they conflict, the later, explicit decision governs. This is a **product requirements document**, not evidence of implemented capabilities, legal clearance, app-store approval or software certification. This v2.2 working edition records the Founder-approved Anywhere release-decoupling decision of 20 November 2026 and reconciles engineering review controls. Other untested implementation choices remain candidates or ADR gates. Overall Product Bible signoff and independently verified implementation are separate acts. No production implementation is implied.

**Status labels used throughout:** **AGREED** = approved product behaviour; **TARGET** = agreed intent subject to technical/platform validation; **OPEN** = a genuine product or engineering decision not yet made; **DEFERRED** = expressly outside the first commercial release; **PROTOTYPE** = observed or reported experimental capability, not proof of production readiness; **SUPERSEDED** = older decision replaced by a later agreement.

---

# 1. Executive definition and product boundaries

## 1.1 What MaggieVision is

MaggieVision is a modular personal-media ecosystem with independent, native applications on supported devices. Its core commercial release provides two strong experiences: **MaggieMusic**, a standalone player for music stored on the device or accessible from an authorised Windows host; and **MaggieVideo**, a standalone player for locally stored films, television and personal videos or for media streamed from that host. A customer can use either module or both.

A Windows PC can act as the household media host. MaggieVision clients on phones, tablets, TV-streaming devices and, subject to retail approval, Xbox discover it automatically. Separate **MaggieVision Anywhere** functionality makes the authorised Windows media library available over the internet. Core local and LAN use must not rely on Anywhere or an active MaggieVision cloud session.

The experience is not exclusively a Plex replacement. A user may purchase MaggieMusic for advanced local music playback and never configure a PC host. A household may choose MaggieVideo as an attractive, simple LAN alternative. The commercial value lies in convenience, consistency, reliability, polish and control, not claiming that technically inclined users cannot write similar software.

## 1.2 What MaggieVision is not

MaggieVision does not provide a licensed Netflix-style catalogue, torrent client, piracy source, DRM circumvention tool, cloud-hosted copy of a user's library, mandatory subscription for ordinary local playback, advertising-supported content feed, or remotely controlled file manager for the Windows PC. It does not judge the origin of user-accessible files. It must respect third-party software, metadata and media licences.

## 1.3 Non-negotiable principles

- **The nontechnical family-member test.** Installation, host discovery, permission approval and everyday playback must be achievable without IP addresses, command prompts, ADB, Xbox Developer Mode, port forwarding, pairing codes, editing configuration files or installing codec packs.
- **Local first.** Local media and ordinary LAN playback work without a mandatory account, permanent cloud connection or remote relay.
- **Real native interfaces.** The Windows application must be a genuine native desktop app, not a localhost website, browser UI or WebView wrapper. Mobile and TV clients use device-appropriate native interfaces.
- **Simple by default, powerful when requested.** Advanced playback, sorting, audio and network features must not burden the initial journey.
- **No data hostage-taking.** Original files remain where they are; user-owned downloads remain ordinary accessible files where the OS permits; library metadata, corrections and history are durable and recoverable.
- **Security without ceremony.** Automatic discovery does not grant access. Unknown devices require approval, and the Windows host enforces permissions.
- **Modular without bloat.** A music-only installation should not show a television catalogue or ebook controls. Hide modules without deleting their state.
- **No ads and no recurring payment for purchased local features.** Free is genuinely useful; premium local entitlements are lifetime purchases; Anywhere is a distinct optional online service.
- **Proof over promises.** A working alpha test is not a certified build, supported platform or commercially viable feature.

# 2. Decision authority, history and supersessions

## 2.1 How to use this Bible

This is the single source of truth for product behaviour. A subsequent development task should cite a requirement ID and its acceptance test. Where the text is marked OPEN, engineering must not silently pick a new product policy. Where an engineering technology is marked TARGET or CANDIDATE, it remains subject to a reproducible spike/test. Delivery status is tracked separately from product approval.

Priority definitions: **P0** means release-blocking for the applicable *Music/Video* release, **P1** means agreed Music/Video work subject to achievable platform coverage, **A0** means release-blocking **for the independently gated Anywhere release only**, **A1** means agreed Anywhere enhancement (never a Music/Video launch gate), **P2** means later enhancement, and **R&D** means no product commitment until feasibility is proven. Requirement IDs are stable. Agreed requirements need independent implementation evidence.

## 2.2 Important November 2026 supersessions

| Topic | 14 November Bible v1.1 | Later governing decision |
|---|---|---|
| Trial registration | Registration required to begin 30-day trial | **SUPERSEDED:** 30-day Premium Music/Video trial begins automatically on first app use; no registration or payment details required. |
| Post-trial access | Pay £9.99 to restore full product access | **SUPERSEDED:** Revert to permanent, useful Free mode with library/data intact. |
| Commercial packaging | Single £9.99 lifetime household licence | **SUPERSEDED:** modular lifetime Music and Video upgrades and Complete bundle; figures £6.99/£7.99/£16.99 remain provisional. |
| Remote access | Later exploratory capability | **SUPERSEDED 20 Nov:** Anywhere remains a separate optional paid product but has **its own independent release gate**. Its security/relay/economics failures may not block an otherwise ready Music/Video launch. |
| Consumer release modules | Early broad beta expected Video/Music/basic Books | **SUPERSEDED for 1.0:** commercial launch focuses on Music and Video; MaggieReads and MaggiePictures remain later independent modules. Earlier Books test work remains legitimate R&D. |
| Platforms | Fire TV/Android current, Apple/Xbox future | Later target expanded to Windows, Android, iOS/iPadOS, Android/Google TV, Fire OS, Vega OS and Xbox retail subject to real store/device validation. |
| Host service | Earlier browser/Go-host prototype | Target rebuild uses native WinUI 3 desktop and a separate ASP.NET Core Windows Service. Existing Go/browser work is prototype heritage, not the target architecture. |
| Access controls | Device Approval plus optional Open Home mode | Latest security baseline requires host-enforced, authenticated and encrypted device access; a permissive discovery-as-authorisation mode must not be silently inherited from older notes. |
| Offline licensing | Trial/account-based online validation assumed | Purchased local Music/Video functions must continue indefinitely offline after legitimate activation. |
| Source file management | Earlier proposal allowed optional Windows media rename/move/delete | **SUPERSEDED:** source files are strictly read-only everywhere. No MaggieVision option or permission can modify original media. |
| Pricing | Older document retained £6.99/£7.99/£16.99 as working figures | **SUPERSEDED:** all prices and pricing structures are deferred until a testable product exists; no figure is a product decision. |
| Anywhere trial | Separate trial length was unsettled | **AGREED:** 30 days from explicit Anywhere activation, distinct from the account-free local Premium trial. |
| Platform scheduling | Simultaneous launch was unresolved | **AGREED:** staged release by platform readiness; Windows, Android phone/tablet and Fire OS are first public beta priorities. |
| Remote data handling | Earlier agreement required encryption/authentication without detailed cloud limits | **AGREED:** no original customer media or library catalogues on MaggieVision cloud infrastructure; relays carry end-to-end encrypted traffic only. |

**Traceability note.** The November 19 conversation reused certain section numbers and once failed to acknowledge an agreed proposal. Consequently the dialogue's section labels are **not** unique requirement identifiers. This Bible gives requirements unique IDs, retaining the actual decisions: embedded/synchronised lyrics are agreed; distinct multiple-Windows-user accounts and TV/controller navigation are both agreed; remote guest invitations are rejected.

# 3. Customers, jobs and success criteria

| Audience | Job to be done | Experience to prove |
|---|---|---|
| Ordinary household | Install and play without technical assistance | A fresh Windows-to-Fire-TV journey works without manual network work. |
| Music-only user | Play owned MP3/FLAC/ALAC locally; optionally use a PC library | Useful standalone player; outstanding navigation and audio controls, no video clutter. |
| Film/TV enthusiast | Enjoy owned files, artwork, subtitles, audio tracks and resume | Long 4K sessions, correct black letterboxing, reliable episode/film metadata. |
| Household with mixed devices | Share a Windows host but retain profiles and privacy | Different devices and people play concurrently and synchronise state appropriately. |
| Traveller | Access their own Windows media away from home | Secure Anywhere connection with invisible networking complexity and acceptable costs. |
| Nontechnical older user | Install without coaching and operate from sofa | Plain-English setup, large focus targets, error recovery and automatic discovery. |
| Audiophile | Play lossless music and control signal path | Explicit EQ chain and optional bit-perfect output where supported. |
| Personal-archive owner | Browse home videos with original folder organisation | No mistaken TMDB matching, reliable local thumbnails, preserved filenames. |

Commercial success metrics are not yet fixed. The beta must capture unassisted onboarding completion, time to first playback, unexpected stops, audio/subtitle success, discovery and approval errors, repeated use, support contacts, willingness to pay, refunds, conversion and Anywhere usage cost.

# 4. Release definition and platform matrix

## 4.1 First commercial release

**AGREED first commercial product:** MaggieVision core; MaggieMusic; MaggieVideo including films, TV and personal videos; permanent account-free Free tier; automatic 30-day Premium trial; premium lifetime Music/Video purchases; secure Windows host and LAN use. **Founder decision (20 November 2026; DEC-2026-11-20-001): Anywhere has an independent commercial release gate.** It may ship alongside Music/Video only if its own security, functionality, cost, licensing and operational gates pass; otherwise Music/Video may launch without it. Later Anywhere launch does not diminish purchased local lifetime features. An accepted product direction is not a completed/certified application.

**Staged platform delivery is AGREED.** A platform may be published when its own normal-installation, security, codec, accessibility and reliability gates pass; an unready Xbox, Vega or Apple build does not hold back a validated Windows/Android/Fire OS release. Only advertise platforms that actually pass certification and distribution checks. **First public beta priority:** Windows host/native desktop, Android phones/tablets and Android-based Fire OS. Apple, Google TV, Vega OS and Xbox continue through parallel feasibility and development work without delaying those first beta priorities.

**MaggieReads** (Books, Comics and Audiobooks including local AI narration) and **MaggiePictures** (photographs) remain later independent modules, not Music/Video v1.0 gates. Existing MaggieBooks experiments are retained as legitimate R&D.

## 4.2 Platform commitments

| Platform | Form | Functional role | Status / release condition |
|---|---|---|---|
| Windows 10/11 (supported versions TBD) | Native WinUI 3 C# UI + separate Windows Service | Host, local Music/Video client, setup and device approvals | P0 target; build and OS baseline to validate. |
| Android phone/tablet | Kotlin, Jetpack Compose, Media3 | Standalone local Music/Video; authorised LAN/Anywhere client | P0 target; form-factor-specific UX. |
| iPhone/iPad | Swift/SwiftUI, AVFoundation plus codec fallback | Standalone local Music/Video; authorised LAN/Anywhere client | P0 target; codec and App Store validation. |
| Fire TV on Android-based Fire OS | Native Android TV-style client | Ten-foot LAN/Anywhere playback | P0 target; actual hardware and storefront acceptance. |
| Android TV / Google TV | Kotlin TV-appropriate native app | Ten-foot LAN/Anywhere playback | P0 target; relevant TV hardware/store approval. |
| Fire TV on Vega OS | Separate Vega-compatible client implementation | Ten-foot LAN/Anywhere playback | TARGET; early SDK and platform capability gate. |
| Xbox Series consoles | Dedicated approved Xbox Retail-Mode app | Ten-foot LAN/Anywhere playback with controller | TARGET; retail distribution approval and viable native/client engine required. Never require customer's Dev Mode. |
| Smart TV native Tizen / webOS | No native application | TV displays through Fire TV/Xbox/etc. | OUT OF 1.0 scope. |
| Mac as host; Linux as host; NAS as host | No host server | May access NAS through Windows host | OUT OF 1.0 scope. |

All targeted storefronts and codec capabilities are **release gates**, not claims that a submission has been accepted. A client cannot be advertised as supported until its relevant build, release signing, normal installation and end-to-end tests pass. Platform-by-platform release is approved. If a platform fails certification or tests, it remains unpublished until corrected; other passing clients may ship.

## 4.3 Standalone clients and host relationship

Mobile local files need **no Windows host**. A Windows PC is needed to serve the PC-based household library and to perform host-side remux/transcoding/remote streaming. At 1.0, mobile-local files are **not themselves shared as a server to other devices**. One Windows host per household initially. Clients may contain both device-local and authorised Windows media in one logical catalogue.

# 5. Product and commercial model

## 5.1 Free, Premium and Anywhere

| Offering | Commercial rule | Agreed scope / remaining work |
|---|---|---|
| MaggieVision Free | Permanent, useful, **no ads**, no mandatory account | Core local Music/Video playback, LAN browse/stream, folders, essential subtitles and audio tracks, 4K/HDR when supported, ordinary queues/playlists, watchlists, basic media management, manual Music downloads from authorised Windows host on LAN. |
| Premium Music/Video trial | **30 days, automatically from first application use**, account-free and no card | Both Music and Video premium local features unlocked during trial; on expiry simply revert to Free with no data/media/history loss. Beta use must not start the commercial clock. |
| MaggieMusic Premium | Lifetime local entitlement; final price **DEFERRED** | Advanced EQ/ReplayGain, Audiophile Mode and Smart Shuffle; **Keep Available Offline** automatic music sync is Premium. Remaining detailed gates are documented as open where not settled. |
| MaggieVideo Premium | Lifetime local entitlement; final price **DEFERRED** | Windows-host video offline downloads and seamless cross-device video handoff are Premium. Essential local/LAN playback stays Free. |
| Music + Video bundle | One-time entitlement covering both launch modules; final price **DEFERRED** | Preserve purchase/upgrade credit; do not assume that a Music+Video bundle confers automatic rights to every future module. |
| MaggieVision Anywhere | Independent optional online release; final pricing/business model **DEFERRED** | Its own **30-day** no-card trial upon account sign-in and first activation; remote authenticated streaming and, with the appropriate module entitlement, remote downloads. No automatic subscription conversion. |

**Commercial lock:** No list price, discounted bundle price, Anywhere subscription interval or device fee is to be treated as approved. Prior numeric examples remain historical discussion only. Pricing will be decided when an adequately tested product and real support/relay costs exist. The Free/Premium feature boundary is partially settled above; remaining advanced features must be classified deliberately before purchase-gating code is built.

## 5.2 Registration, trial and purchase

First use starts the **30-day Premium Music/Video trial automatically**, with no account, payment details or trial opt-in screen. No automatic charging at the end. The app retains libraries, metadata, progress, history and playlists and downgrades cleanly to Free features. Registration is mandatory **only** to purchase/restorably hold premium entitlements or enable Anywhere; sign-in options ideally support passkeys, Apple/Google and email where available. Purchases can be restored on replacement devices; an entitlement service maps genuine store/direct purchases to the user's account.

A local signed licence enables **indefinite offline use of legitimately activated lifetime Music and Video features**, without compulsory periodic online checks. Fraud/device-revocation policy must be reasonable and cannot undermine this promise. Remote service access necessarily requires appropriate current authorisation. Free local/LAN playback must not become account-dependent because licence services are unavailable.

## 5.3 Household and sharing

A **household licence is anchored to one primary Windows host**, with multiple authorised profiles and **no arbitrary numerical cap on household phones, tablets or TV devices**. Actual concurrent playback remains constrained by hardware/network capacity and permissions, not an invented licensing ceiling. The customer may transfer the licensed host role when replacing the PC without buying again. The licence design must prevent simple commercial-scale abuse without intrusive address checks or arbitrary recurrent account verification.

**Standalone premium players remain possible.** A customer who buys only MaggieMusic or MaggieVideo Premium on a phone/tablet does not need to own, install or register a Windows host. Household role and licence ownership are distinct from user profiles and registered device credentials. Cross-store entitlement recognition is a target subject to app-store obligations; do not claim it works until verified.

## 5.4 Commercial boundaries

No behavioural ads, intrusive upsells, forced catalogue recommendations or selling of personal library information. No unlimited remote relay or cloud transcoding bundled irresponsibly into a cheap lifetime local licence. Pricing, VAT, app-store commission, refunds, chargebacks, privacy obligations, software/codec licensing and support unit costs require commercial/legal sign-off.

# 6. Information architecture and interface system

## 6.1 Global navigation

Native Windows sidebar: **Video, Music, Pictures, Books, Settings**, with Settings anchored at the bottom and future modules absent or hidden unless selected. The interface can show only active modules. Future labels may be refined to MaggieReads and MaggiePictures while retaining clear user-facing categories. Windows has a real desktop window and tray behaviour; no browser or embedded website.

**Films and TV:** Home centres on Continue Watching and Recently Added, with clear entry to Films, TV Series, Personal Videos, Folders, Collections and Search. No compulsory recommendation feed. **Music:** Artists, Albums, Tracks, Playlists, Genres, Years, Folders, Smart Playlists, now-playing and queue. Both preserve original folder hierarchy in dedicated Folders views.

Every video library card and media thumbnail is **landscape 16:9**, not a default portrait poster. Use proper stills/backdrops when available; never distort artwork. Responsive layout adapts to Windows, touch tablet, narrow phone and ten-foot TV; no one-size-fits-all stretched phone layout.

## 6.2 Appearance and accessibility

Light, Dark and Follow System modes; music skins (official initial skins, safe non-executable community skin framework later). Adjustable text sizes, strong selected-focus states, high contrast, reduced motion, screen reader support (Narrator, TalkBack, VoiceOver and relevant TV services), usable touch targets, keyboard navigation and optional Simplified Interface. Accessibility settings are independent of skin choice.

English UI at first release, but localisation-ready strings, Unicode filenames/metadata and regional number/date conventions. Media language remains independent of UI language. Other UI languages are later according to demand.

## 6.3 Profiles and family operation

Distinct MaggieVision profiles per household/device with personal watch/listen history, playlists, favourites, ratings, queue, settings and permissions. One profile on a TV opens directly to Home; multiple profiles show **Who's Watching?**; optional profile PIN. Secure per-profile/private cache handling. Multi-Windows-user PCs share one Windows host/catalogue but keep separate MaggieVision profile state and permission boundaries. A profile PIN does not substitute for OS file protection.

# 7. Windows host, installation and lifecycle

## 7.1 Host model

A Windows machine is the **only host server in 1.0**. Target technology: ASP.NET Core/.NET Windows Service running before user login, with a native WinUI 3 desktop control/client. The host maintains authoritative catalogues, IDs, library permissions, streaming and host-mediated sync. The desktop UI can be closed while the service continues working. Service identity should use least privilege, explicit file/share access and securely stored credentials where needed.

**Service-access feasibility gate:** A before-login service does not automatically inherit user-profile and SMB/NAS share access. Test a least-privilege service identity and appropriately checked named-pipe/SID impersonation/credentials during signed-out operation. **No source ACL write exception is approved.** If one is demonstrably necessary, request Founder approval for a limited, consented, rollback-capable change and add source ACL before/after verification to QA-001.

One Windows installer integrates desktop UI, service, dependencies, signed components, automatic start, firewall rules where appropriately authorised, logs and update machinery. Normal one-time administrator approval at install is accepted. Routine consumer use must not require elevated privileges, shell commands or service management. Both official website download and Microsoft Store are desired Windows channels; installer/package/store route needs actual validation.

The host has Balanced default, Performance and Low Impact processing modes. Background library work, metadata, thumbnails and transcoding must not overwhelm the machine. Active streams prevent inappropriate sleep where OS permissions allow; return to normal idle power behaviour when no active sessions. Optional prevent-sleep setting. Wake-from-sleep remote support only where the network/hardware supports it; no promise to power on a switched-off PC.

## 7.2 File-access and media safety — absolute read-only policy

**NON-NEGOTIABLE: The original source media and all files within user-selected source libraries are strictly read-only to MaggieVision on every platform, including Windows.** MaggieVision must **never** rename, relocate, retag, rewrite, overwrite or delete a source media file. There is no administrator-only override, advanced setting or optional file-manager mode. **This overrides every earlier suggestion of Windows rename/move/delete and Recycle Bin integration.** No automatic sidecar writing into source folders. Physical library management remains the responsibility of Windows Explorer or other user-chosen tools, outside MaggieVision.

MaggieVision may write **only to its own application database, backups, caches and designated derived-output/download destinations**. Those destinations must not be confused with the source library, and must not rewrite or silently delete user-downloaded media. Host access to library roots should use OS-level read-only permissions/handles wherever the filesystem supports them; automated tests must verify that source file bytes, metadata and paths remain unchanged through scans, playback, relinking, downloads, metadata export/import, crash recovery and updates.

Host indexes **only explicitly user-selected source folders**. Windows network shares/NAS are accessed through the Windows host, with service credentials/availability handled securely. A client uses opaque media IDs, not arbitrary user-supplied host filesystem paths. User edits inside MaggieVision change only catalogue/collection metadata, and immediately update the local catalogue/search index, not the original file. Changes made externally are discovered only by explicitly requested scans.

## 7.3 One-time onboarding

Install → start native app → select Music and/or Video (all optional module selections initially unchecked on Windows) → choose folders or Skip → scan by explicit request → show a ready state. Phone/tablet first-run setup is optional: add local media, connect to Windows host or explore. OS file permissions are requested contextually. TV/Fire/Xbox first run automatically discovers friendly host names and requests approval; no entering IP or code. If PC locked/unattended, approval from already authorised owner mobile app is permitted where secure verification works; otherwise it remains pending.

## 7.4 Device trust and guests

Discovery never authorises. **First-contact trust must pass a security ADR and threat test** against rogue mDNS hosts, spoofing, replay and approval flooding. Propose pinning the host and approved device keys using a cryptographically sound handshake and a short **transcript-derived on-screen match** confirmed by tapping, without typing any pairing code. A decorative word match is not security. The local Windows UI must not be allowed to assert its Windows user identity to the service; test named-pipe restrictions and SID checks.

The host receives an explicit owner-approved device request, creates revocable, device-bound credentials and reconnects silently thereafter. Approval must be available from Windows and securely from an already authorised owner's mobile device. Guests can be granted **LAN-only temporary access**, chosen expiry (e.g., 24 hours or one week), selected library scope and streaming/download permissions; downloads disabled by default. Remote invited guest sharing was **rejected for 1.0**. Only authorised household users use Anywhere.

# 8. Library ingestion, identities and metadata

## 8.1 Scanning and folder fidelity

The original hierarchy always remains browseable; never automatically reorganise, move, rename or merge physical files. User adds exact roots; manually starts initial scans, **Scan for Changes** and **Rebuild Library**. There is **no silent periodic or filesystem-triggered background rescan**. Long scans are staged and resumable: catalogue/playable entries first, optional artwork enrichment and heavy analysis after; show pause/resume/progress. Catalogue-only actions (title corrections, collections, ratings and library-root administration) initiated **inside MaggieVision** update its catalogue immediately without a rescan. MaggieVision never edits physical source files; externally changed files are reconciled by explicit scans.

Overlapping roots, symlinks/junctions, inaccessible folders, external drives and identical files across sources need a formal scanner policy and acceptance tests. Offline/unavailable host sources remain in the UI with cached metadata and a clear unavailable indicator, not silently erased.

## 8.2 Content identity, matching and grouping

Use stable media identity across rescans, renames and duplicate sources. Automatically unify device-local and Windows copies **only at high confidence**; distinct cuts, remasters, releases, encodes or user-declared editions must remain selectable. Prefer device-local copy where practical; let the user explicitly choose another source/quality. Manual user corrections override embedded tags, filename conventions and online enrichment. Cache should not make third-party metadata a dependency for playback.

Music grouping handles Track Artist vs Album Artist, Various Artists compilations, multi-disc sets, box sets, actual Greatest Hits compilation albums, and separate virtual artist Essentials/Greatest Hits assembled from owned tracks. Allow corrections without rewriting originals. Television grouping handles Season 0/specials, episode ordering, multi-episode files and manually corrected numbering. Films handle extras, bonus folders, multiple editions, film franchises and user-defined collections. Personal Videos use filename/folder/recorded date, not film matching.

## 8.3 Metadata, search and cache

Use embedded tags and local file structure first. Apply third-party metadata/artwork lookup only where allowed, with per-library opt-out and a provider-independent adapter. No customer-provided API keys required for ordinary enriched browsing. For Music, retain original embedded artwork and user overrides. Use local SQLite FTS5/search index for fast search, fuzzy matching/typo tolerance as feasible, and permissions-aware results. Do not log personal search strings on remote services by default.

Separate cache categories: protected catalogues/settings/history/corrections; replaceable artwork/metadata thumbnails; disposable playback cache; **explicit user downloads are never cache**. Storage screen shows sizes, configurable cache limits and safe per-category clear actions. Do not delete original media, downloaded files, playlists or corrections in automatic cache cleanup.

The user may **export and re-import library metadata**, including corrections, classifications, collections and artwork associations, for reinstall or host migration. Exports use stable media identities and safe relinking, report ambiguities for user review and never write to source files. Sensitive personal metadata should be protected appropriately.

Removing a library has two separate commands: **Remove Library** retains dormant catalogue/history/corrections for possible restoration; **Remove Library and Forget** warns and permanently clears its stored associated metadata but still **never deletes original files**.

# 9. MaggieMusic requirements

## 9.1 Browsing and collection model

Artists, Albums, Tracks, Genres, Years, Playlists, Smart Playlists, Folders and Recently Played. Unified local/host sources without breaking folder hierarchy. Per-view sort by artist, album, title, year, date added, last played and other relevant fields; ascending/descending; grid/list; filters such as genre and audio format; saved per-section preferences. Actual Greatest Hits albums must not be mixed up with generated Essentials lists.

Personal favourites, optional five-star track/album ratings, profile-specific play counts and last-played. A meaningful listening threshold distinguishes an actual play from briefly seeking/skipping; this threshold needs engineering definition. Listening-history tracking is **on by default** but may be disabled, cleared and kept profile-private. The minimum play-count threshold is an engineering calibration item, not another product-policy approval. All can drive Smart Playlists and local intelligent shuffle.

## 9.2 Playback and queues

Core formats: MP3, AAC/M4A, FLAC, ALAC, WAV and other legally supported mainstream formats, with exact codec matrix determined by tests. True background playback and OS media sessions, lock-screen controls, media keys, Bluetooth/USB output, headset events, audio focus, call interruption/resume behaviour, gapless default, repeat, shuffle and seek. Optional crossfade, off by default, lower priority. Music pauses when the same device plays video, preserving the queue, and does not auto-resume after video unless the user chooses that setting.

**Queue commands:** Play Now, Play Next and Add to Queue; reorder; remove; clear; save to playlist; durable queue after app restart and device reboot; cross-source support. Manual Play Next takes priority over shuffle. Standard Shuffle plays each track once per cycle; Album Shuffle randomises albums and preserves album order; both Free. **Smart Shuffle Premium** uses local listening preferences while avoiding repeats/recent artists; no cloud dependency.

**Sleep timer Free:** 15/30/60 minute presets, after current track, after album/queue, optional gradual fade, affects current device only. **Spoken-word audio Free:** local podcasts, lectures and broadcasts with saved position, bookmarks, playback speed and configurable skips; no podcast-directory/RSS subscription service at 1.0.

## 9.3 Audio processing and fidelity

**Premium audio tools:** a ten-band graphic EQ with frequency response and optional real-time spectrum; preamp, bass, treble, limiter, user and device-specific presets; ReplayGain; relevant playback visualisation (Windows, tablet, TV; simplified on phone); Now Playing on TVs with screen-protection/OLED considerations. Individual output settings by headphones, desktop speakers, DAC and compatible peripherals.

Headphones disconnect => pause rather than blast through speakers; reconnection stays paused. Manual output switching should retain position and queue. Calls pause playback, and optional auto-resume happens only when it was playing and not manually paused; allow opt-out. Respect OS audio-focus rules. USB DAC/hi-fi preferences require testing on supported OS and devices.

**Audiophile Mode Premium** offers bit-perfect output where possible (e.g., Windows WASAPI Exclusive Mode as a candidate). It disables any EQ, normalisation or processing that would alter samples and warns of exclusive-mode trade-offs. Do **not** label playback bit-perfect on unsupported devices or where the OS resamples/mixes it. User can always return to Standard playback.

## 9.4 Lyrics, playlists and sharing

Read embedded lyrics and local `.lrc` synchronisation/timestamps; highlight current line and permit seek from lyric where practical. Full-screen phone lyrics, artwork-and-lyrics desktop/tablet and TV view. External online lyric retrieval is **DEFERRED**, pending licensing and API evaluation.

Import/export common M3U/M3U8/PLS/WPL playlists where feasible; smart rules, local and host tracks, stable identities. Household playlist permissions: private default, shared read-only, or collaborative authorised edits; copies do not alter original. Sync conflicts should not silently discard either person's edits. Free includes ordinary playlist editing/sharing and queue management; Premium gating must be explicitly defined for advanced features only.

## 9.5 Out-of-scope for Music 1.0

Synchronised multi-room audio across diverse devices is deferred; independent household simultaneous playback is agreed. RSS podcast subscriptions, internet radio directory, automatic lyrics licences, commercial streaming accounts and sophisticated cloud recommendations are not 1.0 dependencies. Google Cast/CarPlay/Android Auto were discussed as desirable device integrations, but each requires relevant capability, approval and acceptance testing; do not advertise until working.

# 10. MaggieVideo requirements

## 10.1 Library, home and navigation

Movies and TV Series → Seasons → Episodes, plus Personal Videos and Folders. 16:9 landscape media cards and backdrops, artwork enrichment with user corrections, Recently Added and Continue Watching on Home. Search, actor/cast details where metadata/licensing supports it, and appropriately grouped collections. No compulsory recommendations. Personal Videos are separate, with locally generated stills, recorded-date browsing, catalogue labels/descriptions and preserved folders; no mandatory online matching.

User-created and automatic franchise collections may overlap and have release/story/custom order. Under-film extras include making-of footage, deleted scenes, interviews and trailers from the user's own files, with manual association and common subfolder detection. Multiple film editions remain distinct. TV specials/Season 0, placement in chronological playback order, multi-episode files and manual episode correction are first-release requirements.

## 10.2 Playback, video and audio compatibility

Prioritise direct file playback, then container remux, then audio-only transcode, then video transcode only if necessary. Device-specific capability negotiation, seek/range support, hardware decode where possible, hardware transcode where supported, guarded software fallback and meaningful errors when unavailable. Exact matrix includes MP4/MKV/AVI and common video codecs; subtitles SRT/ASS/PGS where supported, multiple audio tracks, HDR/SDR and tone mapping, surround formats/passthrough, audio delay and downmix. No promise that every proprietary codec or passthrough path works on every device without testing and licensing analysis.

Black letterboxing (never grey). Aspect choices: Fit default, Fill/Crop, Original Size, Stretch in Advanced. Subtitle selection, separate forced/full flags, language preferences, external sidecar subtitle discovery (e.g., `.en.srt`, `.fr.srt`, `.forced.srt`) and embedded tracks. Profile-specific sizing down to practical small scales, foreground/background colour, placement and synchronisation delay. Optional series-specific preferences. Online subtitle search/download **DEFERRED**, pending provider/legal design.

## 10.3 Viewing continuity and controls

Continue Watching includes only incomplete items, with position bar; completion logic uses chapters/credits detection where reliable, fallback configurable threshold and manual override. Personal watchlist; manual watched/unwatched at film, episode, season and series level; Up Next next-unwatched episode; tracked state separate from playhead position. End-of-film default returns gracefully to library, TV episode autoplays with visible countdown and Cancel, configurable Ask First and Return to Library. No unwanted surprise autoplay on relaunch.

10-foot TV remote and Xbox pad: directional focus, Select/A, Back/B, play/pause, seek and context actions; no essential function should require pointer, keyboard or touch. Touch video gestures (volume/brightness/seek) optional and off by default; avoid accidental input. Picture-in-Picture explicit opt-in with OS support; video normally pauses when leaving unless PiP active. Thumbnail seek previews, screen controls and appropriate inactivity behaviour. Avoid screensaver stopping active playback wherever platform capabilities permit.

## 10.4 Video Free and Premium boundary

**Free:** core local video playback and LAN viewing; browsable original folders, supported 4K/HDR, essential audio and subtitle selection, media collections, personal videos, watchlists, viewing history and Continue Watching.

**MaggieVideo Premium (AGREED):** downloading Windows-hosted films/TV episodes onto mobile/tablet storage for offline use, and seamless cross-device video playback handoff when the destination can access the media. Handoff is not a claim that a phone-local file is streamed magically to the next device. Remote use additionally requires Anywhere. There is no permission to manipulate source files.

Other optional advanced Video-only premium features require an explicit final entitlement audit; essential playback cannot be artificially restricted merely to manufacture an upsell. During beta all premium functions are unlocked without reducing customers' eventual commercial trials.

## 10.5 Out-of-scope for Video 1.0

No TV provider/Netflix/Disney+ login integration or stream aggregation. The proposed search-to-provider deep link/launcher is separate exploratory work. No optical-disc folder playback for DVD/BDMV, no DRM circumvention. Scene-by-scene actor identification like Amazon X-Ray is not promised; optional conventional cast/crew metadata is a separately licensable capability.

# 11. Playback transport, offline media and device handoff

## 11.1 Local, LAN and internet routing

MaggieVision should distinguish (a) on-device files, (b) trusted LAN-hosted files, and (c) authenticated Anywhere remote media. Prefer an available local copy. On loss of LAN, retain offline-capable local media and show cached catalogue entries with unavailable state. Across Wi-Fi→mobile data, an authorised Anywhere session should transition with minimal interruption, retaining progress and security; do not silently start using paid mobile data contrary to user preferences.

## 11.2 Offline downloads and ownership

**Music Free:** A user may manually download authorised tracks, albums and playlists from a Windows host while on the LAN to a supported phone/tablet. **Music Premium:** automatic **Keep Available Offline** synchronisation, including new tracks/playlist membership, under explicit opt-in. **Video Premium:** manual authorised Windows-host film and TV downloads. All remote downloads additionally require an active Anywhere entitlement on the hosting household. Host-enforced **Download** rights remain separate from **Stream** rights, and parental/library restrictions apply.

**Quality choices for video downloads:** **Optimised (default)** for device capability/storage, **Original** without source modification if the destination can decode it, and **Save Space** for a smaller derivative. Any conversion occurs in constrained, temporary host working storage and writes a new output; it must never alter the source. Remember per-device quality choices.

Downloads must be resumable, cancellable, auditable, bandwidth/storage-aware and **Wi-Fi-only by default for remote transfers**, with an explicit mobile-data override. The app shows progress, available space, warnings for expensive data transfers and failure recovery. Downloaded media remain ordinary accessible user files wherever OS policy allows, not proprietary DRM containers. **Never auto-delete explicitly downloaded files** due to cache cleanup, trial expiry, loss of connectivity, playlist changes or failed synchronisation. A user may manage downloaded copies using OS file tools. Support removable storage where available without making it a launch blocker on unsupported devices.

## 11.3 Handoff and background lifecycle

A **MaggieVideo Premium** authorised profile can transfer playing position and appropriate queue/subtitle/audio state to another device **if that destination can access the source media**. Local-only file on one phone is not magically available elsewhere. Application restarts preserve queues and positions. Playback session recovery should be robust but should not unexpectedly autoplay upon app reopen. Remote control between devices is a lower-priority nice-to-have, requiring explicit authorisation.

# 12. Secure identity, profiles and permissions

## 12.1 Identity model

Separate three identities: (1) a human MaggieVision licence/Anywhere account, (2) a household profile for personal history and parental settings, and (3) an authorised physical device with cryptographic credentials. Local Free/LAN functions must not require (1). A Windows host is the authority for its own library and remote permissions. Stored device credentials must use platform secure storage, have revocation support and be protected against copied backups.

## 12.2 Access controls

Host-authoritative authentication, encryption and permissions for protected services; no UI-only hiding. Fine-grained per-device/per-profile/per-library browse, stream and download rights; deny arbitrary file-path requests and separate artwork/metadata visibility from the right to stream content. Guest access is **LAN-only**, temporary, automatically expiring, limited to selected libraries and **downloads disabled by default**. Remote invited guest sharing was expressly rejected for v1.0.

For a **child profile**, unrated or unidentified titles are **hidden by default** until the household owner explicitly approves them. Owner exceptions and library restrictions are enforced by the Windows host, including search/metadata/artwork paths. Switching to an unrestricted profile requires the appropriate owner/PIN authentication. MaggieVision's controls cannot protect source files opened outside its application through the OS.

An active household owner may view approved devices, revoke device credentials and terminate applicable sessions. Remote-specific controls are described in Section 13. Windows source files remain permanently outside application write authority.

## 12.3 Security and privacy

Minimum collection and retention of account data; local history defaults and configurable opt-outs; no sales of user data or advertising profiles. Diagnostic export and crash reports are local-first and consensual; redact file paths, personal media titles and tokens where possible. Third-party metadata queries are optional and disclosed. Secure input handling and sandboxed metadata/codec workers are mandatory; media files are untrusted input. Secure update chain, credential storage, permissions review, audit of dependencies, reproducible release artifacts and incident response process are release gates.

# 13. MaggieVision Anywhere

## 13.1 Product contract, privacy and trials

Anywhere is an **optional paid remote access service independently gated for commercial release**; it is **not** a prerequisite for a Music/Video launch. It lets an authorised household member securely access **their own Windows-hosted music/video** away from home. It is not a media-sharing social network or a cloud media locker. Ordinary local and LAN playback remain independent of the Anywhere backend and its uptime.

**Trial:** Anywhere has its **own independent 30-day trial**, starting only when the customer signs into a MaggieVision account and first activates Anywhere. No payment information is required and the trial **never automatically converts** into a paid subscription. The account-free Music/Video Premium trial is separate and starts on first app use. Beta participants' trial countdowns begin only at commercial release, not during beta testing.

**Privacy invariant:** MaggieVision's cloud infrastructure **never stores customers' original media or their media library catalogues**. The Windows host serves the content. Cloud services may maintain the minimal account, licence, device-session, connection-routing and billing/abuse metadata needed to operate Anywhere, with defined retention and redaction; they must **not collect titles, playback history or media content**. Relays must pass **end-to-end encrypted** traffic; the host and authorised client are the media security endpoints, and the relay must not gain the means to decrypt streams or downloads.

Remote streaming and downloads are distinct permissions. Remote **video downloads** require MaggieVideo Premium plus an active Anywhere entitlement; remote **manual music downloads** may be made by Music Free users with an active Anywhere entitlement, while automatic music sync additionally requires Music Premium. The receiving device must respect available storage, mobile-data preferences and platform file access.

**Pricing, recurring interval, relay traffic allowances and remote concurrency limits are intentionally deferred** until successful beta and actual operating cost measurement. Anywhere must not be commercially enabled or advertised before its own security, economics, operation and legal gates pass; those gates do not delay ready local/LAN Music/Video. No unvalidated unlimited-4K or remote uptime claim may be made.

## 13.2 Network engineering constraints and customer controls

No manual consumer VPN/Tailscale, router ports, dynamic DNS, fixed IP address or pairing code. Prior Tailscale-over-5G playback is a **prototype proof only**, not the consumer solution. ICE/NAT traversal, QUIC/TLS and managed relay approaches remain engineering candidates pending adversarial tests, viability and bandwidth economics. Test double NAT, CGNAT, mobile roaming, hotels, IPv6/IPv4 and failed network transitions. Only encrypted, authenticated channels are permitted.

**Streaming quality:** **Automatic (default)** adjusts to device/network capabilities; **Original Quality** prefers original source characteristics when technically supportable; **Data Saver** uses lower bitrates. Users set separate Wi-Fi and mobile-data defaults, optional data-use warnings and estimated consumption where reliable. The selected mode may need an available host-side remux/transcode, subject to processing and licensing limits.

**Owner Anywhere Security screen:** show currently active remote devices/sessions; terminate selected sessions; revoke device credentials; temporarily disable all Anywhere connections without interrupting LAN/local playback. Revocation must close already-active affected streams promptly, not merely block the next session. These controls are available from Windows and authorised owner's mobile where supported and must require appropriate owner authentication.

An unreachable/sleeping Windows host is reported plainly; supported wake-from-sleep can be attempted but **powered-off remote startup is not promised**. Temporary derived streams/downloads are hosted or transferred securely without storing users' media in the service provider's cloud. Relay egress, transcode expense, abuse load and failure rates must be measured before any paid service launches.

# 14. Sync, persistence, recovery and backup

## 14.1 Synchronised state

Profile-specific favourites, ratings, music queues, playlists, history (unless disabled), watched status, continue positions, books later, and supported preferences synchronise over trusted LAN and Anywhere when available. Offline device edits queue locally and reconcile when connection returns. Conflict rules must preserve user intent; concurrent collaborative playlist changes cannot silently discard one side. Retain clear recoverable history where appropriate. The Windows host owns hosted-media catalogue authority, while each client owns its local media catalogue and offline cache.

## 14.2 Database and storage

Per-device SQLite persistence, authoritative host-side catalogue/state, robust ID mapping for cross-source media, versioned migrations, pre-migration backup and transactional rollback/recovery. The user can create encrypted metadata/configuration backups and restore on replacement Windows installations. **Export Library Metadata / Import Library Metadata** is also required for portable corrections, classifications, collection membership and artwork associations, with deterministic mapping, conflict preview and no writes to the source media. Such backups do not silently copy original films/music. User media remains separate from thumbnails, catalogues, logs, temp transcode caches and explicit offline downloads.

## 14.3 Resilience

Host restarts, Windows sign-out, client process death, library temporary disconnection, update interruption and device reboot should recover without repeated pairing or data corruption. Previously unavailable media stays represented with a clear status. If metadata provider goes down, media still plays. If licence service goes down, Free/local purchased offline capabilities still work according to the entitlement rule.

# 15. Proposed engineering architecture

## 15.1 Component contracts

**Windows host:** ASP.NET Core (.NET) Windows Service, local media indexing, file/permission services, ranged streaming, capability negotiation, optional remux/transcode, profiles and secure discovery. **Windows UI:** WinUI 3 C# desktop client with tray, settings and owner-approval surface. **Android:** Kotlin/Compose/Media3/MediaSessionService/Room or equivalent SQLite. **Apple:** Swift/SwiftUI/AVFoundation, SQLite/Keychain and a supplementary local decoder where Apple's built-in framework does not natively handle the supported container/audio/subtitle format. **Android TV/Fire OS:** native 10-foot Android adaptation. **Vega:** separate client/toolchain after early proof. **Xbox:** supported retail app architecture after early proof. **Shared business models:** Kotlin Multiplatform is a **candidate, deferred** pending practical cross-platform maintenance benefit. First Android/Fire OS clients may share pure Kotlin domain contracts and versioned OpenAPI models without forcing KMP adoption.

A versioned **OpenAPI contract is authoritative** for host/client APIs, with generated client models and compatibility negotiation. Host/media service accepts opaque IDs and structured capability information, never exposes arbitrary local paths. Workers handling ffmpeg, thumbnails and untrusted metadata run in isolated processes with limits. Every decoder/codec/runtime must pass commercial-licence and security review.

## 15.2 Media decision chain

Source identification → client decode capability check → **Direct Play** → if necessary **remux** → if necessary **audio transcode** → finally constrained **video transcode**. Playback compatibility wins over an elegant architecture that cannot actually play the user's media. Test original video, multiple audio tracks, subtitles and HDR independently. Hardware acceleration preferred when reliable; software fallback limited by host performance profiles.

## 15.3 Development discipline

One private Git monorepo with sources, API schema, tests, docs, signed artifacts, release manifests and clear build instructions. Stable main branch with required checks. Automate CI using GitHub Actions and existing Windows/Mac build machines where suitable; do not incur cloud expenditure before discussing it. Every AI-generated change must compile, pass tests and undergo normal review. Cross-platform vertical slices (install → discover → stream → resume) are prioritised over completing one client in isolation. Maintain a tested stable build with source/version fingerprints and a rollback path. The Windows app must remain native; do not regress to browser management UI for commercial release.

# 16. Installation, updates, accessibility and support

Official app stores for Android/iOS/Fire TV/Google TV and Xbox retail where published; no direct consumer Android sideloaded APKs in the commercial release. Windows website and Microsoft Store distribution, one consistent experience. Signed installers and updates; Windows service starts before login, appropriate admin approval once, no customer scripting. Signed background updates where supported; notify important changes, allow safe deferral for non-security updates when practical; migration is backed up and recoverable.

Error text should be plain English and actionable (host asleep/offline, no shared folder, unsupported format, permission denied, unavailable library, internet/Anywhere down). Diagnostics should be inspectable and shareable only by user choice. Support load must fit affordable lifetime pricing, including a useful self-service help centre and consent-based problem reports. Beta builds must have a straightforward, consent-based problem-reporting path with no bundled media content or unsolicited history disclosure.

# 17. MaggieReads — future module definition, not launch gate

MaggieReads comprises **Books**, **Comics**, and **Audiobooks**. Books reads DRM-free EPUB; Comics supports CBZ/CBR/PDF, manga right-to-left and spreads where appropriate; Audiobooks plays existing MP3/M4B/M4A/FLAC with chapters, bookmarks, adjustable speed, sleep timer and resume. Locally narrated ebooks become linked entries in Audiobooks and remain anchored to the source book. This structure is separate from MaggieMusic's generic spoken-word files.

**MaggieNarrator:** offline/on-device Windows-host CPU/GPU synthesis, **never cloud narration**; optional download of supported voices/models; production benchmark English British voices is a user requirement. Initial experiment included narration of the user's own work, *The Legend of the Silver Hand*, with voice-engine experiments including VoxCPM2. This proves experimentation occurred, not that the consumer narration pipeline is shippable. The customer may Preview, Play Chapter, Play Entire Book or Generate Entire Book. Generate enough audio to start promptly, continue ahead, cache segments, resume jobs, regenerate only changed passages when feasible, and export for offline use.

Book Intelligence Profile (BIP) holds stable source paragraph anchors, book metadata, pronunciation lexicon, alias/character map, voice assignments, provenance and user corrections. Classic single-narrator mode first, possible character-aware dialogue voices later with safe fallback. Paragraph-level read/listen sync is agreed; word-level highlighting later. Embedded EPUB data first; optional metadata enrichment with opt-out. Never alter the author's prose or circumvent DRM. This entire area is deliberately **DEFERRED from Music/Video commercial v1.0**, though separate alpha work can continue.

# 18. MaggiePictures and other future proposals

MaggiePictures will be a selectable photo-focused module with folders, local albums, slideshows, metadata and optional TV viewing. It is **not a 1.0 release gate**. Personal home videos are already in MaggieVideo 1.0. Later candidates—not approved launch features—include photo memories, local catalogue intelligence, remote content-provider launchers, scene-aware cast identification, official cross-platform streaming-portal integration, multi-room synchronised audio, online podcast/RSS subscriptions, online lyrics and subtitle services, broad international UI translations, native Mac/Linux hosts, source-hosted NAS software and advanced community skins.

# 19. Current implementation evidence and known gaps

| Capability | Evidence class | What has actually been demonstrated / not established |
|---|---|---|
| Windows prototype host | PROTOTYPE | Earlier LAN host delivered HTTP/range video streaming; production .NET service architecture is a rebuild target. |
| Fire TV 4K with subtitles | PROTOTYPE | User reported smooth long-form 4K/subtitle playback in home setup; not production hardware/codec matrix certification. |
| Android tablet local music | PROTOTYPE | Installed, scanned and played internal-storage music; integration of that local catalogue with Windows-hosted music remained in development. |
| Windows/Fire TV UI | PROTOTYPE | Video browsing functional; past bugs included posters, search input, screen saver and occasional long-session stops. |
| Tailscale over 5G | PROTOTYPE | Remote stream demonstrated as a feasibility experiment only; no consumer zero-VPN Anywhere solution yet. |
| Xbox | PROTOTYPE / R&D | Source/development attempts exist; **no validated retail-mode public Xbox app**. Developer Mode must not be required of customers. |
| iOS/iPadOS | TARGET | Planned native clients; unsupported MKV/codec paths and store build must be proven. |
| Vega Fire TV | TARGET / R&D | Separate Vega app/toolchain needed; no proof of retail readiness. |
| MaggieBooks narration | PROTOTYPE / R&D | Hands-on Windows lab work, including the user's novel and voice tests; not a production MaggieReads release. |
| Entitlements, payment and account recovery | TARGET | Product model agreed; implementation and compliance not demonstrated. |
| Device approval, full sync, downloads, Anywhere | TARGET | Requirements agreed; end-to-end security/reliability and production build not certified. |

Never use prototype version numbers as evidence that the overall commercial product is at v1.0. Maintain an independently verified build/version matrix for every client.

# 20. Release sequencing and quality gates

## 20.1 Workstreams and staged beta priorities

**R0 — Greenfield engineering baseline:** Establish a clean monorepo, reproducible native host skeleton, pinned tooling, initial CI, secure configuration and rights-safe test fixture; no retrieval or reuse of historical prototype code is required. Record actual build evidence from the new codebase. **R1 — Native Windows foundation:** Signed clean installer, before-login service, truly native WinUI UI, strict source-file read-only policy, explicit library scan, secure device approval. **R2 — End-to-end Music/Video vertical slices:** Windows + Android phone/tablet + Android-based Fire OS as the first public-beta priority; local and host catalogue, background music, video subtitles/audio, user sessions and basic synch. **R3 — Cross-device premium functionality:** Profiles, correct entitlements, offline download split, media export/recovery, handoff and accessibility. **R4 — Independently gated Anywhere connectivity/cost proof:** Account-based enablement, E2EE direct/relay, honest monitoring and remote download/quality/security. **R5 — Music/Video commercial readiness, with separate Anywhere readiness where applicable:** Trial boundaries, stores, legal/licensing review, signed updates, incident response and go/no-go evidence.

**Private Alpha:** approximately **5–10 trusted testers**; experimental or incomplete functionality permissible with clear warning. **Invitation-based Public Beta:** approximately **25–50 participants per controlled wave**; essential install, playback, access-control and recovery paths must be dependable **before** invitation. **Open Beta:** only after hardware/platform stability and support load justify it. Anywhere starts with a smaller expressly authorised remote test cohort. These are recruitment planning targets, not product-usage promises.

**Beta entitlement:** All beta testers have Premium Music and Video without their commercial 30-day trial clock running; Anywhere beta time likewise does not consume the separate Anywhere trial. At each product’s commercial launch, eligible testers receive the applicable full trial. Do not silently bill or enroll anyone; test environments use appropriate sandbox transactions and test identities. Account-free local beta use remains possible; remote beta uses the authentication required by Anywhere.

Parallel feasibility spikes are mandatory for **Vega OS, Xbox Retail Mode, Apple codec coverage, Cast/CarPlay/Android Auto and Anywhere relay economics**. Their failure can defer that individual platform/integration without blocking otherwise passing releases. No fabricated deadlines or assumption that all platforms ship together.

## 20.2 Non-negotiable acceptance tests

| Gate | Pass condition |
|---|---|
| Install | Fresh nontechnical user completes Windows host + official client install without CLI/IP/port/pairing codes. |
| Discovery & approval | Unknown device denied; owner approves once; reconnect works after PC/device restart and locked Windows login. |
| Video | Representative legal test files stream/play, seek, resume and finish; subtitles/audio switching work; bars black. |
| Music | Local phone/tablet plays with background controls and calls/headphones; host/music unified catalogue functions. |
| Library | Selected roots only, no auto rescan, original folders preserved; catalogue-only edits take effect without rescanning; bytes, tags, timestamps and paths of source media remain unchanged. |
| Offline | Music manual downloads Free; Music auto-sync and Video downloads Premium; remote entitlement combinations enforced; selected Original/Optimised/Save Space settings; source never changed; explicit downloads not automatically deleted. |
| Profiles | Different users have separate histories, ACLs and no catalogue leaks; child PIN limitations clear. |
| Licence | Account-free trial starts on first use; day 31 transitions to Free without data loss; Anywhere has distinct 30-day activation trial; beta never spends either trial; purchased local features work offline indefinitely. |
| Anywhere — independent release | No VPN/port/IP setup; E2EE host-to-client including relay; no cloud media/catalogues; owner session revocation kills active streams; adaptive quality and controlled remote downloads tested; acceptable relay economics. **Failure blocks Anywhere only, not Music/Video.** |
| Reliability | Long playback, update, reboot, crash, host sleep and unplugged drive scenarios produce expected recovery. |
| Security | Threat modelling, API fuzzing, malicious-file processing, transport encryption, least privilege, signed updates. |
| Store/Legal | Normal Retail-mode installations on every claimed platform; store policy, data protection, licence obligations satisfied. |
| Accessibility | Screen readers, focus, scaling and simplified UI tested with actual users. |
| Evidence | Signed reproducible artifacts and actual device/OS/store test results; staged platform releases; private-alpha versus public-beta gates; documented release go/no-go approval. |

## 20.3 Representative end-to-end journeys

Each journey requires a clean or precisely documented setup, actual device evidence and an associated QA record in Section 26.

| Journey | End-to-end proof |
|---|---|
| J01 | Fresh Windows install to Fire TV playback without CLI, IP or manual pairing. |
| J02 | Standalone Android local music, background playback and lock-screen controls without host/account. |
| J03 | iPad local MKV/unsupported-codec fallback with embedded/external subtitles where legally supported. |
| J04 | Two simultaneous household streams with independent profile progress and permissions. |
| J05 | LAN-only guest sees selected library, download denied by default, access expires automatically. |
| J06 | Windows user signs out; media service continues to serve already-authorised devices. |
| J07 | Drive is disconnected, library stays visible as unavailable, then returns without silent rescan. |
| J08 | Manual scan and catalogue metadata corrections, followed by export/import and recovery. |
| J09 | Account-free 30-day Premium trial expires to Free; lifetime purchase works offline and restores correctly. |
| J10 | Anywhere over mobile network, with session continuity or safe recovery during Wi-Fi to mobile-network transition. |
| J11 | MaggieReads/audiobook lab is tracked separately and cannot block Music/Video 1.0. |
| J12 | Original source hashes, modified times, paths and directory trees stay unchanged across all library operations. |
| J13 | Beta-to-release trials and Music Free / Music Premium / Video Premium / Anywhere entitlement combinations. |
| J14 | End-to-end encrypted relay, owner kill-switch, remote revocation and no cloud media/catalogue storage. |
| J15 | Child-profile unrated media hidden from search, artwork and streaming except owner-approved cases. |
| J16 | Objective alpha quality gate and invitation-based beta waves tested with uncoached participants. |

# 21. Decision register — remaining product calls versus engineering validation

The following IDs preserve the v2.0 register for audit purposes. A **CLOSED** item is no longer a question to ask the product owner. **DEFERRED** is intentionally not decided, and **ENGINEERING GATE** is a test/procurement/implementation task rather than a new product-choice conversation.

| ID | Status | Resolved policy or remaining gate |
|---|---|---|
| O-001 | PARTLY CLOSED | Video offline downloads and video handoff Premium; Music manual host download Free, auto-sync Premium. Remaining obscure features need a final entitlement matrix before commercial gating; no speculative upselling. |
| O-002 | DEFERRED | All Music, Video, bundle and Anywhere prices intentionally postponed until a testable product and cost evidence exist. Do not resurface pricing before that milestone. |
| O-003 | PARTLY CLOSED | Anywhere trial **30 days**, account-triggered, no card/autobilling; price, payment cadence, concurrency and relay economics deferred. |
| O-004 | CLOSED | One primary Windows host/household licence, unlimited authorised household devices subject to hardware, transferable host, standalone local Premium allowed. |
| O-005 | ENGINEERING GATE | Validate no-VPN consumer Anywhere connection, host-to-client E2EE including relay, performance and costs. |
| O-006 | ENGINEERING GATE | Xbox Retail Mode distribution/engine feasibility; no customer Developer Mode. |
| O-007 | ENGINEERING GATE | Vega SDK, native app/DRM-free codec capabilities and normal storefront installation. |
| O-008 | ENGINEERING GATE | Apple local MKV/audio/subtitle fallback, HDR and app-store/licensing feasibility. |
| O-009 | ENGINEERING GATE | Minimum actual OS and hardware versions set from passing compatibility testing. |
| O-010 | ENGINEERING/LEGAL GATE | Codec licensing, surround audio, FFmpeg/libmpv obligations, patents and store policies. |
| O-011 | ENGINEERING POLICY | Formal rules/tests for overlapping roots, path junctions and symlinks; no source modification or root escape. |
| O-012 | ENGINEERING POLICY | Conflict-safe offline sync, collaborative playlists, queues, deletions and profile permissions. |
| O-013 | CLOSED | Strictly read-only original media on all platforms; no optional source file manager or delete/rename/move. |
| O-014 | ENGINEERING/LEGAL GATE | Providers/artwork/cast data contracts and compliance. **Metadata export/import is separately agreed**, not an unapproved licence contract. |
| O-015 | CLOSED TARGET + GATE | Cast, Android Auto and CarPlay are desired launch integrations subject to platform testing; none individually blocks other ready releases. |
| O-016 | CLOSED | Staged platform launch; Windows/Android/Fire OS first beta priority; others launch when proven. |
| O-017 | PARTLY CLOSED | Listening history on by default, private per profile, can disable/clear. Precise play-count threshold is engineering calibration. |
| O-018 | ENGINEERING POLICY | Default cache/scan resource budgets and scan exclusions; must protect source files and explicit downloads. |
| O-019 | ENGINEERING/PRIVACY POLICY | Best-effort account-free trial abuse prevention; no invasive tracking and no user burden. |
| O-020 | LAUNCH OPERATIONS GATE | Refunds, lost licence recovery, support queues, incident/security reporting, privacy deletion and Anywhere service monitoring. |
| O-021 | **CLOSED — FOUNDER APPROVED 20 NOV** | Anywhere independently gated; does not block Music/Video. Supersedes coupling of MV-A001/MV-P006 only, not security or cost requirements. See DEC-2026-11-20-001. |

**Additional closed decisions from post-v2.0 scoping:** metadata corrections portable export/import; account-free beta Premium; separate 30-day Anywhere beta/commercial trial; guest invitations remotely rejected; owner remote session kill-switch; child profiles hide unrated content; server/relay cannot retain media or catalogue; original/optimised/save-space download choices; Wi-Fi-only remote download default; automatic/original/data-saver remote streaming; restricted Music/Video download entitlements. See Sections 7–14 and the requirement catalogue; these are not questions to ask again.

# 22. Explicitly deferred or rejected

**DEFERRED:** MaggieReads commercial module (Books/Comics/Audiobooks, offline TTS), MaggiePictures; synchronised multi-room music; online lyrics and subtitle downloading; podcast subscriptions/RSS; personal online media-provider integrations; native LG webOS/Samsung Tizen; Mac/Linux/NAS servers; automatic external provider recommendation feeds; sophisticated X-Ray scene actor recognition; advanced word-level ebook follow-along; native shared phone/server broadcasting; automated film/media acquisition; comprehensive TV disc-folder playback; multi-Windows-host households; consumer community plugins or runnable skin scripts.

**REJECTED FOR 1.0:** remote guest invitations/media sharing outside household; any in-app renaming, moving, rewriting or deleting original source media; any requirement for consumer Tailscale/VPN, port forwarding, fixed IP, developer tools or Xbox Developer Mode; browser/WebView Windows product; ads in Free tier; mandatory account for Free, LAN or trial; paying again for already-purchased lifetime functionality; automatic deletion of original media or explicitly downloaded files; unapproved client access; transparent DRM bypass.

# 23. Requirement catalogue — implementation traceability

The IDs below are stable references for tasks and tests. Unless otherwise noted, the requirement is **AGREED** but still **not independently verified implemented**. P0/P1 indicate target release priority, not implementation status.

| ID | Requirement | Priority |
|---|---|---|
| MV-C001 | Local-first, account-free basic Music/Video playback | P0 |
| MV-C002 | Modular show/hide without data deletion | P0 |
| MV-C003 | Native Windows UI; no browser/WebView shell | P0 |
| MV-C004 | Windows host runs as service before login | P0 |
| MV-C005 | Native official-install clients with accessible UX | P0 |
| MV-C006 | One Windows host initially; mobile local playback independent | P0 |
| MV-C007 | 16:9 video artwork/cards across clients | P0 |
| MV-C008 | Light/dark/system, scaling, screen-reader, focus | P0 |
| MV-C009 | Native OS media-session and background audio | P0 |
| MV-C010 | Device-specific simplified/10-foot controls | P0 |
| MV-L001 | Explicit library roots; no automatic rescans | P0 |
| MV-L002 | Original nested folder structure always browseable | P0 |
| MV-L003 | Resumable staged scans, pause/progress | P1 |
| MV-L004 | Catalogue-only edits update DB and search index immediately; never mutate source files | P0 |
| MV-L005 | Stable media identity and high-confidence source dedupe | P0 |
| MV-L006 | Offline sources remain visible with cached metadata | P0 |
| MV-L007 | Corrections override tags/filenames/online providers | P0 |
| MV-L008 | Protected metadata vs disposable cache, safe purge | P0 |
| MV-L009 | Remove Library vs Remove and Forget distinct | P1 |
| MV-L010 | Local permissions-aware fast search | P0 |
| MV-M001 | Artists/albums/tracks/genre/years/folders/playlists | P0 |
| MV-M002 | Compilation album, multi-disc, Greatest Hits correct grouping | P0 |
| MV-M003 | Separate virtual artist Essentials without copying files | P1 |
| MV-M004 | Native background controls, calls/headphones semantics | P0 |
| MV-M005 | Durable queue Play Now/Next/Add/reorder/save | P0 |
| MV-M006 | Shuffle Standard and Album Free; Smart Premium | P1 |
| MV-M007 | Sleep timer and local spoken-word controls | P1 |
| MV-M008 | Ratings, favourites, history and smart playlist rules | P1 |
| MV-M009 | Per-output EQ, ReplayGain, 10-band/spectrum tools | P1 |
| MV-M010 | Audiophile bit-perfect where supported; truthful detection | P1 |
| MV-M011 | Embedded and `.lrc` lyrics, synchronised display | P1 |
| MV-M012 | Household shared/collaborative playlists | P1 |
| MV-M013 | Independent simultaneous household playback | P0 |
| MV-V001 | Movies/TV/Seasons/Episodes/Personal Videos | P0 |
| MV-V002 | Continue Watching, Recently Added and Up Next | P0 |
| MV-V003 | Subtitle language/forced/external and style controls | P0 |
| MV-V004 | Multiple audio tracks, timing, downmix/passthrough candidates | P0 |
| MV-V005 | Direct Play → remux → audio → video transcode | P0 |
| MV-V006 | Landscape thumbs, correct black bars and aspect modes | P0 |
| MV-V007 | TV specials and multi-episode filenames | P1 |
| MV-V008 | Film extras, editions, franchises, custom collections | P1 |
| MV-V009 | Profile Watchlists and watched/unwatched states | P1 |
| MV-V010 | Configurable episode-next/end-of-film transitions | P0 |
| MV-V011 | TV remote/Xbox controller UX with no mouse dependence | P0 |
| MV-V012 | Personal Videos free; no forced commercial metadata match | P1 |
| MV-D001 | Owner approves devices once; revocable credentials | P0 |
| MV-D002 | Approve from Windows or authorised owner mobile | P1 |
| MV-D003 | Guest LAN-only expiry and selected library permissions | P1 |
| MV-D004 | Server-enforced scope; opaque IDs; encryption | P0 |
| MV-D005 | Profiles/PIN and per-device stream vs download rights | P0 |
| MV-D006 | **No platform**, including Windows, can modify, rename, move or delete originals | P0 |
| MV-D007 | Source originals **strictly read-only**, enforced at OS/service boundary where feasible | P0 |
| MV-D008 | Per-profile history/privacy and secure credential storage | P0 |
| MV-S001 | Sync progress, queues, favourites and playlists | P0 |
| MV-S002 | Offline operation queue/reconcile; preserve conflicts | P1 |
| MV-S003 | Manual and Keep Available Offline authorised downloads | P1 |
| MV-S004 | Offline downloads never auto-deleted as cache | P0 |
| MV-S005 | Device handoff when media accessible to target | P1 |
| MV-S006 | Encrypted export and safe database migration | P1 |
| MV-S007 | Host wake from sleep where possible, never promise power-on | P1 |
| MV-P001 | 30-day account-free automatic Premium trial | P0 |
| MV-P002 | Trial expires to useful permanent Free, preserving data | P0 |
| MV-P003 | Account for purchase/Anywhere, not Free/LAN/trial | P0 |
| MV-P004 | Lifetime local paid features remain usable offline indefinitely | P0 |
| MV-P005 | Modular purchase/upgrade credit/restore entitlements | P0 |
| MV-P006 | Anywhere separate sign-in triggered trial/service, independent of Music/Video release | A0 |
| MV-P007 | No ads or automatic subscription conversion | P0 |
| MV-A001 | Optional Anywhere independently releasable when its own gates pass; Music/Video may launch first | A0 |
| MV-A002 | No consumer VPN, ports, IPs, DDNS or pairing codes | A0 |
| MV-A003 | Encryption, authenticated remote access, revocation | A0 |
| MV-A004 | NAT traversal and cost/usage measurement validated | A0 |
| MV-O001 | Official stores for mobile/TV, Windows direct + Store | P0 |
| MV-O002 | Signed installer and updates, rollback/recovery | P0 |
| MV-O003 | No CLI/ADB/Dev Mode required for ordinary users | P0 |
| MV-O004 | Private Git, OpenAPI contract and required CI checks | P0 |
| MV-O005 | Crash logs/diagnostics opt-in/redacted | P0 |
| MV-O006 | Malicious file parser/decoder worker isolation | P0 |
| MV-O007 | Privacy, codec/license and storefront review | P0 |
| MV-O008 | Reality-based platform/test matrix before support claims | P0 |
| MV-C011 | Standalone mobile Premium works without a Windows host | P0 |
| MV-L011 | Portable metadata export/import with safe relinking and source-read-only verification | P1 |
| MV-L012 | No catalogue operation, scan, transcode or update writes sidecar/source files | P0 |
| MV-M014 | Listening history default on; user can disable/clear; profile-private | P1 |
| MV-M015 | Manual host music downloads are Free on LAN; remotely require Anywhere | P1 |
| MV-M016 | Automatic Keep Available Offline music sync requires Music Premium | P1 |
| MV-V013 | Host-to-mobile video downloads require MaggieVideo Premium | P1 |
| MV-V014 | Cross-device video handoff requires MaggieVideo Premium and shared media reachability | P1 |
| MV-D009 | One primary host per household; no arbitrary authorised device cap; transferable | P0 |
| MV-D010 | Child profiles default-deny unrated content; owner exceptions enforced host-side | P0 |
| MV-D011 | Unauthorised profile switches and metadata/artwork leakage blocked | P0 |
| MV-S008 | Video downloads offer Optimised default, Original, Save Space; source untouched | P1 |
| MV-S009 | Remote download resumable and encrypted; Wi-Fi-only default, mobile opt-in | P1 |
| MV-S010 | Export/import metadata independent of source folders and user account | P1 |
| MV-P008 | Anywhere independent 30-day activation trial, no card/no automatic billing | A0 |
| MV-P009 | Beta access never consumes either commercial 30-day trial | P0 |
| MV-P010 | Local Premium purchases remain usable on standalone devices, host optional | P0 |
| MV-P011 | All pricing decisions explicitly deferred; no provisional numbers as implementation defaults | P0 |
| MV-A005 | No hosted customer media files or media catalogues in cloud | A0 |
| MV-A006 | Relay remains unable to decrypt end-to-end media payloads | A0 |
| MV-A007 | Automatic/Original/Data Saver stream quality + separate Wi-Fi/cellular preferences | A1 |
| MV-A008 | Owner can terminate, revoke and disable remote sessions without disabling LAN | A0 |
| MV-A009 | Remote Video download = Video Premium + Anywhere; remote Music = Anywhere (+ Music Premium for auto-sync) | A0 |
| MV-O009 | Staged platform launch; Windows/Android/Fire OS public beta first | P0 |
| MV-O010 | Private Alpha 5–10; invite beta waves ~25–50; beta quality gates before publication | P0 |
| MV-O011 | Third-party Cast/CarPlay/Android Auto individually validated, nonblocking | P1 |
| MV-O012 | Read-only provenance hashes and launch/security/privacy evidence archived per release | P0 |

# 24. Governance, updates and vocabulary

## 24.1 Updating the document

Every proposed change requires a unique decision ID; problem/rationale; alternatives; explicit decision; changed sections/requirements; commercial/privacy/security effect; effective version; approver; and implementation issue. When a decision is superseded, retain its history and replace governing text here; never leave conflicting rules in independent developer notes. Weekly scoping conversations should cover **only truly OPEN questions** and update this file, not restart numbering or re-litigate approved decisions.

## 24.2 Version history

| Version | Date | Meaning |
|---|---|---|
| 1.0 | 14 Nov 2026 | Early product baseline with exploratory architecture and monetisation. |
| 1.1 | 14 Nov 2026 | Ten agreed historical decisions; £9.99/account-based trial model now partly superseded. |
| 2.0 working consolidation | 19 Nov 2026 | First large consolidation, containing outdated open decisions and optional source-file management language. |
| **2.1 reconciled specification** | **19 Nov 2026** | Historical reconciled baseline; source read-only, commercial and delivery requirements. |
| **2.2 controlled working revision** | **20 Nov 2026** | Adds Founder-approved independent Anywhere gate and records independently reviewed engineering safeguards; not proof of implementation or overall formal signoff. |

## 24.3 Glossary

**Host:** Windows background media service. **Client:** native player on another device. **Local:** files on the current device. **LAN:** home network. **Anywhere:** optional authenticated remote streaming from the host. **Library root:** folder selected for indexing. **Media identity:** logical item ID independent of path. **Profile:** personal history, settings and permissions. **Device approval:** owner authorisation of a client. **Cache:** replaceable application data, never explicit downloads. **Direct Play:** play without transcoding. **Remux:** rewrap streams without re-encoding. **Transcode:** re-encode for compatibility. **Lifetime local licence:** perpetual offline entitlement to purchased local features, distinct from remote-service entitlement.

---


# 25. Engineering execution pack — implementation instructions, not new product votes

## 25.1 Engineering non-negotiable invariants

- **INV-01 Source immutability:** library media directories are read-only inputs. No code path writes tags, sidecars, altered timestamps or changed paths there; any newly created/downloaded/converted files use separately designated outputs. Write attempts are prohibited even for the household owner.
- **INV-02 Host-side permissions:** every catalogue, search, artwork, stream, download and profile operation checks an authenticated device plus profile entitlement; client hiding is not authorisation.
- **INV-03 Offline first:** local playback and basic LAN streaming remain functional when licensing and Anywhere cloud services are unavailable.
- **INV-04 User-owned copies:** explicitly downloaded files are not disposable cache and never auto-deleted; conversions use temporary working storage separate from originals.
- **INV-05 Separation of three identities:** user licence, household profile and device credential have distinct data models and lifecycles.
- **INV-06 No covert media cloud:** Anywhere control/routing services may process minimal operational metadata but cannot store customer media or catalogues, or decrypt relayed streams.
- **INV-07 No silent trial loss:** alpha/beta use cannot spend the commercial Premium or Anywhere trial entitlement; trial expiry cannot delete state.
- **INV-08 Conservative child security:** unrated media is invisible to restricted profiles unless owner approved; all access paths including search and art are checked.
- **INV-09 Explicit scans:** all library discovery/rescan work starts only on user instruction, except delayed/resumed phases of the same requested job.
- **INV-10 Truthful platform claims:** nothing is declared commercially supported until it installs normally, plays accepted corpus and passes security/recovery checks on real target hardware.

## 25.2 Implementation work packages in dependency order

| Work package | Concrete deliverable | Definition of done / evidence |
|---|---|---|
| WP-00 Greenfield bootstrap | Create a clean production .NET solution/host skeleton and unit-test project in the private monorepo, pin SDK/dependencies, establish repeatable local build and CI smoke tests, map first requirements, and prepare rights-safe fixtures. Historical prototypes are **not inputs or dependencies**. | First clean build/test logs, initial requirement-to-code trace, documented toolchain and security boundaries; no prototype recovery or reuse signoff. |
| WP-01 Windows service and desktop | Windows Service before sign-in, native WinUI client, one installer/updater, secure library roots and lifecycle controls. | Fresh install on clean Windows user machine; UI closed and Windows locked while host still responds; source immutability test passes. |
| WP-02 Catalogue and stream vertical slice | Explicit scan jobs, playable source IDs, Browse/Folders/search, direct stream/seek, secure device approval. | Android + Fire OS discover approved host with no IP/CLI; denied client cannot enumerate, browse, stream or download. |
| WP-03 Native client playback | Android phone/tablet music + background media, Fire OS TV video + audio/subtitles, Windows native player and current media corpus. | Real-device play, seek, pause, resume, blackout/letterbox, disconnect/reconnect and background tests; no UI WebView. |
| WP-04 Profiles, privacy and persistence | Separate profile state, children/unrated restrictions, durable local SQLite, offline operation queues, correction export/import. | Simultaneous users cannot see each other's private data; backups restore without writing originals; unattended crash/restart recovery. |
| WP-05 Entitlements and downloads | Free/Video Premium/Music Premium gate matrix, beta flags, 30-day trial transitions, authorised local and remote derivative downloads. | Test every entitlement combination; beta→commercial trial preserved; original/hash invariant; user download never treated as cache. |
| WP-06 Anywhere secure access **(separate release gate)** | Consumer automatic connectivity, E2EE direct/relay, separate trial, quality control, owner security and cost instrumentation. | No VPN/router steps; public/mobile/NAT test matrix; relays cannot decrypt; remote revoke closes active stream; egress cost report. Failure blocks **Anywhere**, not ready Music/Video. |
| WP-07 Distribution and staged beta | Signed Windows delivery, official store/private-testing channels, 5–10 person alpha and invitation waves, crash recovery and release checks. | Unassisted installs, signed builds, true store/retail approvals for advertised platforms, documented go/no-go and known limitations. |

**Engineering review incorporation, 20 November 2026:** Split WP-01 into **WP-01a** (minimal service, explicit roots, opaque IDs, default-deny, independently verified QA-001; QA-003 remains NOT STARTED) and **WP-01b** (identity ADR, real host/device handshake and revocation, independent QA-003 plus spoofing/flood tests). No security stub passes QA-003. Protect conformance suites from Codex write access. Define identity/edition/source and per-domain offline sync merge policies **before** WP-02 schema; permissions remain host-authoritative and cannot be revived by offline replay. Begin bounded S-01 player/artwork/audio analysis, S-02 host/service trust and S-04 remote cost modeling alongside WP-00. External spend is **£0 until Founder approval**. See `docs/architecture/Architecture_Review_Decisions_2026-11-20.md` and `docs/engineering/Engineering_Delivery_and_Governance_2026-11-20.md`.

**Founder greenfield instruction (20 November 2026, DEC-2026-11-20-002):** The commercial production implementation is a **fresh codebase**. Historical ChatGPT prototypes are informative only; do not recover, import, audit for reuse, or adapt their source as a precondition for development. No tasks may impose prototype retrieval as a release or coding gate. The old §19 prototype status table records historical observations only.

**Delivery method:** build and prove thin end-to-end slices; do not complete every Windows screen or every speculative feature before testing Android/Fire OS connectivity. Work packages may overlap only where their API contracts and shared tests are explicit. Early parallel proof-of-feasibility remains required for iOS codec fallback, Vega, Xbox Retail, Cast/car integrations and Anywhere relay economics. No expenditure or service subscription should be committed without product-owner approval.

## 25.3 Data ownership and boundaries

| Data domain | Authority | Can be edited by MaggieVision? | Backup/sync policy |
|---|---|---|---|
| Original user media and source library folders | User/OS filesystem | **NO — read only absolutely** | Never copied into cloud; not part of MaggieVision metadata backup. |
| Host catalogue, IDs and library metadata corrections | Windows host database | Yes, catalogue entries only | Export/import and encrypted backup; client cache may mirror authorised fields. |
| Device-local catalogue and offline playlist state | Local device SQLite | Yes, application database only | Sync authorised profile-specific changes when connected; local use independent. |
| Explicit user offline downloads | User-selected destination/OS file area | New copies/derivatives may be created; **never automatic deletion** | Never treated as cache, and never silently uploaded to company cloud. |
| Derived thumbnails, analysis and transcode working files | App-owned cache/temp areas | Yes, safely disposable | Bounded cache, no source-directory write, no backup requirement. |
| Licence account, payment proof and remote routing tokens | Entitlement/identity service | Only minimal necessary account records | Secure retention, recovery and deletion processes; no media titles/listening data. |

## 25.4 Required software contracts before broad feature work

The OpenAPI/source-of-truth design must define typed, versioned operations for: **HostInfo/Capabilities**, **DeviceApproval/Revocation**, **Profile and ACL**, **LibraryRoot and ExplicitScanJob**, **MediaItem and MediaSource**, **MetadataCorrection**, **PlaybackSession/Seek/StreamGrant**, **DownloadGrant/Job**, **SyncOperation/Conflict**, **Entitlement/Trial**, and **AnywhereSession/Revocation**. The exact HTTP routes and table column names are implementation decisions, not additional product choices. Every operation must state its authorisation scope, idempotency, offline retry semantics, error types, observability with redaction and schema migration rule.

The master requirements register must map every WP to stable `MV-` IDs and executable test IDs. Implementation status uses **Not started / Prototype only / In progress / Verified on device / Released**, never the product decision word **AGREED** as a synonym for implemented.

# 26. QA and acceptance scenario catalogue

Use these as the minimum cross-platform release scenarios. Tests must be automated where feasible and supplemented by real-device trials. **Conformance and negative tests are authored independently from the implementation agent and protected against implementation-agent modifications.** Unit tests written by Codex alone do not establish independent acceptance. **Pass** requires logs or reproducible observations, not screenshots of code or unverified AI summaries.

| Test ID | Scenario | Objective pass criteria |
|---|---|---|
| QA-001 | Absolute source read-only | Record content hashes, size, modification time and paths of fixture originals before/after scan, search, playback, tagging correction, backup, downloads, transcoding, error recovery and upgrade; no changes. Deny source write permission and verify no operation needs it. |
| QA-002 | Native clean Windows install | Single signed install with one authorised elevation, service starts before sign-in, app launches from normal shortcut, no console/localhost/browser steps. |
| QA-003 | LAN discovery and trust | Android/Fire TV find host with no IP entry; unapproved device cannot enumerate; owner approves once, then silent reconnect after lock/reboot. |
| QA-004 | Folder fidelity and requested scans | Only user roots indexed, nested folders exact, no silent rescan; external rename shown after explicit Scan for Changes; in-app correction updates catalogue only. |
| QA-005 | Unplugged/disconnected source | Missing library remains visible with cached art/status; reconnect does not implicitly rescan; user can initiate recovery. |
| QA-006 | Standalone Android local music | Play user-picked local files with no host/account; background play, lockscreen, calls and Bluetooth connect/disconnect are correct. |
| QA-007 | Fire OS video long-play corpus | Real Fire TV plays 4K and ordinary 1080p test media, selects audio/subtitle, seeks, resumes and completes without premature idle screensaver; black bars. |
| QA-008 | Local Music download Free | A Free user with Download grant downloads track/album/playlist on LAN and plays offline; no Premium block. |
| QA-009 | Automatic Music sync Premium | Music Premium adds playlist tracks to offline collection on permitted connection; Free cannot enable automation; no silent deletion of previously downloaded files. |
| QA-010 | Video Premium download and formats | Free cannot download hosted video; Video Premium can download Original/Optimised/Save Space; original file hash stays unchanged and file plays offline. |
| QA-011 | Remote entitlement matrix | Music remote manual = Anywhere; Music auto = Music Premium + Anywhere; Video remote = Video Premium + Anywhere; expired trials deny paid operations without breaking local play. |
| QA-012 | Premium trial and beta transfer | Unregistered fresh install gets 30 days from first use; expiry returns to Free preserving data; beta use does not consume post-release 30 days. |
| QA-013 | Anywhere trial | Registered user activates separate 30-day Anywhere trial; no card; expiry stops remote only, never LAN/local or user downloads. |
| QA-014 | Cloud/relay privacy | Inspect backend storage, routes and logs: no source media, catalogue or titles. E2EE media payload unreadable at relay; account/routing metadata limited and redacted. |
| QA-015 | Anywhere owner kill/revoke | Terminating one active remote session and disabling all Anywhere works promptly; concurrent LAN stream continues unaffected. |
| QA-016 | Anywhere quality/network transition | Auto/Original/Data Saver and separate Wi-Fi/mobile preference persist; warnings fire appropriately; 5G/Wi-Fi transitions preserve or recover session safely. |
| QA-017 | Child profile restriction | Unrated item never appears through search, poster, queue or direct stream; approved exception works; restricted profile cannot switch owner unauthorised. |
| QA-018 | Household and host transfer | Multiple profiles and authorised devices on one host without arbitrary device licence caps; transfer household host without buying again; standalone phone Premium unaffected. |
| QA-019 | Metadata export/import | Export corrections/art associations/collections, reinstall to clean profile/host, import safely, flag ambiguous matches; source unchanged. |
| QA-020 | Public beta quality gate | Private-alpha cohort logged; invited beta is withheld until install, security, content access, native playback, update/recovery and diagnostics pass on target hardware. |
| QA-021 | Staged platform claims | Verify each advertised platform has signed/retail install and device-specific regression test; one missing platform does not block other passing platforms. |
| QA-022 | Privacy and diagnostic consent | Problem report contains no media files or private histories by default; user opt-in for any screenshots; exported logs redact credential/path/title data. |

**Evidence retention:** test case ID, build SHA, app/host version, OS/device model, media fixture identifiers, date, expected versus observed, pass/fail, diagnostic reference and reviewer. Major security or source-integrity failure blocks release regardless of other green tests.

# 27. Reconciliation audit: errors removed from v2.0

| Earlier location | Audit finding | Correct v2.1 governing rule |
|---|---|---|
| 5.1 and 21 O-002 | Specific £6.99/£7.99/£16.99 prices still presented as working assumptions | All prices **intentionally deferred**, not eligible for default configuration or marketing. |
| 5.3 and 21 O-004 | Household/device limits described as undecided | One primary host, unlimited authorised household devices subject to physical capacity, transferable host and no PC needed for standalone Premium. |
| 7.2, 8.1, 23 MV-L004/D007 | Optional original-file writes and vague “read-only by default” | Source files **strictly read only** on every platform; in-app metadata edits never reach source files. |
| 10.4, 11.2, 23 | Music/Video and auto-download gates not explicit | Manual Music downloads Free; automatic Music sync Premium; hosted Video downloads and handoff Premium; remote requires Anywhere. |
| 12.2 | Child unrated content policy unspecified | Default hide unrated; owner explicit exceptions, all access paths host enforced. |
| 13.1 and O-003 | Anywhere trial duration marked OPEN | Separate **30-day** activation trial, no card or automatic billing, beta participation does not consume it. |
| 13 and O-005 | Remote cloud handling could be interpreted as storing library metadata | No media or catalogues stored centrally; end-to-end encryption through relays. |
| 13.2 | Remote quality, owner controls, download quality omitted | Auto/Original/Data Saver, separate Wi-Fi/cellular preferences, user-controlled revoke/kill switch; encrypted resumable downloads and three download formats. |
| 20 and O-016 | Simultaneous versus staged release unresolved | Staged platforms; Windows/Android/Fire OS first public beta; high-risk platforms independently gated. |
| 20 | Beta effect on paid trials and tester groups missing | 5–10 alpha, 25–50 per invite wave, later open beta; commercial 30-day trials start after commercial release. |
| 14 and 21 | Portable metadata export/import missing | Explicit export/import of catalogue corrections/collections/artwork with safe relink and no source writes. |
| 21 | Routine engineering matters posed as consumer-facing choices | Marked ENGINEERING GATE or POLICY; don't re-ask owner unless material cost, security, scope or fundamental user-experience trade-off emerges. |

**End of reconciled Product Bible v2.2 working edition.**  
The Founder has explicitly approved **Anywhere release decoupling**, not hereby signed off all other untested architectural choices or the entirety of v2.2. Overall canonical baseline approval, implementation, platform approval and commercial readiness remain separate evidence-backed milestones.