# Compiles ArkhamKnightStars.exe from Launcher.cs using the C# compiler that ships with Windows (.NET Framework 4).
$ErrorActionPreference = 'Stop'
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) { $csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe' }
& $csc /nologo /target:winexe /optimize+ /out:"$PSScriptRoot\ArkhamKnightStars.exe" /reference:System.Windows.Forms.dll "$PSScriptRoot\Launcher.cs"
if ($LASTEXITCODE -ne 0) { throw "csc failed ($LASTEXITCODE)" }
"Built $PSScriptRoot\ArkhamKnightStars.exe"
