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
    MyData selectedData;

    [MenuItem("ZEPETO/ImportManager")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ZepetoImportManager));
    }
    
    void OnGUI()
    {
        string assetPath = Application.dataPath;
        string jsonString = System.IO.File.ReadAllText(assetPath+"/ZepetoImporter/Data/urlPath.json");
        ItemArray dataUrlArray = JsonUtility.FromJson<ItemArray>(jsonString);
        
        GUILayout.BeginHorizontal();

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


        // 우측에 선택한 버튼의 설명을 표시합니다.
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        if (selectedData != null)
        {
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
            GUILayout.Label(selectedData.Description);
            string filePath = Application.dataPath + "/ZepetoImporter/Data/Image/"+selectedData.Title+".png";
            if (File.Exists(filePath))
            {
                Texture2D image = new Texture2D(2, 2);
                image.LoadImage(File.ReadAllBytes(filePath));
                GUILayout.Box(image, GUILayout.Height(500), GUILayout.Width(500));
            }

            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();


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
        public string DownloadPath;
    }
    
    [System.Serializable]
    public class ItemArray
    {
        public List<MyData> Items;
    }
    
}
