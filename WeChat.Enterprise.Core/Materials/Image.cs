using System.IO;

namespace WeChat.Enterprise
{
    sealed class Image : Material
    {
        public override int MaxLength => 2 * 1024 * 1024;

        public override string Type => MediaTypes.Image;

        internal Image(FileInfo info) : base(info)
        {
        }
    }
}
