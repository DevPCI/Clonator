/*
' Copyright (c) 2013  DevPCI.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Linq;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Entities.Portals;
using Telerik.Web.UI;
using System.Globalization;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;
using System.Text;
using DotNetNuke.Web.UI.WebControls;
using DotNetNuke.Security;
using MyModule = DevPCI.Modules.DNN_Clones_Module_Manager_DevPCI_Clonator;


namespace DevPCI.Modules.DNN_Clones_Module_Manager_DevPCI_Clonator
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewDNN_Clones_Module_Manager_DevPCI_Clonator class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : DNN_Clones_Module_Manager_DevPCI_ClonatorModuleBase //, IActionable
    {
        public static readonly string VIEWSTATE_FILTER = "filter";
        
        public static readonly string FILTER_PRE = "Pre";
        public static readonly string FILTER_PAGE = "Page";
        public static readonly string FILTER_PANE = "Pane";
        public static readonly string FILTER_TITLE = "Title";
        public static readonly string FILTER_MID = "Mid";
        public static readonly string FILTER_ALL_MODULES = "AllModules";
        public static readonly string FILTER_SINGLE_ONLY = "SingleOnly";
        public static readonly string FILTER_CLONED_ONLY = "ClonedOnly";
        public static readonly string FILTER_ALL_TABS_TRUE = "AllTabsTrue";

        public static readonly string SELECT_PANE_VALUE = "SelectaPane";
        public static readonly string SELECT_TITLE_VALUE = "SelectaTitle";

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            //111113
            dgOnTabs.NeedDataSource += OnPagesGridNeedDataSource;
            //111113

        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (ViewState[VIEWSTATE_FILTER] != null)
                {
                    string filter = ViewState[VIEWSTATE_FILTER].ToString();
                }

                if (Settings[MyModule.Settings.SETTING_SHOW_HELP] != null)
                {
                    bool showHelp = Convert.ToBoolean(Settings[MyModule.Settings.SETTING_SHOW_HELP]);
                    PlaceHolder1.Visible = showHelp;
                    PlaceHolder2.Visible = showHelp;
                }

                if (!IsPostBack)
                {
                    ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_ALL_MODULES);
                    try
                    {

                        //custom code
                        ModuleController mc = new ModuleController();
                        var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>();
                        var modulesListFull = mc.GetModules(PortalId).Cast<ModuleInfo>();
                        BuildDropDownModulesList(modulesList, false);

                        //build the list of Panes
                        var panesList = mc.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.PaneName }).Select(g => g.First());
                        //ListItem sp = new ListItem();
                        DnnComboBoxItem sp = new DnnComboBoxItem();
                        sp.Text = "Select a Pane";
                        sp.Value = SELECT_PANE_VALUE;
                        cbPanesList.Items.Add(sp);
                        foreach (ModuleInfo moduleInfo in panesList)
                        {
                            //ListItem p = new ListItem();
                            DnnComboBoxItem p = new DnnComboBoxItem();
                            p.Text = moduleInfo.PaneName;
                            p.Value = moduleInfo.PaneName;
                            cbPanesList.Items.Add(p);

                        }

                        //build the list of Titles
                        var titlesList = mc.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleTitle }).Select(g => g.First());
                        //ListItem st = new ListItem();
                        DnnComboBoxItem st = new DnnComboBoxItem();
                        st.Text = "Select a Title";
                        st.Value = SELECT_TITLE_VALUE;
                        cbTitlesList.Items.Add(st);

                        foreach (ModuleInfo moduleInfo in titlesList)
                        {
                            //ListItem t = new ListItem();
                            DnnComboBoxItem t = new DnnComboBoxItem();
                            t.Text = moduleInfo.ModuleTitle;
                            t.Value = moduleInfo.ModuleTitle;
                            cbTitlesList.Items.Add(t);
                        }

                    }
                    catch (Exception exc) //Module failed to load
                    {
                        Exceptions.ProcessModuleLoadException(this, exc);
                    }
                }

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Filters

        protected void RadioButtonListPreFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblWarn.Text = "";
            ModuleController mc = new ModuleController();

            if (RadioButtonListPreFilter.SelectedValue == FILTER_ALL_MODULES)
            {
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>();
                BuildDropDownModulesList(modulesList, false);
                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_ALL_MODULES);
            }

            if (RadioButtonListPreFilter.SelectedValue == FILTER_SINGLE_ONLY)
            {
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleID }).Where(g => g.Count() == 1).Select(g => g.First());
                BuildDropDownModulesList(modulesList, true);
                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_SINGLE_ONLY);

            }
            if (RadioButtonListPreFilter.SelectedValue == FILTER_CLONED_ONLY)
            {
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleID }).Where(g => g.Count() > 1).Select(f => f.First()); // ok1
                //var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleID }).Where(g => g.Count() > 1).Select(f => new { f, Count = f.Count() }); // avec count du nb de clones d'un module
                if (modulesList.Count() > 0)
                {
                    BuildDropDownModulesList(modulesList, true);
                }
                else { lblWarn.Text = "<span style=\"color:red\">No Clone</span>"; }
                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_CLONED_ONLY);

            }
            if (RadioButtonListPreFilter.SelectedValue == FILTER_ALL_TABS_TRUE)
            {
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.AllTabs == true).GroupBy(i => new { i.ModuleID }).Select(g => g.First());
                if (modulesList.Count() > 0)
                {
                    BuildDropDownModulesList(modulesList, true);
                }
                else 
                { 
                    lblWarn.Text = "<span style=\"color:red\"> No module with All tabs True</span>"; 
                }

                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_ALL_TABS_TRUE);
            }
            string filter = Convert.ToString(ViewState[VIEWSTATE_FILTER]);
            reinitfilters(filter);

        }

        protected void ddlPage_SelectionChanged(object sender, EventArgs e)
        {
            int tabID = ddlPage.SelectedItemValueAsInt;
            //TabController tc = new TabController();
            //TabInfo tab = tc.GetTab(tabID, PortalId, false);
            ModuleController mc = new ModuleController();
            var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.TabID == tabID);
            BuildDropDownModulesList(modulesList, false);
            ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PAGE, tabID);
            string filter = Convert.ToString(ViewState[VIEWSTATE_FILTER]);
            reinitfilters(filter);

        }

        protected void cbPanesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pane = cbPanesList.SelectedValue;
            if (pane != SELECT_PANE_VALUE)
            {
                ModuleController mc = new ModuleController();
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.PaneName == pane);
                BuildDropDownModulesList(modulesList, false);
                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PANE, pane);
                string filter = Convert.ToString(ViewState[VIEWSTATE_FILTER]);
                reinitfilters(filter);
            }

        }

        protected void cbTitlesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string title = cbTitlesList.SelectedValue;
            if (title != SELECT_TITLE_VALUE)
            {
                ModuleController mc = new ModuleController();
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.ModuleTitle == title);
                BuildDropDownModulesList(modulesList, false);
                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_TITLE, title);
                string filter = Convert.ToString(ViewState[VIEWSTATE_FILTER]);
                reinitfilters(filter);
            }

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            int i = 0;
            string s = tbModuleID.Text;
            bool isint = int.TryParse(s, out i);
            if (isint) { args.IsValid = true; }
            else { args.IsValid = false; }
        }

        protected void ibGoModuleID_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (CustomValidator1.IsValid)
            {
                int mid = Convert.ToInt32(tbModuleID.Text);
                ModuleController mc = new ModuleController();
                var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.ModuleID == mid);
                BuildDropDownModulesList(modulesList, false);
                ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_MID, mid);
                string filter = Convert.ToString(ViewState[VIEWSTATE_FILTER]);
                reinitfilters(filter);

            }

        }

        protected void reinitfilters(string filter)

        {   
            //RadioButtonListPreFilter.ClearSelection();
            //ddlPage.SelectedPage = null;
            //cbPanesList.ClearSelection();
            //cbTitlesList.ClearSelection();
            //tbModuleID.Text = "Enter ModuleID";
            
            if (!String.IsNullOrEmpty(filter))
            {
                if (filter.StartsWith(FILTER_PRE))    
                {
                    ddlPage.SelectedPage = null;
                    cbPanesList.ClearSelection();
                    cbTitlesList.ClearSelection();
                    tbModuleID.Text = "Enter ModuleID";
                }
                if (filter.StartsWith(FILTER_PAGE))
                {
                    RadioButtonListPreFilter.ClearSelection();
                    cbPanesList.ClearSelection();
                    cbTitlesList.ClearSelection();
                    tbModuleID.Text = "Enter ModuleID";
                }
                if (filter.StartsWith(FILTER_PANE))
                {
                    RadioButtonListPreFilter.ClearSelection();
                    ddlPage.SelectedPage = null;
                    cbTitlesList.ClearSelection();
                    tbModuleID.Text = "Enter ModuleID";
                }
                if (filter.StartsWith(FILTER_TITLE))
                {
                    RadioButtonListPreFilter.ClearSelection();
                    ddlPage.SelectedPage = null;
                    cbPanesList.ClearSelection();
                    tbModuleID.Text = "Enter ModuleID";
                }
                if (filter.StartsWith(FILTER_MID))
                {
                    RadioButtonListPreFilter.ClearSelection();
                    ddlPage.SelectedPage = null;
                    cbPanesList.ClearSelection();
                    cbTitlesList.ClearSelection();
                }
            }
                 
        }



        #endregion

        #region Modules List

        private static int GetNbOfClone(IEnumerable<ModuleInfo> modulesListfull, int moduleID)
        {
            return modulesListfull.Where(x => x.ModuleID == moduleID).Count();
        }

        private void BuildDropDownModulesList(IEnumerable<ModuleInfo> modulesList, bool isFiltred)
        {
            TabController tc = new TabController();
            ModuleController mc = new ModuleController();
            cbModulesList.Items.Clear();
            int totnbc = 0;
            int cnt = 0;
            var modulesListfull = mc.GetModules(PortalId).Cast<ModuleInfo>();
            foreach (ModuleInfo moduleInfo in modulesList)
            {
                int nbc = GetNbOfClone(modulesListfull, moduleInfo.ModuleID);
                totnbc += nbc;
                //ListItem li = new ListItem();
                DnnComboBoxItem li = new DnnComboBoxItem();
                
                li.Text = string.Format("({0}) - ModuleID : {1} - Title : {2} - Page : {3} ({4}) - Pane : {5} - IsDelete : {6} - All Tabs : {7}",
                    nbc, 
                    moduleInfo.ModuleID, 
                    moduleInfo.ModuleTitle, 
                    tc.GetTab(moduleInfo.TabID, 
                    moduleInfo.PortalID, true).LocalizedTabName, 
                    moduleInfo.TabID, 
                    moduleInfo.PaneName, 
                    moduleInfo.IsDeleted, 
                    moduleInfo.AllTabs);

                //li.Text = "(" + nbc + ") - TabModuleID :" + moduleInfo.TabModuleID + " - ModuleID : " + moduleInfo.ModuleID + " - ModuleDefID : " + moduleInfo.ModuleDefID + " - FriendlyName : " + moduleInfo.DesktopModule.FriendlyName + " - Title : " + moduleInfo.ModuleTitle + " - Page : " + tc.GetTab(moduleInfo.TabID, moduleInfo.PortalID, true).LocalizedTabName + "(" + moduleInfo.TabID + ") - Pane : " + moduleInfo.PaneName + " - IsDelete : " + moduleInfo.IsDeleted + " - All Tabs : " + moduleInfo.AllTabs;
                li.Value = string.Format("{0}|{1}",moduleInfo.ModuleID, moduleInfo.TabID);
                cbModulesList.Items.Add(li);
                cnt++;
            }
            int c = modulesList.Count();
            if (isFiltred)
            {
                lblNbTotal.Text = totnbc.ToString();
            }
            else
            {
                lblNbTotal.Text = c.ToString();
            }

            //DropDownList1.Items.Cast<DropDownList>().Reverse();
            //061113
            var portalController = new PortalController();
            var portalInfo = portalController.GetPortal(PortalId);
            BindTree(portalInfo);
            //061113
        }

        protected void cbModulesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sv = cbModulesList.SelectedValue;
            string svmid = sv.Split('|')[0];
            string svtid = sv.Split('|')[1];
            int moduleId = Convert.ToInt32(svmid);
            //int tabId = Convert.ToInt32(svtid);
            dgOnTabs.Rebind();
            var portalController = new PortalController();
            var portalInfo = portalController.GetPortal(PortalId);
            BindTree(portalInfo);


        }

        #endregion

        #region treeview option
        protected void cbChangeTreeMode_CheckedChanged(object sender, EventArgs e)
        {
            if (cbChangeTreeMode.Checked)
            {
                ctlPages.CheckChildNodes = true;
            }
            else
            {
                ctlPages.CheckChildNodes = false;
            }
        }

        #endregion

        #region Pages tree
        // RadTreeView copied from \DesktopModules\Admin\Portals\Template.ascx.cs and modified a little
        private bool IsAdminTab(TabInfo tab)
        {
            var perms = tab.TabPermissions;
            return perms.Cast<TabPermissionInfo>().All(perm => perm.RoleName == PortalSettings.AdministratorRoleName || !perm.AllowAccess);
        }
        private bool IsRegisteredUserTab(TabInfo tab)
        {
            var perms = tab.TabPermissions;
            return perms.Cast<TabPermissionInfo>().Any(perm => perm.RoleName == PortalSettings.RegisteredRoleName && perm.AllowAccess);
        }
        private static bool IsSecuredTab(TabInfo tab)
        {
            var perms = tab.TabPermissions;
            return perms.Cast<TabPermissionInfo>().All(perm => perm.RoleName != "All Users" || !perm.AllowAccess);
        }
        private string IconPortal
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_Portal.png");
            }
        }
        private string GetNodeStatusIcon(TabInfo tab)
        {
            // enhencement for IconRedirect and distinguish IconPageDisabled and IconPageHidden = add image + add resx
            string s = string.Empty;
            if (tab.DisableLink)
            {
                s += string.Format("<img src=\"{0}\" alt=\"\" title=\"{1}\" class=\"statusicon\" />", IconPageDisabled, LocalizeString("lblDisabled"));
            }
            if (! tab.IsVisible)
            {
                s += string.Format("<img src=\"{0}\" alt=\"\" title=\"{1}\" class=\"statusicon\" />", IconPageHidden, LocalizeString("lblHidden"));
            }
            if (! string.IsNullOrEmpty(tab.Url))
            {
                s += string.Format("<img src=\"{0}\" alt=\"\" title=\"{1}\" class=\"statusicon\" />", IconRedirect, LocalizeString("lblRedirect"));
            }
            return s;
            //fin redirect Icon

            return "";
        }
        private string IconPageDisabled
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_Disabled.png");
            }
        }
        private string IconPageHidden
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_Hidden.png");
            }
        }
        private string AllUsersIcon
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_Everyone.png");
            }
        }
        private string AdminOnlyIcon
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_UserAdmin.png");
            }
        }
        private string IconHome
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_Home.png");
            }
        }
        private string RegisteredUsersIcon
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_User.png");
            }
        }
        private string SecuredIcon
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_UserSecure.png");
            }
        }
        // enhencement redirect Icon
        private string IconRedirect
        {
            get
            {
                return ResolveUrl("~/DesktopModules/Admin/Tabs/images/Icon_Redirect.png");
            }
        }
        //fin enhencement redirect Icon
        private string GetNodeIcon(TabInfo tab, out string toolTip)
        {
            toolTip = "";
            if (PortalSettings.HomeTabId == tab.TabID)
            {
                toolTip = LocalizeString("lblHome");
                return IconHome;
            }

            if (IsSecuredTab(tab))
            {
                if (IsAdminTab(tab))
                {
                    toolTip = LocalizeString("lblAdminOnly");
                    return AdminOnlyIcon;
                }

                if (IsRegisteredUserTab(tab))
                {
                    toolTip = LocalizeString("lblRegistered");
                    return RegisteredUsersIcon;
                }

                toolTip = LocalizeString("lblSecure");
                return SecuredIcon;
            }

            toolTip = LocalizeString("lblEveryone");
            return AllUsersIcon;
        }

        private void BindTree(PortalInfo portal)
        {
            // get all tabs by moduleId
            List<int> tabslist = null;
            var objTabs = new TabController();
            if (!string.IsNullOrEmpty(cbModulesList.SelectedValue))
            {
                string sv = cbModulesList.SelectedValue;
                string svmid = sv.Split('|')[0];
                string svtid = sv.Split('|')[1];
                int fmoduleId = Convert.ToInt32(svmid);

                var tabsbymodule = objTabs.GetTabsByModuleID(fmoduleId);
                tabslist = tabsbymodule.Keys.ToList();
            }

            BindTree(tabslist, portal);
        }

        private void BindTree(List<int> tabslist, PortalInfo portal)
        {
            #region Re-initialize RadTree
            ctlPages.Nodes.Clear();

            var tabController = new TabController();
            var rootNode = new RadTreeNode
            {
                Text = PortalSettings.PortalName,
                ImageUrl = IconPortal,
                Value = Null.NullInteger.ToString(CultureInfo.InvariantCulture),
                Expanded = true,
                AllowEdit = false,
                EnableContextMenu = true,
                Checked = false
            };
            rootNode.Attributes.Add("isPortalRoot", "True");

            // diff with copied code :
            // strip the case a check multilanguage as the checkbox have no inetrest here.
            // based on System.Threading.Thread.CurrentThread.CurrentCulture vs combo selected before)
            // change the checked from default true to false.
            // Switch from TabName to LocalizedTabName
            // recheck all the checkbox with a check if clone module
            List<TabInfo> tabs = TabController.GetPortalTabs(TabController.GetTabsBySortOrder(portal.PortalID, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), true),
                     Null.NullInteger,
                     false,
                     "<" + Localization.GetString("None_Specified") + ">",
                     true,
                     false,
                     true,
                     false,
                     false);

            //tabs = tabController.GetTabsByPortal(portal.PortalID);

            foreach (var tab in tabs) //.Values)
            {
                if (tab.Level == 0)
                {
                    string tooltip;
                    var nodeIcon = GetNodeIcon(tab, out tooltip);
                    var node = new RadTreeNode
                    {
                        Text = string.Format("{0} {1}", tab.LocalizedTabName, GetNodeStatusIcon(tab)),
                        Value = tab.TabID.ToString(CultureInfo.InvariantCulture),
                        AllowEdit = true,
                        ImageUrl = nodeIcon,
                        ToolTip = tooltip,
                        Checked = false
                    };

                    AddChildNodes(node, portal);
                    rootNode.Nodes.Add(node);
                }
            }

            ctlPages.Nodes.Add(rootNode);
            #endregion

            //111113 custom code
            foreach (RadTreeNode n in ctlPages.GetAllNodes())
            {
                int thetabid = Convert.ToInt32(n.Value);
                n.Checked = getbchk(tabslist, thetabid);
            }
            //111113 custom code end
            ctlPages.DataBind();
        }

        // diff with copied code :
        // switch from languageComboBox.SelectedValue to System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
        private void AddChildNodes(RadTreeNode parentNode, PortalInfo portal)
        {
            parentNode.Nodes.Clear();

            var parentId = int.Parse(parentNode.Value);

            var Tabs = new TabController().GetTabsByPortal(portal.PortalID).WithCulture(System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), true).WithParentId(parentId);

            foreach (var tab in Tabs)
            {
                if (tab.ParentId == parentId)
                {
                    string tooltip;
                    var nodeIcon = GetNodeIcon(tab, out tooltip);
                    var node = new RadTreeNode
                    {
                        Text = string.Format("{0} {1}", tab.LocalizedTabName, GetNodeStatusIcon(tab)),
                        Value = tab.TabID.ToString(CultureInfo.InvariantCulture),
                        AllowEdit = true,
                        ImageUrl = nodeIcon,
                        ToolTip = tooltip,
                        Checked = false
                    };
                    AddChildNodes(node, portal);
                    parentNode.Nodes.Add(node);
                }
            }
        }
        // end copy from \DesktopModules\Admin\Portals\Template.ascx.cs
        //111113 custom code return true if module is on the tab
        private bool getbchk(List<int> tabslist, int thetabid)
        {
            return (tabslist != null) && tabslist.Contains(thetabid);
        }
        //111113 custom code end

        #endregion

        #region Action

        //131113 custom code
        protected void UpdateClones(object sender, EventArgs e)
        {
            List<int> tabslist = null;
            ModuleController objmodules = new ModuleController();
            TabController objTabs = new TabController();
            string sv = string.Empty;
            int ftabId = -1;
            int fmoduleId = -1;
            if (! string.IsNullOrEmpty(cbModulesList.SelectedValue))
            {
                sv = cbModulesList.SelectedValue;
                string svmid = sv.Split('|')[0];
                string svtid = sv.Split('|')[1];
                fmoduleId = Convert.ToInt32(svmid);
                ftabId = Convert.ToInt32(svtid);

                var tabsbymodule = objTabs.GetTabsByModuleID(fmoduleId);
                
                // get all tabs by moduleId
                tabslist = tabsbymodule.Keys.ToList();
            }

            StringBuilder actionBuilder = new StringBuilder();
            actionBuilder.Append("<div class=\"dnnFormMessage dnnFormSuccess\">");

            // clone or delete tabs modules
            CloneOrDeleteTabModules(tabslist, objmodules, objTabs, ftabId, fmoduleId, actionBuilder);

            lblActions.Text = actionBuilder.ToString();

            // refresh RadTree
            var portalController = new PortalController();
            var portalInfo = portalController.GetPortal(PortalId);
            BindTree(tabslist, portalInfo);

            BindFilters(objmodules, sv);

        }

        private void BindFilters(ModuleController objmodules, string sv)
        {
            if (!string.IsNullOrEmpty(ViewState[VIEWSTATE_FILTER] as string))
            {
                string filter = ViewState[VIEWSTATE_FILTER].ToString();
                string filter1 = filter.Split('|')[0];
                string filter2 = filter.Split('|')[1];
                //string filter3 = filter.Split('|')[2];
                if (filter1 == FILTER_PRE)
                {
                    if (filter2 == FILTER_ALL_MODULES)
                    {
                        var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>();
                        BuildDropDownModulesList(modulesList, false);
                        cbModulesList.SelectedValue = sv;
                        ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_ALL_MODULES);

                    }
                    if (filter2 == FILTER_SINGLE_ONLY)
                    {
                        var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleID }).Where(g => g.Count() == 1).Select(g => g.First());
                        BuildDropDownModulesList(modulesList, true);
                        cbModulesList.SelectedValue = sv;
                        ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_SINGLE_ONLY);

                    }
                    if (filter2 == FILTER_CLONED_ONLY)
                    {
                        var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleID }).Where(g => g.Count() > 1).Select(f => f.First()); // ok1
                        //var modulesList = mc.GetModules(PortalId).Cast<ModuleInfo>().GroupBy(i => new { i.ModuleID }).Where(g => g.Count() > 1).Select(f => new { f, Count = f.Count() }); // avec count du nb de clones d'un module
                        if (modulesList.Count() > 0)
                        {
                            BuildDropDownModulesList(modulesList, true);
                        }
                        else { lblWarn.Text = "<span style=\"color:red\">No Clone</span>"; }
                        cbModulesList.SelectedValue = sv;
                        ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_CLONED_ONLY);

                    }
                    if (filter2 == FILTER_ALL_TABS_TRUE)
                    {
                        var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.AllTabs == true).GroupBy(i => new { i.ModuleID }).Select(g => g.First());
                        if (modulesList.Count() > 0)
                        {
                            BuildDropDownModulesList(modulesList, true);
                        }
                        else 
                        { 
                            lblWarn.Text = "<span style=\"color:red\"> No module with All tabs True</span>"; 
                        }
                        cbModulesList.SelectedValue = sv;
                        ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PRE, FILTER_ALL_TABS_TRUE);

                    }

                }
                if (filter1 == FILTER_PAGE)
                {
                    var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.TabID == Convert.ToInt32(filter2));
                    BuildDropDownModulesList(modulesList, false);
                    cbModulesList.SelectedValue = sv;
                    ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PAGE, Convert.ToInt32(filter2));

                }
                if (filter1 == FILTER_PANE)
                {
                    var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.PaneName == filter2);
                    BuildDropDownModulesList(modulesList, false);
                    cbModulesList.SelectedValue = sv;
                    ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_PANE, filter2);

                }
                if (filter1 == FILTER_TITLE)
                {
                    var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.ModuleTitle == filter2);
                    BuildDropDownModulesList(modulesList, false);
                    cbModulesList.SelectedValue = sv;
                    ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_TITLE, filter2);

                }
                if (filter1 == FILTER_MID)
                {
                    var modulesList = objmodules.GetModules(PortalId).Cast<ModuleInfo>().Where(module => module.ModuleID == Convert.ToInt32(filter2));
                    BuildDropDownModulesList(modulesList, false);
                    cbModulesList.SelectedValue = sv;
                    ViewState[VIEWSTATE_FILTER] = string.Format("{0}|{1}", FILTER_MID, Convert.ToInt32(filter2));
                }
            }
        }

        private void CloneOrDeleteTabModules(List<int> tabslist, ModuleController objmodules, TabController objTabs, int ftabId, int fmoduleId, StringBuilder actionBuilder)
        {
            foreach (RadTreeNode n in ctlPages.GetAllNodes())
            {
                // if this is the root node, don't make action
                int nval = Convert.ToInt32(n.Value);
                if (nval > 0)
                {
                    //Check if checked has changed
                    int ttabId = Convert.ToInt32(n.Value);
                    string pname = n.Text;

                    //The value in DB
                    bool dbcheck = getbchk(tabslist, ttabId);

                    //The value in the Tree
                    bool nodecheck = n.Checked;

                    //if changed Add or Delete the clone
                    if (dbcheck != nodecheck)
                    {
                        //Add case
                        if (!dbcheck && nodecheck)
                        {
                            #region OLD CODE COMMENTED V1
                            //We have a new localiezd version GUID when use the CopyModule(as Display On All Pages) V1 
                            //(bug with new localized GUID instead of copy original one like Add existing Module (clone) from new dashboard
                            // BUG ? in DNN Platform\Library\Entities\Modules\ModuleController.cs  l
                            // l1022             destinationModule.LocalizedVersionGuid = Guid.NewGuid();   

                            //Add V1 (use CopyModule)
                            //ModuleInfo fmi = objmodules.GetModule(fmoduleId, ftabId);
                            //ModuleInfo tmi = objmodules.GetModule(fmoduleId, ttabId);
                            //TabInfo tti = objTabs.GetTab(ttabId, PortalId, true);
                            ////Does detination module exist ?
                            ////NO
                            //if (tmi == null)
                            //{
                            //    objmodules.CopyModule(fmi, tti, "", Convert.ToBoolean(rbSettings.SelectedValue));
                            //    Action += "Clone Module " + fmoduleId + " on the page : " + pname + "(" + ttabId + ") <br />";
                            //}
                            //else
                            //{
                            //    if (tmi.IsDeleted)
                            //    {
                            //        objmodules.RestoreModule(tmi);
                            //        Action += "Restore Module " + fmoduleId + " on the page : " + pname + "(" + ttabId + ") <br />";
                            //    }
                            //}
                            //end V1
                            #endregion

                            //V2 (inspired from ControlBarController.cs DoAddExistingModule line 589)
                            ModuleInfo fmi = objmodules.GetModule(fmoduleId, ftabId);
                            ModuleInfo newModule = fmi.Clone();

                            newModule.UniqueId = Guid.NewGuid(); // Cloned Module requires a different uniqueID
                            newModule.TabID = ttabId;
                            objmodules.AddModule(newModule);

                            actionBuilder.AppendFormat("<div class=\"logLine addModule\">Clone or restore Module {0} ({1}) on the page : {2} ({3}) </div>", newModule.ModuleTitle, fmoduleId, pname, ttabId);

                            //Add Event Log
                            //var objEventLog = new EventLogController();
                            //objEventLog.AddLog(newModule, PortalSettings.Current, userID, "", EventLogController.EventLogType.MODULE_CREATED);

                        }
                        //Delete case
                        if (dbcheck && !nodecheck)
                        {
                            //Delete
                            bool softDelete = Convert.ToBoolean(rbDelete.SelectedValue);
                            TabInfo ti = objTabs.GetTab(ttabId, PortalId, true);
                            ModuleInfo mi = objmodules.GetModule(fmoduleId, ttabId);
                            objmodules.DeleteTabModule(ttabId, fmoduleId, softDelete);

                            actionBuilder.AppendFormat("<div class=\"logLine delModule\">{0} Module {1} ({2}) of the page : {3} ({4}) </div>", 
                                (softDelete ? "Send to recycle bin":"Delete"), 
                                mi.ModuleTitle, 
                                fmoduleId, 
                                pname,
                                ttabId);
                        }

                    }
                }


            }
            //            Response.Redirect(Request.RawUrl);
            actionBuilder.Append("</div>");
        }
        //131113 custom code end

        #endregion

        #region radGrid (module setting like)
        // RadGrid with list of Page which have the clone module on.
        // copied from \admin\Modules\Modulesettings.ascx.cs ascx visible=false

        protected void OnPagesGridNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            TabController TabCtrl = new TabController();

            string sv = cbModulesList.SelectedValue;
            string svmid = sv.Split('|')[0];
            string svtid = sv.Split('|')[1];
            int moduleId = Convert.ToInt32(svmid);
            var tabsByModule = TabCtrl.GetTabsByModuleID(moduleId);
            // diff with copied code :
            // here we want the actual tab id too) so comment the next line.
            //tabsByModule.Remove(TabId);   
            dgOnTabs.DataSource = tabsByModule.Values;
        }

        protected string GetInstalledOnLink(object dataItem)
        {
            TabController TabCtrl = new TabController();
            var returnValue = new StringBuilder();
            var tab = dataItem as TabInfo;
            if (tab != null)
            {
                var index = 0;
                TabCtrl.PopulateBreadCrumbs(ref tab);
                foreach (TabInfo t in tab.BreadCrumbs)
                {
                    if ((index > 0))
                    {
                        returnValue.Append(" > ");
                    }
                    if ((tab.BreadCrumbs.Count - 1 == index))
                    {
                        returnValue.AppendFormat("<a href=\"{0}\">{1}</a>", t.FullUrl, t.LocalizedTabName);
                    }
                    else
                    {
                        returnValue.AppendFormat("{0}", t.LocalizedTabName);
                    }
                    index = index + 1;
                }
            }
            return returnValue.ToString();
        }

        protected string GetInstalledOnSite(object dataItem)
        {
            string returnValue = String.Empty;
            var tab = dataItem as TabInfo;
            if (tab != null)
            {
                var portalController = new PortalController();
                var portal = portalController.GetPortal(tab.PortalID);
                if (portal != null)
                {
                    returnValue = portal.PortalName;
                }
            }
            return returnValue;
        }
        // end copy from \admin\Modules\Modulesettings.ascx.cs
        #endregion


        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(), Localization.GetString("EditModule", this.LocalResourceFile), "", "", "", EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }

}
