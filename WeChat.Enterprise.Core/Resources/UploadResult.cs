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

        public long CreateAt { get; private set; }
    }
}
