<%@ Page Title="Главная" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" MasterPageFile="~/Views/Common.master" %>

<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead">
</asp:Content>
<asp:Content runat="server" ID="ContentPlaceHolderBody" ContentPlaceHolderID="ContentPlaceHolderBody">
    <h1 align="center">Это еще один бесполезный сайт. А ведь мог бы поехать покататься на лыжах.</h1>
    <table>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>2</td>
        </tr>
        <% for (var i = 0; i < 100; i++)
           {
        %><tr>
            <td><%= i %></td>
        </tr>
        <% }
        %>
    </table>
</asp:Content>
