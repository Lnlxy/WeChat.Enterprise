namespace WeChat.Enterprise
{
    static class MediaTypeStrings
    {
        static readonly string image = "image";

        static readonly string voice = "voice";

        static readonly string video = "video";

        static readonly string file = "file";

        public static string GetMediaTypeString(MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Image:
                    return image;
                case MediaType.Voice:
                    return voice;
                case MediaType.Video:
                    return video;
                case MediaType.File:
                default:
                    return file;
            }
        }

        public static string GetMessageTypeString(MediaType mediaType)
        {
            return GetMediaTypeString(mediaType);
        }
    }
}
