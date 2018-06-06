using System;
using System.Globalization;
using System.Windows.Media;
using PluginInterfaceNamespace;

namespace Plugin2Namespace
{
    public class Plugin2 : PluginInterface
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
                return "Blue";
            }
            else
                return "Niebieski";
        }

        public Color getColor()
        {
            return Colors.Blue;
        }
    }
}
