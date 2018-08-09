using Sync.Plugins;
using Sync.Source;
using Sync.Tools;
using Sync.Tools.ConfigurationAttribute;
using SyncMultiSourceWrapperPlugin.Filter;
using SyncMultiSourceWrapperPlugin.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncMultiSourceWrapperPlugin
{
    public class SyncMultiSourceWrapperPlugin : Plugin
    {
        private PluginConfigurationManager config;

        WrapperSource wrapper_source;

        public SyncMultiSourceWrapperPlugin() : base("SyncMultiSourceWrapperPlugin", "MikiraSora")
        {
            config = new PluginConfigurationManager(this);
            config.AddItem(Setting.Instance);

            EventBus.BindEvent<PluginEvents.LoadCompleteEvent>(OnLoadComplete);
            EventBus.BindEvent<PluginEvents.InitSourceEvent>(OnInitSource);
            EventBus.BindEvent<PluginEvents.InitFilterEvent>(OnInitFilter);
        }

        private void OnInitFilter(PluginEvents.InitFilterEvent e)
        {
            if (Setting.Instance.MarkSource.ToBool())
            {
                Log.Output("add SourceUserMarkFilter filter.");
                e.Filters.AddFilter(new SourceUserMarkFilter());
            }
        }

        private void OnInitSource(PluginEvents.InitSourceEvent e)
        {
            wrapper_source = new WrapperSource();
            e.Sources.AddSource(wrapper_source);
        }

        private void OnLoadComplete(PluginEvents.LoadCompleteEvent e)
        {
            foreach (var source_name in Setting.Instance.SourceList.ToString().Split(','))
            {
                SourceBase source=null;

                switch (source_name.ToLower())
                {
                    case "bilibili":
                        source = WrapSourcesFactory.GetBililiveSource();
                        break;
                    case "twitch":
                        source = WrapSourcesFactory.GetTwitchSource();
                        break;
                    default:
                        Log.Warn("unknown required source name:" + source_name);
                        break;
                }

                if (source!=null)
                {
                    Log.Output("loaded source:" + source_name);
                    wrapper_source.AddSource(source);
                }
                else
                    Log.Warn("loaded source failed:"+source_name);
            }
        }
    }
}
