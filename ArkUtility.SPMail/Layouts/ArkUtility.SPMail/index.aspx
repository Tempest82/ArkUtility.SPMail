<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ArkUtility.SPMail.Layouts.ArkUtility.SPMail.index" %>

<html>
<head>
    <title>ArkUtility.SPMail Web Feature - Review Page</title>
</head>
<body>
    <h2>ArkUtility.SPMail Web Feature Basic review page</h2>
    <div>
        Primary (relative path) Links
        <a href="../../_vti_bin/ArkUtility.SPMail/mail.svc/license">Test Service by viewing details about the license and will download a text file</a><br />
        <a href="../../_vti_bin/ArkUtility.SPMail/mail.svc/getinstalledfullname">Test Service by viewing details about the dll installation and will download a text file</a><br />
        <a href="../../_vti_bin/ArkUtility.SPMail/mail.svc/send">Test Service email endpoint up,This should result in a response such as: method should not allowed, requires a POST operation.</a><br />
        Secondary(root path) Links<br />
        <a href="/_vti_bin/ArkUtility.SPMail/mail.svc/license">Test Service by viewing details about the license and will download a text file</a><br />
        <a href="/_vti_bin/ArkUtility.SPMail/mail.svc/getinstalledfullname">Test Service by viewing details about the dll installation and will download a text file</a><br />
        <a href="/_vti_bin/ArkUtility.SPMail/mail.svc/send">Test Service email endpoint up,This should result in a response such as: method should not allowed, requires a POST operation.</a><br />
    </div>
    <hr />
    <div>
        JSON Email Object:
        <br />
        {"Bcc":"Bcc@Address","Cc":"Cc@Address","From":"noreply@location.com","ImageLibraryUrls":["https://thishost/site/web/library/","https://thishost/site2/web2/library2/"],"LogListUrl":"https://thishost/site/web/LogList/","MessageBody":"Body Goes Here\u000d\u000aMore here\u000d\u000a And More here","MessageSubject":"Subject","ShouldEmbedImages":true,"ShouldWrapBodyWithHtml":true,"To":"To@Address"}<br/>
        JSON Send Response Object: <br/>
        {"Message":"Successful","Status":"Successful","Successful":true}<br/>
        
        Below is a javascript test for an average data entry SharePoint page with jquery available.
        <pre>
function testMail(){

        //if (typeof window.UpdateFormDigest === 'function') {
        //        window.UpdateFormDigest(window._spPageContextInfo.webServerRelativeUrl, window._spFormDigestRefreshInterval);
        //}
        //else {
        //    throw 'Unable to update __REQUESTDIGEST. Missing SharePoint Javascript Function. Ensure init.js is loaded or use another method.';
        //}
        var mail ={"To":"To@Address",
            "Cc":"Cc@Address",
            "Bcc":"Bcc@Address",
            "From":"noreply@location.com",
            "ImageLibraryUrls":["https://thishost/site/web/library/","https://thishost/site2/web2/library2/"],
            "LogListUrl":"https://thishost/site/web/LogList/",
            "MessageBody":"Body Goes Here\u000d\u000aMore here\u000d\u000a And More here",
            "MessageSubject":"Subject",
            "ShouldEmbedImages":true,
            "ShouldWrapBodyWithHtml":true
        };
        var urlTemplate = window.location.protocol+"//"+window.location.host+"/_vti_bin/ArkUtility.SPMail/mail.svc/send";
        $ajax({
            contentType:"application/json",
            url: urlTemplate,
            type:"POST",
            data:JSON.stringify(mail),
            headers: {"Accept":"application/json;odata=verbose", "content-type":"application/json;odata=verbose","X-RequestDigest":$('#__REQUESTDIGEST').val() },
            success:function(data){alert('Email Sent');},
            error:function(data){alert('Failure to send');},
            complete:function(data){alert('Complete');},
        });
};
testMail();
        </pre>
    </div>
    <hr />
    <h2>License (LGPLv3)</h2>
    <div>
        This file/service is part of ArkUtility SPMail.<br />

        ArkUtility SPMail is free software: you can redistribute it and/or modify<br />
        it under the terms of the GNU Lesser General Public License as published by<br />
        the Free Software Foundation, either version 3 of the License, or<br />
        (at your option) any later version.<br />
        <br />
        ArkUtility SPMail is distributed in the hope that it will be useful,<br />
        but WITHOUT ANY WARRANTY; without even the implied warranty of<br />
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the<br />
        GNU Lesser General Public License for more details.<br />
        <br />
        You should have received a copy of the GNU Lesser General Public License<br />
        along with ArkUtility SPMail.  If not, see &lt; http://www.gnu.org/licenses/ &gt;.<br />
        <br />
        <a href="copying.txt">GPLv3</a><br />
        <a href="copying.lesser.txt">LGPLv3</a><br/>
    </div>
    <div></div>
    <div></div>
</body>
</html>



