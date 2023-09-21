using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using zmi.Internal;
using zmi.Utilities;
using zmi.Constant;

namespace zmi
{
    public class ModuleImporter : EditorWindow
        {
            private Content _selectedData;
            private ContentList _contentList;
            private string _lastUpdateTime = "";

            private Language _selectedLanguage = Language.English;
            private readonly string[] _languages = Enum.GetNames(typeof(Language));
            private static EditorWindow _window;

            [MenuItem(ModuleStrings.UI_MENU_ITEM)]
            public static void ShowWindow()
            {
                Rect windowRect = new Rect(0, 0, ImageSize.RECT_WIDTH, ImageSize.RECT_HEIGHT);
                _window = EditorWindow.GetWindowWithRect(typeof(ModuleImporter), windowRect, true,
                    ModuleStrings.UI_TEXT_ZEPETO_MODULE_IMPORTER);
            }

            private void OnGUI()
            {
                if (!_window)
                {
                    ShowWindow();
                }

                if (_contentList == null)
                {
                    _selectedLanguage =
                        Application.systemLanguage == SystemLanguage.Korean ? Language.Korean : Language.English;
                    DoTopBarGUI();
                    EditorCoroutineUtility.StartCoroutine(LoadDataAsync(), this);

                    GUILayout.BeginArea(new Rect(position.width * 0.5f, position.height * 0.5f, 400, 100));
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
                    DoDescriptionGUI();
                    DoDependencyInfoGUI();
                    DoPreviewImageGUI();

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

                GUILayout.Label(ModuleStrings.UI_TEXT_ZEPETO_MODULE_IMPORTER, labelStyle);
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));

                GUILayout.FlexibleSpace();
                _selectedLanguage =
                    (Language)EditorGUILayout.Popup((int)_selectedLanguage, _languages, GUILayout.Width(150),
                        GUILayout.Height(30));

                GUILayout.EndHorizontal();
                GUILayout.Label(ModuleStrings.UI_TEXT_FREQUENTLY_USED_MODULES, EditorStyles.label);

                GUILayout.Box("", GUILayout.Height(3), GUILayout.ExpandWidth(true));
            }

            private void DoSideButtonGUI()
            {
                const int buttonWidth = 200;
                GUILayout.BeginVertical(GUILayout.Width(buttonWidth));
                if (_selectedData == null)
                    _selectedData = _contentList.Items[0] ?? null;

                foreach (Content data in _contentList.Items)
                {
                    if (GUILayout.Button("", GUILayout.Width(buttonWidth), GUILayout.Height(30)))
                    {
                        _selectedData = data;
                    }

                    Rect guiRect = GUILayoutUtility.GetLastRect();
                    Rect statusRect = new Rect(guiRect.x + (guiRect.width * 0.01f), guiRect.y, guiRect.width,
                        guiRect.height);
                    Rect titleRect = new Rect(guiRect.x + (guiRect.width * 0.12f), guiRect.y, guiRect.width,
                        guiRect.height);
                    Rect versionRect = new Rect(guiRect.x + (guiRect.width * 0.82f), guiRect.y, guiRect.width,
                        guiRect.height);

                    GUI.Label(titleRect, data.Title);
                    string version = VersionHandler.VersionCheck(StringUtil.GetRemoveSpace(data.Title));
                    if (version != ModuleStrings.UNKNOWN_VERSION)
                    {
                        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                        GUI.Label(versionRect, version, EditorStyles.miniLabel);
                        Texture2D statusTexture = version == data.LatestVersion
                            ? EditorGUIUtility.FindTexture(ModuleStrings.UI_ICON_LATEST_MODULE_VERSION)
                            : EditorGUIUtility.FindTexture(ModuleStrings.UI_ICON_OLD_MODULE_VERSION);
                        GUI.Label(statusRect, statusTexture);
                    }
                }

                DoUpdateButtonGUI();
                DoContibuteButtonGUI();
                GUILayout.EndVertical();
                GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.Width(3));
            }
            
            private void DoUpdateButtonGUI()
            {
                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();
                GUILayout.Label(ModuleStrings.UI_TEXT_LATEST_UPDATE + _lastUpdateTime, EditorStyles.boldLabel, GUILayout.Height(30));
                if (GUILayout.Button(EditorGUIUtility.FindTexture(ModuleStrings.UI_ICON_REFRESH), GUILayout.Width(30),
                        GUILayout.Height(30)))
                {
                    _contentList = null;
                    _lastUpdateTime = "";
                    EditorCoroutineUtility.StartCoroutine(LoadDataAsync(), this);
                }

                GUILayout.EndHorizontal();
            }

            private void DoContibuteButtonGUI()
            {
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(ModuleStrings.UI_TEXT_REPORT_ISSUE, GUILayout.Width(200), GUILayout.Height(20)))
                {
                    string url = ModulePath.ISSUE_REPORT_PATH;
                    OpenLocalizeURL((url));
                }

                if (GUILayout.Button(ModuleStrings.UI_TEXT_CONTRIBUTE, GUILayout.Width(200), GUILayout.Height(20)))
                {
                    string url = ModulePath.CONTRIBUTE_PATH;
                    OpenLocalizeURL(url);
                }

                GUILayout.Space(3);
                GUILayout.EndVertical();
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

                if (GUILayout.Button(ModuleStrings.UI_BUTTON_VIEW_IMPORT_GUIDE, GUILayout.Height(20), GUILayout.ExpandWidth(false)))
                {
                    string url = Path.Combine(ModulePath.REPO_PATH, StringUtil.GetRemoveSpace(_selectedData.Title),
                        ModulePath.README_PATH);
                    OpenLocalizeURL(url);
                }

                
                ImportButtonGUI();
                RemoveButtonGUI();

                GUILayout.EndHorizontal();

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            }

            private void ImportButtonGUI()
            {
                string downloadedVersion = VersionHandler.VersionCheck(StringUtil.GetRemoveSpace(_selectedData.Title));
                string UIImportButton = ModuleStrings.UI_BUTTON_IMPORT;
                bool moduleIsImportable = true;
                if(downloadedVersion != ModuleStrings.UNKNOWN_VERSION)
                {
                    UIImportButton = ModuleStrings.UI_BUTTON_UPDATE;
                    moduleIsImportable = ModuleInstaller.isUpdatable(downloadedVersion, _selectedData.LatestVersion);
                }
                EditorGUI.BeginDisabledGroup(!moduleIsImportable);
                if (GUILayout.Button(UIImportButton + _selectedData.LatestVersion, GUILayout.Height(20),
                        GUILayout.ExpandWidth(false)))
                {
                    string title = StringUtil.GetRemoveSpace(_selectedData.Title);
                    string version = "v" + _selectedData.LatestVersion;

                    if (UIImportButton == ModuleStrings.UI_BUTTON_UPDATE)
                    {
                        ModuleInstaller.UpdateModule(_selectedData.Title, version, this);
                    }
                    else
                    {
                        EditorCoroutineUtility.StartCoroutine(ModuleInstaller.ImportModule(title, version), this);

                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            private void RemoveButtonGUI()
            {
                string downloadedVersion = VersionHandler.VersionCheck(StringUtil.GetRemoveSpace(_selectedData.Title));
                bool moduleIsRemovable = ModuleInstaller.IsRemovable(downloadedVersion, _selectedData.Title);
                EditorGUI.BeginDisabledGroup(!moduleIsRemovable);
                if (GUILayout.Button(ModuleStrings.UI_BUTTON_REMOVE, GUILayout.Height(20),
                        GUILayout.ExpandWidth(false)))
                {
                    ModuleInstaller.RemoveModule(_selectedData.Title);
                }
                EditorGUI.EndDisabledGroup();
            }

            private void DoVersionInfoGUI()
            {
                string className = StringUtil.GetRemoveSpace(_selectedData.Title);
                string downloadedVersion = VersionHandler.VersionCheck(className);

                GUILayout.Label($"Version : {downloadedVersion}", EditorStyles.boldLabel);

                GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
                linkStyle.normal.textColor = new Color(0.0f, 0.47f, 1.0f);
                linkStyle.hover.textColor = Color.yellow;
                linkStyle.fontStyle = FontStyle.Italic;

                GUILayout.BeginHorizontal();

                if (GUILayout.Button(ModuleStrings.UI_TEXT_SEE_OTHER_VERSION, linkStyle))
                {
                    string versionUrl = Path.Combine(ModulePath.REPO_PATH, StringUtil.GetRemoveSpace(_selectedData.Title));
                    Application.OpenURL(versionUrl);
                }

                GUILayout.Label("-");
                if (GUILayout.Button(ModuleStrings.UI_TEXT_API_DOCS, linkStyle))
                {
                    string docsUrl = _selectedData.DocsUrl;
                    OpenLocalizeURL(docsUrl);
                }

                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            }

            private void DoDependencyInfoGUI()
            {
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
                GUILayout.Label(ModuleStrings.UI_TEXT_DEPENDENCIES, EditorStyles.boldLabel);
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
                GUILayout.Label(ModuleStrings.UI_TEXT_IS_USING, EditorStyles.boldLabel);

                GUILayout.BeginVertical();
                foreach (string dependency in _selectedData.Dependencies)
                {
                    GUILayout.Label("\t" + dependency, EditorStyles.label);
                }

                GUILayout.EndVertical();


                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(3));
            }

            private void DoDescriptionGUI()
            {
                GUIStyle style = new GUIStyle();
                style.wordWrap = true;
                style.normal.textColor = Color.white;

                GUILayout.Label(_selectedLanguage == 0 ? _selectedData.Description : _selectedData.Description_ko,
                    style);
            }

            private void DoPreviewImageGUI()
            {
                GUILayout.Label(ModuleStrings.UI_TEXT_PREVIEW, EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (_selectedData.previewImage)
                    GUILayout.Box(_selectedData.previewImage, GUILayout.Width(_selectedData.previewImage.width),
                        GUILayout.Height(_selectedData.previewImage.height));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            private IEnumerator LoadDataAsync()
            {
                yield return DownloadGithubHandler.GetDataAsync((data) =>
                {
                    if (_contentList == null && data != null)
                    {
                        _contentList = JsonUtility.FromJson<ContentList>(data);
                        _lastUpdateTime = DateTime.Now.ToString("HH:mm");
                        for (int i = 0; i < _contentList.Items.Count; i++)
                        {
                            EditorCoroutineUtility.StartCoroutine(LoadImageAsync(i), this);
                        }
                    }
                });
            }

            private IEnumerator LoadImageAsync(int i)
            {
                string url = Path.Combine(ModulePath.DOWNLOAD_PATH, StringUtil.GetRemoveSpace(_contentList.Items[i].Title),
                    ModulePath.PREVIEW_IMAGE_NAME + ModuleStrings.EXTENSION_PNG);
                yield return DownloadGithubHandler.GetTextureAsync(url, (texture) =>
                {
                    if (texture != null)
                        _contentList.Items[i].previewImage = texture;
                });
            }

            private void OpenLocalizeURL(string url)
            {
                string localizeUrl = url;

                switch (_selectedLanguage)
                {
                    case Language.Korean:
                        localizeUrl = Regex.Replace(url, ModuleStrings.LANGUAGE_EN, ModuleStrings.LANGUAGE_KR);
                        localizeUrl = Regex.Replace(url, ModuleStrings.EXTENSION_MD, "_KR" + ModuleStrings.EXTENSION_MD);
                        break;

                    case Language.English:
                    default:
                        break;
                }
                Application.OpenURL(localizeUrl);
            }
    }
}