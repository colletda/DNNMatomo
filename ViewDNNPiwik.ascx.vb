'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports DotNetNuke
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection

Namespace Trapias.Modules.DNNPiwik

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The ViewDynamicModule class displays the content
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class ViewDNNPiwik
        Inherits Entities.Modules.PortalModuleBase

#Region "Private Members"

#End Region

#Region "Event Handlers"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Not Me.IsEditable Then
                Me.ContainerControl.Visible = False
            Else
                Dim msg As New Label
                msg.CssClass = "Normal"
                msg.Text = "DNN PIWIK Integration Module"
                Me.Controls.Add(msg)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If CType(Settings("HideRoles"), String) <> "" Then
                    'HideRoles
                    Dim elencoRuoli As List(Of Integer) = StringToList(Settings("HideRoles")), rc As New DotNetNuke.Security.Roles.RoleController()
                    'if user is member of an hidden role do not add tracker to page
                    For Each hiddenRoleID As Integer In elencoRuoli
                        If DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().IsInRole(rc.GetRole(hiddenRoleID, Me.PortalId).RoleName) Then
                            Exit Sub
                        End If
                    Next
                End If

                If CType(Settings("piwik_site_id"), String) <> "" Then

                    'Piwik Host: if not given assume it's on "localhost/piwik"
                    Dim PiwikHost As String = ""
                    If CType(Settings("PiwikHost"), String) <> "" Then
                        PiwikHost = Settings("PiwikHost")
                        If Not PiwikHost.EndsWith("/") Then PiwikHost &= "/"
                    Else
                        PiwikHost = "localhost/piwik/"
                    End If

                    Dim sScript As String = "<script type=""text/javascript"">"
                    sScript &= "var pkBaseURL = ((""https:"" == document.location.protocol) ? ""https://" & PiwikHost & """ : ""http://" & PiwikHost & """);"
                    sScript &= "document.write(unescape(""%3Cscript src='"" + pkBaseURL + ""piwik.js' type='text/javascript'%3E%3C/script%3E""));"""""
                    sScript &= "</script><script type=""text/javascript"">"
                    sScript &= "try {var piwikTracker = Piwik.getTracker(pkBaseURL + ""piwik.php"", " & Settings("piwik_site_id") & ");"

                    'page title
                    '01.00.01: new method to save
                    Dim sTitle As String = ""
                    Try
                        'first get title from page (some module might have modified it)
                        If Not Page.FindControl("PageTitle") Is Nothing Then
                            sTitle = CType(Page.FindControl("PageTitle"), HtmlGenericControl).InnerText
                        End If
                    Catch ex As Exception
                    End Try
                    'if empty string load DNN tab title
                    If sTitle.Trim = "" Then
                        Dim tab As DotNetNuke.Entities.Tabs.TabInfo = TabController.CurrentPage
                        sTitle = IIf(tab.Title = String.Empty, tab.TabName, tab.Title)
                    End If
                    sScript &= "piwikTracker.setDocumentTitle(""" & sTitle & """);"

                    'track
                    sScript &= "piwikTracker.trackPageView();"
                    sScript &= "piwikTracker.enableLinkTracking();"

                    'noscript
                    sScript &= "} catch( err ) {}</script><noscript><p><img src=""http://" & PiwikHost & "/piwik.php?idsite=" & Settings("piwik_site_id") & """ style=""border:0"" alt=""""/></p></noscript>"

                    Me.Page.ClientScript.RegisterStartupScript(GetType(System.String), "DNNPiwik", sScript)

                Else
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, "Module must be configured", Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If

            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Function StringToList(ByVal s As String) As List(Of Integer)
            Dim outList As New List(Of Integer)
            Dim valori() As String = s.Split(",")
            For Each v As String In valori
                If IsNumeric(v) Then outList.Add(v)
            Next
            Return outList
        End Function

#End Region


    End Class

End Namespace
