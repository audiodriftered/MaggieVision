# MaggieVision — WP-00 Controlled Engineering Baseline

**Input authority:** MaggieVision Product Bible v2.2 (20 Nov 2026), preserving v2.1 as historical baseline, §19–§26, plus *UI and Navigation Decision Addendum* (20 Nov 2026).  
**Purpose:** Move from discussion to reproducible, evidence-backed engineering without rebuilding already working prototypes or inventing new product policy.

## Mission

Produce a factual inventory of the existing MaggieVision development assets: Windows host/server and native desktop UI, video/music player components, Android tablet/phone and Fire TV clients, Apple/Xbox experiments if present, MaggieReads/MaggieNarrator labs, build scripts, test fixtures, manifests, API contracts, dependencies and release artifacts.

**WP-00 is inventory and verification work.** It does not authorise an arbitrary framework migration or rewrite. Never declare a platform “working” because its source directory exists.

## Required evidence outputs

1. **Repository map:** repository URL/path, commit SHA, branch, language/toolchain, component role, ownership and working-tree status for every discovered project.
2. **Build matrix:** commands, installed toolchains, dependency versions, reproducible compile/pass/fail outcome and exact failure log reference; identify what can actually build on the current machine.
3. **Prototype matrix:** host, Windows native UI, Android local music, Fire TV video, Apple/Xbox, books narration — evidenced features vs untested claims, defects and missing files.
4. **Playback/test-device inventory:** actual hardware/OS, tested codecs/subtitles/audio outputs, LAN discovery results, build identity and observed failures.
5. **Requirements trace:** link existing modules/tests to governing `MV-*` IDs and addendum `MV-UI*` IDs; classify each as *Not started / Prototype only / In progress / Verified on device / Released*.
6. **Risk register:** critical gaps blocking native Windows service/UI, network security, source-read-only guarantee, device approval, signed install and stable audio/video playback.
7. **Next implementation slice:** one small, demonstrable WP-01/WP-02 vertical slice with acceptance criteria and rollback plan, selected from actual code evidence.

## Non-negotiable checks

- Original media libraries are strictly read-only. Indexing and metadata edits must never write into them.
- No automatic rescans; only explicit user scan jobs.
- Windows client is truly native, not a localhost/WebView UI. The Windows service continues independently of the UI after sign-out/lock when implemented.
- Android/Fire TV ordinary users must never need command prompts, IP addresses, ADB, pairing codes or Xbox Developer Mode.
- Local media continues without host/cloud; device approval is enforced server-side for shared libraries.
- Any claimed bit-perfect status is demonstrated with the actual processing/output path, not a decorative label.
- Product Bible's commercial scope remains Music/Video 1.0; Podcasts RSS, MaggieReads and Pictures are not launch blockers.

## Coding-agent execution rules

- First inspect, build, test and report; **do not** edit source, install paid services or migrate frameworks until the evidence inventory is reviewed.
- Preserve current known-good build artifacts and tag/identify reproducible baselines before development.
- Prefer focused end-to-end working slices; changes require compile checks, unit/integration tests, regression coverage and a rollback path.
- Record actual command output, environment and build SHA. Do not mark a task verified solely from code review or an AI narrative.
- Any conflict with a settled product rule must be escalated as an engineering blocker rather than silently substituting a new product decision.

## WP-00 completion criteria

The developer can identify exactly which components exist, reproduce their builds (or document precise blockers), distinguish demonstrated from hypothesised capabilities, and propose the first code change tied to an explicit `MV-*` acceptance criterion. Only then is WP-00 closed.

## Independent architecture review — execution corrections (20 November 2026)

WP-00 remains source recovery and evidence capture. In parallel, run bounded **S-01** native composition/PCM output, **S-02** device/host trust and signed-out service file access, and **S-04** local network-emulation/relay cost modeling. S-04 external expenditure is **£0 until Founder approval**.

**WP-01a:** assign agent writes only `src/host/` and `tests/unit/host/` (not `tests/conformance/`). Build minimum service, explicit-root scan, opaque IDs, strict read-only source protection and deny-all without approved device. Independent QA-001; QA-003 remains **NOT STARTED**.

**WP-01b:** first review ADR-001 (transcript-authenticated key exchange, host key pinning, one-tap match without typed code); implement **real** approval and revocation; run independently authored QA-003 and spoofed-host/approval-flood tests. A stub never passes QA-003.

Before schema: ADR-002 for least-privilege signed-out access to user folders and SMB/NAS, with **no ACL exception preapproved**; ADR-003 for stable media/edition/source identity and domain-specific sync conflict rules, then clocks. Revocation is host authoritative.

Work follows the [controlled governance document](../engineering/Engineering_Delivery_and_Governance_2026-11-20.md). Founder retains product, spend, ACL-exception and release authority; protected conformance testing remains outside implementation-agent write scope.