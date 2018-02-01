# Get and/or set PowerShell session to only run scripts targeting standaloneapp Wingtip deployment 
$Global:ErrorActionPreference = "Stop"
$scriptsTarget = 'standaloneapp'
if ($Global:WingtipScriptsTarget -and ($Global:WingtipScriptsTarget -ne $scriptsTarget))
{
    throw "This PowerShell session is setup to only run scripts targeting Wingtip '$Global:WingtipScriptsTarget' architecture. Open up a new PowerShell session to run scripts targeting Wingtip '$scriptsTarget' architecture."  
}
elseif (!$Global:WingtipScriptsTarget)
{
    Write-Verbose "Configuring PowerShell session to only run scripts targeting Wingtip '$scriptsTarget' architecture ..."
    Set-Variable WingtipScriptsTarget -option Constant -value $scriptsTarget -scope global
}


<#
.SYNOPSIS
    Returns default configuration values that will be used by the Wingtip Tickets Platform application
#>
function Get-Configuration
{
    $configuration = @{`
        TemplatesLocationUrl = "https://wingtipsaas.blob.core.windows.net/templates-sa"
        DatabaseAndBacpacTemplate = "databaseandbacpactemplate.json"
        BacpacUrl = "https://wingtipsaas.blob.core.windows.net/bacpacs-sa/wingtiptenantdb.bacpac"
        CatalogResourceGroupNameStem = "wingtip-sa-catalog-"
        CatalogTemplateUrl = "https://wingtipsaas.blob.core.windows.net/templates-sa/catalog.json"
        ContosoTemplateUrl = "https://wingtipsaas.blob.core.windows.net/templates-sa/contosoconcerthall.json"
        DogwoodTemplateUrl = "https://wingtipsaas.blob.core.windows.net/templates-sa/dogwooddojo.json"
        FabrikamTemplateUrl = "https://wingtipsaas.blob.core.windows.net/templates-sa/fabrikamjazzclub.json"
        WingtipTicketsAppTemplateUrl = "https://wingtipsaas.blob.core.windows.net/templates-sa/wingtipticketsapp.json"
        CatalogDatabaseName = "tenantcatalog"
        CatalogServerNameStem = "catalog-sa-"
        TenantServerNameStem = "tenants1-"
        CatalogShardMapName = "tenantcatalog"
        CatalogAdminUserName = "developer"
        CatalogAdminPassword = "P@ssword1"
        TenantAdminUserName = "developer"
        TenantAdminPassword = "P@ssword1"
        DefaultVenueType = "multipurpose"
        }
    return $configuration
}
