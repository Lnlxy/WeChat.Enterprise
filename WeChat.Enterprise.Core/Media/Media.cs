using System;
using System.IO;
using System.Net.Http;

namespace WeChat.Enterprise
{
    public abstract class Media
    {
        private readonly FileInfo _fileInfo;
        public abstract int MaxLength { get; }

        public int MinLength => 5;

        public int Length => (int)_fileInfo.Length;

        public abstract string Type { get; }

        public string FileName => _fileInfo.Name;

        public string Path => _fileInfo.FullName;

        public string Extension => _fileInfo.Extension;

        public string ContentType { get; private set; }

        protected Media(FileInfo info)
        {
            _fileInfo = info;
            ContentType = MimeMapping.GetMimeMapping(info.Name);
        }

        public static Media LoadFrom(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            var extension = info.Extension;

            if (string.Equals(".jpg", extension, StringComparison.OrdinalIgnoreCase)
                || string.Equals(".png", extension, StringComparison.OrdinalIgnoreCase))
            {
                return new Image(info);
            }
            else if (string.Equals(".arm", extension, StringComparison.OrdinalIgnoreCase))
            {
                return new Voice(info);
            }
            else if (string.Equals(".mp4", extension, StringComparison.OrdinalIgnoreCase))
            {
                return new Video(info);
            }
            else
            {
                return new File(info);
            }
        }

        internal ByteArrayContent CreateByteArrayContent()
        {
            if (Length < MinLength || Length > MaxLength)
            {
                throw new OutOfMemoryException($"media sizes of length must between {MinLength} and {MaxLength} bytes.");
            }
            using (var stream = _fileInfo.OpenRead())
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                var content = new ByteArrayContent(bytes);
                content.Headers.Remove("Content-Disposition");
                content.Headers.TryAddWithoutValidation("Content-Disposition", $"form-data;name=\"media\";filename=\"{FileName}\"");
                content.Headers.Remove("Content-Type");
                content.Headers.TryAddWithoutValidation("Content-Type", ContentType);
                return content;
            }
        }

        internal MultipartFormDataContent CreateMultipartFormDataContent(string boundary = null)
        {
            if (string.IsNullOrEmpty(boundary))
            {
                boundary = $"update_{Type}_{Extension.Replace(".", "")}";
            }
            MultipartFormDataContent content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", $"multipart/form-data; boundary={boundary}");
            content.Add(CreateByteArrayContent());
            return content;
        }
    }
}
