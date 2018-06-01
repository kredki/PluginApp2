using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Media;
using PluginInterfaceNamespace;

namespace PluginNamespace
{
    public class Plugin:PluginInterface
    {
        public bool setLng()
        {
            return true;
        }
        public String getName()
        {
            Assembly a = Assembly.Load("Plugin");
            ResourceManager rm = new ResourceManager("Plugin.Resources", a);
            return rm.GetString("Name"); ;
        }

        public Color getColor()
        {
            return Colors.Green;
        }
    }
}
