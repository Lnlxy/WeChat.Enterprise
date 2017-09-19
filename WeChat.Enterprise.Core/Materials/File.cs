using System.IO;

namespace WeChat.Enterprise
{
    sealed class File : Material
    {
        public override int MaxLength => 20 * 1024 * 1024;

        public override string Type => MediaTypes.File;

        internal File(FileInfo info) : base(info)
        {
        }
    }
}
