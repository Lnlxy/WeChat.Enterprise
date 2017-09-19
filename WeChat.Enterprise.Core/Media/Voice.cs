using System.IO;

namespace WeChat.Enterprise
{
    public sealed class Voice : Media
    {
        public override int MaxLength => 2 * 1024;

        public override string Type => MediaTypes.Voice;

        internal Voice(FileInfo info) : base(info)
        {
        }
    }
}
