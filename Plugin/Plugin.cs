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
        public void setLng(String lng)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lng);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lng);
        }
        public String getName()
        {
            /*Assembly a = Assembly.Load("Plugin");
            ResourceManager rm = new ResourceManager("Plugin.Resources", a);
            return rm.GetString("Name");*/
            return "Green";
        }

        public Color getColor()
        {
            return Colors.Green;
        }
    }
}
