using DefaultPlugin.Sources.BiliBili;
using DefaultPlugin.Sources.Twitch;
using Sync;
using Sync.Plugins;
using Sync.Source;
using Sync.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyncMultiSourceWrapperPlugin
{
    public static class WrapSourcesFactory
    {
        /*
        public static ConfigurationElement GetConfigurationElement(string plugin_name,string sub_section_name,string config_name)
        {
            Type config_manager_type = typeof(PluginConfigurationManager);
            var config_manager_list = config_manager_type.GetField("ConfigurationSet", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null) as IEnumerable<PluginConfigurationManager>;

            //each configuration manager
            foreach (var manager in config_manager_list)
            {
                //get plguin name
                var plguin = config_manager_type.GetField("instance", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(manager) as Plugin;
                if (plguin.Name == plugin_name)
                {

                    //get List<PluginConfiuration>
                    var config_items_field = config_manager_type.GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
                    var config_items_list = config_items_field.GetValue(manager);

                    //List<PluginConfiuration>.GetEnumerator
                    var enumerator = config_items_field.FieldType.GetMethod("GetEnumerator", BindingFlags.Public | BindingFlags.Instance)
                        .Invoke(config_items_list, null) as IEnumerator;

                    //each List<PluginConfiuration>
                    while (enumerator.MoveNext())
                    {
                        var config_item = enumerator.Current;
                        var instance = config_item.GetType().GetField("config", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(config_item);
                        if (instance.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance)?.GetValue(instance).ToString() == sub_section_name)
                        {
                            var configInstance = instance;
                            var config_type = configInstance.GetType();

                            foreach (var prop in config_type.GetProperties())
                            {
                                if (prop.Name == config_name)
                                    return prop.GetValue(configInstance) as ConfigurationElement;
                            }
                        }
                    }

                    break;
                }
            }

            return null;
        }
        */

        public static SourceBase GetBililiveSource()
        {
            var default_plugin = (from plugin in SyncHost.Instance.EnumPluings() where plugin is DefaultPlugin.DefaultPlugin select plugin).FirstOrDefault();

            if (default_plugin == null)
                return null;

            var bilibili_instance = default_plugin.GetType().GetField("srcBili", BindingFlags.NonPublic|BindingFlags.Instance)?.GetValue(default_plugin) as BiliBili;

            bilibili_instance = bilibili_instance != null ? bilibili_instance : (from info in default_plugin.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                                                           where info.FieldType == typeof(BiliBili)
                                                                           select info.GetValue(default_plugin) as BiliBili).FirstOrDefault();


            return bilibili_instance;
        }

        public static SourceBase GetTwitchSource()
        {
            var default_plugin = (from plugin in SyncHost.Instance.EnumPluings() where plugin is DefaultPlugin.DefaultPlugin select plugin).FirstOrDefault();

            if (default_plugin == null)
                return null;

            var twitch_instance = default_plugin.GetType().GetField("srcTwitch", BindingFlags.NonPublic| BindingFlags.Instance)?.GetValue(default_plugin) as Twitch;

            //兼容查询
            twitch_instance = twitch_instance!=null? twitch_instance:(from info in default_plugin.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                               where info.FieldType == typeof(Twitch)
                               select info.GetValue(default_plugin) as Twitch).FirstOrDefault();

            return twitch_instance;
        }
    }
}
