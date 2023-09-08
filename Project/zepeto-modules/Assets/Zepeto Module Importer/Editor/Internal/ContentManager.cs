using UnityEngine;
using System.Collections.Generic;

namespace zmi.Internal
{
    [System.Serializable]
    public class Content
    {
        public string Title;
        public string Description;
        public string Description_ko;
        public string DocsUrl;
        public string LatestVersion;
        public string[] Dependencies;
        public Texture2D previewImage;
    }

    [System.Serializable]
    public class ContentList
    {
        public List<Content> Items;
    }

    public enum Language
    {
        English = 0,
        Korean = 1
    }

    public class DirectoryPath
    {
        public string oldPath;
        public string newPath;
        
        public DirectoryPath(string oldDirPath, string newDirPath)
        {
            oldPath = oldDirPath;
            newPath = newDirPath;
        }
    }
}