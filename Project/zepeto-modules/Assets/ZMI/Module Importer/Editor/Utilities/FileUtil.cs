using System.IO;
using UnityEditor;
using zmi.Constant;
using zmi.Internal;

namespace zmi.Utilities
{
    public static class FileUtil
    {
        // Delete a directory
        public static void DeleteDirectory(string directoryPath)
        {
            AssetDatabase.DeleteAsset(directoryPath);
        }

        public static bool CheckIfDirectoryExist(string moduleName)
        {
            DirectoryPath paths = GenerateFolderPaths(moduleName);
            if (Directory.Exists(paths.oldPath) || Directory.Exists(paths.newPath))
            {
                return true;
            }
            return false;
        }
        
        public static DirectoryPath GenerateFolderPaths(string moduleName)
        {
            string oldModuleName = StringUtil.renameModule(moduleName);

            return new DirectoryPath(Path.Combine(ModulePath.ASSET_DIRECTORY, oldModuleName),
                Path.Combine(ModulePath.ROOT_DIRECTORY, moduleName));
        }
    }
}