using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnityEditor.Ebook
{
    public class Downloader
    {
        public Action<string> complete;

        private BookInformation book;

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

            complete?.Invoke(book.key);
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
            string path = string.Format("{0}/{1}.txt", book.path, book.key);

            if (File.Exists(path)) File.Delete(path);

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter writer = new StreamWriter(stream);

                for (int i = 0; i < chapters.Count; i++)
                {
                    writer.WriteLine(chapters[i].key);
                    writer.Write(Format(chapters[i].content));
                    writer.Flush();
                }
                writer.Dispose();
            }
        }

        private string Format(string content)
        {
            int start = content.LastIndexOf("content");

            int end = content.LastIndexOf("</div>");

            return content.Substring(start, end - start);
        }

        public void Startup(BookInformation book)
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