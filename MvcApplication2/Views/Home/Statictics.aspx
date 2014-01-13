<%@ Page Title="InfoImage" Language="C#" Inherits="System.Web.Mvc.ViewPage<MvcApplication2.Models.FotoName>" MasterPageFile="~/Views/Common.master" %>
<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead"></asp:Content>

<asp:Content runat="server" ID="ContentPlaceHolderBody" ContentPlaceHolderID="ContentPlaceHolderBody">
    <img src="/pics/info/<%=Model.name %>" alt="info"></img>
</asp:Content>
