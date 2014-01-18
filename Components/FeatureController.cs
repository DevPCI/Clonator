/*
' Copyright (c) 2013 DevPCI.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Collections.Generic;
//using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace DevPCI.Modules.DNN_Clones_Module_Manager_DevPCI_Clonator.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for DNN_Clones_Module_Manager_DevPCI_Clonator
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------

    //uncomment the interfaces to add the support.
    public class FeatureController //: IPortable, ISearchable, IUpgradeable
    {


        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<DNN_Clones_Module_Manager_DevPCI_ClonatorInfo> colDNN_Clones_Module_Manager_DevPCI_Clonators = GetDNN_Clones_Module_Manager_DevPCI_Clonators(ModuleID);
        //if (colDNN_Clones_Module_Manager_DevPCI_Clonators.Count != 0)
        //{
        //    strXML += "<DNN_Clones_Module_Manager_DevPCI_Clonators>";

        //    foreach (DNN_Clones_Module_Manager_DevPCI_ClonatorInfo objDNN_Clones_Module_Manager_DevPCI_Clonator in colDNN_Clones_Module_Manager_DevPCI_Clonators)
        //    {
        //        strXML += "<DNN_Clones_Module_Manager_DevPCI_Clonator>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objDNN_Clones_Module_Manager_DevPCI_Clonator.Content) + "</content>";
        //        strXML += "</DNN_Clones_Module_Manager_DevPCI_Clonator>";
        //    }
        //    strXML += "</DNN_Clones_Module_Manager_DevPCI_Clonators>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlDNN_Clones_Module_Manager_DevPCI_Clonators = DotNetNuke.Common.Globals.GetContent(Content, "DNN_Clones_Module_Manager_DevPCI_Clonators");
        //foreach (XmlNode xmlDNN_Clones_Module_Manager_DevPCI_Clonator in xmlDNN_Clones_Module_Manager_DevPCI_Clonators.SelectNodes("DNN_Clones_Module_Manager_DevPCI_Clonator"))
        //{
        //    DNN_Clones_Module_Manager_DevPCI_ClonatorInfo objDNN_Clones_Module_Manager_DevPCI_Clonator = new DNN_Clones_Module_Manager_DevPCI_ClonatorInfo();
        //    objDNN_Clones_Module_Manager_DevPCI_Clonator.ModuleId = ModuleID;
        //    objDNN_Clones_Module_Manager_DevPCI_Clonator.Content = xmlDNN_Clones_Module_Manager_DevPCI_Clonator.SelectSingleNode("content").InnerText;
        //    objDNN_Clones_Module_Manager_DevPCI_Clonator.CreatedByUser = UserID;
        //    AddDNN_Clones_Module_Manager_DevPCI_Clonator(objDNN_Clones_Module_Manager_DevPCI_Clonator);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<DNN_Clones_Module_Manager_DevPCI_ClonatorInfo> colDNN_Clones_Module_Manager_DevPCI_Clonators = GetDNN_Clones_Module_Manager_DevPCI_Clonators(ModInfo.ModuleID);

        //foreach (DNN_Clones_Module_Manager_DevPCI_ClonatorInfo objDNN_Clones_Module_Manager_DevPCI_Clonator in colDNN_Clones_Module_Manager_DevPCI_Clonators)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objDNN_Clones_Module_Manager_DevPCI_Clonator.Content, objDNN_Clones_Module_Manager_DevPCI_Clonator.CreatedByUser, objDNN_Clones_Module_Manager_DevPCI_Clonator.CreatedDate, ModInfo.ModuleID, objDNN_Clones_Module_Manager_DevPCI_Clonator.ItemId.ToString(), objDNN_Clones_Module_Manager_DevPCI_Clonator.Content, "ItemId=" + objDNN_Clones_Module_Manager_DevPCI_Clonator.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion

    }

}
