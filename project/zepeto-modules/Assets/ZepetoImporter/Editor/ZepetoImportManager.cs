using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;


public class ZepetoImportManager : EditorWindow
{
    private MyData selectedData;
    private ItemArray dataUrlArray;
    
    [MenuItem("ZEPETO/ImportManager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ZepetoImportManager));
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
        // 좌측에 버튼을 생성합니다.
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
        if (GUILayout.Button("Docs", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            Application.OpenURL(selectedData.DocsUrl);
        }
            
        if (GUILayout.Button("Import Sample", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            string path = selectedData.Title.Replace(" ", ""); 
            EditorCoroutineUtility.StartCoroutine(DownloadFileCoroutine(path), this);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }
    private void DoVersionInfo()
    {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(100));
        GUILayout.BeginHorizontal();
        GUILayout.Label("downloaded version : ", EditorStyles.boldLabel);
        GUILayout.Label("v1.0.0", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label("latest version : ", EditorStyles.boldLabel);
        GUILayout.Label("v1.0.0", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
    }
    private void DoDependencyInfo()
    {
        GUILayout.Label("Dependency : ", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Zepeto World : ", EditorStyles.boldLabel);
        GUILayout.Label("v1.0.0 or higher", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
    }
    
    private void DoDescription()
    {
        GUILayout.Label(selectedData.Description);
        string filePath = Application.dataPath + "/ZepetoImporter/Data/Image/"+selectedData.Title+".png";
        if (File.Exists(filePath))
        {
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(File.ReadAllBytes(filePath));
            GUILayout.Box(image, GUILayout.Height(500), GUILayout.Width(500));
        }
    }

    private static IEnumerator DownloadFileCoroutine(string downloadPath)
    {
        string mainPath = "https://github.com/JasperGame/zepeto-modules/raw/main/release/";
        string version = "v1.0.0.unitypackage";
        string downloadUrl = Path.Combine(mainPath, downloadPath, version);
        Debug.Log(downloadUrl);
        string tempFilePath = Path.Combine(Application.temporaryCachePath, downloadPath);

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
    }
    
    [System.Serializable]
    public class ItemArray
    {
        public List<MyData> Items;
    }
    
}
