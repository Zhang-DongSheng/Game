using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;

namespace Game
{
    public static partial class Utility
    {
        public static class _Compress
        {
            private const int Length = 0x1000;

            private static readonly byte[] buffer = new byte[Length];

            public static bool Compress(byte[] bytes, int offset, int length, Stream stream)
            {
                if (bytes == null || stream == null)
                {
                    return false;
                }
                if (offset < 0 || length < 0 || offset + length > bytes.Length)
                {
                    return false;
                }

                try
                {
                    GZipOutputStream gZipOutputStream = new GZipOutputStream(stream);
                    gZipOutputStream.Write(bytes, offset, length);
                    gZipOutputStream.Finish();
                    ProcessHeader(stream);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static bool Compress(Stream input, Stream output)
            {
                if (input == null || output == null)
                {
                    return false;
                }

                try
                {
                    GZipOutputStream gZipOutputStream = new GZipOutputStream(output);
                    int bytesRead = 0;
                    while ((bytesRead = input.Read(buffer, 0, Length)) > 0)
                    {
                        gZipOutputStream.Write(buffer, 0, bytesRead);
                    }

                    gZipOutputStream.Finish();
                    ProcessHeader(output);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    Array.Clear(buffer, 0, Length);
                }
            }

            public static bool Decompress(byte[] bytes, int offset, int length, Stream stream)
            {
                if (bytes == null || stream == null)
                {
                    return false;
                }
                if (offset < 0 || length < 0 || offset + length > bytes.Length)
                {
                    return false;
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, offset, length, false);
                    using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
                    {
                        int bytesRead = 0;
                        while ((bytesRead = gZipInputStream.Read(buffer, 0, Length)) > 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                    }
                    Array.Clear(buffer, 0, Length);
                }
            }

            public static bool Decompress(Stream input, Stream output)
            {
                if (input == null || output == null)
                {
                    return false;
                }
                try
                {
                    GZipInputStream gZipInputStream = new GZipInputStream(input);
                    int bytesRead = 0;
                    while ((bytesRead = gZipInputStream.Read(buffer, 0, Length)) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    Array.Clear(buffer, 0, Length);
                }
            }

            private static void ProcessHeader(Stream stream)
            {
                if (stream.Length >= 8L)
                {
                    long current = stream.Position;
                    stream.Position = 4L;
                    stream.WriteByte(25);
                    stream.WriteByte(134);
                    stream.WriteByte(2);
                    stream.WriteByte(32);
                    stream.Position = current;
                }
            }
        }
    }
}