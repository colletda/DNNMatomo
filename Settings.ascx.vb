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
Imports DotNetNuke.Entities.Modules
Imports System.Collections.Generic

Namespace Trapias.Modules.DNNPiwik

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages Module Settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Database and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    'piwik_site_id
                    If CType(TabModuleSettings("piwik_site_id"), String) <> "" Then
                        txtpiwik_site_id.Text = CType(TabModuleSettings("piwik_site_id"), String)
                    End If

                    'PiwikHost
                    If CType(TabModuleSettings("PiwikHost"), String) <> "" Then
                        txtPiwikHost.Text = CType(TabModuleSettings("PiwikHost"), String)
                    End If

                    'HideRoles
                    Dim rc As New DotNetNuke.Security.Roles.RoleController()
                    Dim elencoRuoli As ArrayList = rc.GetPortalRoles(Me.PortalId)
                    rptHideRoles.DataSource = elencoRuoli
                    rptHideRoles.DataBind()

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                'piwik_site_id
                objModules.UpdateTabModuleSetting(TabModuleId, "piwik_site_id", txtpiwik_site_id.Text)

                'PiwikHost
                objModules.UpdateTabModuleSetting(TabModuleId, "PiwikHost", txtPiwikHost.Text)

                'HideRoles
                Dim sHideRoles As String = ""
                For Each ri As RepeaterItem In rptHideRoles.Items
                    Dim hidRoleId As HiddenField = ri.FindControl("hidRoleId")
                    Dim ch As CheckBox = ri.FindControl("hideRole")
                    If ch.Checked = True Then
                        sHideRoles &= hidRoleId.Value.ToString & ","
                    End If
                Next
                objModules.UpdateTabModuleSetting(TabModuleId, "HideRoles", sHideRoles)

                ' refresh cache
                ModuleController.SynchronizeModule(Me.ModuleId)

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

        Protected Sub rptHideRoles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptHideRoles.ItemDataBound

            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

                If CType(TabModuleSettings("HideRoles"), String) <> "" Then

                    'find roleid
                    Dim hidRoleId As HiddenField = e.Item.FindControl("hidRoleId")

                    'check/uncheck 
                    Dim ch As CheckBox = e.Item.FindControl("hideRole")
                    Dim elencoRuoli As List(Of Integer) = StringToList(Settings("HideRoles"))
                    If elencoRuoli.Contains(Integer.Parse(hidRoleId.Value.ToString)) Then
                        ch.Checked = True
                    End If
                End If

            End If

        End Sub

        Private Function StringToList(ByVal s As String) As List(Of Integer)
            Dim outList As New List(Of Integer)
            Dim valori() As String = s.Split(",")
            For Each v As String In valori
                If IsNumeric(v) Then outList.Add(v)
            Next
            Return outList
        End Function

    End Class

End Namespace

