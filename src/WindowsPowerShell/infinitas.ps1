Import-Module ScreenResolution

function Get-CurrentResolution {
    # current values
    [int]$currentFrequency = Get-CurrentRefreshRate
    [int]$currentWidth = Get-CurrentHorizontalResolution
    [int]$currentHeight = Get-CurrentVerticalResolution

    # adjust
    if ($currentFrequency -eq 239) {
        $currentFrequency = 240
    }
    if ($currentFrequency -eq 119) {
        $currentFrequency = 120
    }

    return [System.Tuple]::Create(
        $currentWidth,
        $currentHeight,
        $currentFrequency
    )
}

# infinitas values
[int]$infinitasFrequency = 120
[int]$infinitasWidth = 1280
[int]$infinitasHeight = 720

$infinitas = [System.Tuple]::Create(
    $infinitasWidth,
    $infinitasHeight,
    $infinitasFrequency
)

# default values
[int]$defaultFrequency = 240
[int]$defaultWidth = 1920
[int]$defaultHeight = 1080

$default = [System.Tuple]::Create(
    $defaultWidth,
    $defaultHeight,
    $defaultFrequency
)

function Main {
    Write-Host "Start"

    $current = Get-CurrentResolution
    Write-Host "Current Resolution: ${current}"

    # change resolution
    if (!$current.Equals($infinitas)) {
        Write-Host "Change Resolution: ${infinitas}"
        Set-ScreenResolution $infinitasWidth $infinitasHeight $infinitasFrequency
    }

    # open infinitas
    Write-Host "Open beatmania IIDX INFINITAS"
    & 'C:\Users\Public\Desktop\beatmania IIDX INFINITAS.url'

    # process name
    [string]$name = "bm2dx"

    # wait for start process
    Write-Host -NoNewline "Wait For Start Process: ${name}"
    while ($true) {
        $count = (Get-Process -ErrorAction 0 $name).Count
        if ($count -eq 1) {
            break
        }
        # sleep 10s
        Start-Sleep -s 10
        Write-Host -NoNewline "."
    }
    Write-Host "`r`nDetect Start Process: ${name}"

    # wait for finish process
    Write-Host "Wait For Finish Process: ${name}"
    Wait-Process -Name $name
    Write-Host "Detect Finish Process: ${name}"

    $current = Get-CurrentResolution
    if (!$current.Equals($default)) {
        Write-Host "Change Resolution: ${default}"
        Set-ScreenResolution $defaultWidth $defaultHeight $defaultFrequency
    }

    Write-Host "Finish"
}

Main