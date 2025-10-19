$ErrorActionPreference = "Stop"

$unitySource = "C:\Data\Personal\Documents\C#\Mod\KoikatsuModdingTools\Assets\IngameDebugConsole"

$centralRepo = "C:\Data\Personal\Documents\C#\Mod\IngameDebugConsole\"

$destination = Join-Path $centralRepo "UnityProject"

Write-Host "Syncing Unity project from: $unitySource"
Write-Host "To: $destination"
Write-Host ""

# Create destination folder if it doesn't exist
if (-not (Test-Path $destination)) {
    New-Item -ItemType Directory -Path $destination | Out-Null
}

# Copy all contents (not the top folder itself)
robocopy "$unitySource" "$destination" /E /XD ".git" "Library" "Temp" /NFL /NDL /NJH /NJS | Out-Null

Write-Host "Unity project synced"