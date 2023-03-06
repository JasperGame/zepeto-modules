using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;

public class AssetVersionManager : EditorWindow
{
    private const string RepoUrl = "https://github.com/JasperGame/zepeto-modules/releases/latest/download/";

    [MenuItem("Tools/Asset Version Manager")]
    public static void OpenWindow()
    {
        GetWindow<AssetVersionManager>("Asset Version Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Available Asset Versions", EditorStyles.boldLabel);

        // Get the latest release folder from the Github repository
        string latestReleaseFolder = GetLatestReleaseFolder();
        Debug.Log("11111");

        if (string.IsNullOrEmpty(latestReleaseFolder))
        {
            Debug.Log("222222");

            GUILayout.Label("Failed to get available versions.");
            return;
        }

        string[] availableVersions = latestReleaseFolder.Split('-');
        if (availableVersions.Length < 2)
        {
            Debug.Log("33333");

            GUILayout.Label("Failed to get available versions.");
            return;
        }

        string majorVersion = availableVersions[0];
        string minorVersion = availableVersions[1];

        GUILayout.Label($"Latest Version: {majorVersion}.{minorVersion}", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        GUILayout.Label("Install Asset Package", EditorStyles.boldLabel);

        if (GUILayout.Button("Install Latest Version"))
        {
            string downloadUrl = $"{RepoUrl}{latestReleaseFolder}.unitypackage";
            DownloadAndInstallPackage(downloadUrl);
        }

        GUILayout.Space(5f);

        GUILayout.Label("Installed Versions", EditorStyles.boldLabel);

        string[] installedVersions = Directory.GetFiles("Packages", "*.unitypackage");
        if (installedVersions.Length == 0)
        {
            GUILayout.Label("No installed versions.");
            return;
        }

        foreach (string installedVersion in installedVersions)
        {
            GUILayout.Label(installedVersion);
        }
    }

    private static string GetLatestReleaseFolder()
    {
        string apiEndpoint = "https://api.github.com/repos/JasperGame/zepeto-modules/releases/latest";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiEndpoint);
        request.UserAgent = "Unity Asset Version Manager";
        request.Method = "GET";

        try
        {
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    Debug.Log("dasdasdasdasd");
                    StreamReader reader = new StreamReader(stream);
                    string responseJson = reader.ReadToEnd();
                    int index = responseJson.IndexOf("tag_name") + 11;
                    string tagName = responseJson.Substring(index, responseJson.IndexOf(",") - index - 1);
                    return tagName;
                }
            }
        }
        catch
        {
            return null;
        }
    }

    private static void DownloadAndInstallPackage(string url)
    {
        string fileName = Path.GetFileName(url);

        WebClient webClient = new WebClient();
        webClient.DownloadFile(url, fileName);

        AssetDatabase.ImportPackage(fileName, true);
        File.Delete(fileName);
    }
}