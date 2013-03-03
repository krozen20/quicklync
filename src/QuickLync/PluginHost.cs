using LaunchySharp;
using Microsoft.Lync.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QuickLync
{
    public class PluginHost : IPlugin
    {

        const string PLUGIN_NAME = "PluginHost";

        protected IPluginHost Host { get; set; }

        protected List<IPluginProvider> Plugins { get; set; }

        public IntPtr doDialog()
        {
            return IntPtr.Zero;
        }

        public void endDialog(bool acceptedByUser)
        {
            //not supported
        }

        public void getCatalog(List<ICatItem> catalogItems)
        {
            foreach (var plugin in Plugins)
            {
                plugin.Refresh();
            }
        }

        public uint getID()
        {
            return Host.hash(PLUGIN_NAME);
        }

        public void getLabels(List<IInputData> inputDataList)
        {
            
        }

        public string getName()
        {
            return PLUGIN_NAME;
        }

        public void getResults(List<IInputData> inputDataList, List<ICatItem> resultsList)
        {
            var factory = Host.catItemFactory();
            string query = inputDataList[0].getText();

            var pluginItems = new List<PluginItem>();

            foreach (var plugin in Plugins)
            {
                var plugins = plugin.Search(query);

                if (plugins != null)
                {
                    pluginItems.AddRange(plugins);
                }
            }

            foreach (var pluginItem in pluginItems)
            {
                resultsList.Add(factory.createCatItem(pluginItem.Uri, pluginItem.Name, getID(), pluginItem.Icon ?? getIcon()));
            }

        }

        private string getIcon()
        {
            return Path.Combine(Host.launchyPaths().getIconsPath(), "Lync.ico");
        }

        public bool hasDialog()
        {
            return false;
        }

        public void init(IPluginHost pluginHost)
        {
            Host = pluginHost;

            Plugins = new List<IPluginProvider>();
            Plugins.Add(new LyncProvider());
        }

        public void launchItem(List<IInputData> inputDataList, ICatItem item)
        {
            var uri = item.getFullPath();
            Process.Start(uri);
        }

        public void launchyHide()
        {
            
        }

        public void launchyShow()
        {
            
        }
    }
}
