#Requires -Version 5.1
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

Import-Module (Join-Path $PSScriptRoot '..\..\Contensive5\scripts\contensive-build.psm1') -Force

$projectRoot = (Resolve-Path "$PSScriptRoot\..").Path

Invoke-ContensiveBuild `
    -CollectionName    'aoRss' `
    -CollectionPath    "$projectRoot\Collections\aoRss" `
    -SolutionPath      "$projectRoot\Server\aoRss.sln" `
    -BinPath           "$projectRoot\Server\aoRss\bin\Release\netstandard2.0" `
    -DeploymentRoot    'C:\Deployments\aoRSS' `
    -CleanFolders      @(
                           "$projectRoot\Server\aoRss\bin"
                           "$projectRoot\Server\aoRss\obj"
                       ) `
    -UiPath            "$projectRoot\ui"
