<%@ Page Title="Visits" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Concurrent.ConcurrentDictionary<string, MvcApplication2.Models.UserRegistation>>" MasterPageFile="~/Views/Common.master" %>

<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead"></asp:Content>

<asp:Content runat="server" ID="ContentPlaceHolderBody" ContentPlaceHolderID="ContentPlaceHolderBody">
    <% foreach (var ip in Model.Keys)
           foreach (var visit in Model[ip].Visits)
           {
    %><%=String.Format("{0} on {1} with ip {2}",visit.Item1, visit.Item2, ip)%><br />
    <%
           } %>
</asp:Content>
