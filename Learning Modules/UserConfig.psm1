<#
.SYNOPSIS
    Returns the User name used during the Wingtip Tickets SaaS application deployment.  
    The value defined here is used in the learning module scripts.
#>


## Update the $User variable below    

$User = "<user>"   # the User value entered when the Contoso Concert Hall application was deployed


##  DO NOT CHANGE VALUES BELOW HERE -------------------------------------------------------------

function Get-UserConfig {

    $userConfig = @{`
        Name = $User.ToLower()   
    }
   
    if ($userConfig.Name -eq "<user>")
    {
        throw 'UserConfig is not set.  Modify $User in UserConfig.psm1 and try again.'
    }

    return $userConfig

}
