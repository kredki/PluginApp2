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
        private bool isEnglish = false;
        public void setLng(String lng)
        {
            if (lng.Equals("en"))
                isEnglish = true;
            else
                isEnglish = false;
        }
        public String getName()
        {
            if (isEnglish)
            {
                return "Pink";
            }
            else
                return "Różowy";
        }

        public Color getColor()
        {
            return Colors.Pink;
        }
    }
}
