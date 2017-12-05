# ArkUtility.SPMail
A utility created for on-premise SharePoint 2013 farms as a Farm Scoped Solution that provides a RESTFul API to create HTML email messages with inline images.

## Version 1.0
Primary functionality to accept an HTML email and embed inline images as they are found in the email by searching a sharepoint library within the web application.
- Added SendMail RESTful Web Service accepts POST - "/_vti_bin/ArkUtility.SPMail/mail.svc/Send"
- Added Dll version RESTful Web Service accepts GET - "/_vti_bin/ArkUtility.SPMail/mail.svc/GetInstalledFullName"
- Added Licese version RESTful Web Service accepts GET - "/_vti_bin/ArkUtility.SPMail/mail.svc/License"
- Added Sharepoint ULS logging into the service
- Page at  "/_layouts/15/ArkUtility.SPMail/index.aspx" provides test details and license information without loading the SharePoint Masterpages
- Support for Large emails (>50K requests) (Required Custom Service Factory)
- Added AdditionalImageLibraryUrls to allow an array of additional libraries to be checked for matching images. Libraries are processed in the order listed to attempt to find a matching filename.
- Log Lists must have the field "EmailResult" and title available to log. 
- ULS and Log lists will handle email message Successes/Failures by logging the following : Success, UTC Datetime, Subject, Addressees, Server, User, Error Stack.

# Installation and Removal Instructions 
The Setup folder provided in the project has a pre-compiled .wsp to allow those without easy access to a SharePoint Server to compile the project. The included Setup2013.exe can be used to install and retract OR you can use the following SharePoint Management Shell (PowerShell) commands in the event the Setup software fails( software provided as-is, see Setup Configuration Readme)

## Initial Deployment Process – SharePoint Management Shell
The following steps are applicable to a clean install of the product.  They would also be applicable if the product were to be retracted and redeployed.
- Open SharePoint 2013 Management Shell
- Run the following command from the directory the desired WSP file is located in
Add-SPSolution "{yourPath}\ArkUtility.SPMail.wsp"
- Run the following command.
Install-SPSolution -Identity ArkUtility.SPMail.wsp http://{yourWebApplicationUrl/ -GACDeployment

## Removal Process
- Open SharePoint 2013 Management Shell
- Run the following command.
Uninstall-SPSolution -Identity "ArkUtility.SPMail.wsp" -WebApplication yourWebApplicationUrl
- Run the following command to complete the SharePoint removal.
Remove-SPSolution -Identity "ArkUtility.SPMail.wsp"

