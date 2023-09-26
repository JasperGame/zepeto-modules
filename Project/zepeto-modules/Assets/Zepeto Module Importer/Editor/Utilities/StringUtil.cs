using zmi.Constant;

using UnityEngine;
namespace zmi.Utilities
{
    public class StringUtil 
    {
        internal static string GetRemoveSpace(string anyString)
        {
            return anyString.Replace(" ", "");
        }

        internal static string renameModule(string moduleName)
        {
            if( !moduleName.Contains("Multiplay") && moduleName.Contains("Module"))
            {
                return "Zepeto " + moduleName + " Module";
            }
            return "Zepeto " + moduleName;
        }

        internal static string GenerateDialogMessage(string message, string[] modulePath, bool isList)
        {
            if (isList)
            {
                foreach (var dependencies in modulePath)
                {
                    message = message + "\n- " + dependencies;
                }

                return message;
            }
            return message + "\n- " + modulePath[0];
        }
    }
}