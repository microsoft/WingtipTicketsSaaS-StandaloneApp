<#
.SYNOPSIS
  Provisions a new tenant app and database and registers it in the catalog   
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$TenantName,

    [Parameter(Mandatory=$false)]
    [string]$VenueType = "multipurpose",

    [Parameter(Mandatory=$false)]
    [string]$PostalCode = "98052",

    [Parameter(Mandatory=$false)]
    [string]$countryCode = "USA"

)

Import-Module "$PSScriptRoot\..\Common\CatalogAndDatabaseManagement" -Force
Import-Module "$PSScriptRoot\..\Common\SubscriptionManagement" -Force


# Get Azure credentials if not already logged on,  Use -Force to select a different subscription 
Initialize-Subscription

# Get the resource group and user names used when the WTP application was deployed from UserConfig.psm1.  
$wtpUser = Get-UserConfig

# Get the WTP app configuration
$config = Get-Configuration

# verify the catalog database is deployed 
$catalogResourceGroupName = $config.CatalogResourceGroupNameStem + $wtpUser.Name
$catalogServerName = $config.CatalogServerNameStem + $wtpuser.Name 
$catalogDatabase = Get-AzureRmSqlDatabase `
    -ResourceGroupName $catalogResourceGroupName `
    -ServerName $catalogServerName `
    -DatabaseName $config.CatalogDatabaseName `
    -ErrorAction SilentlyContinue

if (!$catalogDatabase)
{
    throw "The tenant catalog is required for this action.  Provision the catalog and then try again."
    exit
}
     
# Get the location for the Contoso Concert Hall deployment (sample app assumes all tenants are deployed to same location)
$location = (Find-AzureRmResource -ResourceNameContains "ContosoConcertHall" -ResourceType Microsoft.Sql/Servers)[0].Location 
    
$normalizedTenantName = (Get-NormalizedTenantName -TenantName $TenantName)
$tenantResourceGroupName = 'wingtip-sa-' + $normalizedTenantName + '-' + $wtpUser.Name
$tenantServerName = $normalizedTenantName + '-' + $wtpUser.Name

# Compute the tenant key from the tenant name, key to be used to register the tenant in the catalog 
$tenantKey = Get-TenantKey -TenantName $TenantName 

# Get the catalog 
$catalog = Get-Catalog -ResourceGroupName $catalogResourceGroupName -WtpUser $wtpUser.Name

# Check if a tenant with this key is aleady registered in the catalog
if (Test-TenantKeyInCatalog -Catalog $catalog -TenantKey $tenantKey)
{
    throw "A tenant with name '$TenantName' is already registered in the catalog."    
}

if(-not(Test-DatabaseExists -ServerName $tenantServerName -DatabaseName $normalizedTenantName))
{
    Write-Host "Deploying $TenantName.  This may take several minutes..."

    $normalizedTenantName = Get-NormalizedTenantName -TenantName $tenantName 
    $serverName = $normalizedTenantName + '-' + $wtpUser.Name


    # Deploy the resource group
    $resourceGroup = New-AzureRmResourceGroup `
                        -Name $tenantResourceGroupName `
                        -Location $location `
                        -ErrorAction Stop

    # Deploy the app and database
    $deployment = New-AzureRmResourceGroupDeployment `
        -TemplateFile $config.WingtipTicketsAppTemplateUrl `
        -ResourceGroupName $tenantResourceGroupName `
        -TenantName $normalizedTenantName `
        -User $wtpUser.Name `
        -ErrorAction Stop `
        -Verbose
}
# Register the new tenant's database in the catalog
        
# Verify database has been deployed

if(Test-DatabaseExists -ServerName $serverName -DatabaseName $normalizedTenantName)
{
    Write-Host "Registering $tenantName..."

    # Compute the tenant key from the tenant name, used to register the tenant in the catalog
    $tenantKey = Get-TenantKey -TenantName $tenantName
     
    #initialize venue information in the tenant database, includes resetting the default event dates
    Initialize-TenantDatabase `
        -ServerName $serverName `
        -DatabaseName $normalizedTenantName `
        -TenantKey $TenantKey `
        -TenantName $TenantName `
        -VenueType $VenueType `
        -PostalCode $PostalCode `
        -CountryCode $CountryCode


    # Register the tenant database (idempotent)
    Add-TenantDatabaseToCatalog `
        -Catalog $catalog `
        -TenantKey $tenantKey `
        -TenantName $tenantName `
        -TenantServerName $serverName `
        -TenantDatabasename $normalizedTenantName

}

Write-Output "Provisioning complete for tenant '$TenantName'"