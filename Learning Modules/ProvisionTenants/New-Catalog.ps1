<#
.SYNOPSIS
  Provisions the tenant catalog and registers those pre-provided tenants that have been deployed  
#>

param(

)

Import-Module "$PSScriptRoot\..\Common\CatalogAndDatabaseManagement" -Force
Import-Module "$PSScriptRoot\..\Common\SubscriptionManagement" -Force


# Get Azure credentials if not already logged on,  Use -Force to select a different subscription 
Initialize-Subscription

# Get the resource group and user names used when the WTP application was deployed from UserConfig.psm1.  
$wtpUser = Get-UserConfig

# Get the WTP app configuration
$config = Get-Configuration

# Get the Azure location used for the Contoso application and database to use for the catalog
$contosoServer = @()
$contosoServer += Find-AzureRmResource -ResourceNameContains "ContosoConcertHall" -ResourceType Microsoft.Sql/Servers
if ($contosoServer.Count -le 0)
{
    throw "Contoso Concert Hall server not found. Deploy the Contoso Concert Hall app and try again."
}     

$location = ($contosoServer)[0].Location 

  
# Deploy the catalog to a new 'management' resource group in the same location.  
# Catalog is used for management scenarios only, not application routing which is configured
# using app settings deployed with the app

$catalogResourceGroupName = $config.CatalogResourceGroupNameStem + $wtpUser.Name
$catalogServerName = $config.CatalogServerNameStem + $wtpuser.Name
 
# check if catalog database exists    
$catalogDatabase = Get-AzureRmSqlDatabase `
    -ResourceGroupName $catalogResourceGroupName `
    -ServerName $catalogServerName `
    -DatabaseName $config.CatalogDatabaseName `
    -ErrorAction SilentlyContinue

if (!$catalogDatabase)
{
    Write-Output "Deploying catalog. This may take several minutes..."
        
    # Create the resource group
    New-AzureRmResourceGroup -Name $catalogResourceGroupName -Location $location `
        > $null

    # Create the catalog database using an ARM template and import the base schema from a bacpac 
    New-AzureRmResourceGroupDeployment `
        -TemplateFile $config.CatalogTemplateUrl `
        -Name "TenantCatalog" `
        -ResourceGroupName $catalogResourceGroupName `
        -User $wtpUser.Name `          
}
    
# Initialize catalog object from the catalog database
$catalog = Get-Catalog `
    -ResourceGroupName $catalogResourceGroupName `
    -WtpUser $wtpUser.Name `
    -ErrorAction Stop

# Register Contoso Concert Hall, Dogwood Dojo and Fabrikam Jazz Club in the catalog        

$tenantNames = {Contoso Concert Hall},{Dogwood Dojo},{Fabrikam Jazz Club}

foreach($tenantName in $TenantNames)
{  
    $normalizedTenantName = Get-NormalizedTenantName -TenantName $tenantName 
    $serverName = $normalizedTenantName + '-' + $wtpUser.Name
        
    # Check database has been deployed and then register

    if(Test-DatabaseExists -ServerName $serverName -DatabaseName $normalizedTenantName)
    {
        Write-Host "Registering $tenantName..."

        # Compute the tenant key from the tenant name, used to register the tenant in the catalog
        $tenantKey = Get-TenantKey -TenantName $tenantName

        # Register the tenant database (idempotent)
        Add-TenantDatabaseToCatalog `
            -Catalog $catalog `
            -TenantKey $tenantKey `
            -TenantName $tenantName `
            -TenantServerName $serverName `
            -TenantDatabasename $normalizedTenantName
    }
}