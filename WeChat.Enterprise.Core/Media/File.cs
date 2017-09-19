using System.IO;

namespace WeChat.Enterprise
{
    public sealed class File : Media
    {
        public override int MaxLength => 20 * 1024;

        public override string Type => MediaTypes.File;

        internal File(FileInfo info) : base(info)
        {
        }
    }
}
