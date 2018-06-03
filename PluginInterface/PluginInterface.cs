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
        void setLng(String lng);
        String getName();
        Color getColor();
    }
}
