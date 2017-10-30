## WingtipSaaS-StandaloneApp
A sample standalone single-tenant SaaS application plus management scripts, built on Azure SQL Database. 

This project contains a sample Web application and management scripts that embody common patterns used with Azure SQL Database.  It is a simple event-management and ticket-selling app for a single venue, with data stored in a single database.  As a standalone app, it could be installed in the ISV's or the venue's Azure subscription. The app uses the same patterns that might be used if the venue wrote the application for themselves.    

The application, which includes sample data for one of three venues, can be installed in your Azure subscription within a single Azure resource group. To uninstall the application, delete the resource group using the Azure Portal. 

NOTE: if you install the application you will be charged for the Azure resources created.  Actual costs incurred are based on your subscription offer type but are nominal if the application is not scaled up unreasonably and is deleted promptly after you have finished exploring the tutorials.

More information about the sample app and the associated tutorials is here: [https://aka.ms/sqldbsaastutorial](https://aka.ms/sqldbsaastutorial)

Click the buttons below to deploy venue-specific versions of the app to Azure. Deploy each app in a new resource group, and provide a short *user* value that will be appended to resource names to make them globally unique.  Your initials and a number is a good pattern to use.  You can use the same user value for all three applications.

<a href="https://aka.ms/deploywingtipsa-contoso" target="_blank">
<img src="http://azuredeploy.net/deploybutton.png"/>
</a> Contoso Concert Hall
</p>
<a href="https://aka.ms/deploywingtipsa-fabrikam" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a> Fabrikam Jazz Club</p>
<a href="https://aka.ms/deploywingtipsa-dogwood" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a> Dogwood Dojo


After deployment completes, launch the app by browsing to the corresponding URL, substituting *USER* with the value you set during deployment: </p>```http://events.contosoconcerthall.USER.trafficmanager.net``` </p>
```http://events.fabrikamjazzclub.USER.trafficmanager.net```</p>
```http://events.dogwooddojo.USER.trafficmanager.net```  

**IMPORTANT:** If you download and extract the repo or [Learning Modules](https://github.com/Microsoft/WingtipSaaS/tree/master/Learning%20Modules) from a zip file, make sure you unblock the .zip file before extracting. Executable contents (scripts, dlls) may be blocked by Windows when zip files are downloaded from an external source.

To avoid scripts from being blocked by Windows:

1. Right click the zip file and select **Properties**.
1. On the **General** tab, select **Unblock** and select **OK**.


## License
Microsoft Wingtip SaaS sample application and tutorials are licensed under the MIT license. See the [LICENSE](https://github.com/Microsoft/WingtipSaaS/blob/master/license) file for more details.

# Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
