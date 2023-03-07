using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;


public class ZepetoImportManager : EditorWindow
{
    private MyData selectedData;
    private ItemArray dataUrlArray;
    
    [MenuItem("ZEPETO/ImportManager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ZepetoImportManager));
    }
    
    IEnumerator GetData()
    {
        string url = "https://raw.githubusercontent.com/JasperGame/zepeto-modules/main/project/zepeto-modules/Assets/ZepetoImporter/Data/urlPath.json";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            dataUrlArray = JsonUtility.FromJson<ItemArray>(www.downloadHandler.text);
        }
    }
    
    void OnGUI()
    {
        if (dataUrlArray == null)
        {
            string assetPath = Application.dataPath;
            string jsonString = System.IO.File.ReadAllText(assetPath + "/ZepetoImporter/Data/urlPath.json");
            dataUrlArray = JsonUtility.FromJson<ItemArray>(jsonString);
        }
        GUILayout.BeginHorizontal();
        DoSideButton();

        if (selectedData != null)
        {       
            GUILayout.BeginVertical();

            DoTopButton();
            DoVersionInfo();
            DoDependencyInfo();
            DoDescription();

            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();


    }
    
    private void DoSideButton()
    {
        GUILayout.BeginVertical(GUILayout.Width(200));
        foreach (MyData data in dataUrlArray.Items)
        {
            if (GUILayout.Button (data.Title, GUILayout.Height(50))) {
                selectedData = data;
            }
        }
        GUILayout.EndVertical();
        GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.Width(3));
    }

    private void DoTopButton()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(selectedData.Title, EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("API Docs", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            Application.OpenURL(selectedData.DocsUrl);
        }
        
        if (GUILayout.Button("Guide Book", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            Application.OpenURL(selectedData.DocsUrl);
        }
            
        if (GUILayout.Button("Import Latest Sample", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            string title = selectedData.Title.Replace(" ", ""); 
            string version = selectedData.LatestVersion; 
            EditorCoroutineUtility.StartCoroutine(DownloadFileCoroutine(title, version), this);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }
    private void DoVersionInfo()
    {
        string downloadedVersion = "UKNOWN";
        
        string className = "InteractionAPIManager";
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type type = assembly.GetType(className);
        if (type != null)
        {                

            FieldInfo field = type.GetField("VERSION", BindingFlags.Static | BindingFlags.Public);

            if (field != null)
            {
                string version = (string)field.GetValue(null);
                Debug.Log("VERSION: " + version);
            }
            else
            {
            }
        }
        else
        {
            Debug.Log("VERSION: empty");
        }
        
        //GUILayout.BeginHorizontal();
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 12;
        GUILayout.Box($"downloaded version : {downloadedVersion}\t" +
                       $"latest version : {selectedData.LatestVersion}", labelStyle);
    }
    
    private void DoDependencyInfo()
    {
        GUILayout.Label("Dependencies : ", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical();
        GUILayout.Label("\t ZEPETO.World : ", EditorStyles.label);
        GUILayout.Label("\t ZEPETO.Product : ", EditorStyles.label);
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("1.12.0+", EditorStyles.boldLabel);        
        GUILayout.Label("1.0.0+", EditorStyles.boldLabel);
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }
    
    private void DoDescription()
    {
        GUILayout.Label(selectedData.Description);
        string filePath = Application.dataPath + "/ZepetoImporter/Data/Image/"+selectedData.Title+".png";
        if (File.Exists(filePath))
        {
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(File.ReadAllBytes(filePath));
            GUILayout.Box(image,GUILayout.Width(500), GUILayout.ExpandHeight(true));
        }
    }

    private static IEnumerator DownloadFileCoroutine(string title, string version)
    {
        string mainPath = "https://github.com/JasperGame/zepeto-modules/raw/main/release/";
        string fileName = ".unitypackage";
        string downloadUrl = Path.Combine(mainPath, title, version,fileName);
        Debug.Log(downloadUrl);
        
        string tempFilePath = Path.Combine(Application.temporaryCachePath, title);

        using (var webClient = new WebClient())
        {
            webClient.DownloadProgressChanged += (sender, e) =>
            {
                float progress = (float)e.BytesReceived / (float)e.TotalBytesToReceive;
                EditorUtility.DisplayProgressBar("Downloading Package", $"{(progress * 100f):F1}%", progress);
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
    
    
    [System.Serializable]
    public class MyData
    {
        public string Title;
        public string Description;
        public string DocsUrl;
        public string LatestVersion;
    }
    
    [System.Serializable]
    public class ItemArray
    {
        public List<MyData> Items;
    }
    
}
