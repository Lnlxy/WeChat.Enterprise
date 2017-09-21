using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 定义企业微信支持的媒体素材信息。
    /// </summary>
    public sealed class Material
    {
        private static readonly Dictionary<MediaType, Tuple<int, int>> storageLimits = new Dictionary<MediaType, Tuple<int, int>>()
        {
            [MediaType.File] = new Tuple<int, int>(5, 20 * 1024 * 1024),
            [MediaType.Image] = new Tuple<int, int>(5, 2 * 1024 * 1024),
            [MediaType.Video] = new Tuple<int, int>(5, 10 * 1024 * 1024),
            [MediaType.Voice] = new Tuple<int, int>(5, 2 * 1024 * 1024),
        };

        private readonly FileInfo _fileInfo;

        /// <summary>
        /// 获取一个值，该值表示资源Id。
        /// </summary>
        public string MediaId { get; internal set; }

        /// <summary>
        /// 获取一个值，该值表示素材最大长度。
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示素材最小长度。
        /// </summary>
        public int Minimum { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示素材长度。
        /// </summary>
        public int Length => (int)_fileInfo.Length;

        /// <summary>
        /// 获取一个值，该值表示素材媒体类型。
        /// </summary>
        public MediaType MediaType { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示素材名称。
        /// </summary>
        public string FileName => _fileInfo.Name;

        /// <summary>
        /// 获取一个值，该值表示素材文件路径。
        /// </summary>
        public string Path => _fileInfo.FullName;


        /// <summary>
        /// 获取一个值，该值表示素材媒体内容类型。
        /// </summary>
        public string ContentType { get; private set; }

        private Material(MediaType mediaType, FileInfo info)
        {
            MediaType = mediaType;
            var limit = storageLimits[mediaType];
            Minimum = limit.Item1;
            Maximum = limit.Item2;
            _fileInfo = info;
            ContentType = MimeMapping.GetMimeMapping(info.Name);
        }

        /// <summary>
        /// 加载一个本地媒体文件。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns>返回一个素材内容。</returns>
        public static Task<Material> LoadFromAsync(string fileName)
        {
            return Task.Run(() =>
            {
                FileInfo info = new FileInfo(fileName);
                var extension = info.Extension;

                if (string.Equals(".jpg", extension, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(".png", extension, StringComparison.OrdinalIgnoreCase))
                {
                    return new Material(MediaType.Image, info);
                }
                else if (string.Equals(".amr", extension, StringComparison.OrdinalIgnoreCase))
                {
                    return new Material(MediaType.Voice, info);
                }
                else if (string.Equals(".mp4", extension, StringComparison.OrdinalIgnoreCase))
                {
                    return new Material(MediaType.Video, info);
                }
                else
                {
                    return new Material(MediaType.File, info);
                }
            });
        }


        internal ByteArrayContent CreateByteArrayContent()
        {
            if (Length < Minimum || Length > Maximum)
            {
                throw new OutOfMemoryException($"media sizes of length must between {Minimum} and {Maximum} bytes.");
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

        internal static async Task<Material> LoadFromAsync(HttpContent content)
        {
            var fileName = content.Headers.ContentDisposition.FileName.Trim('"');
            var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);
            ;
            await File.WriteAllBytesAsync(path, await content.ReadAsByteArrayAsync());
            return await LoadFromAsync(path);
        }

        internal MultipartFormDataContent CreateMultipartFormDataContent(string boundary = null)
        {
            if (string.IsNullOrEmpty(boundary))
            {
                boundary = $"update_{MediaType}_{_fileInfo.Extension.Replace(".", "")}";
            }
            MultipartFormDataContent content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", $"multipart/form-data; boundary={boundary}");
            content.Add(CreateByteArrayContent());
            return content;
        }
    }
}
