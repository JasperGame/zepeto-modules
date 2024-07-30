namespace zmi.Constant
{
    public class ModuleStrings
    {
        public const string VERSION_UPPERCASE = "VERSION";
        public const string VERSION_CAPITAL = "Version";
        public const string UNKNOWN_VERSION = "UNKNOWN";
        public const string MODULE_IMPORTER = "Module Importer";
        
        /* LANGUAGE */
        public const string LANGUAGE_EN = "lang-en";
        public const string LANGUAGE_KR = "lang_kr";
        
        /*DIALOG TEXT */
        public const string DIALOG_REMOVE_MESSAGE = "The following folder will be removed: ";
        public const string DIALOG_REMOVE_CONFIRM_BUTTON = "Remove";
        public const string DIALOG_REMOVE_CANCEL_BUTTON = "Cancel";
        public const string DIALOG_OK_BUTTON = "Ok";
        public const string DIALOG_REMOVE_DEPENDENCY_MESSAGE = "You first need to remove the following dependencies: ";
        public const string DIALOG_UPDATE_MESSAGE = "The following folder will be updated: ";


        
        /* UI BUTTON */
        public const string UI_BUTTON_REMOVE = "Remove";
        public const string UI_BUTTON_IMPORT = "Import ";
        public const string UI_BUTTON_VIEW_IMPORT_GUIDE = "View Import Guide";
        public const string UI_TEXT_REPORT_ISSUE = "Report Issue";
        public const string UI_TEXT_CONTRIBUTE = "Contribute";
        
        /* UI TEXT */
        public const string UI_MENU_ITEM = "ZEPETO/Module Importer";
        public const string UI_TEXT_ZEPETO_MODULE_IMPORTER = "ZEPETO Module Importer";
        public const string UI_TEXT_SEE_OTHER_VERSION = "See other version";
        public const string UI_TEXT_API_DOCS = "API Docs";
        public const string UI_TEXT_FREQUENTLY_USED_MODULES = " Easily add frequently used modules.";
        public const string UI_TEXT_LATEST_UPDATE = "Last Update : ";
        public const string UI_TEXT_DEPENDENCIES = "Dependencies";
        public const string UI_TEXT_IS_USING = "Is Using";
        public const string UI_TEXT_PREVIEW = "Preview";
        public const string UI_TEXT_DOWNLOADING_PACKAGE = "Downloading Package";
        

        /* UI ICON */
        public const string UI_ICON_LATEST_MODULE_VERSION = "d_winbtn_mac_max";
        public const string UI_ICON_OLD_MODULE_VERSION = "d_winbtn_mac_min";
        public const string UI_ICON_UNSUPPORTED_MODULE_VERSION = "d_winbtn_mac_close";
        public const string UI_ICON_REFRESH = "d_Refresh";
        
        /* FILE EXTENSION */
        public const string EXTENSION_MD = ".md";
        public const string EXTENSION_UNITYPACKAGE = ".unitypackage";
        
        
        /*  ERROR TITLE */
        public const string ERR_TITLE_MODULE_FOLDER_NOT_FOUND = "Module Folder not found";
        
        
        /* ERROR MESSAGE */
        public const string ERR_MESSAGE_FAILED_TO_DOWNLOAD_IMAGE = "Failed to download image.";
        public const string ERR_MESSAGE_FOLDER_DOESNT_EXIST =
                "The folder you are tying to remove doesn't exist. Make sure the module folder is named correctly.";
    }
}