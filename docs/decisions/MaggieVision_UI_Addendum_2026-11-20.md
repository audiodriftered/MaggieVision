# MaggieVision — UI and Navigation Decision Addendum

**Working addendum to:** *MaggieVision Comprehensive Product Bible, reconciled engineering edition v2.1, 19 November 2026*  
**Addendum date:** 20 November 2026  
**Status:** Reconciliation of decisions agreed during subsequent UI discussions. Prepared for incorporation into the next controlled revision; this document is not a replacement for the Product Bible or proof of implementation.

## 1. Change-control rules

1. Preserve the entirety of v2.1's established product, security, library, licensing, platform and release decisions unless an explicit later decision supersedes a specific provision.
2. Do not renumber or repurpose existing `MV-` requirement IDs, QA IDs or work packages. Assign fresh IDs only to genuinely additional behaviours.
3. Distinguish **approved UI direction**, **implementation target**, **optional refinement** and **deferred scope**. User agreement on a screen design does not imply that its module ships in commercial Music/Video 1.0.
4. No native Windows browser, localhost website or WebView wrapper. No UI feature may modify original media, trigger an unsolicited library scan, bypass profile permissions or require a cloud service for local playback.
5. Do not interpret this addendum as authorisation to change production source code or update the original Bible in-place before controlled review.

## 2. Decisions to incorporate

| New decision ID | Status | Governing UI behaviour | Product Bible mapping |
|---|---|---|---|
| UI-2026-01 | AGREED | The sidebar is **one dynamic, collapsible navigation list** for the currently active module, not permanently split into a list of modules plus a separate sub-menu. | §6.1, §6.2; MV-C002, MV-C003 |
| UI-2026-02 | AGREED | Built-in and user-added navigation entries have **equal placement** in that single list. User-added `Documentaries` is immediately alongside `Films` and `TV Programmes`, not inside a `My Sections` group. | §6.1; new MV-UI001 |
| UI-2026-03 | AGREED | Users can add, reorder, rename, hide and remove menu shortcuts. Removing a shortcut must **never delete media, a playlist or an underlying collection**. Original folder hierarchy remains independently browsable. | §6.1, §7.2, §8.1; new MV-UI002 |
| UI-2026-04 | AGREED | Custom sidebar destinations may represent a built-in page, saved content filter, pinned folder, playlist or collection. **A sidebar destination is not itself a collection**. No implicit movement/duplication of source files. | §6.1, §8, §9, §10; new MV-UI003 |
| UI-2026-05 | AGREED | **Favourites** is a navigable destination in each enabled module, with module-appropriate content and profile-specific state. Do not conflate Favourites with Video Watchlist, Music Playlists or Books Reading List. | §6.3, §9.1, §10, §14.1; new MV-UI004 |
| UI-2026-06 | AGREED FOR INFORMATION ARCHITECTURE | Long-term section set is **Video, Music, Podcasts, Books/MaggieReads and Pictures**. Each can have its own navigation and Home view. Modules that are not enabled/installed must not clutter the interface. | §4, §6, §17, §18; new MV-UI005 |
| UI-2026-07 | AGREED | Music has a dedicated **Now Playing** sidebar destination in addition to a compact persistent mini-player for browsing elsewhere. Both bind to **one playback state and queue**, not two independent players. | §6.1, §9.2–9.3; new MV-UI006 |
| UI-2026-08 | AGREED VISUAL DIRECTION | On Windows Music → Now Playing, **blended album artwork fills the entire content page**, excluding permanent application navigation. It is not a split cover-art/control-panel layout and should not have a fixed top-screen toolbar in the default immersive state. | §6.2, §9.3; new MV-UI007 |
| UI-2026-09 | AGREED VISUAL DIRECTION | Track information, playback controls, progress, ratings/favourite actions, audio details and a live visualiser **float over the artwork** on legible semi-transparent surfaces. Palette is derived from the current artwork; the approved Dire Straits example is a *quality/mood reference*, **not** a fixed colour scheme for every album. | §6.2, §9.3; new MV-UI008 |
| UI-2026-10 | AGREED INTERACTION DIRECTION | Queue, equaliser, lyrics and detailed information use **on-demand translucent overlays/widgets**, rather than a permanently visible right-hand panel. The queue may appear as a temporary preview on tap/click; its expanded view remains usable during interaction. | §9.2–9.4; new MV-UI009 |
| UI-2026-11 | AGREED | Equaliser icon opens a floating interactive **10-band EQ widget** with presets and live audio-driven visual feedback. Actual EQ processing must be bypassable; bit-perfect output must never be misrepresented while DSP changes samples. | §9.3; existing MV-M009/MV-M010 plus new MV-UI010 |
| UI-2026-12 | AGREED | Lyrics overlay initially uses **embedded and local `.lrc` lyrics** already in the Bible. The overlay does not create an obligation to license internet-sourced lyrics for 1.0. | §9.4, §22; existing MV-M011 |

### Practical interpretation of the visual reference

The approved image shows the default, immersive Music Now Playing experience **without** queue, EQ or lyrics widgets open. There is no requirement to copy Poweramp skins or reproduce its protected artwork. The desired behaviour is a native full-content-area artwork blend with transparent controls and a genuine responsive visualiser. Windows primary navigation remains available outside the art-driven content region.

### Details to implement as adjustable defaults, not additional founder votes

- Animation transition duration; blur strength; contrast management; visualiser geometry; overlay opacity.
- Queue-preview inactivity timeout (five seconds was a conversational suggestion, **not a hard safety or usability constraint**). Never auto-dismiss an overlay while it has active focus, is being dragged or is being adjusted.
- Precise location of the permanent module switcher and platform-specific layout adaptation. The user approved a dynamic sidebar and artwork-only Now Playing content area; the specific top-bar arrangement should not be reintroduced as a settled requirement.
- Widget repositioning/resizing and multiple simultaneously open widgets are **optional later refinements**, not initial implementation gates.

## 3. Release scope and supersessions — explicit reconciliation

**No change to commercial 1.0 scope:** MaggieMusic and MaggieVideo remain the first commercially delivered modules, with Windows/Android/Fire OS first public beta priority. This follows v2.1 §4 and §20. Designing Podcasts, Books and Pictures navigation does **not** make them dependencies for Music/Video 1.0.

**Podcasts:** A *future standalone Podcast module* has been explored and its initial navigation sketched. This does **not** override v2.1 §9.2/§9.5, which includes **local podcasts and spoken-word playback in Music Free 1.0** but explicitly defers online podcast-directory/RSS subscriptions.

**MaggieReads:** Long-term Books, Comics, Audiobooks and offline MaggieNarrator navigation has been elaborated; v2.1 §17 remains the governing scope. Prior narration experiments are R&D, not release certification.

**Pictures:** Long-term Gallery, Albums, Timeline, Favourites, Slideshows and Folders navigation has been elaborated; §18 still places MaggiePictures outside Music/Video v1.0 release gates.

**Video:** The new flat configurable sidebar adds to existing Films/TV/Personal Videos/Collections/Watchlist/Folders requirements. Keep all existing landscape 16:9 cards, personal-video treatment and permissions. **Do not omit Personal Videos** from the product merely because an illustrative sidebar omitted it.

**Music:** The new Now Playing experience is a visual/navigation addition. Existing specifications for queue durability, gapless, audio focus, EQ, output profiles, local `.lrc`, local+host catalogue and bit-perfect remain binding.

**Security:** A user-pinned destination is a *view*, not a capability grant; host-side ACLs apply to direct navigation, search, artwork and streams. A missing or unauthorised source must not leak metadata.

## 4. Proposed append-only requirement catalogue

These IDs are addenda; they do not claim implementation status or supersede existing `MV-*` IDs.

| ID | Requirement | Proposed priority | Key evidence |
|---|---|---|---|
| MV-UI001 | Module-scoped, dynamic, collapsible, unified navigation list; no separate custom item grouping | P0 Music/Video | Native Windows list switches correctly with active module; collapse does not alter selection |
| MV-UI002 | Editable menu ordering/labels/visibility and safe shortcut removal | P1 Music/Video | Reorder, relaunch, restore; underlying destination and source media unchanged |
| MV-UI003 | Typed sidebar destinations for built-in routes, filters, folders, playlists and collections | P1 Music/Video | Deep-link/navigation tests; permissions and missing targets handled safely |
| MV-UI004 | Favourites destination in every installed module, private per profile | P1 launch modules; future module-specific gates | Add/remove favourite, sync/relaunch, no cross-profile leak |
| MV-UI005 | Additional Podcasts/MaggieReads/Pictures menu definitions configurable without enabling deferred modules | P2 | Disabled modules invisible; enabling later preserves user state |
| MV-UI006 | Dedicated Music Now Playing route and shared mini-player playback state | P1 Music | Browsing does not interrupt playback; no duplicate playback session |
| MV-UI007 | Full-content-area album-art blend, leaving only permanent navigation outside; native rendering | P1 Music Windows | Resizes; correct artwork-derived palette; readability and reduced-motion fallback |
| MV-UI008 | Semi-transparent default player controls with real audio-derived spectrum when enabled | P1 Music Windows | Actual audio drives visualiser; low-resource/reduced-motion mode; no playback interruption |
| MV-UI009 | Queue, EQ, lyrics and info overlays with usable focus/dismissal rules | P1 Music Windows | Keyboard/mouse/touch focus; queue editable; overlays never prematurely disappear during interaction |
| MV-UI010 | Floating EQ widget linked to the existing DSP path, with truthful EQ bypass/bit-perfect status | P1 Music Windows | Audible gain change in Standard mode; no modified output presented as bit-perfect |

**Traceability note:** Link MV-UI006/009 to existing MV-M005 (queue), MV-UI010 to MV-M009/M010 (EQ/fidelity), and lyrics to MV-M011. Avoid duplicate engines or parallel queue state machines.

## 5. Suggested append-only QA scenarios

| New QA ID | Scenario | Pass criterion |
|---|---|---|
| QA-023 | Custom sidebar persistence and safety | Add/move/hide/rename a user destination; relaunch; configuration intact; no source file mutation; no unwanted rescan |
| QA-024 | Direct custom view permissions | A restricted profile cannot browse a pinned folder/filter/collection beyond authorised media, including art and search |
| QA-025 | Now Playing state continuity | Switch Music ↔ Video/Pictures (when enabled), minimise and restore; Now Playing and mini-player show consistent session/queue; expected audio-focus rules hold |
| QA-026 | Artwork blending and accessibility | Test high-contrast/low-contrast artwork, different monitor sizes, reduced motion and screen readers; playback stays smooth and controls readable |
| QA-027 | Overlay interactions | Queue does not dismiss during drag/keyboard focus; Escape/back closes appropriately; EQ widget updates safely and respects bypass |
| QA-028 | Scope regression | Deferred standalone Podcast RSS, MaggieReads and Pictures are absent from Music/Video 1.0 launch dependency graph unless explicitly re-scoped |

## 6. Engineering insertion points

- **WP-00:** Attach this addendum to the existing baseline; inventory actual UI source and screenshots. Mark code features `not started / prototype only / in progress / verified on device` separately from owner agreement. Do not modify source before collecting a reproducible build.
- **WP-01:** Implement the native navigation shell and typed route/configuration state with Music/Video first, modular extension points for later sections.
- **WP-03:** Implement native Music Now Playing and mini-player against the actual playback engine; prototype artwork composition and analyser with measured CPU/GPU cost.
- **WP-04:** Persist per-profile navigation preferences and favourites and reconcile safely offline.
- **WP-07:** Add QA-023..028 to acceptance checks and verify accessibility and performance on real hardware.

## 7. Immediate handoff

**Next work is WP-00, not another product design workshop.** Retrieve the existing MaggieVision codebases and built prototypes, identify precise working commits, inventory music/video playback, Windows shell, Android/Fire TV clients, narration lab and tests, then output an evidence matrix. Only then open WP-01 engineering changes. The approved Now Playing screenshot is a visual acceptance reference; it is not a source-code implementation.

**Change-control status:** This is a working addendum intended for founder review and controlled integration into the next Product Bible version. No unresolved item above justifies reopening the old decisions or changing the agreed business/release baseline.