using System;
using System.Globalization;
using System.Windows.Media;
using PluginInterfaceNamespace;

namespace Plugin2Namespace
{
    public class Plugin2 : PluginInterface
    {
        public void setLng(String lng)
        {
        }
        public String getName()
        {
            return "Blue";
        }

        public Color getColor()
        {
            return Colors.Blue;
        }
    }
}
