using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WeChat.Enterprise
{
    /// <summary>
    /// 定义微信企业信息。
    /// </summary>
    public class Agent
    {
        public int Id { get; private set; }
        public bool IsClosed { get; internal set; }

        public string Name { get; set; }

        public string SqureLogoUrl { get; set; }

        public string Description { get; set; }

        public string RedirectDomain { get; set; }

        public bool ReportLocationFlag { get; set; }

        public bool IsReportEnter { get; set; }

        public string HomeUrl { get; set; }

        internal Agent(int agentId)
        {
            Id = agentId;
        }


        internal HttpContent CreateUpdateContent()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8);
                writer.WriteStartDocument();
                writer.WriteName("agentid");
                writer.WriteValue(Id);
                writer.WriteName("report_location");
                writer.WriteValue(Convert.ToInt32(ReportLocationFlag));
                writer.WriteName("logo_mediaid");
                writer.WriteValue(SqureLogoUrl);
                writer.WriteName("name");
                writer.WriteValue(Name);
                writer.WriteName("description");
                writer.WriteValue(Description);
                writer.WriteName("redirect_domain");
                writer.WriteValue(RedirectDomain);
                writer.WriteName("isreportenter");
                writer.WriteValue(Convert.ToInt32(IsReportEnter));
                writer.WriteName("home_url");
                writer.WriteValue(HomeUrl);
                writer.WriteEndDocument();
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return new StringContent(Encoding.Default.GetString(bytes));
            }
        }
    }
}
