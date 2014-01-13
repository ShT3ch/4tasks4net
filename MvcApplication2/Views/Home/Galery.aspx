<%@ Page Title="Galery" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<string>>" MasterPageFile="~/Views/Common.master" %>

<%@ Import Namespace="System.IO" %>

<asp:Content runat="server" ID="ContentPlaceHolderHead" ContentPlaceHolderID="ContentPlaceHolderHead">
    <script>
        var currentPic = "";
        function findPosX(obj) {
            var curleft = 0;
            if (obj.offsetParent) {
                while (1) {
                    curleft += obj.offsetLeft;
                    if (!obj.offsetParent) {
                        break;
                    }
                    obj = obj.offsetParent;
                }
            } else if (obj.x) {
                curleft += obj.x;
            }
            return curleft;
        }

        function findPosY(obj) {
            var curtop = 0;
            if (obj.offsetParent) {
                while (1) {
                    curtop += obj.offsetTop;
                    if (!obj.offsetParent) {
                        break;
                    }
                    obj = obj.offsetParent;
                }
            } else if (obj.y) {
                curtop += obj.y;
            }
            return curtop;
        }
        function onMouseOverHandler(ev, picName, obj) {

            var d = document.createElement('div');
            d.id = 'preview';
            d.style.position = 'absolute';
            d.style.left = (findPosX(obj) + obj.width) + 'px';
            d.style.top = (findPosY(obj) + obj.height) + 'px';
            d.style.width = '200px';
            d.style.height = '100px';
            d.innerHTML = '<img style="margin: 5px;" src="/pics/mediums/' + picName + '" alt="pic" />';
            document.body.appendChild(d);
        }

        function onMouseOutHandler() {
            var d = document.getElementById('preview');
            d.parentNode.removeChild(d);
        }

        function showBigFoto(picName) {
            currentPic = picName;

            var dBack = document.createElement('div');
            dBack.style.position = 'fixed';
            dBack.style.width = '100%';
            dBack.style.height = '100%';
            dBack.style.top = '0';
            dBack.style.left = '0';
            dBack.style.backgroundColor = 'green';
            dBack.style.opacity = '0.7';
            dBack.style.zIndex = '10';
            dBack.id = 'bigviewBack';
            document.body.appendChild(dBack);

            var d = document.createElement('div');
            d.id = 'bigview';
            d.style.position = 'absolute';
            d.style.top = '0';
            d.style.left = '0';
            d.style.marginHeight = '150px';
            d.style.marginWidth = '150px';
            d.style.zIndex = '100';
            d.style.align = 'center';
            d.innerHTML = '<img style="margin: 5px;" src="/pics/full/' + picName + '" alt="pic" />' +
                '<div id="votes">' +
                '<div id="markVotes"></div>'+
                '<input value="Голосовать! (+)" onclick="vote(1,' + "'" + picName + "'" + ');" type="button" />' +
                '<input value="Голосовать! (-)" onclick="vote(-1,' + "'" + picName + "'" + ');" type="button" />' +
                '<div id="vote_status">Поголосуем?</div>' +
                '</div>' +
                '<div id="comments"></div>' +
                '<form action="/Home/AddCommentPage" method="POST">' +
                '<textarea name="review"> Здесь не должно быть вашего XSS</textarea>' +
                '<input type="hidden" name="fotoname" value="' + picName + '"/>' +
                '<input type="submit" value = "submit"/>' +
                '</form>';
            document.body.appendChild(d);
            getComment(picName);
            getVote(picName);
        }

        function identifyEscKeyPressedEvent(keyEvent) {
            var pressedKeyValue = keyEvent.keyCode;
            if (pressedKeyValue == 27) {
                var d = document.getElementById('bigviewBack');
                d.parentNode.removeChild(d);
                var d = document.getElementById('bigview');
                d.parentNode.removeChild(d);
                currentPic = "";
            }
        }

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
            req.onreadystatechange = function () {
                if (req.readyState == 4) {
                    var commField = document.getElementById("comments");
                    if (req.status == 200) {
                        commField.innerHTML = req.responseText;
                    } else {
                        commField.innerHTML = '<h1>something went wrong</h1>';
                    }
                    if (!((currentPic != picName) || (currentPic == "")))
                        setTimeout('getComment(' + "'" + picName + "'" + ')', 1000);
                }

            }; // (3) задать адрес подключения
            req.open('GET', '/Home/AjaxGetComments/12312?name=' + picName, true);
            req.send(null);
        }

        function getVote(picName) {
            var req = getXmlHttp();
            req.onreadystatechange = function () {
                if (req.readyState == 4) {
                    var commField = document.getElementById("markVotes");
                    if (req.status == 200) {
                        commField.innerHTML = req.responseText;
                    } else {
                        commField.innerHTML = '<h1>something went wrong</h1>';
                    }
                    if (!((currentPic != picName) || (currentPic == "")))
                        setTimeout('getVote(' + "'" + picName + "'" + ')', 1000);
                }
            };
            req.open('GET', '/Home/AjaxGetVote/12312?name=' + picName, true);
            req.send(null);

        }

        function vote(voteSign, picName) {
            var req = getXmlHttp();
            var statusElem = document.getElementById('vote_status');
            req.onreadystatechange = function () {
                if (req.readyState == 4) {
                    statusElem.innerHTML = req.statusText;
                    if (req.status == 200) {
                        statusElem.innerHTML=(req.responseText);
                    }
                }
            };
            req.open('GET', '/Home/AjaxVote/12312?name='+picName+'&sign='+voteSign, true);

            req.send(null);

            statusElem.innerHTML = 'Ожидаю ответа сервера...';
        }
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
