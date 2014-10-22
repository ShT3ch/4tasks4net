<%@ Page Title="Galery" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<string>>" MasterPageFile="~/Views/Common.master" %>

<%@ Import Namespace="System.IO" %>

<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead">
    <script src="/resos/galery.js" type="text/javascript">
    
    </script>
</asp:Content>
<asp:Content runat="server" ID="ContentPlaceHolderBody" ContentPlaceHolderID="ContentPlaceHolderBody">
    <% foreach (var pic in Model)
       {
    %><img style="margin: 5px;" onclick="showBigFoto('<%=Path.GetFileName(pic)%>');" onmouseout="onMouseOutHandler();" onmouseover="onMouseOverHandler(event,'<%=Path.GetFileName(pic)%>', this);" src="<%= pic %>" alt="pic" /><%
           
       } %>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolderBODYTAGS">
    onkeypress="return identifyEscKeyPressedEvent(event);"
</asp:Content>
