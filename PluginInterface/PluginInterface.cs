using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Media;

namespace PluginInterfaceNamespace
{
    public interface PluginInterface
    {
        //bool setLng();
        String getName();
        Color getColor();
    }
}
