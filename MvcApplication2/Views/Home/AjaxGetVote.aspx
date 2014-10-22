<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<MvcApplication2.Models.VoteForFoto>" %><div style="width: 200px; background-color: gray;">
                                                                                                    <div style="width: <%=Model.negative%>%; background-color: red;">.</div>(-) <%=Model.negative%>%
                                                                                                    <div style="width: <%=Model.positive%>%; background-color: green;">.</div>(+) <%=Model.positive%>%<br/>
                                                                                                    Голосов всего: <%=Model.status %>                                                                                                    
</div>