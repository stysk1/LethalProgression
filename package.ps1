#Requires -Version 5
<#
.SYNOPSIS
    Build the mod and assemble a Thunderstore-format zip in ./dist for sharing or upload.
.DESCRIPTION
    Produces dist/<name>-<version>.zip (name + version from manifest.json), flat layout:
    manifest.json, icon.png, README.md, CHANGELOG.md, LethalProgression.dll.
    The skillmenu AssetBundle is embedded in the DLL — no separate bundle file needed.
.EXAMPLE
    ./package.ps1
#>
[CmdletBinding()]
param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot

$manifest = Get-Content (Join-Path $root "manifest.json") -Raw | ConvertFrom-Json
$name    = $manifest.name
$version = $manifest.version_number

Write-Host "Packaging $name v$version..." -ForegroundColor Cyan

Write-Host "Restoring tools..." -ForegroundColor Cyan
dotnet tool restore
if ($LASTEXITCODE -ne 0) { throw "dotnet tool restore failed." }

Write-Host "Building ($Configuration)..." -ForegroundColor Cyan
dotnet build "$root\LethalProgression\LethalProgression.csproj" -c $Configuration
if ($LASTEXITCODE -ne 0) { throw "Build failed." }

$outDir = Join-Path $root "LethalProgression\bin\$Configuration\netstandard2.1"
$dll    = Join-Path $outDir "LethalProgression.dll"
if (-not (Test-Path $dll)) { throw "Build output not found: $dll" }

$dist    = Join-Path $root "dist"
$staging = Join-Path $dist "_staging"
if (Test-Path $staging) { Remove-Item $staging -Recurse -Force }
New-Item -ItemType Directory -Force -Path $staging | Out-Null

Copy-Item (Join-Path $root "manifest.json")  (Join-Path $staging "manifest.json")  -Force
Copy-Item (Join-Path $root "icon.png")        (Join-Path $staging "icon.png")        -Force
Copy-Item (Join-Path $root "README.md")       (Join-Path $staging "README.md")       -Force
Copy-Item (Join-Path $root "CHANGELOG.md")    (Join-Path $staging "CHANGELOG.md")    -Force
Copy-Item $dll                                (Join-Path $staging "LethalProgression.dll") -Force

$zip = Join-Path $dist "$name-$version.zip"
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path (Join-Path $staging "*") -DestinationPath $zip -Force
Remove-Item $staging -Recurse -Force

Write-Host "Packaged: $zip" -ForegroundColor Green
