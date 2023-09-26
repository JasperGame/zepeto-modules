
using System;
using System.Reflection;
using zmi.Constant;

namespace zmi.Utilities
{
    public class VersionHandler
    {
        public static string VersionCheck(string className)
        {
            string downloadedVersion = ModuleStrings.UNKNOWN_VERSION;

            Type type = GetTypeByName(className + ModuleStrings.VERSION_CAPITAL);

            if (type != null)
            {
                FieldInfo field = type.GetField(ModuleStrings.VERSION_UPPERCASE, BindingFlags.Static | BindingFlags.Public);

                if (field != null)
                {
                    downloadedVersion = (string)field.GetValue(null);
                }
            }
            return downloadedVersion;
        }

        private static Type GetTypeByName(string className)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type type = assembly.GetType(className);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
