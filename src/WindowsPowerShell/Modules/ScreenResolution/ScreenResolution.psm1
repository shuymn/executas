Function Set-ScreenResolution { 
    param ( 
        [Parameter(Mandatory = $true, Position = 0)] 
        [int] $width, 
     
        [Parameter(Mandatory = $true, Position = 1)] 
        [int] $height,

        [Parameter(Mandatory = $true, Position = 2)]
        [int] $frequency
    ) 
     
    Add-Type -Path "$HOME\Documents\WindowsPowerShell\Modules\ScreenResolution\resolution.cs" -ErrorAction SilentlyContinue 
    [Resolution.PrimaryScreenResolution]::ChangeResolution($width, $height, $frequency) 
} 

function Get-CurrentRefreshRate {
    $frequency = Get-CimInstance Win32_VideoController | Select-Object -ExpandProperty CurrentRefreshRate
    return [int]$frequency
}

function Get-CurrentHorizontalResolution {
    $width = Get-CimInstance Win32_VideoController | Select-Object -ExpandProperty CurrentHorizontalResolution
    return [int]$width
}

function Get-CurrentVerticalResolution {
    $height = Get-CimInstance Win32_VideoController | Select-Object -ExpandProperty CurrentVerticalResolution
    return [int]$height
}