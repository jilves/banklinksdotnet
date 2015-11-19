param ( 
	[Parameter(Mandatory=$true,Position=1)]
    [string]
	$NuGetAPIKey,
    [Parameter(Mandatory=$false, Position=2)]
    [string]
    $MsbuildExe = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe",
    [Parameter(Mandatory=$false, Position=3)]
    [string]
    # Assume nuget.exe is in Path variable
    $NugetExe = "nuget"
) 


$projectNames = @('BankLinksDotNet', 'BanklinksDotNet.EstcardProvider', 'BanklinksDotNet.IPizzaProvider')


$liveProjectconfigurationNames = @("Nuget 4.5.1")

function Delete-BuildFolders($baseDir)
{
    foreach($folderToDelete in @("bin", "obj"))
    {
        $absoluteFolderPath = [System.IO.Path]::Combine($baseDir, $folderToDelete)
        if(Test-Path $absoluteFolderPath)
        {
            Remove-Item $absoluteFolderPath -Force -Recurse
        }
    }
}

Get-ChildItem -Path $PSScriptRoot -Filter "*.nupkg" | Remove-Item

foreach($projectName in $projectNames) 
{
    $projectDirectory = [System.IO.Path]::Combine($PSScriptRoot, $projectName)

    Delete-BuildFolders $projectDirectory

    foreach($configName in $liveProjectconfigurationNames) 
    {
        $csProjFileFullPath = [System.IO.Path]::Combine($projectDirectory, $projectName + ".csproj")

        & $MsbuildExe @($csProjFileFullPath, "/p:Configuration=$configName")

        & $NugetExe @("pack", $csProjFileFullPath, "-Properties", "Configuration=$configName")

        # We don't nuget push just yet, all project builds should succeed before proceeding
    }
}

$projectNames | %{ 
    $nuspecFile = Get-ChildItem -Path $PSScriptRoot -Filter "$_.1.*.nupkg" | Select-Object -First 1
    $shouldProceed = Read-Host "Attempting to push: '$nuspecFile', Enter 'Yes' to proceed."
    if($shouldProceed -ne 'Yes') {
        Exit
    }

    & $NugetExe @("push", $nuspecFile, $NuGetAPIKey)
}
