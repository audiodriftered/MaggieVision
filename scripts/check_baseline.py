from pathlib import Path
import csv

root = Path(__file__).resolve().parent.parent
required = [
    "docs/product/MaggieVision_Product_Bible_v2.2_2026-11-20.md",
    "docs/decisions/MaggieVision_UI_Addendum_2026-11-20.md",
    "docs/engineering/MaggieVision_WP00_Execution_Brief_2026-11-20.md",
    "docs/engineering/Prototype_Recovery_Register.md",
    "docs/engineering/Engineering_Delivery_and_Governance_2026-11-20.md",
    "docs/architecture/Architecture_Review_Decisions_2026-11-20.md",
    "docs/decisions/DEC-2026-11-20-001_Anywhere_Independent_Release.md",
]
missing = [p for p in required if not (root / p).is_file()]
assert not missing, f"Missing baseline documents: {missing}"
assert "# MAGGIEVISION" in (root / required[0]).read_text(encoding="utf-8")
with (root / "docs/engineering/Requirements_Trace_Template.csv").open(
    encoding="utf-8", newline=""
) as handle:
    headers = next(csv.reader(handle))
assert {"requirement_id", "status", "evidence"} <= set(headers)
print("Documentation baseline checks PASS. NOTE: no product source build/test was performed.")