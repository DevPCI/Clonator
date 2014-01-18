<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="DevPCI.Modules.DNN_Clones_Module_Manager_DevPCI_Clonator.View" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="dnnForm">
    <fieldset>
<%--        <div class="dnnFormItem">
            <h2>
                <asp:Label ID="lblTitle" runat="server" Text="Label" ResourceKey="lblTitle"></asp:Label></h2>
        </div>--%>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <div class="dnnFormMessage dnnFormInfo">
            <asp:Label ID="lblInfo" ResourceKey="lblInfo" runat="server" Text="This is informational!"></asp:Label>
        </div>
        </asp:PlaceHolder>
        <div class="dnnFormItem">
            <h3>
                <asp:Label ID="lblFilters" runat="server" Text="Filters (not cumulatives) the Modules List" ResourceKey="lblFilters"></asp:Label><asp:Label ID="lblthefilter" runat="server" Text=""></asp:Label></h3>
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="RadioButtonPreFilter" ResourceKey="lblRadioButtonPreFilter" />
            <asp:RadioButtonList ID="RadioButtonListPreFilter" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" CssClass="dnnFormRadioButtons" OnSelectedIndexChanged="RadioButtonListPreFilter_SelectedIndexChanged" ViewStateMode="Enabled">
                <asp:ListItem Selected="True" Value="AllModules" ResourceKey="AllModules"></asp:ListItem>
                <asp:ListItem Value="SingleOnly" ResourceKey="SingleOnly"></asp:ListItem>
                <asp:ListItem Value="ClonedOnly" ResourceKey="ClonedOnly"></asp:ListItem>
                <asp:ListItem Value="AllTabsTrue" ResourceKey="AllTabsTrue"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="ddlPage" ResourceKey="lblOr" />
            <dnn:DnnPageDropDownList ID="ddlPage" runat="server" autopostback="true" OnSelectionChanged="ddlPage_SelectionChanged"  />
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="ddlPanesList" ResourceKey="lblOr" />
            <dnn:DnnComboBox ID="cbPanesList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbPanesList_SelectedIndexChanged" CssClass="dnnFixedSizeComboBox"></dnn:DnnComboBox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="ddlTitlesList" ResourceKey="lblOr" />
            <dnn:DnnComboBox ID="cbTitlesList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbTitlesList_SelectedIndexChanged" CssClass="dnnFixedSizeComboBox"></dnn:DnnComboBox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label runat="server" ControlName="tbModuleID" ResourceKey="lblOr" />
            <asp:TextBox ID="tbModuleID" runat="server" Text="Enter ModuleID" Width="132px"></asp:TextBox>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Must be an integer" ControlToValidate="tbModuleID" ValidationGroup="tbmid" OnServerValidate="CustomValidator1_ServerValidate" CssClass="dnnFormMessage dnnFormError" ResourceKey="ModuleID.MustBeInt"></asp:CustomValidator>
            <asp:ImageButton ID="ibGoModuleID" runat="server" AlternateText="Go" OnClick="ibGoModuleID_Click" ValidationGroup="tbmid" />
        </div>
        <div class="dnnFormItem">
            <h3>
                <asp:Label ID="lblModuleList" ResourceKey="lblModuleList" runat="server" Text="Modules List (select one module to see page(s) with it)"></asp:Label>&nbsp;
                <asp:Label ID="lblWarn" runat="server" Text=""></asp:Label>
            </h3>
        </div>
        <div id="ModulesList" class="dnnFormItem">
            <dnn:Label ID="lblTotal" runat="server" Text="Total : " ResourceKey="lblTotal"></dnn:Label>
            <div class="dnnLabel">
                <asp:Label ID="lblNbTotal" runat="server" Text=""></asp:Label>
            </div>
            <dnn:DnnComboBox ID="cbModulesList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbModulesList_SelectedIndexChanged" Style="width: 750px; max-width: 750px"></dnn:DnnComboBox>
        </div>
        <div class="dnnFormItem">
            <h3>
                <asp:Label ID="lblpageswith" ResourceKey="lblpageswith" runat="server" Text="Pages with this module"></asp:Label></h3>
        </div>
        <asp:PlaceHolder ID="PlaceHolder2" runat="server">
        <div class="dnnFormMessage dnnFormInfo">
            <asp:Label ID="lblInfoPages" ResourceKey="lblInfoPages" runat="server" Text="(check/uncheck to clone/delete(soft) Once a module is deleted (soft), it will be restore if a clone is done."></asp:Label>
        </div>
        </asp:PlaceHolder>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCheckChildNodes" runat="server" Text="Checkbox mode Option : CheckChildNodes" ResourceKey="lblCheckChildNodes"></dnn:Label>
            <asp:CheckBox ID="cbChangeTreeMode" runat="server" AutoPostBack="True" OnCheckedChanged="cbChangeTreeMode_CheckedChanged" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCheckUncheck" runat="server" Text="Checkbox mode Option : CheckChildNodes" ResourceKey="lblCheckUncheck"></dnn:Label>
            <div class="dnnLeft">
                <dnn:dnntreeview id="ctlPages" cssclass="dnnTreePages" runat="server" CheckChildNodes="false"
                    CheckBoxes="true" TriStateCheckBoxes="false">
                </dnn:dnntreeview>
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCloneOption" runat="server" Text="Clone option :" ResourceKey="lblCloneOption" Visible="false"></dnn:Label>
            <asp:RadioButtonList ID="rbSettings" runat="server" RepeatDirection="Horizontal" CssClass="dnnFormRadioButtons" Visible="false">
                <asp:ListItem Selected="True" Value="True" Text="With Setting"></asp:ListItem>
                <asp:ListItem Value="False" Text="Without Setting"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDeleOption" runat="server" Text="Delete option :" ResourceKey="lblDeleteOption"></dnn:Label>
            <asp:RadioButtonList ID="rbDelete" runat="server" RepeatDirection="Horizontal" CssClass="dnnFormRadioButtons">
                <asp:ListItem Selected="True" Value="True" Text="Soft Delete (Recycle Bin)" ResourceKey="lblsoftdelete"></asp:ListItem>
                <asp:ListItem Value="False" Text="Definitive Delete" ResourceKey="lbldefinitivedelete"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <ul class="dnnActions dnnClear">
                <li>
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="dnnPrimaryAction" ResourceKey="CloneDelete" OnClick="UpdateClones" /></li>
                <li>
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="dnnSecondaryAction" NavigateUrl="/" ResourceKey="Cancel" /></li>
            </ul>
        </div>
        <div class="dnnFormItem">
            <asp:Label ID="lblActions" runat="server" Text=""></asp:Label>
        </div>
        <div class="dnnFormItem">
            <asp:Label ID="lblpagewith" ResourceKey="lblpagewith" runat="server" Text="Page(s) with module/clones" Visible="false"></asp:Label>
            <dnn:DnnGrid ID="dgOnTabs" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                PageSize="20" Visible="false">
                <mastertableview>
            <Columns>
                <dnn:DnnGridTemplateColumn HeaderText="Site" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <%#GetInstalledOnSite(Container.DataItem)%>
                    </ItemTemplate>
                </dnn:DnnGridTemplateColumn>
                <dnn:DnnGridTemplateColumn HeaderText="Page">
                    <ItemTemplate>
                        <%#GetInstalledOnLink(Container.DataItem)%>
                    </ItemTemplate>
                </dnn:DnnGridTemplateColumn>
            </Columns>
            <NoRecordsTemplate>
                <div class="dnnFormMessage dnnFormWarning">
                    <asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" />
                </div>
            </NoRecordsTemplate>
            </mastertableview>
            </dnn:DnnGrid>
        </div>
    </fieldset>
</div>