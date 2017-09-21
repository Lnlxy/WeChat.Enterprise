using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    public struct MaterialKey
    {
        public string MediaId { get; private set; }

        public MediaType MediaType { get; private set; }

        public MaterialKey(string mediaId, MediaType mediaType)
        {
            MediaId = mediaId;
            MediaType = mediaType;
        }
    }
}
