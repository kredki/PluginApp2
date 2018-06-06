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
        private bool isEnglish = false;
        public void setLng(String lng)
        {
            if (lng.Equals("en"))
                isEnglish = true;
            else
                isEnglish = false;
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(lng);
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(lng);
        }
        public String getName()
        {
            //Assembly a = Assembly.Load("Plugin");
            /*Assembly a = Assembly.GetExecutingAssembly();
            ResourceManager rm = new ResourceManager("Plugin.Resources", a);
            return rm.GetString("Name");*/
            if (isEnglish)
            {
                return "Green";
            }
            else
                return "Zielony";
        }

        public Color getColor()
        {
            return Colors.Green;
        }
    }
}
