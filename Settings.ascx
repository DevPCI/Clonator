<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DevPCI.Modules.DNN_Clones_Module_Manager_DevPCI_Clonator.Settings" %>


<!-- uncomment the code below to start using the DNN Form pattern to create and update settings -->
  

<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

	<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowHelp" runat="server" /> 
            <asp:CheckBox ID="cbShowHelp" runat="server" />
        </div>
<%--        <div class="dnnFormItem">
            <dnn:label ID="lblSetting2" runat="server" />
            <asp:TextBox ID="txtSetting2" runat="server" />
        </div>--%>
    </fieldset>


