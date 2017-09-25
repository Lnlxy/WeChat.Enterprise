using Newtonsoft.Json.Linq;

namespace WeChat.Enterprise
{
    public abstract class Article
    {
        public string Title { get; set; }

        public abstract JObject ToJobject();
    }

    public class NewsArticle : Article
    {
        public string Description { get; set; }

        public string Url { get; set; }

        public string PictureUrl { get; set; }

        public string BtnTxt { get; set; }

        public override JObject ToJobject()
        {
            return new JObject() {
                new JProperty("title",Title),
                new JProperty("description",Description),
                new JProperty("url",Url),
                new JProperty("picurl",PictureUrl),
                new JProperty("btntxt",BtnTxt),
            };
        }
    }

    public class MpNewsArticle : Article
    {
        public string ThumbMediaId { get; set; }

        public string Author { get; set; }

        public string ContentSourceUrl { get; set; }

        public string Content { get; set; }

        public string Digest { get; set; }
        public override JObject ToJobject()
        {
            return new JObject()
            {
                new JProperty("title",Title),
                new JProperty("thumb_media_id",ThumbMediaId),
                new JProperty("author",Author),
                new JProperty("content_source_url",ContentSourceUrl),
                new JProperty("content",Content),
                new JProperty("digest",Digest),
            };
        }
    }
}
