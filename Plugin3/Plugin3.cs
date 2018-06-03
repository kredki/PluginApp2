using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Media;
using PluginInterfaceNamespace;

namespace Plugin3Namespace
{
    public class Plugin3 : PluginInterface
    {
        public void setLng(String lng)
        {
        }
        public String getName()
        {
            /*Assembly a = Assembly.Load("Plugin");
            ResourceManager rm = new ResourceManager("Plugin.Resources", a);
            return rm.GetString("Name");*/
            return "Pink";
        }

        public Color getColor()
        {
            return Colors.Pink;
        }
    }
}
