# Helper script for provisioning tenants and their databases in the standalone app or app-per-tenant model.

# IMPORTANT: 
#  - Ensure that at least the Contoso Concert Hall application has been deployed  
#  - Ensure that the SAME USER NAME was used for Contoso Concert Hall and all other sample tenants deployed  
#  - Before provisioning additional tenants deploy and initialize the catalog using scenario 1

# Parameters for scenarios #2, provision a single tenant 
$TenantName = "Red Maple Racing" # name of the venue to be added/removed as a tenant
$VenueType  = "motorracing"      # valid types: blues, classicalmusic, dance, jazz, judo, motorracing, multipurpose, opera, rockmusic, soccer 
$PostalCode = "98052"
$countryCode = "USA"

$Scenario = 2
<# Select the scenario to run
    Scenario
      1       Provision the tenant catalog and register the sample tenants  
      2       Provision a single tenant (requires the tenant catalog is provisioned first)
#>

## ------------------------------------------------------------------------------------------------

Import-Module "$PSScriptRoot\..\Common\CatalogAndDatabaseManagement" -Force
Import-Module "$PSScriptRoot\..\Common\SubscriptionManagement" -Force
Import-Module "$PSScriptRoot\..\WtpConfig" -Force
Import-Module "$PSScriptRoot\..\UserConfig" -Force

# Get Azure credentials if not already logged on,  Use -Force to select a different subscription 
Initialize-Subscription -NoEcho

# Get the user value used when the WTP application was deployed from UserConfig.psm1.  
$wtpUser = Get-UserConfig

# Get the WTP app configuration
$config = Get-Configuration


## Provision the Catalog then register the sample tenant databases
if ($Scenario -eq 1)
{
    & $PSScriptRoot\New-Catalog.ps1  
    exit
}    

## Provision a standalone tenant app with database
if ($Scenario -eq 2)
{
    & $PSScriptRoot\New-TenantApp.ps1 `
       -TenantName $TenantName `
       -VenueType $VenueType `
       -PostalCode "98052" `
       -CountryCode USA
    
    # Open the events page for the new venue
    $normalizedTenantName = Get-NormalizedTenantName -$TenantName
    Start-Process "http://events.$normalizedTenantName.$($wtpUser.Name).trafficmanager.net"
    
    exit
}

Write-Output "Invalid scenario selected"              