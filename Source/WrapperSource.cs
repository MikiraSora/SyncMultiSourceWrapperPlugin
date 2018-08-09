using Sync.MessageFilter;
using Sync.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyncMultiSourceWrapperPlugin.Source
{
    class WrapperSource : SendableSource
    {
        private List<SourceBase> managed_sources=new List<SourceBase>();

        private Regex dispatch_message_regex = new Regex(@"((\w+):)?(.+)");

        public WrapperSource() : base("SyncMultiSourceWrapper", "MikiraSora")
        {

        }

        public void AddSource(SourceBase source)
        {
            managed_sources.Add(source);

            if (Status == SourceStatus.CONNECTED_WORKING)
            {
                //他喵的柑橘灰渣
                source.Disconnect();
                source.Connect();
            }
        }

        public override void Connect()
        {
            if (Status == SourceStatus.CONNECTED_WORKING)
                return;

            //先关掉再开
            Disconnect();

            bool include_send = false;

            foreach (var source in managed_sources)
            {
                try
                {
                    source.Connect();

                    if (source is SendableSource)
                        include_send = true;
                }
                catch (Exception e)
                {
                    Log.Error($"Cant connect source {source.Name} cause: {e.Message}");
                    source.Disconnect();
                }
            }

            Status = SourceStatus.CONNECTED_WORKING;
            SendStatus = include_send;
        }

        public override void Disconnect()
        {
            foreach (var source in managed_sources)
            {
                try
                {
                    source.Disconnect();
                }
                catch (Exception e)
                {
                    Log.Error($"Cant disconnect source {source.Name} cause: {e.Message}");
                }
            }

            Status = SourceStatus.USER_DISCONNECTED;
            SendStatus = false;
        }

        public override void Login(string user, string password)
        {
            throw new NotImplementedException();
        }

        private void ParseMessage(Match match,out IEnumerable<SendableSource> source,IMessageBase actual_message)
        {
            if (!match.Success)
            {
                source = managed_sources.Where(s => s is SendableSource).Cast<SendableSource>();
                return;
            }

            var prefix = match.Groups["2"].Value;
            //adjust message
            actual_message.Message = match.Groups["3"].Value;

            if (int.TryParse(prefix,out var index))
            {
                var list = new List<SendableSource>();

                var s = (managed_sources.Count>index?managed_sources.AsEnumerable().ElementAt(2):null) as SendableSource;
                if (s != null)
                    list.Add(s);
                source = list;
            }
            else
            {
                source = managed_sources.Where(s => s is SendableSource&&s.Name.ToLower().StartsWith(prefix)).Cast<SendableSource>();
            }
        }

        public override void Send(IMessageBase msg)
        {
            var match = dispatch_message_regex.Match(msg.Message.RawText);

            ParseMessage(match, out var sources, msg);

            foreach (var s in sources)
                s.Send(msg);
        }
    }
}
