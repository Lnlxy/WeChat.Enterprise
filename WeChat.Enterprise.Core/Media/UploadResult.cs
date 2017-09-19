using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat.Enterprise
{
    public class UploadResult
    {
        public int ErrCode { get; private set; }

        public string ErrMsg { get; private set; }

        public string Type { get; private set; }

        public string MediaId { get; private set; }

        public Media Media { get; private set; }

        public string CreateAt { get; private set; }

        public UploadResult(int errCode, string errMsg, string type, string mediaId, string createAt, Media media)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
            Type = type;
            MediaId = mediaId;
            CreateAt = createAt;
            Media = media;
        }
    }
}
