from pathlib import Path
import csv

root = Path(__file__).resolve().parent.parent
files = [
    "docs/product/MaggieVision_Product_Bible_v2.1_2026-11-19.md",
    "docs/decisions/MaggieVision_UI_Addendum_2026-11-20.md",
    "docs/engineering/MaggieVision_WP00_Execution_Brief_2026-11-20.md",
    "docs/engineering/Prototype_Recovery_Register.md",
]
missing = [p for p in files if not (root / p).is_file()]
assert not missing, f"Missing documentation: {missing}"
assert "# MAGGIEVISION" in (root / files[0]).read_text(encoding="utf-8")
with (root / "docs/engineering/Requirements_Trace_Template.csv").open(encoding="utf-8", newline="") as handle:
    columns = next(csv.reader(handle))
assert {"requirement_id", "status", "evidence"} <= set(columns)
print("Documentation baseline PASS — no MaggieVision build or device tests run.")
