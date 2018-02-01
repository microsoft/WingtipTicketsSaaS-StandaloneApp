<#
.SYNOPSIS
  Resets the event dates in all tenant databases registered in the catalog   

.DESCRIPTION
  Resets the event dates in all tenant databases registered in the catalog.  Calls sp_ResetEventDates
  in each database. Two events are set in the recent past, the remainder are rescheduled into the future.  

#>

Import-Module $PSScriptRoot\..\Common\CatalogAndDatabaseManagement -Force
Import-Module "$PSScriptRoot\..\UserConfig" -Force

# Get Azure credentials if not already logged on,  Use -Force to select a different subscription 
Initialize-Subscription -NoEcho

# Get the user name used when the Contoso Concert Hall application was deployed.  
$wtpUser = Get-UserConfig
$config = Get-Configuration

# Get the catalog 

$catalogResourceGroupName = $config.CatalogResourceGroupNameStem + $wtpUser.Name
 
$catalog = Get-Catalog -ResourceGroupName $catalogResourceGroupName -WtpUser $wtpUser.Name 

$databaseLocations = Get-TenantDatabaseLocations -Catalog $catalog

$commandText = "EXEC sp_ResetEventDates"

foreach ($dbLocation in $databaseLocations)
{ 
    Write-Output "Resetting event dates for '$($dblocation.Location.Database)'."
    Invoke-Sqlcmd `
        -ServerInstance $($dbLocation.Location.Server) `
        -Username $($config.TenantAdminuserName) `
        -Password $($config.TenantAdminPassword) `
        -Database $($dblocation.Location.Database) `
        -Query $commandText `
        -ConnectionTimeout 30 `
        -QueryTimeout 30 `
        -EncryptConnection

}