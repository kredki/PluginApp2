using System;
using System.Windows.Media;
using PluginInterfaceNamespace;

namespace PluginNamespace
{
    public class Plugin:PluginInterface
    {
        public String getName()
        {
            return "Green";
        }

        public Color getColor()
        {
            return Colors.Green;
        }
    }
}
