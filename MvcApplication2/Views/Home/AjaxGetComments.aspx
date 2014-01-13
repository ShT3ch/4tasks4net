<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<string>>" %>

<% foreach (var comment in Model)
   {
%>#<b><%= comment %></b><br />
<br />
<%
   } %>
