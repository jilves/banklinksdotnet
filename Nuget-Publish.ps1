$projectNames = @('BankLinksDotNet', 'BanklinksDotNet.EstcardProvider', 'BanklinksDotNet.IPizzaProvider')
$configurationNames = @('Nuget 4.5.1')

function Delete-BuildFolders($baseDir)
{
    foreach($folderToDelete in @('bin', 'obj'))
    {
        $absoluteFolderPath = [System.IO.Path]::Combine($baseDir, $folderToDelete)
        if(Test-Path $absoluteFolderPath)
        {
            Remove-Item $absoluteFolderPath -Force -Recurse
        }
    }
}

foreach($projectName in $projectNames) 
{
    $projectDirectory = [System.IO.Path]::Combine($PSScriptRoot, $projectName)
    Delete-BuildFolders $projectDirectory
    # http://stackoverflow.com/questions/4677222/powershell-script-to-build-visual-studio-project
}