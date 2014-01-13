<%@ Page Title="Visits" Language="C#" Inherits="System.Web.Mvc.ViewPage<MvcApplication2.Models.UserRegistation>" MasterPageFile="~/Views/Common.master" %>

<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead"></asp:Content>

<asp:Content runat="server" ID="ContentPlaceHolderBody" ContentPlaceHolderID="ContentPlaceHolderBody">
    <% foreach (var visit in Model.Visits)
       {
    %><%=String.Format("{0} on {1}",visit.Item1, visit.Item2)%><br />
    <%
       } %>
</asp:Content>
