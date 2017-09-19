using System.IO;

namespace WeChat.Enterprise
{
    sealed class Video : Material
    {
        public override int MaxLength => 10 * 1024 * 1024;

        public override string Type => MediaTypes.Video;

        internal Video(FileInfo info) : base(info)
        {
        }
    }
}
