using System.IO;

namespace WeChat.Enterprise
{
    public sealed class Video : Media
    {
        public override int MaxLength => 10 * 1024;

        public override string Type => MediaTypes.Video;

        internal Video(FileInfo info) : base(info)
        {
        }
    }
}
