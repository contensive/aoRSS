using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace Contensive.Addons.Rss {





    public static class constants {
        // 
        // -- sample
        // =======================================================================
        // sitepropertyNames
        // =======================================================================
        // 
        public const string siteproperty_serverPageDefault_name = "serverPageDefault";
        public const string siteproperty_serverPageDefault_defaultValue = "index.php";
        // 
        // =======================================================================
        // content replacements
        // =======================================================================
        // 
        public const string contentReplaceEscapeStart = "{%";
        public const string contentReplaceEscapeEnd = "%}";
        // 
        // Public Type fieldEditorType
        // Public Const fieldId as Integer
        // Public Const addonid as Integer
        // End Type
        // 
        public const string protectedContentSetControlFieldList = "ID,CREATEDBY,DATEADDED,MODIFIEDBY,MODIFIEDDATE,EDITSOURCEID,EDITARCHIVE,EDITBLANK,CONTENTCONTROLID";
        // Public Const protectedContentSetControlFieldList = "ID,CREATEDBY,DATEADDED,MODIFIEDBY,MODIFIEDDATE,EDITSOURCEID,EDITARCHIVE,EDITBLANK"
        // 
        public const string HTMLEditorDefaultCopyStartMark = "<!-- cc -->";
        public const string HTMLEditorDefaultCopyEndMark = "<!-- /cc -->";
        public const string HTMLEditorDefaultCopyNoCr = HTMLEditorDefaultCopyStartMark + "<p><br></p>" + HTMLEditorDefaultCopyEndMark;
        public const string HTMLEditorDefaultCopyNoCr2 = "<p><br></p>";
        // 
        public const string IconWidthHeight = " width=21 height=22 ";
        // Public Const IconWidthHeight = " width=18 height=22 "
        // 
        public const string CoreCollectionGuid = "{8DAABAE6-8E45-4CEE-A42C-B02D180E799B}"; // contains core Contensive objects, loaded from Library
        public const string ApplicationCollectionGuid = "{C58A76E2-248B-4DE8-BF9C-849A960F79C6}"; // exported from application during upgrade
                                                                                                  // 
        public const string adminCommonAddonGuid = "{76E7F79E-489F-4B0F-8EE5-0BAC3E4CD782}";
        public const string DashboardAddonGuid = "{4BA7B4A2-ED6C-46C5-9C7B-8CE251FC8FF5}";
        public const string PersonalizationGuid = "{C82CB8A6-D7B9-4288-97FF-934080F5FC9C}";
        public const string TextBoxGuid = "{7010002E-5371-41F7-9C77-0BBFF1F8B728}";
        public const string ContentBoxGuid = "{E341695F-C444-4E10-9295-9BEEC41874D8}";
        public const string DynamicMenuGuid = "{DB1821B3-F6E4-4766-A46E-48CA6C9E4C6E}";
        public const string ChildListGuid = "{D291F133-AB50-4640-9A9A-18DB68FF363B}";
        public const string DynamicFormGuid = "{8284FA0C-6C9D-43E1-9E57-8E9DD35D2DCC}";
        public const string AddonManagerGuid = "{1DC06F61-1837-419B-AF36-D5CC41E1C9FD}";
        public const string FormWizardGuid = "{2B1384C4-FD0E-4893-B3EA-11C48429382F}";
        public const string ImportWizardGuid = "{37F66F90-C0E0-4EAF-84B1-53E90A5B3B3F}";
        public const string JQueryGuid = "{9C882078-0DAC-48E3-AD4B-CF2AA230DF80}";
        public const string JQueryUIGuid = "{840B9AEF-9470-4599-BD47-7EC0C9298614}";
        public const string ImportProcessAddonGuid = "{5254FAC6-A7A6-4199-8599-0777CC014A13}";
        public const string StructuredDataProcessorGuid = "{65D58FE9-8B76-4490-A2BE-C863B372A6A4}";
        public const string jQueryFancyBoxGuid = "{24C2DBCF-3D84-44B6-A5F7-C2DE7EFCCE3D}";
        // 
        public const string DefaultLandingPageGuid = "{925F4A57-32F7-44D9-9027-A91EF966FB0D}";
        public const string DefaultLandingSectionGuid = "{D882ED77-DB8F-4183-B12C-F83BD616E2E1}";
        public const string DefaultTemplateGuid = "{47BE95E4-5D21-42CC-9193-A343241E2513}";
        public const string DefaultDynamicMenuGuid = "{E8D575B9-54AE-4BF9-93B7-C7E7FE6F2DB3}";
        // 
        public const string fpoContentBox = "{1571E62A-972A-4BFF-A161-5F6075720791}";
        // 
        public const string sfImageExtList = "jpg,jpeg,gif,png";
        // 
        public const string PageChildListInstanceID = "{ChildPageList}";
        // 
        public const string cr = Constants.vbCrLf + Constants.vbTab;
        public const string cr2 = cr + Constants.vbTab;
        public const string cr3 = cr2 + Constants.vbTab;
        public const string cr4 = cr3 + Constants.vbTab;
        public const string cr5 = cr4 + Constants.vbTab;
        public const string cr6 = cr5 + Constants.vbTab;
        // 
        public const string AddonOptionConstructor_BlockNoAjax = "Wrapper=[Default:0|None:-1|ListID(Wrappers)]" + Constants.vbCrLf + "css Container id" + Constants.vbCrLf + "css Container class";
        public const string AddonOptionConstructor_Block = "Wrapper=[Default:0|None:-1|ListID(Wrappers)]" + Constants.vbCrLf + "As Ajax=[If Add-on is Ajax:0|Yes:1]" + Constants.vbCrLf + "css Container id" + Constants.vbCrLf + "css Container class";
        public const string AddonOptionConstructor_Inline = "As Ajax=[If Add-on is Ajax:0|Yes:1]" + Constants.vbCrLf + "css Container id" + Constants.vbCrLf + "css Container class";
        // 
        // Constants used as arguments to SiteBuilderClass.CreateNewSite
        // 
        public const int SiteTypeBaseAsp = 1;
        public const int sitetypebaseaspx = 2;
        public const int SiteTypeDemoAsp = 3;
        public const int SiteTypeBasePhp = 4;
        // 
        // Public Const AddonNewParse = True
        // 
        public const string AddonOptionConstructor_ForBlockText = "AllowGroups=[listid(groups)]checkbox";
        public const string AddonOptionConstructor_ForBlockTextEnd = "";
        public const string BlockTextStartMarker = "<!-- BLOCKTEXTSTART -->";
        public const string BlockTextEndMarker = "<!-- BLOCKTEXTEND -->";
        // 
        [DllImport("kernel32")]
        private static extern void Sleep(int dwMilliseconds);
        [DllImport("kernel32")]
        private static extern int GetExitCodeProcess(int hProcess, int lpExitCode);
        [DllImport("winmm.dll")]
        private static extern int timeGetTime();
        [DllImport("kernel32")]
        private static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);
        [DllImport("kernel32")]
        private static extern int CloseHandle(int hObject);
        // 
        public const string InstallFolderName = "Install";
        public const string DownloadFileRootNode = "collectiondownload";
        public const string CollectionFileRootNode = "collection";
        public const string CollectionFileRootNodeOld = "addoncollection";
        public const string CollectionListRootNode = "collectionlist";
        // 
        public const string LegacyLandingPageName = "Landing Page Content";
        public const string DefaultNewLandingPageName = "Home";
        public const string DefaultLandingSectionName = "Home";
        // 
        // ----- Errors Specific to the Contensive Objects
        // 
        // Public Const KmaccErrorUpgrading = KmaObjectError + 1
        // Public Const KmaccErrorServiceStopped = KmaObjectError + 2
        // 
        public const string UserErrorHeadline = "<p class=\"ccError\">There was a problem with this page.</p>";
        // 
        // ----- Errors connecting to server
        // 
        public const int ccError_InvalidAppName = 100;
        public const int ccError_ErrorAddingApp = 101;
        public const int ccError_ErrorDeletingApp = 102;
        public const int ccError_InvalidFieldName = 103;     // Invalid parameter name
        public const int ccError_InvalidCommand = 104;
        public const int ccError_InvalidAuthentication = 105;
        public const int ccError_NotConnected = 106;             // Attempt to execute a command without a connection
                                                                 // 
                                                                 // 
                                                                 // 
                                                                 // Public Const ccStatusCode_Base = KmaErrorBase
                                                                 // Public Const ccStatusCode_ControllerCreateFailed = ccStatusCode_Base + 1
                                                                 // Public Const ccStatusCode_ControllerInProcess = ccStatusCode_Base + 2
                                                                 // Public Const ccStatusCode_ControllerStartedWithoutService = ccStatusCode_Base + 3
                                                                 // 
                                                                 // ----- Previous errors, can be replaced
                                                                 // 
                                                                 // Public Const KmaError_UnderlyingObject_Msg = "An error occurred in an underlying routine."
        public const string KmaccErrorServiceStopped_Msg = "The Contensive CSv Service is not running.";
        public const string KmaError_BadObject_Msg = "Server Object is not valid.";
        public const string KmaError_UpgradeInProgress_Msg = "Server is busy with internal upgrade.";
        // 
        // Public Const KmaError_InvalidArgument_Msg = "Invalid Argument"
        // Public Const KmaError_UnderlyingObject_Msg = "An error occurred in an underlying routine."
        // Public Const KmaccErrorServiceStopped_Msg = "The Contensive CSv Service is not running."
        // Public Const KmaError_BadObject_Msg = "Server Object is not valid."
        // Public Const KmaError_UpgradeInProgress_Msg = "Server is busy with internal upgrade."
        // Public Const KmaError_InvalidArgument_Msg = "Invalid Argument"
        // 
        // -----------------------------------------------------------------------
        // GetApplicationList indexes
        // -----------------------------------------------------------------------
        // 
        public const int AppList_Name = 0;
        public const int AppList_Status = 1;
        public const int AppList_ConnectionsActive = 2;
        public const int AppList_ConnectionString = 3;
        public const int AppList_DataBuildVersion = 4;
        public const int AppList_LicenseKey = 5;
        public const int AppList_RootPath = 6;
        public const int AppList_PhysicalFilePath = 7;
        public const int AppList_DomainName = 8;
        public const int AppList_DefaultPage = 9;
        public const int AppList_AllowSiteMonitor = 10;
        public const int AppList_HitCounter = 11;
        public const int AppList_ErrorCount = 12;
        public const int AppList_DateStarted = 13;
        public const int AppList_AutoStart = 14;
        public const int AppList_Progress = 15;
        public const int AppList_PhysicalWWWPath = 16;
        public const int AppListCount = 17;
        // 
        // -----------------------------------------------------------------------
        // System MemberID - when the system does an update, it uses this member
        // -----------------------------------------------------------------------
        // 
        public const int SystemMemberID = 0;
        // 
        // -----------------------------------------------------------------------
        // ----- old (OptionKeys for available Options)
        // -----------------------------------------------------------------------
        // 
        public const int OptionKeyProductionLicense = 0;
        public const int OptionKeyDeveloperLicense = 1;
        // 
        // -----------------------------------------------------------------------
        // ----- LicenseTypes, replaced OptionKeys
        // -----------------------------------------------------------------------
        // 
        public const int LicenseTypeInvalid = -1;
        public const int LicenseTypeProduction = 0;
        public const int LicenseTypeTrial = 1;
        // 
        // -----------------------------------------------------------------------
        // ----- Active Content Definitions
        // -----------------------------------------------------------------------
        // 
        public const string ACTypeDate = "DATE";
        public const string ACTypeVisit = "VISIT";
        public const string ACTypeVisitor = "VISITOR";
        public const string ACTypeMember = "MEMBER";
        public const string ACTypeOrganization = "ORGANIZATION";
        public const string ACTypeChildList = "CHILDLIST";
        public const string ACTypeContact = "CONTACT";
        public const string ACTypeFeedback = "FEEDBACK";
        public const string ACTypeLanguage = "LANGUAGE";
        public const string ACTypeAggregateFunction = "AGGREGATEFUNCTION";
        public const string ACTypeAddon = "ADDON";
        public const string ACTypeImage = "IMAGE";
        public const string ACTypeDownload = "DOWNLOAD";
        public const string ACTypeEnd = "END";
        public const string ACTypeTemplateContent = "CONTENT";
        public const string ACTypeTemplateText = "TEXT";
        public const string ACTypeDynamicMenu = "DYNAMICMENU";
        public const string ACTypeWatchList = "WATCHLIST";
        public const string ACTypeRSSLink = "RSSLINK";
        public const string ACTypePersonalization = "PERSONALIZATION";
        public const string ACTypeDynamicForm = "DYNAMICFORM";
        // 
        public const string ACTagEnd = "<ac type=\"" + ACTypeEnd + "\">";
        // 
        // ----- PropertyType Definitions
        // 
        public const int PropertyTypeMember = 0;
        public const int PropertyTypeVisit = 1;
        public const int PropertyTypeVisitor = 2;
        // 
        // -----------------------------------------------------------------------
        // ----- Port Assignments
        // -----------------------------------------------------------------------
        // 
        public const int WinsockPortWebOut = 4000;
        public const int WinsockPortServerFromWeb = 4001;
        public const int WinsockPortServerToClient = 4002;
        // 
        public const int Port_ContentServerControlDefault = 4531;
        public const int Port_SiteMonitorDefault = 4532;
        // 
        public const int RMBMethodHandShake = 1;
        public const int RMBMethodMessage = 3;
        public const int RMBMethodTestPoint = 4;
        public const int RMBMethodInit = 5;
        public const int RMBMethodClosePage = 6;
        public const int RMBMethodOpenCSContent = 7;
        // 
        // ----- Position equates for the Remote Method Block
        // 
        private const int RMBPositionLength = 0;             // Length of the RMB
        private const int RMBPositionSourceHandle = 4;       // Handle generated by the source of the command
        private const int RMBPositionMethod = 8;             // Method in the method block
        private const int RMBPositionArgumentCount = 12;     // The number of arguments in the Block
        private const int RMBPositionFirstArgument = 16;     // The offset to the first argu
                                                             // 
                                                             // -----------------------------------------------------------------------
                                                             // Remote Connections
                                                             // List of current remove connections for Remote Monitoring/administration
                                                             // -----------------------------------------------------------------------
                                                             // 
                                                             // Public Type RemoteAdministratorType
                                                             // RemoteIP As String
                                                             // RemotePort as Integer
                                                             // End Type
                                                             // 
                                                             // Default username/password
                                                             // 
        public const string DefaultServerUsername = "root";
        public const string DefaultServerPassword = "contensive";
        // 
        // -----------------------------------------------------------------------
        // Form Contension Strategy
        // 
        // all Contensive Forms contain a hidden "ccFormSN"
        // The value in the hidden is the FormID string. All input
        // elements of the form are named FormID & "ElementName"
        // 
        // This prevents form elements from different forms from interfearing
        // with each other, and with developer generated forms.
        // 
        // GetFormSN gets a new valid random formid to be used.
        // All forms requires:
        // a FormId (text), containing the formid string
        // a [formid]Type (text), as defined in FormTypexxx in CommonModule
        // 
        // Forms have two primary sections: GetForm and ProcessForm
        // 
        // Any form that has a GetForm method, should have the process form
        // in the main.init, selected with this [formid]type hidden (not the
        // GetForm method). This is so the process can alter the stream
        // output for areas before the GetForm call.
        // 
        // System forms, like tools panel, that may appear on any page, have
        // their process call in the main.init.
        // 
        // Popup forms, like ImageSelector have their processform call in the
        // main.init because no .asp page exists that might contain a call
        // the process section.
        // 
        // -----------------------------------------------------------------------
        // 
        public const string FormTypeToolsPanel = "do30a8vl29";
        public const string FormTypeActiveEditor = "l1gk70al9n";
        public const string FormTypeImageSelector = "ila9c5s01m";
        public const string FormTypePageAuthoring = "2s09lmpalb";
        public const string FormTypeMyProfile = "89aLi180j5";
        public const string FormTypeLogin = "login";
        // Public Const FormTypeLogin = "l09H58a195"
        public const string FormTypeSendPassword = "lk0q56am09";
        public const string FormTypeJoin = "6df38abv00";
        public const string FormTypeHelpBubbleEditor = "9df019d77sA";
        public const string FormTypeAddonSettingsEditor = "4ed923aFGw9d";
        public const string FormTypeAddonStyleEditor = "ar5028jklkfd0s";
        public const string FormTypeSiteStyleEditor = "fjkq4w8794kdvse";
        // Public Const FormTypeAggregateFunctionProperties = "9wI751270"
        // 
        // -----------------------------------------------------------------------
        // Hardcoded profile form const
        // -----------------------------------------------------------------------
        // 
        public const string rnMyProfileTopics = "profileTopics";
        // 
        // -----------------------------------------------------------------------
        // Legacy - replaced with HardCodedPages
        // Intercept Page Strategy
        // 
        // RequestnameInterceptpage = InterceptPage number from the input stream
        // InterceptPage = Global variant with RequestnameInterceptpage value read during early Init
        // 
        // Intercept pages are complete pages that appear instead of what
        // the physical page calls.
        // -----------------------------------------------------------------------
        // 
        public const string RequestNameInterceptpage = "ccIPage";
        // 
        public const string LegacyInterceptPageSNResourceLibrary = "s033l8dm15";
        public const string LegacyInterceptPageSNSiteExplorer = "kdif3318sd";
        public const string LegacyInterceptPageSNImageUpload = "ka983lm039";
        public const string LegacyInterceptPageSNMyProfile = "k09ddk9105";
        public const string LegacyInterceptPageSNLogin = "6ge42an09a";
        public const string LegacyInterceptPageSNPrinterVersion = "l6d09a10sP";
        public const string LegacyInterceptPageSNUploadEditor = "k0hxp2aiOZ";
        // 
        // -----------------------------------------------------------------------
        // Ajax functions intercepted during init, answered and response closed
        // These are hard-coded internal Contensive functions
        // These should eventually be replaced with (HardcodedAddons) remote methods
        // They should all be prefixed "cc"
        // They are called with cj.ajax.qs(), setting RequestNameAjaxFunction=name in the qs
        // These name=value pairs go in the QueryString argument of the javascript cj.ajax.qs() function
        // -----------------------------------------------------------------------
        // 
        // Public Const RequestNameOpenSettingPage = "settingpageid"
        public const string RequestNameAjaxFunction = "ajaxfn";
        public const string RequestNameAjaxFastFunction = "ajaxfastfn";
        // 
        public const string AjaxOpenAdminNav = "aps89102kd";
        public const string AjaxOpenAdminNavGetContent = "d8475jkdmfj2";
        public const string AjaxCloseAdminNav = "3857fdjdskf91";
        public const string AjaxAdminNavOpenNode = "8395j2hf6jdjf";
        public const string AjaxAdminNavOpenNodeGetContent = "eieofdwl34efvclaeoi234598";
        public const string AjaxAdminNavCloseNode = "w325gfd73fhdf4rgcvjk2";
        // 
        public const string AjaxCloseIndexFilter = "k48smckdhorle0";
        public const string AjaxOpenIndexFilter = "Ls8jCDt87kpU45YH";
        public const string AjaxOpenIndexFilterGetContent = "llL98bbJQ38JC0KJm";
        public const string AjaxStyleEditorAddStyle = "ajaxstyleeditoradd";
        public const string AjaxPing = "ajaxalive";
        public const string AjaxGetFormEditTabContent = "ajaxgetformedittabcontent";
        public const string AjaxData = "data";
        public const string AjaxGetVisitProperty = "getvisitproperty";
        public const string AjaxSetVisitProperty = "setvisitproperty";
        public const string AjaxGetDefaultAddonOptionString = "ccGetDefaultAddonOptionString";
        public const string ajaxGetFieldEditorPreferenceForm = "ajaxgetfieldeditorpreference";
        // 
        // -----------------------------------------------------------------------
        // 
        // no - for now just use ajaxfn in the cj.ajax.qs call
        // this is more work, and I do not see why it buys anything new or better
        // 
        // Hard-coded addons
        // these are internal Contensive functions
        // can be called with just /addonname?querystring
        // call them with cj.ajax.addon() or cj.ajax.addonCallback()
        // are first in the list of checks when a URL rewrite is detected in Init()
        // should all be prefixed with 'cc'
        // -----------------------------------------------------------------------
        // 
        // Public Const HardcodedAddonGetDefaultAddonOptionString = "ccGetDefaultAddonOptionString"
        // 
        // -----------------------------------------------------------------------
        // Remote Methods
        // ?RemoteMethodAddon=string
        // calls an addon (if marked to run as a remote method)
        // blocks all other Contensive output (tools panel, javascript, etc)
        // -----------------------------------------------------------------------
        // 
        public const string RequestNameRemoteMethodAddon = "remotemethodaddon";
        // 
        // -----------------------------------------------------------------------
        // Hard Coded Pages
        // ?Method=string
        // Querystring based so they can be added to URLs, preserving the current page for a return
        // replaces output stream with html output
        // -----------------------------------------------------------------------
        // 
        public const string RequestNameHardCodedPage = "method";
        // 
        public const string HardCodedPageLogin = "login";
        public const string HardCodedPageLoginDefault = "logindefault";
        public const string HardCodedPageMyProfile = "myprofile";
        public const string HardCodedPagePrinterVersion = "printerversion";
        public const string HardCodedPageResourceLibrary = "resourcelibrary";
        public const string HardCodedPageLogoutLogin = "logoutlogin";
        public const string HardCodedPageLogout = "logout";
        public const string HardCodedPageSiteExplorer = "siteexplorer";
        // Public Const HardCodedPageForceMobile = "forcemobile"
        // Public Const HardCodedPageForceNonMobile = "forcenonmobile"
        public const string HardCodedPageNewOrder = "neworderpage";
        public const string HardCodedPageStatus = "status";
        public const string HardCodedPageGetJSPage = "getjspage";
        public const string HardCodedPageGetJSLogin = "getjslogin";
        public const string HardCodedPageRedirect = "redirect";
        public const string HardCodedPageExportAscii = "exportascii";
        public const string HardCodedPagePayPalConfirm = "paypalconfirm";
        public const string HardCodedPageSendPassword = "sendpassword";
        // 
        // -----------------------------------------------------------------------
        // Option values
        // does not effect output directly
        // -----------------------------------------------------------------------
        // 
        public const string RequestNamePageOptions = "ccoptions";
        // 
        public const string PageOptionForceMobile = "forcemobile";
        public const string PageOptionForceNonMobile = "forcenonmobile";
        public const string PageOptionLogout = "logout";
        public const string PageOptionPrinterVersion = "printerversion";
        // 
        // convert to options later
        // 
        public const string RequestNameDashboardReset = "ResetDashboard";
        // 
        // -----------------------------------------------------------------------
        // DataSource constants
        // -----------------------------------------------------------------------
        // 
        public const int DefaultDataSourceID = -1;
        // 
        // -----------------------------------------------------------------------
        // ----- Type compatibility between databases
        // Boolean
        // Access      YesNo       true=1, false=0
        // SQL Server  bit         true=1, false=0
        // MySQL       bit         true=1, false=0
        // Oracle      integer(1)  true=1, false=0
        // Note: false does not equal NOT true
        // Integer (Number)
        // Access      Long        8 bytes, about E308
        // SQL Server  int
        // MySQL       integer
        // Oracle      integer(8)
        // Float
        // Access      Double      8 bytes, about E308
        // SQL Server  Float
        // MySQL
        // Oracle
        // Text
        // Access
        // SQL Server
        // MySQL
        // Oracle
        // -----------------------------------------------------------------------
        // 
        // Public Const SQLFalse = "0"
        // Public Const SQLTrue = "1"
        // 
        // -----------------------------------------------------------------------
        // ----- Style sheet definitions
        // -----------------------------------------------------------------------
        // 
        public const string defaultStyleFilename = "ccDefault.r5.css";
        public const string StyleSheetStart = "<STYLE TYPE=\"text/css\">";
        public const string StyleSheetEnd = "</STYLE>";
        // 
        public const string SpanClassAdminNormal = "<span class=\"ccAdminNormal\">";
        public const string SpanClassAdminSmall = "<span class=\"ccAdminSmall\">";
        // 
        // remove these from ccWebx
        // 
        public const string SpanClassNormal = "<span class=\"ccNormal\">";
        public const string SpanClassSmall = "<span class=\"ccSmall\">";
        public const string SpanClassLarge = "<span class=\"ccLarge\">";
        public const string SpanClassHeadline = "<span class=\"ccHeadline\">";
        public const string SpanClassList = "<span class=\"ccList\">";
        public const string SpanClassListCopy = "<span class=\"ccListCopy\">";
        public const string SpanClassError = "<span class=\"ccError\">";
        public const string SpanClassSeeAlso = "<span class=\"ccSeeAlso\">";
        public const string SpanClassEnd = "</span>";
        // 
        // -----------------------------------------------------------------------
        // ----- XHTML definitions
        // -----------------------------------------------------------------------
        // 
        public const string DTDTransitional = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
        // 
        public const string BR = "<br>";
        // 
        // -----------------------------------------------------------------------
        // AuthoringControl Types
        // -----------------------------------------------------------------------
        // 
        public const int AuthoringControlsEditing = 1;
        public const int AuthoringControlsSubmitted = 2;
        public const int AuthoringControlsApproved = 3;
        public const int AuthoringControlsModified = 4;
        // 
        // -----------------------------------------------------------------------
        // ----- Panel and header colors
        // -----------------------------------------------------------------------
        // 
        // Public Const "ccPanel" = "#E0E0E0"    ' The background color of a panel (black copy visible on it)
        // Public Const "ccPanelHilite" = "#F8F8F8"  '
        // Public Const "ccPanelShadow" = "#808080"  '
        // 
        // Public Const HeaderColorBase = "#0320B0"   ' The background color of a panel header (reverse copy visible)
        // Public Const "ccPanelHeaderHilite" = "#8080FF" '
        // Public Const "ccPanelHeaderShadow" = "#000000" '
        // 
        // -----------------------------------------------------------------------
        // ----- Field type Definitions
        // Field Types are numeric values that describe how to treat values
        // stored as ContentFieldDefinitionType (FieldType property of FieldType Type.. ;)
        // -----------------------------------------------------------------------
        // 
        public const int FieldTypeInteger = 1;       // An long number
        public const int FieldTypeText = 2;          // A text field (up to 255 characters)
        public const int FieldTypeLongText = 3;      // A memo field (up to 8000 characters)
        public const int FieldTypeBoolean = 4;       // A yes/no field
        public const int FieldTypeDate = 5;          // A date field
        public const int FieldTypeFile = 6;          // A filename of a file in the files directory.
        public const int FieldTypeLookup = 7;        // A lookup is a FieldTypeInteger that indexes into another table
        public const int FieldTypeRedirect = 8;      // creates a link to another section
        public const int FieldTypeCurrency = 9;      // A Float that prints in dollars
        public const int FieldTypeTextFile = 10;     // Text saved in a file in the files area.
        public const int FieldTypeImage = 11;        // A filename of a file in the files directory.
        public const int FieldTypeFloat = 12;        // A float number
        public const int FieldTypeAutoIncrement = 13; // long that automatically increments with the new record
        public const int FieldTypeManyToMany = 14;    // no database field - sets up a relationship through a Rule table to another table
        public const int FieldTypeMemberSelect = 15; // This ID is a ccMembers record in a group defined by the MemberSelectGroupID field
        public const int FieldTypeCSSFile = 16;      // A filename of a CSS compatible file
        public const int FieldTypeXMLFile = 17;      // the filename of an XML compatible file
        public const int FieldTypeJavascriptFile = 18; // the filename of a javascript compatible file
        public const int FieldTypeLink = 19;           // Links used in href tags -- can go to pages or resources
        public const int FieldTypeResourceLink = 20;   // Links used in resources, link <img or <object. Should not be pages
        public const int FieldTypeHTML = 21;           // LongText field that expects HTML content
        public const int FieldTypeHTMLFile = 22;       // TextFile field that expects HTML content
        public const int FieldTypeMax = 22;
        // 
        // ----- Field Descriptors for these type
        // These are what are publicly displayed for each type
        // See GetFieldDescriptorByType and vise-versa to translater
        // 
        public const string FieldDescriptorInteger = "Integer";
        public const string FieldDescriptorText = "Text";
        public const string FieldDescriptorLongText = "LongText";
        public const string FieldDescriptorBoolean = "Boolean";
        public const string FieldDescriptorDate = "Date";
        public const string FieldDescriptorFile = "File";
        public const string FieldDescriptorLookup = "Lookup";
        public const string FieldDescriptorRedirect = "Redirect";
        public const string FieldDescriptorCurrency = "Currency";
        public const string FieldDescriptorImage = "Image";
        public const string FieldDescriptorFloat = "Float";
        public const string FieldDescriptorManyToMany = "ManyToMany";
        public const string FieldDescriptorTextFile = "TextFile";
        public const string FieldDescriptorCSSFile = "CSSFile";
        public const string FieldDescriptorXMLFile = "XMLFile";
        public const string FieldDescriptorJavascriptFile = "JavascriptFile";
        public const string FieldDescriptorLink = "Link";
        public const string FieldDescriptorResourceLink = "ResourceLink";
        public const string FieldDescriptorMemberSelect = "MemberSelect";
        public const string FieldDescriptorHTML = "HTML";
        public const string FieldDescriptorHTMLFile = "HTMLFile";
        // 
        public const string FieldDescriptorLcaseInteger = "integer";
        public const string FieldDescriptorLcaseText = "text";
        public const string FieldDescriptorLcaseLongText = "longtext";
        public const string FieldDescriptorLcaseBoolean = "boolean";
        public const string FieldDescriptorLcaseDate = "date";
        public const string FieldDescriptorLcaseFile = "file";
        public const string FieldDescriptorLcaseLookup = "lookup";
        public const string FieldDescriptorLcaseRedirect = "redirect";
        public const string FieldDescriptorLcaseCurrency = "currency";
        public const string FieldDescriptorLcaseImage = "image";
        public const string FieldDescriptorLcaseFloat = "float";
        public const string FieldDescriptorLcaseManyToMany = "manytomany";
        public const string FieldDescriptorLcaseTextFile = "textfile";
        public const string FieldDescriptorLcaseCSSFile = "cssfile";
        public const string FieldDescriptorLcaseXMLFile = "xmlfile";
        public const string FieldDescriptorLcaseJavascriptFile = "javascriptfile";
        public const string FieldDescriptorLcaseLink = "link";
        public const string FieldDescriptorLcaseResourceLink = "resourcelink";
        public const string FieldDescriptorLcaseMemberSelect = "memberselect";
        public const string FieldDescriptorLcaseHTML = "html";
        public const string FieldDescriptorLcaseHTMLFile = "htmlfile";
        // 
        // ------------------------------------------------------------------------
        // ----- Payment Options
        // ------------------------------------------------------------------------
        // 
        public const int PayTypeCreditCardOnline = 0;   // Pay by credit card online
        public const int PayTypeCreditCardByPhone = 1;  // Phone in a credit card
        public const int PayTypeCreditCardByFax = 9;    // Phone in a credit card
        public const int PayTypeCHECK = 2;              // pay by check to be mailed
        public const int PayTypeCREDIT = 3;             // pay on account
        public const int PayTypeNONE = 4;               // order total is $0.00. Nothing due
        public const int PayTypeCHECKCOMPANY = 5;       // pay by company check
        public const int PayTypeNetTerms = 6;
        public const int PayTypeCODCompanyCheck = 7;
        public const int PayTypeCODCertifiedFunds = 8;
        public const int PayTypePAYPAL = 10;
        public const int PAYDEFAULT = 0;
        // 
        // ------------------------------------------------------------------------
        // ----- Credit card options
        // ------------------------------------------------------------------------
        // 
        public const int CCTYPEVISA = 0;                // Visa
        public const int CCTYPEMC = 1;                  // MasterCard
        public const int CCTYPEAMEX = 2;                // American Express
        public const int CCTYPEDISCOVER = 3;            // Discover
        public const int CCTYPENOVUS = 4;               // Novus Card
        public const int CCTYPEDEFAULT = 0;
        // 
        // ------------------------------------------------------------------------
        // ----- Shipping Options
        // ------------------------------------------------------------------------
        // 
        public const int SHIPGROUND = 0;                // ground
        public const int SHIPOVERNIGHT = 1;             // overnight
        public const int SHIPSTANDARD = 2;              // standard, whatever that is
        public const int SHIPOVERSEAS = 3;              // to overseas
        public const int SHIPCANADA = 4;                // to Canada
        public const int SHIPDEFAULT = 0;
        // 
        // ------------------------------------------------------------------------
        // Debugging info
        // ------------------------------------------------------------------------
        // 
        public const int TestPointTab = 2;
        public const string TestPointTabChr = "-";
        public static double CPTickCountBase;
        // 
        // ------------------------------------------------------------------------
        // project width button defintions
        // ------------------------------------------------------------------------
        // 
        public const string ButtonApply = "  Apply ";
        public const string ButtonLogin = "  Login  ";
        public const string ButtonLogout = "  Logout  ";
        public const string ButtonSendPassword = "  Send Password  ";
        public const string ButtonJoin = "   Join   ";
        public const string ButtonSave = "  Save  ";
        public const string ButtonOK = "     OK     ";
        public const string ButtonReset = "  Reset  ";
        public const string ButtonSaveAddNew = " Save + Add ";
        // Public Const ButtonSaveAddNew = " Save > Add "
        public const string ButtonCancel = " Cancel ";
        public const string ButtonRestartContensiveApplication = " Restart Contensive Application ";
        public const string ButtonCancelAll = "  Cancel  ";
        public const string ButtonFind = "   Find   ";
        public const string ButtonDelete = "  Delete  ";
        public const string ButtonDeletePerson = " Delete Person ";
        public const string ButtonDeleteRecord = " Delete Record ";
        public const string ButtonDeleteEmail = " Delete Email ";
        public const string ButtonDeletePage = " Delete Page ";
        public const string ButtonFileChange = "   Upload   ";
        public const string ButtonFileDelete = "    Delete    ";
        public const string ButtonClose = "  Close   ";
        public const string ButtonAdd = "   Add    ";
        public const string ButtonAddChildPage = " Add Child ";
        public const string ButtonAddSiblingPage = " Add Sibling ";
        public const string ButtonContinue = " Continue >> ";
        public const string ButtonBack = "  << Back  ";
        public const string ButtonNext = "   Next   ";
        public const string ButtonPrevious = " Previous ";
        public const string ButtonFirst = "  First   ";
        public const string ButtonSend = "  Send   ";
        public const string ButtonSendTest = "Send Test";
        public const string ButtonCreateDuplicate = " Create Duplicate ";
        public const string ButtonActivate = "  Activate   ";
        public const string ButtonDeactivate = "  Deactivate   ";
        public const string ButtonOpenActiveEditor = "Active Edit";
        public const string ButtonPublish = " Publish Changes ";
        public const string ButtonAbortEdit = " Abort Edits ";
        public const string ButtonPublishSubmit = " Submit for Publishing ";
        public const string ButtonPublishApprove = " Approve for Publishing ";
        public const string ButtonPublishDeny = " Deny for Publishing ";
        public const string ButtonWorkflowPublishApproved = " Publish Approved Records ";
        public const string ButtonWorkflowPublishSelected = " Publish Selected Records ";
        public const string ButtonSetHTMLEdit = " Edit WYSIWYG ";
        public const string ButtonSetTextEdit = " Edit HTML ";
        public const string ButtonRefresh = " Refresh ";
        public const string ButtonOrder = " Order ";
        public const string ButtonSearch = " Search ";
        public const string ButtonSpellCheck = " Spell Check ";
        public const string ButtonLibraryUpload = " Upload ";
        public const string ButtonCreateReport = " Create Report ";
        public const string ButtonClearTrapLog = " Clear Trap Log ";
        public const string ButtonNewSearch = " New Search ";
        public const string ButtonReloadCDef = " Reload Content Definitions ";
        public const string ButtonImportTemplates = " Import Templates ";
        public const string ButtonRSSRefresh = " Update RSS Feeds Now ";
        public const string ButtonRequestDownload = " Request Download ";
        public const string ButtonFinish = " Finish ";
        public const string ButtonRegister = " Register ";
        public const string ButtonBegin = "Begin";
        public const string ButtonAbort = "Abort";
        public const string ButtonCreateGUID = " Create GUID ";
        public const string ButtonEnable = " Enable ";
        public const string ButtonDisable = " Disable ";
        public const string ButtonMarkReviewed = " Mark Reviewed ";
        // 
        // ------------------------------------------------------------------------
        // member actions
        // ------------------------------------------------------------------------
        // 
        public const int MemberActionNOP = 0;
        public const int MemberActionLogin = 1;
        public const int MemberActionLogout = 2;
        public const int MemberActionForceLogin = 3;
        public const int MemberActionSendPassword = 4;
        public const int MemberActionForceLogout = 5;
        public const int MemberActionToolsApply = 6;
        public const int MemberActionJoin = 7;
        public const int MemberActionSaveProfile = 8;
        public const int MemberActionEditProfile = 9;
        // 
        // -----------------------------------------------------------------------
        // ----- note pad info
        // -----------------------------------------------------------------------
        // 
        public const int NoteFormList = 1;
        public const int NoteFormRead = 2;
        // 
        public const string NoteButtonPrevious = " Previous ";
        public const string NoteButtonNext = "   Next   ";
        public const string NoteButtonDelete = "  Delete  ";
        public const string NoteButtonClose = "  Close   ";
        // ' Submit button is created in CommonDim, so it is simple
        public const string NoteButtonSubmit = "Submit";
        // 
        // -----------------------------------------------------------------------
        // ----- Admin site storage
        // -----------------------------------------------------------------------
        // 
        public const int AdminMenuModeHidden = 0;       // menu is hidden
        public const int AdminMenuModeLeft = 1;     // menu on the left
        public const int AdminMenuModeTop = 2;      // menu as dropdowns from the top
                                                    // 
                                                    // ----- AdminActions - describes the form processing to do
                                                    // 
        public const int AdminActionNop = 0;            // do nothing
        public const int AdminActionDelete = 4;         // delete record
        public const int AdminActionFind = 5;           // 
        public const int AdminActionDeleteFilex = 6;        // 
        public const int AdminActionUpload = 7;         // 
        public const int AdminActionSaveNormal = 3;         // save fields to database
        public const int AdminActionSaveEmail = 8;          // save email record (and update EmailGroups) to database
        public const int AdminActionSaveMember = 11;        // 
        public const int AdminActionSaveSystem = 12;
        public const int AdminActionSavePaths = 13;     // Save a record that is in the BathBlocking Format
        public const int AdminActionSendEmail = 9;          // 
        public const int AdminActionSendEmailTest = 10;     // 
        public const int AdminActionNext = 14;               // 
        public const int AdminActionPrevious = 15;           // 
        public const int AdminActionFirst = 16;              // 
        public const int AdminActionSaveContent = 17;        // 
        public const int AdminActionSaveField = 18;          // Save a single field, fieldname = fn input
        public const int AdminActionPublish = 19;            // Publish record live
        public const int AdminActionAbortEdit = 20;          // Publish record live
        public const int AdminActionPublishSubmit = 21;      // Submit for Workflow Publishing
        public const int AdminActionPublishApprove = 22;     // Approve for Workflow Publishing
        public const int AdminActionWorkflowPublishApproved = 23;    // Publish what was approved
        public const int AdminActionSetHTMLEdit = 24;        // Set Member Property for this field to HTML Edit
        public const int AdminActionSetTextEdit = 25;        // Set Member Property for this field to Text Edit
        public const int AdminActionSave = 26;               // Save Record
        public const int AdminActionActivateEmail = 27;      // Activate a Conditional Email
        public const int AdminActionDeactivateEmail = 28;    // Deactivate a conditional email
        public const int AdminActionDuplicate = 29;          // Duplicate the (sent email) record
        public const int AdminActionDeleteRows = 30;         // Delete from rows of records, row0 is boolean, rowid0 is ID, rowcnt is count
        public const int AdminActionSaveAddNew = 31;         // Save Record and add a new record
        public const int AdminActionReloadCDef = 32;         // Load Content Definitions
        public const int AdminActionWorkflowPublishSelected = 33; // Publish what was selected
        public const int AdminActionMarkReviewed = 34;       // Mark the record reviewed without making any changes
        public const int AdminActionEditRefresh = 35;        // reload the page just like a save, but do not save
                                                             // 
                                                             // ----- Adminforms (0-99)
                                                             // 
        public const int AdminFormRoot = 0;             // intro page
        public const int AdminFormIndex = 1;            // record list page
        public const int AdminFormHelp = 2;             // popup help window
        public const int AdminFormUpload = 3;           // encoded file upload form
        public const int AdminFormEdit = 4;             // Edit form for system format records
        public const int AdminFormEditSystem = 5;       // Edit form for system format records
        public const int AdminFormEditNormal = 6;       // record edit page
        public const int AdminFormEditEmail = 7;        // Edit form for Email format records
        public const int AdminFormEditMember = 8;       // Edit form for Member format records
        public const int AdminFormEditPaths = 9;        // Edit form for Paths format records
        public const int AdminFormClose = 10;           // Special Case - do a window close instead of displaying a form
        public const int AdminFormReports = 12;         // Call Reports form (admin only)
                                                        // Public Const AdminFormSpider = 13          ' Call Spider
        public const int AdminFormEditContent = 14;     // Edit form for Content records
        public const int AdminFormDHTMLEdit = 15;       // ActiveX DHTMLEdit form
        public const int AdminFormEditPageContent = 16; // 
        public const int AdminFormPublishing = 17;       // Workflow Authoring Publish Control form
        public const int AdminFormQuickStats = 18;       // Quick Stats (from Admin root)
        public const int AdminFormResourceLibrary = 19;  // Resource Library without Selects
        public const int AdminFormEDGControl = 20;       // Control Form for the EDG publishing controls
        public const int AdminFormSpiderControl = 21;    // Control Form for the Content Spider
        public const int AdminFormContentChildTool = 22; // Admin Create Content Child tool
        public const int AdminformPageContentMap = 23;   // Map all content to a single map
        public const int AdminformHousekeepingControl = 24; // Housekeeping control
        public const int AdminFormCommerceControl = 25;
        public const int AdminFormContactManager = 26;
        public const int AdminFormStyleEditor = 27;
        public const int AdminFormEmailControl = 28;
        public const int AdminFormCommerceInterface = 29;
        public const int AdminFormDownloads = 30;
        public const int AdminformRSSControl = 31;
        public const int AdminFormMeetingSmart = 32;
        public const int AdminFormMemberSmart = 33;
        public const int AdminFormEmailWizard = 34;
        public const int AdminFormImportWizard = 35;
        public const int AdminFormCustomReports = 36;
        public const int AdminFormFormWizard = 37;
        public const int AdminFormLegacyAddonManager = 38;
        public const int AdminFormIndex_SubFormAdvancedSearch = 39;
        public const int AdminFormIndex_SubFormSetColumns = 40;
        public const int AdminFormPageControl = 41;
        public const int AdminFormSecurityControl = 42;
        public const int AdminFormEditorConfig = 43;
        public const int AdminFormBuilderCollection = 44;
        public const int AdminFormClearCache = 45;
        public const int AdminFormMobileBrowserControl = 46;
        public const int AdminFormMetaKeywordTool = 47;
        public const int AdminFormIndex_SubFormExport = 48;
        // 
        // ----- AdminFormTools (11,100-199)
        // 
        public const int AdminFormTools = 11;           // Call Tools form (developer only)
        public const int AdminFormToolRoot = 11;         // These should match for compatibility
        public const int AdminFormToolCreateContentDefinition = 101;
        public const int AdminFormToolContentTest = 102;
        public const int AdminFormToolConfigureMenu = 103;
        public const int AdminFormToolConfigureListing = 104;
        public const int AdminFormToolConfigureEdit = 105;
        public const int AdminFormToolManualQuery = 106;
        public const int AdminFormToolWriteUpdateMacro = 107;
        public const int AdminFormToolDuplicateContent = 108;
        public const int AdminFormToolDuplicateDataSource = 109;
        public const int AdminFormToolDefineContentFieldsFromTable = 110;
        public const int AdminFormToolContentDiagnostic = 111;
        public const int AdminFormToolCreateChildContent = 112;
        public const int AdminFormToolClearContentWatchLink = 113;
        public const int AdminFormToolSyncTables = 114;
        public const int AdminFormToolBenchmark = 115;
        public const int AdminFormToolSchema = 116;
        public const int AdminFormToolContentFileView = 117;
        public const int AdminFormToolDbIndex = 118;
        public const int AdminFormToolContentDbSchema = 119;
        public const int AdminFormToolLogFileView = 120;
        public const int AdminFormToolLoadCDef = 121;
        public const int AdminFormToolLoadTemplates = 122;
        public const int AdminformToolFindAndReplace = 123;
        public const int AdminformToolCreateGUID = 124;
        public const int AdminformToolIISReset = 125;
        public const int AdminFormToolRestart = 126;
        public const int AdminFormToolWebsiteFileView = 127;
        // 
        // ----- Define the index column structure
        // IndexColumnVariant( 0, n ) is the first column on the left
        // IndexColumnVariant( 0, IndexColumnField ) = the index into the fields array
        // 
        public const int IndexColumnField = 0;          // The field displayed in the column
        public const int IndexColumnWIDTH = 1;          // The width of the column
        public const int IndexColumnSORTPRIORITY = 2;       // lowest columns sorts first
        public const int IndexColumnSORTDIRECTION = 3;      // direction of the sort on this column
        public const int IndexColumnSATTRIBUTEMAX = 3;      // the number of attributes here
        public const int IndexColumnsMax = 50;
        // 
        // ----- ReportID Constants, moved to ccCommonModule
        // 
        public const int ReportFormRoot = 1;
        public const int ReportFormDailyVisits = 2;
        public const int ReportFormWeeklyVisits = 12;
        public const int ReportFormSitePath = 4;
        public const int ReportFormSearchKeywords = 5;
        public const int ReportFormReferers = 6;
        public const int ReportFormBrowserList = 8;
        public const int ReportFormAddressList = 9;
        public const int ReportFormContentProperties = 14;
        public const int ReportFormSurveyList = 15;
        public const int ReportFormOrdersList = 13;
        public const int ReportFormOrderDetails = 21;
        public const int ReportFormVisitorList = 11;
        public const int ReportFormMemberDetails = 16;
        public const int ReportFormPageList = 10;
        public const int ReportFormVisitList = 3;
        public const int ReportFormVisitDetails = 17;
        public const int ReportFormVisitorDetails = 20;
        public const int ReportFormSpiderDocList = 22;
        public const int ReportFormSpiderErrorList = 23;
        public const int ReportFormEDGDocErrors = 24;
        public const int ReportFormDownloadLog = 25;
        public const int ReportFormSpiderDocDetails = 26;
        public const int ReportFormSurveyDetails = 27;
        public const int ReportFormEmailDropList = 28;
        public const int ReportFormPageTraffic = 29;
        public const int ReportFormPagePerformance = 30;
        public const int ReportFormEmailDropDetails = 31;
        public const int ReportFormEmailOpenDetails = 32;
        public const int ReportFormEmailClickDetails = 33;
        public const int ReportFormGroupList = 34;
        public const int ReportFormGroupMemberList = 35;
        public const int ReportFormTrapList = 36;
        public const int ReportFormCount = 36;
        // 
        // =============================================================================
        // Page Scope Meetings Related Storage
        // =============================================================================
        // 
        public const int MeetingFormIndex = 0;
        public const int MeetingFormAttendees = 1;
        public const int MeetingFormLinks = 2;
        public const int MeetingFormFacility = 3;
        public const int MeetingFormHotel = 4;
        public const int MeetingFormDetails = 5;
        // 
        // ------------------------------------------------------------------------------
        // Form actions
        // ------------------------------------------------------------------------------
        // 
        // ----- DataSource Types
        // 
        public const int DataSourceTypeODBCSQL99 = 0;
        public const int DataSourceTypeODBCAccess = 1;
        public const int DataSourceTypeODBCSQLServer = 2;
        public const int DataSourceTypeODBCMySQL = 3;
        public const int DataSourceTypeXMLFile = 4;      // Use MSXML Interface to open a file
                                                         // 
                                                         // ------------------------------------------------------------------------------
                                                         // Application Status
                                                         // ------------------------------------------------------------------------------
                                                         // 
        public const int ApplicationStatusNotFound = 0;
        public const int ApplicationStatusLoadedNotRunning = 1;
        public const int ApplicationStatusRunning = 2;
        public const int ApplicationStatusStarting = 3;
        public const int ApplicationStatusUpgrading = 4;
        // Public Const ApplicationStatusConnectionBusy = 5    ' can not open connection because already open
        public const int ApplicationStatusKernelFailure = 6;     // can not create Kernel
        public const int ApplicationStatusNoHostService = 7;     // host service process ID not set
        public const int ApplicationStatusLicenseFailure = 8;    // failed to start because of License failure
        public const int ApplicationStatusDbFailure = 9;         // failed to start because ccSetup table not found
        public const int ApplicationStatusUnknownFailure = 10;   // failed to start because of unknown error, see trace log
        public const int ApplicationStatusDbBad = 11;            // ccContent,ccFields no records found
        public const int ApplicationStatusConnectionObjectFailure = 12; // Connection Object FAiled
        public const int ApplicationStatusConnectionStringFailure = 13; // Connection String FAiled to open the ODBC connection
        public const int ApplicationStatusDataSourceFailure = 14; // DataSource failed to open
        public const int ApplicationStatusDuplicateDomains = 15; // Can not locate application because there are 1+ apps that match the domain
        public const int ApplicationStatusPaused = 16;           // Running, but all activity is blocked (for backup)
                                                                 // 
                                                                 // Document (HTML, graphic, etc) retrieved from site
                                                                 // 
        public const int ResponseHeaderCountMax = 20;
        public const int ResponseCookieCountMax = 20;
        // 
        // ----- text delimiter that divides the text and html parts of an email message stored in the queue folder
        // 
        public const string EmailTextHTMLDelimiter = Constants.vbCrLf + " ----- End Text Begin HTML -----" + Constants.vbCrLf;
        // 
        // ------------------------------------------------------------------------
        // Common RequestName Variables
        // ------------------------------------------------------------------------
        // 
        public const string RequestNameDynamicFormID = "dformid";
        // 
        public const string RequestNameRunAddon = "addonid";
        public const string RequestNameEditReferer = "EditReferer";
        public const string RequestNameRefreshBlock = "ccFormRefreshBlockSN";
        public const string RequestNameCatalogOrder = "CatalogOrderID";
        public const string RequestNameCatalogCategoryID = "CatalogCatID";
        public const string RequestNameCatalogForm = "CatalogFormID";
        public const string RequestNameCatalogItemID = "CatalogItemID";
        public const string RequestNameCatalogItemAge = "CatalogItemAge";
        public const string RequestNameCatalogRecordTop = "CatalogTop";
        public const string RequestNameCatalogFeatured = "CatalogFeatured";
        public const string RequestNameCatalogSpan = "CatalogSpan";
        public const string RequestNameCatalogKeywords = "CatalogKeywords";
        public const string RequestNameCatalogSource = "CatalogSource";
        // 
        public const string RequestNameLibraryFileID = "fileEID";
        public const string RequestNameDownloadID = "downloadid";
        public const string RequestNameLibraryUpload = "LibraryUpload";
        public const string RequestNameLibraryName = "LibraryName";
        public const string RequestNameLibraryDescription = "LibraryDescription";

        public const string RequestNameTestHook = "CC";       // input request that sets debugging hooks

        public const string RequestNameRootPage = "RootPageName";
        public const string RequestNameRootPageID = "RootPageID";
        public const string RequestNameContent = "ContentName";
        public const string RequestNameOrderByClause = "OrderByClause";
        public const string RequestNameAllowChildPageList = "AllowChildPageList";
        // 
        public const string RequestNameCRKey = "crkey";
        public const string RequestNameAdminForm = "af";
        public const string RequestNameAdminSubForm = "subform";
        public const string RequestNameButton = "button";
        public const string RequestNameAdminSourceForm = "asf";
        public const string RequestNameAdminFormSpelling = "SpellingRequest";
        public const string RequestNameInlineStyles = "InlineStyles";
        public const string RequestNameAllowCSSReset = "AllowCSSReset";
        // 
        public const string RequestNameReportForm = "rid";
        // 
        public const string RequestNameToolContentID = "ContentID";
        // 
        public const string RequestNameCut = "a904o2pa0cut";
        public const string RequestNamePaste = "dp29a7dsa6paste";
        public const string RequestNamePasteParentContentID = "dp29a7dsa6cid";
        public const string RequestNamePasteParentRecordID = "dp29a7dsa6rid";
        public const string RequestNamePasteFieldList = "dp29a7dsa6key";
        public const string RequestNameCutClear = "dp29a7dsa6clear";
        // 
        public const string RequestNameRequestBinary = "RequestBinary";
        // removed -- this was an old method of blocking form input for file uploads
        // Public Const RequestNameFormBlock = "RB"
        public const string RequestNameJSForm = "RequestJSForm";
        public const string RequestNameJSProcess = "ProcessJSForm";
        // 
        public const string RequestNameFolderID = "FolderID";
        // 
        public const string RequestNameEmailMemberID = "emi8s9Kj";
        public const string RequestNameEmailOpenFlag = "eof9as88";
        public const string RequestNameEmailOpenCssFlag = "8aa41pM3";
        public const string RequestNameEmailClickFlag = "ecf34Msi";
        public const string RequestNameEmailSpamFlag = "9dq8Nh61";
        public const string RequestNameEmailBlockRequestDropID = "BlockEmailRequest";
        public const string RequestNameVisitTracking = "s9lD1088";
        public const string RequestNameBlockContentTracking = "BlockContentTracking";
        public const string RequestNameCookieDetectVisitID = "f92vo2a8d";

        public const string RequestNamePageNumber = "PageNumber";
        public const string RequestNamePageSize = "PageSize";
        // 
        public const string RequestValueNull = "[NULL]";
        // 
        public const string SpellCheckUserDictionaryFilename = @"SpellCheck\UserDictionary.txt";
        // 
        public const string RequestNameStateString = "vstate";
        public const int Version = 1;
        // 
        // -- sample
        public const string rnInputValue = "inputValue";
        // 
        // -- errors for resultErrList
        public enum resultErrorEnum {
            errPermission = 50,
            errDuplicate = 100,
            errVerification = 110,
            errRestriction = 120,
            errInput = 200,
            errAuthentication = 300,
            errAdd = 400,
            errSave = 500,
            errDelete = 600,
            errLookup = 700,
            errLoad = 710,
            errContent = 800,
            errMiscellaneous = 900
        }
        // 
        // -- http errors
        public enum httpErrorEnum {
            badRequest = 400,
            unauthorized = 401,
            paymentRequired = 402,
            forbidden = 403,
            notFound = 404,
            methodNotAllowed = 405,
            notAcceptable = 406,
            proxyAuthenticationRequired = 407,
            requestTimeout = 408,
            conflict = 409,
            gone = 410,
            lengthRequired = 411,
            preconditionFailed = 412,
            payloadTooLarge = 413,
            urlTooLong = 414,
            unsupportedMediaType = 415,
            rangeNotSatisfiable = 416,
            expectationFailed = 417,
            teapot = 418,
            methodFailure = 420,
            enhanceYourCalm = 420,
            misdirectedRequest = 421,
            unprocessableEntity = 422,
            locked = 423,
            failedDependency = 424,
            upgradeRequired = 426,
            preconditionRequired = 428,
            tooManyRequests = 429,
            requestHeaderFieldsTooLarge = 431,
            loginTimeout = 440,
            noResponse = 444,
            retryWith = 449,
            redirect = 451,
            unavailableForLegalReasons = 451,
            sslCertificateError = 495,
            sslCertificateRequired = 496,
            httpRequestSentToSecurePort = 497,
            invalidToken = 498,
            clientClosedRequest = 499,
            tokenRequired = 499,
            internalServerError = 500
        }
    }
}