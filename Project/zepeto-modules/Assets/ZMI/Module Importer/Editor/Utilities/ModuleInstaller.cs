using System.Collections;
using System.IO;
using System.Net;
using Unity.EditorCoroutines.Editor;
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

        public static void RemoveModule(string moduleName)
        {
            DirectoryPath directoryPaths = FileUtil.GenerateFolderPaths(moduleName);
  
            // Returns an error message if it could not find the directory to remove
            if (!FileUtil.CheckIfDirectoryExist(moduleName))
            {
                if (UiHandler.ErrorDialog(ModuleStrings.ERR_TITLE_MODULE_FOLDER_NOT_FOUND,
                        ModuleStrings.ERR_MESSAGE_FOLDER_DOESNT_EXIST)) return;
            }
            
            //  Remove the Directory if it is located in 'Assets/ZMI"
            DeleteModulePath(directoryPaths.newPath, moduleName, ModuleStrings.DIALOG_REMOVE_MESSAGE,  ModuleStrings.DIALOG_REMOVE_CONFIRM_BUTTON, false);
            
            // Remove the Directory if it is located in "Assets/"
            DeleteModulePath(directoryPaths.oldPath, moduleName,ModuleStrings.DIALOG_REMOVE_MESSAGE,ModuleStrings.DIALOG_REMOVE_CONFIRM_BUTTON, false);

        }

        private static bool DeleteModulePath(string modulePath, string moduleName, string message, string confirmButtonText, bool isUpdate )
        {
            if (Directory.Exists(modulePath))
            {
                if(UiHandler.TryDialog(StringUtil.renameModule(moduleName), StringUtil.GenerateDialogMessage(message, new []{modulePath},false), confirmButtonText, ModuleStrings.DIALOG_REMOVE_CANCEL_BUTTON))
                {
                    FileUtil.DeleteDirectory(modulePath);
                    if (isUpdate)
                    {
                        return true;
                    }
                }
                
            }

            return false;
        }

        public static void UpdateModule(string moduleName, string version, object owner)
        {
            DirectoryPath directoryPaths = FileUtil.GenerateFolderPaths(moduleName);

            if (!FileUtil.CheckIfDirectoryExist(moduleName))
            {
                if (UiHandler.ErrorDialog(ModuleStrings.ERR_TITLE_MODULE_FOLDER_NOT_FOUND,
                        ModuleStrings.ERR_MESSAGE_FOLDER_DOESNT_EXIST));
            }
            EditorApplication.LockReloadAssemblies();

            if (DeleteModulePath(directoryPaths.newPath, moduleName,ModuleStrings.DIALOG_UPDATE_MESSAGE, ModuleStrings.DIALOG_UPDATE_CONFIRM_BUTTON, true) ||
                DeleteModulePath(directoryPaths.oldPath, moduleName,ModuleStrings.DIALOG_REMOVE_MESSAGE, ModuleStrings.DIALOG_UPDATE_CONFIRM_BUTTON, true))
            {
                EditorCoroutineUtility.StartCoroutine(ImportModule(moduleName, version), owner);
            }
            EditorApplication.UnlockReloadAssemblies();
        }
        
        // this method checks if the module is removable
        public static bool IsRemovable(string version, string moduleName)
        {
            if (moduleName != ModuleStrings.MODULE_IMPORTER &&
                version != ModuleStrings.UNKNOWN_VERSION )
            {
                return true;
            }

            return false;
        }

        public static bool isUpdatable(string version, string latestVersion)
        {
            if (version == latestVersion)
            {
                return false;
            }

            return true;
        }
    }
}