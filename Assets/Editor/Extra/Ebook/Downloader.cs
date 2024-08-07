using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnityEditor.Ebook
{
    public class Downloader
    {
        public Action<string> complete;

        private BookDownloadInformation book;

        private readonly HttpClient client = new HttpClient();

        private readonly List<ChapterInformation> chapters = new List<ChapterInformation>();

        private void Download()
        {
            int index = 0;

            for (int i = 0; i < chapters.Count; i++)
            {
                if (chapters[i].download)
                {
                    Download(chapters[i]);
                    break;
                }
                index++;
            }

            if (index < chapters.Count) return;

            Merge();

            complete?.Invoke(book.name);
        }

        private async void Download(ChapterInformation chapter)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(chapter.url);

                response.EnsureSuccessStatusCode();

                Task<string> task = response.Content.ReadAsStringAsync();

                chapter.content = await task;

                chapter.download = false;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            finally
            {
                Download();
            }
        }

        private void Merge()
        {
            string path = string.Format("{0}/{1}.txt", book.path, book.name);

            if (File.Exists(path)) File.Delete(path);

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine(book.name);

                for (int i = 0; i < chapters.Count; i++)
                {
                    writer.WriteLine(chapters[i].key);
                    writer.Write(Format(chapters[i].content, book.regex));
                    writer.WriteLine(string.Empty);
                    writer.Flush();
                }
                writer.Dispose();
            }
        }

        private string Format(string content, Regex regex, params string[] parameter)
        {
            if (regex != null && regex.IsMatch(content))
            {
                content = regex.Match(content).Groups[0].ToString()
                    .Replace("&nbsp;", null)
                    .Replace("<br />", "\r\n");
            }
            return content;
        }

        public void Startup(BookDownloadInformation book)
        {
            this.book = book;

            chapters.Clear();

            for (int i = book.start; i < book.end; i++)
            {
                chapters.Add(new ChapterInformation()
                {
                    key = string.Format("chapter of {0}", i),
                    url = string.Format(book.url, book.key, i),
                    download = true,
                });
            }
            Download();
        }
    }
}