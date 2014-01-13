<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script>
        function getXmlHttp() {
            var xmlhttp;
            try {
                xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
                try {
                    xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (E) {
                    xmlhttp = false;
                }
            }
            if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
                xmlhttp = new XMLHttpRequest();
            }
            return xmlhttp;
        }

        function getComment(picName) {
            var req = getXmlHttp(); // (2)
            req.onreadystatechange = function() {
                if (req.readyState == 4) {
                    if (req.status == 200) {
                        alert("Ответ сервера: " + req.responseText);
                    }
                }

            }; // (3) задать адрес подключения
            req.open('GET', '/Home/ajaxtest/12312?name='+picName, true);
        }
    </script>
    <title>script</title>
</head>
<body>
    <input value="Голосовать!" onclick="vote()" type="button" />
    <div id="vote_status">Здесь будет ответ сервера</div>
</body>
</html>
