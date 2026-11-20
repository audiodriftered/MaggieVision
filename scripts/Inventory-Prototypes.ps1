param([Parameter(Mandatory = $true)][string]$SourceDirectory)
$ErrorActionPreference = "Stop"
$source = (Resolve-Path -LiteralPath $SourceDirectory).Path
$repo = Split-Path -Parent $PSScriptRoot
$reportDir = Join-Path $repo "recovery/reports"
New-Item -ItemType Directory -Force -Path $reportDir | Out-Null
$files = Get-ChildItem -LiteralPath $source -Recurse -File -ErrorAction Stop
$rows = foreach ($file in $files) {
    if ($file.Extension -notin @(".zip",".sln",".slnx",".csproj",".gradle",".kts",".mod",".toml",".json",".ps1")) { continue }
    [pscustomobject]@{ Path=$file.FullName; Bytes=$file.Length; SHA256=(Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash }
}
$out = Join-Path $reportDir "prototype-source-inventory.csv"
$rows | Export-Csv -LiteralPath $out -NoTypeInformation -Encoding UTF8
Write-Host "Source files untouched. Inventory saved to $out"
