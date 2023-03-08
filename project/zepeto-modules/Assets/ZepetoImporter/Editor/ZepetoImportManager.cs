using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public class ZepetoImportManager : EditorWindow
{
    private Content _selectedData;
    private ContentList _contentList;
    private string[] _languages = new string[] { "English", "Korean" };
    private int _selectedLanguage = 0;
    
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
            if (_contentList == null)
            {
                _contentList = JsonUtility.FromJson<ContentList>(www.downloadHandler.text);
            }
        }
    }
    
    void OnGUI()
    {
        string assetPath = Application.dataPath;
        string jsonString = System.IO.File.ReadAllText(assetPath + "/ZepetoImporter/Data/urlPath.json");
        _contentList = JsonUtility.FromJson<ContentList>(jsonString);
        if (_contentList == null)
        {
            EditorCoroutineUtility.StartCoroutine(GetData(), this);
            GUILayout.BeginArea(new Rect(position.width * 0.5f - 100, (position.height) * 0.5f - 50, 400, 100));
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Wait...");
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        {
            DrawAll();
        }
    }

    private void DrawAll()
    {
        DoTopBarGUI();
        GUILayout.BeginHorizontal();

        DoSideButtonGUI();

        if (_selectedData != null)
        {       
            GUILayout.BeginVertical();

            DoTopButtonGUI();
            DoVersionInfoGUI();
            DoDependencyInfoGUI();
            DoDescriptionGUI();

            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
    }
    private void DoTopBarGUI()
    {        
        GUILayout.BeginHorizontal();
        
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 24;
        
        GUILayout.Label("Zepeto Import Manager", labelStyle);
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
        
        GUILayout.FlexibleSpace();
        _selectedLanguage = EditorGUILayout.Popup(_selectedLanguage, _languages, GUILayout.Width(150), GUILayout.Height(30));

        GUILayout.EndHorizontal();

    }
    private void DoSideButtonGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(200));
        foreach (Content data in _contentList.Items)
        {
            if (GUILayout.Button (data.Title, GUILayout.Height(30))) {
                _selectedData = data;
            }
        }
        
        using (new EditorGUILayout.HorizontalScope(GUILayout.Height(30)))
        {
            GUILayout.Label("Refresh");
            if (GUILayout.Button(EditorGUIUtility.FindTexture("d_Refresh"), GUILayout.Width(30), GUILayout.Height(30)) == true)
            {
                EditorCoroutineUtility.StartCoroutine(GetData(), this);
                DrawAll();
            }
        }
        GUILayout.EndVertical();
        GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.Width(3));
    }

    private void DoTopButtonGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(_selectedData.Title, EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("API Docs", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            Application.OpenURL(_selectedData.DocsUrl);
        }
        
        if (GUILayout.Button("Import Guide", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            Application.OpenURL(_selectedData.ImportGuideUrl);
        }
            
        if (GUILayout.Button("Import", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            string title = _selectedData.Title.Replace(" ", ""); 
            string version = _selectedData.LatestVersion; 
            EditorCoroutineUtility.StartCoroutine(DownloadFileCoroutine(title, version), this);
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }
    
    private void DoVersionInfoGUI()
    {
        string downloadedVersion = "UNKNOWN";
        string className = _selectedData.Title.Replace(" ","")+"Manager";

        Type type = GetTypeByName(className);

        if (type != null)
        {
            FieldInfo field = type.GetField("VERSION", BindingFlags.Static | BindingFlags.Public);

            if (field != null)
            {
                downloadedVersion = (string)field.GetValue(null);
            }
        }
        
        //GUILayout.BeginHorizontal();
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 12;
        GUILayout.Box($"downloaded version : {downloadedVersion}\t" +
                       $"latest version : {_selectedData.LatestVersion}", labelStyle);
    }
    
    private void DoDependencyInfoGUI()
    {
        GUILayout.Label("Dependencies : ", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical();
        foreach (string dependency in _selectedData.Dependencies)
        {
            GUILayout.Label("\t"+dependency, EditorStyles.label);
        }
        GUILayout.EndVertical();
        
        GUILayout.EndHorizontal();
        
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }
    
    private void DoDescriptionGUI()
    {
        GUILayout.Label(_selectedLanguage == 0 ? _selectedData.Description : _selectedData.Description_ko);
        string filePath = Application.dataPath + "/ZepetoImporter/Data/Image/"+_selectedData.Title+".png";
        if (File.Exists(filePath))
        {
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(File.ReadAllBytes(filePath));
            GUILayout.Box(image,GUILayout.Width(500),GUILayout.MaxHeight(300));
        }
    }

    private static IEnumerator DownloadFileCoroutine(string title, string version)
    {
        string mainPath = "https://github.com/JasperGame/zepeto-modules/raw/main/release/";
        string fileName = ".unitypackage";
        string downloadUrl = Path.Combine(mainPath, title, version+fileName);
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
    
    Type GetTypeByName(string className)
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
    
    [System.Serializable]
    public class Content
    {
        public string Title;
        public string Description;
        public string Description_ko;
        public string DocsUrl;
        public string ImportGuideUrl;
        public string LatestVersion;
        public string[] Dependencies;
    }
    
    [System.Serializable]
    public class ContentList
    {
        public List<Content> Items;
    }
    
}
