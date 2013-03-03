using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickLync
{
    public interface IPluginProvider
    {

        string Name { get; }

        List<PluginItem> Search(string query);

        void Refresh();

    }
}
