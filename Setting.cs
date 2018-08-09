using Sync.Tools;
using Sync.Tools.ConfigurationAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncMultiSourceWrapperPlugin
{
    public class Setting: IConfigurable
    {
        [List(AllowMultiSelect = true, IgnoreCase = true, SplitSeparator = ',', ValueList = new[] { "bilibili","twitch" })]
        public ConfigurationElement SourceList { get; set; } = "bilibili,twitch";

        [Bool]
        public ConfigurationElement DebugMode { get; set; }

        public static Setting Instance { get; } = new Setting();

        public void onConfigurationLoad()
        {
            Log.IsDebug = DebugMode?.ToBool()??false;
        }

        public void onConfigurationSave()
        {

        }

        public void onConfigurationReload()
        {
            Log.IsDebug = DebugMode.ToBool();
        }
    }
}
