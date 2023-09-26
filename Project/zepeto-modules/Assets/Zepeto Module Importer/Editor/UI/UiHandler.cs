using UnityEditor;
using zmi.Constant;

namespace zmi.UI
{
    public class UiHandler
    {

        public static bool TryDialog(string moduleName, string message, string confirmLabel, string cancelLabel)
        {
            return EditorUtility.DisplayDialog(moduleName, message, confirmLabel, cancelLabel);
        }

        public static bool ErrorDialog(string moduleName, string errorMessage)
        {
            return EditorUtility.DisplayDialog(moduleName, errorMessage, ModuleStrings.DIALOG_OK_BUTTON);
        }
    }
}