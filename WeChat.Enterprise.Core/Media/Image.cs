using System.IO;

namespace WeChat.Enterprise
{
    public sealed class Image : Media
    {
        public override int MaxLength => 2 * 1024;

        public override string Type => MediaTypes.Image;

        internal Image(FileInfo info) : base(info)
        {
        }
    }
}
