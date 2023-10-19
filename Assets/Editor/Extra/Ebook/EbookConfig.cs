using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor.Ebook
{
    public class EbookConfig
    {
        public const string PATH = "Source/Ebook";

        public const int NUMBER = 5000;
    }

    public class BookInformation
    {
        public string name;

        public string path;

        public long size;

        public long time;

        public bool filter;
    }

    public class BookDownloadInformation : BookInformation
    {
        public string key;

        public string url;

        public Regex regex;

        public int start, end;
    }

    public class ChapterInformation
    {
        public string key;

        public string url;

        public string content;

        public bool download;
    }
}