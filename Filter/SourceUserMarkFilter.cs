using Sync.MessageFilter;
using Sync.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncMultiSourceWrapperPlugin.Filter
{
    [FilterPriority(Priority = FilterPriority.Lowest)]
    class SourceUserMarkFilter : IFilter, ISourceDanmaku
    {
        public void onMsg(ref IMessageBase msg)
        {
            StackTrace st = new StackTrace(true);

            for (int i = 1/*skip self*/; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);

                if(frame.GetMethod().DeclaringType.Assembly != typeof(Sync.SyncHost).Assembly)
                {
                    var name = frame.GetMethod().DeclaringType.Name;

                    var new_user = new StringElement(msg.User.perfix+name+".", msg.User.RawText, msg.User.suffix);
                    msg.User = new_user;
                    break;
                }
            }
        }
    }
}
