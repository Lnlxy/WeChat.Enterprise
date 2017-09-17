using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WeChat.Enterprise
{
    public class MessageTargets
    {
        public static readonly MessageTargets All = new MessageTargets();

        private bool _toAll = false;

        private readonly List<string> _users, _parties, _tags;
        public IEnumerable<string> Users => _users;

        public IEnumerable<string> Parties => _parties;

        public IEnumerable<string> Tags => _tags;

        public void ChangeToAll(bool toAll)
        {
            _toAll = toAll;
        }

        public void AddUser(string user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
            }
        }

        public void Addusers(IEnumerable<string> users)
        {
            foreach (var user in users)
            {
                AddUser(user);
            }
        }

        public void RemoveUser(string user)
        {
            _users.Remove(user);
        }

        public void AddParty(string party)
        {
            if (!_parties.Contains(party))
            {
                _parties.Add(party);
            }
        }

        public void AddParties(IEnumerable<string> parties)
        {
            foreach (var party in parties)
            {
                AddParty(party);
            }
        }

        public void RemoveParty(string party)
        {
            _parties.Remove(party);
        }
        public void AddTag(string tag)
        {
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
        }

        public void AddTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }

        public MessageTargets()
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
