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

Namespace Matomo.Modules.DNNMatomo

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The ViewDynamicModule class displays the content
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class View
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
            msg.Text = "DNN Matomo Integration Module"
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

            If CType(Settings("matomo_site_id"), String) <> "" Then

                'Matomo Host: if not given assume it's on "localhost/matomo"
                Dim MatomoHost As String = ""
                If CType(Settings("matomo_host"), String) <> "" Then
                    MatomoHost = Settings("matomo_host")
                    If Not MatomoHost.EndsWith("/") Then MatomoHost &= "/"
                    Else
                        MatomoHost = "localhost/matomo/"
                    End If

                    Dim sScript As String = "<!-- Matomo Image Code -->"
                    sScript &= "<img referrerpolicy=""no-referrer-when-downgrade"" src=""" & MatomoHost & "matomo.php?idsite=" & Settings("matomo_site_id") & "&amp;rec=1"" style=""border:0"" alt="""" />"
                    sScript &= "<!-- End Matomo Code -->"
 
                    Me.Page.ClientScript.RegisterStartupScript(GetType(System.String), "DNNMatomo", sScript)

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
