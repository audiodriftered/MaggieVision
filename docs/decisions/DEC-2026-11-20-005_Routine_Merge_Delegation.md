# DEC-2026-11-20-005 — Routine merges delegated

**Status:** Founder-approved standing operating decision, 20 November 2026. **Class:** Reserved (governance). This replaces the prior rule requiring the Founder to manually merge every PR.

The Founder does not review or click merge for ordinary safe changes. Following real applicable build/test CI, objective scoped-diff review and recorded evidence, the VCTO or authorised automation may merge Routine PRs. Codex cannot approve/merge its own PR.

Boundary changes to auth, crypto, ACLs, downloads, migrations, installer/updater, source integrity, remote transport, `.github/`, governing charters and protected acceptance require independent verification and any Founder approval for Material/Reserved decisions. Once approved, the authorised maintainer performs the merge mechanically. Expenditure, release and policy/security exceptions stay Founder-reserved.

For the already-reviewed WP-00 inert bootstrap, the Founder directed removing the merge-click bottleneck, and PR #4 was squash-merged after VCTO source review and examination of actual local build/test logs; GitHub CI was documentation-only, and that limitation stays explicit. This bootstrap precedent does **not** count as permission to bypass real .NET CI on subsequent executable changes.

**Enforcement status:** Actual Codex credential restrictions and build CI remain UNVERIFIED; policy language is not proof of technical enforcement.
