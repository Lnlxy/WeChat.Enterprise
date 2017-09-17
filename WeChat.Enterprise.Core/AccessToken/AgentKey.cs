using System;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 表示一个企业应用标识。
    /// </summary>
    public struct AgentKey : IEquatable<AgentKey>
    {
        /// <summary>
        /// 获取一个值，该值表示应用Id。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 获取一个值，该值表示应用密钥。
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// 初始化 <see cref="AgentKey"/> 新实例。
        /// </summary>
        /// <param name="id">应用Id。</param>
        /// <param name="secret">应用密钥。</param>
        public AgentKey(int id, string secret)
        {
            Id = id;
            Secret = secret;
        }

        public bool Equals(AgentKey other)
        {
            return Id == other.Id
                && string.Equals(Secret, other.Secret, StringComparison.OrdinalIgnoreCase);
        }
        public override bool Equals(object obj)
        {
            if (obj is AgentKey)
            {
                return Equals((AgentKey)obj);
            }
            return false;
        }
        public override int GetHashCode() => HashCodeHelper.HashCode(Id, Secret.ToLower());

        public override string ToString() => $"Id={Id},Secret={Secret}";

        public static bool operator ==(AgentKey key1, AgentKey key2) => key1.Equals(key2);

        public static bool operator !=(AgentKey key1, AgentKey key2) => !key1.Equals(key2);
    }
}
