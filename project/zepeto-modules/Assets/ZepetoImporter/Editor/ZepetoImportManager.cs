using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        Rect windowRect = new Rect(0, 0, 800, 800);
        EditorWindow.GetWindowWithRect(typeof(ZepetoImportManager), windowRect, true);
    }

    private void OnGUI()
    {
        if (_contentList == null)
        {
            DoTopBarGUI();

            EditorCoroutineUtility.StartCoroutine(GetData(), this);
            GUILayout.BeginArea(new Rect(position.width * 0.5f, (position.height) * 0.5f, 400, 100));
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Wait...");
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        else
        {
            DrawAll();
        }
    }

    private IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(ConstantManager.CONTENT_DATA_PATH);
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
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

        GUILayout.BeginHorizontal();

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 24;

        GUILayout.Label("Zepeto Import Manager", labelStyle);
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

        GUILayout.FlexibleSpace();
        _selectedLanguage = EditorGUILayout.Popup(_selectedLanguage, _languages, GUILayout.Width(150), GUILayout.Height(30));

        GUILayout.EndHorizontal();
        GUILayout.Label(" Easily add frequently used modules.", EditorStyles.label);

        GUILayout.Box("", GUILayout.Height(3), GUILayout.ExpandWidth(true));
    }

    private void DoSideButtonGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(200));
        foreach (Content data in _contentList.Items)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(data.Title, GUILayout.Height(30)))
            {
                _selectedData = data;
            }

            string version = VersionHandler.VersionCheck(GetRemoveSpace(data.Title) + "Manager");
            GUILayout.Label(version == "UNKNOWN" ? "" : version);
            Texture2D statusTexture = version == data.LatestVersion
                ? EditorGUIUtility.FindTexture("d_winbtn_mac_max")
                : EditorGUIUtility.FindTexture("d_winbtn_mac_min");
            GUILayout.Label(statusTexture,GUILayout.Width(30), GUILayout.Height(30));
            GUILayout.EndHorizontal();
        }

        using (new EditorGUILayout.HorizontalScope(GUILayout.Height(30)))
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Refresh");
            if (GUILayout.Button(EditorGUIUtility.FindTexture("d_Refresh"), GUILayout.Width(30), GUILayout.Height(30)))
            {
                _contentList = null;
                EditorCoroutineUtility.StartCoroutine(GetData(), this);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.Width(3));
    }

    private void DoTopButtonGUI()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fontSize = 20;
        labelStyle.fontStyle = FontStyle.Bold;

        GUILayout.BeginHorizontal();
        GUILayout.Label(_selectedData.Title, labelStyle);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("API Docs", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            Application.OpenURL(_selectedData.DocsUrl);
        }

        if (GUILayout.Button("Import Guide", GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            string url = Path.Combine(ConstantManager.README_PATH, GetRemoveSpace(_selectedData.Title), "README.md");
            Application.OpenURL(url);
        }

        if (GUILayout.Button("Import "+_selectedData.LatestVersion, GUILayout.Height(20), GUILayout.ExpandWidth(false)))
        {
            string title = GetRemoveSpace(_selectedData.Title);
            string version = _selectedData.LatestVersion;
            EditorCoroutineUtility.StartCoroutine(ImportHandler.ImportPackage(title, version), this);
        }

        GUILayout.EndHorizontal();

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }

    private void DoVersionInfoGUI()
    {
        string className = GetRemoveSpace(_selectedData.Title) + "Manager";
        string downloadedVersion = VersionHandler.VersionCheck(className);

        GUILayout.Label($"Version : {downloadedVersion}", EditorStyles.boldLabel);
    }

    private void DoDependencyInfoGUI()
    {
        GUILayout.Label("Dependencies : ", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        foreach (string dependency in _selectedData.Dependencies)
        {
            GUILayout.Label("\t" + dependency, EditorStyles.label);
        }

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
    }

    private void DoDescriptionGUI()
    {
        GUILayout.Label(_selectedLanguage == 0 ? _selectedData.Description : _selectedData.Description_ko);
        string filePath = Application.dataPath + ConstantManager.IMAGE_PATH + _selectedData.Title +
                          ConstantManager.EXTENSION_PNG;
        if (File.Exists(filePath))
        {
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(File.ReadAllBytes(filePath));
            GUILayout.Box(image, GUILayout.Width(500), GUILayout.MaxHeight(300));
        }
    }

    private string GetRemoveSpace(string s)
    {
        return s.Replace(" ", "");
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