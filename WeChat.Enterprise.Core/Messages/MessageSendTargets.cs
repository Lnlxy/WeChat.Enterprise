using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WeChat.Enterprise
{
    public class MessageSendTargets
    {
        public static readonly MessageSendTargets All = new MessageSendTargets();

        private bool _toAll = false;

        private readonly List<string> _users, _parties, _tags;

        public bool ToAll => _toAll;

        public List<string> Users => _users;

        public List<string> Parties => _parties;

        public List<string> Tags => _tags;

        public MessageSendTargets ChangeAll(bool all)
        {
            _toAll = all;
            return this;
        }

        public MessageSendTargets AddUser(string user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
            }
            return this;
        }

        public MessageSendTargets Addusers(IEnumerable<string> users)
        {
            foreach (var user in users)
            {
                AddUser(user);
            }
            return this;
        }

        public MessageSendTargets RemoveUser(string user)
        {
            _users.Remove(user);
            return this;
        }

        public MessageSendTargets AddParty(string party)
        {
            if (!_parties.Contains(party))
            {
                _parties.Add(party);
            }
            return this;
        }

        public MessageSendTargets AddParties(IEnumerable<string> parties)
        {
            foreach (var party in parties)
            {
                AddParty(party);
            }
            return this;
        }

        public MessageSendTargets RemoveParty(string party)
        {
            _parties.Remove(party);
            return this;
        }
        public MessageSendTargets AddTag(string tag)
        {
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
            return this;
        }

        public MessageSendTargets AddTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
            return this;
        }

        public MessageSendTargets RemoveTag(string tag)
        {
            _tags.Remove(tag);
            return this;
        }

        public MessageSendTargets()
        {
            _users = new List<string>();
            _parties = new List<string>();
            _tags = new List<string>();
        }

        internal void AppendToObject(JObject obj)
        {
            if (_toAll)
            {
                obj["touser"] = "@all";
            }
            else
            {
                if (_users.Count > 0)
                {
                    obj["touser"] = string.Join("|", _users);
                }
                if (_parties.Count > 0)
                {
                    obj["toparty"] = string.Join("|", _parties);
                }
                if (_tags.Count > 0)
                {
                    obj["toutag"] = string.Join("|", _tags);
                }
            }
        }
    }
}
