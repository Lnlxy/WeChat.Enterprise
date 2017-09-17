namespace WeChat.Enterprise
{
    public static class MessageExtensions
    {
        public static T AppendUsers<T>(this T message, params string[] users) where T : Message
        {
            message.Targets.Addusers(users);
            return message;
        }
        public static T AppendParity<T>(this T message, params string[] parties) where T : Message
        {
            message.Targets.AddParties(parties);
            return message;
        }
        public static T AppendTags<T>(this T message, params string[] tags) where T : Message
        {
            message.Targets.AddTags(tags);
            return message;
        }
    }
}
