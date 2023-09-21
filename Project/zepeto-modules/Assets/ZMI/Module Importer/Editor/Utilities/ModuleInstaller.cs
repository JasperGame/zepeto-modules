using System.Collections;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;
using zmi.Internal;
using zmi.UI;
using zmi.Constant;

namespace zmi.Utilities
{
    public class ModuleInstaller
    {
        public static IEnumerator ImportModule(string title, string version)
        {
            string downloadUrl = Path.Combine(ModulePath.DOWNLOAD_PATH, title,
                version + ModuleStrings.EXTENSION_UNITYPACKAGE);
            Debug.Log(downloadUrl);

            string tempFilePath = Path.Combine(Application.temporaryCachePath,
                title + ModuleStrings.EXTENSION_UNITYPACKAGE);
            using (var webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    float progress = (float)e.BytesReceived / (float)e.TotalBytesToReceive;
                    EditorUtility.DisplayProgressBar(ModuleStrings.UI_TEXT_DOWNLOADING_PACKAGE, $"{(progress * 100f):F1}%", progress);
                };

                webClient.DownloadFileCompleted += (sender, e) =>
                {
                    EditorUtility.ClearProgressBar();
                    AssetDatabase.ImportPackage(tempFilePath, true);
                    File.Delete(tempFilePath);
                };

                yield return webClient.DownloadFileTaskAsync(downloadUrl, tempFilePath);
            }
        }

        public static void RemoveModule(string moduleName, bool isUpdate)
        {
            DirectoryPath directoryPaths = FileUtil.GenerateFolderPaths(moduleName);
            string message = ModuleStrings.DIALOG_REMOVE_MESSAGE ;
            
            if (isUpdate)
            {
                message = ModuleStrings.DIALOG_UPDATE_MESSAGE;
            }
            
            // Returns an error message if it could not find the directory to remove
            if (!FileUtil.CheckIfDirectoryExist(moduleName))
            {
                if (UiHandler.ErrorDialog(ModuleStrings.ERR_TITLE_MODULE_FOLDER_NOT_FOUND,
                        ModuleStrings.ERR_MESSAGE_FOLDER_DOESNT_EXIST)) return;
            }
            
            //  Remove the Directory if it is located in 'Assets/ZMI"
            if (Directory.Exists(directoryPaths.newPath))
            {
                
                if(UiHandler.TryDialog(StringUtil.renameModule(moduleName), StringUtil.GenerateDialogMessage(message, new []{directoryPaths.newPath},false), ModuleStrings.DIALOG_REMOVE_CONFIRM_BUTTON, ModuleStrings.DIALOG_REMOVE_CANCEL_BUTTON))
                {
                    FileUtil.DeleteDirectory(directoryPaths.newPath);
                }
            }
            // Remove the Directory if it is located in "Assets/"
            if (Directory.Exists(directoryPaths.oldPath))
            {
                
                if(UiHandler.TryDialog(StringUtil.renameModule(moduleName), StringUtil.GenerateDialogMessage(message, new []{directoryPaths.oldPath},false), ModuleStrings.DIALOG_REMOVE_CONFIRM_BUTTON, ModuleStrings.DIALOG_REMOVE_CANCEL_BUTTON))
                {
                    FileUtil.DeleteDirectory(directoryPaths.oldPath);
                }
            }

        }
        
        // this method checks if the module is removable
        public static bool IsRemovable(string version, Content module)
        {
            if (module.Title != ModuleStrings.MODULE_IMPORTER &&
                version != ModuleStrings.UNKNOWN_VERSION )
            {
                return true;
            }

            return false;
        }
    }
}