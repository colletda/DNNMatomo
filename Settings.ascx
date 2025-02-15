<%@ Control Language="vb" AutoEventWireup="false" CodeFile="Settings.ascx.vb" Inherits="Matomo.Modules.DNNMatomo.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="2" border="0" summary="DNNMatomo Settings Design Table">
    <tr>
        <td class="SubHead" width="150"><dnn:label id="lblmatomo_site_id" runat="server" controlname="txtmatomo_site_id" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtmatomo_site_id" cssclass="NormalTextBox" columns="4" maxlength="4" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150"><dnn:label id="lblMatomoHost" runat="server" controlname="txtMatomoHost" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:textbox id="txtMatomoHost" cssclass="NormalTextBox" columns="80" maxlength="255" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150"><dnn:label id="lblHideRoles" runat="server" controlname="rptHideRoles" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:Repeater ID="rptHideRoles" runat="server">
            <HeaderTemplate><table></HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><asp:HiddenField ID="hidRoleId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "RoleID")%>' /><asp:Label ID="lblRole" runat="server" CssClass="Normal" Text='<%#DataBinder.Eval(Container.DataItem, "RoleName")%>'></asp:Label></td>
                    <td><asp:CheckBox ID="hideRole" runat="server" CssClass="Normal" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
        </td>
    </tr>
</table>
