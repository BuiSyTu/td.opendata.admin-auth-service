<%@ Page Language="C#" MasterPageFile="~sitecollection/_catalogs/masterpage/default/default.master" inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document"%>
<%@ Register Tagprefix="layout" Namespace="TD.Core.Layouts.Controls" Assembly="TD.Core.Layouts.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fdcb66d7090aabcd" %>
<asp:Content runat="server" ContentPlaceHolderID="PlaceHolderPageHeaderTitle">
	Default page
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <a href="NormalPage.aspx">Normal page</a><br />
    <a href="RestrictedPage.aspx">Restricted page</a>
</asp:Content>