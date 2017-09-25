using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeChat.Enterprise
{
    public sealed class NewsMessage<TArticle> : Message where TArticle : Article, new()
    {
        private string messageType = "";
        private readonly List<TArticle> articles = new List<TArticle>();

        public override string MessageType => messageType;

        public IEnumerable<TArticle> Articles => articles.AsReadOnly();

        public NewsMessage<TArticle> SetMessageType(string msgType)
        {
            messageType = msgType;
            return this;
        }

        public NewsMessage<TArticle> Append(params TArticle[] articles)
        {
            this.articles.AddRange(articles);
            return this;
        }

        protected override Task GetMessageContentAsync(JObject content, AgentKey agentKey)
        {
            return Task.Run(() =>
            {
                var cont = new JObject();
                var jar = new JArray();
                cont.Add("articles", jar);
                foreach (var article in articles)
                {
                    jar.Add(article.ToJobject());
                }
                content.Add(messageType, cont);
            });
        }
    }
}
