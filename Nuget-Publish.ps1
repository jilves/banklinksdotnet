$projectNames = @('BankLinksDotNet', 'BanklinksDotNet.EstcardProvider', 'BanklinksDotNet.IPizzaProvider')
$configurationNames = @('Nuget 4.5.1')

function Delete-BuildFolders($baseDir)
{
    foreach($folderToDelete in @('bin', 'obj'))
    {
        $absoluteFolderPath = [System.IO.Path]::Combine($baseDir, $folderToDelete)
        Remove-Item $absoluteFolderPath -Force -Recurse
    }
}

foreach($projectName in $projectNames) 
{
    $projectDirectory = [System.IO.Path]::Combine($PSScriptRoot, $projectName)
    Delete-BuildFolders $projectDirectory
}