using System.IO;

namespace WeChat.Enterprise
{
    sealed class Voice : Material
    {
        public override int MaxLength => 2 * 1024 * 1024;

        public override string Type => MediaTypes.Voice;

        internal Voice(FileInfo info) : base(info)
        {
        }
    }
}
