<%@ Page Title="Comment info" Language="C#" Inherits="System.Web.Mvc.ViewPage<MvcApplication2.Models.CommentAddedInfo>" MasterPageFile="~/Views/Common.master" %>

<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead">
    <script> var count = 6;
        var redirect = "/Home/galery";

        function countDown() {
            if (count <= 0) {
                window.location = redirect;
            } else {
                count--;
                document.getElementById("timer").innerHTML = "This page will redirect in " + count + " seconds.";
                setTimeout("countDown()", 1000);
            }
        }
    </script>
</asp:Content>


<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolderBODYTAGS">
    onload = "countDown();"
</asp:Content>


<asp:Content runat="server" ID="ContentPlaceHolderBody" ContentPlaceHolderID="ContentPlaceHolderBody">
    <div id="timer"></div>
    <div><%=Model.status %></div>
</asp:Content>
