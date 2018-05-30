using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PluginInterfaceNamespace;

namespace PluginApp
{
    public partial class MainWindow : Window
    {
        System.Windows.Ink.StrokeCollection _added;
        System.Windows.Ink.StrokeCollection _removed;
        private bool handle = true;
        List<PluginInterface> plugins = new List<PluginInterface>();
        public MainWindow()
        {
            InitializeComponent();

            String folderPath = @"L:\vcprojekty\PluginApp\plugins\netstandard2.0";

            List<Assembly> assemblyList = new List<Assembly>();
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.dll"))
            {
                if (!file.Equals("PluginInterface.dll"))
                {
                    var temp = Assembly.LoadFile(file);
                    assemblyList.Add(temp);
                }
            }

            foreach (Assembly assembly in assemblyList)
            {
                System.Type t1 = null;
                foreach (var t in assembly.GetTypes())
                {
                    if (t.IsClass && t.IsPublic && typeof(PluginInterface).IsAssignableFrom(t))
                    {
                        t1 = t;
                        break;
                    }
                }
                if (t1 != null)
                {
                    var o = Activator.CreateInstance(t1);
                    plugins.Add((PluginInterface)o);
                }
            }
            InkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
            InkCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;

            addMenuItems();
        }

        private void addMenuItems()
        {
            MenuItem mi1 = new MenuItem();
            mi1.Header = "Black";
            mi1.Click += new RoutedEventHandler(mi_Click);
            ColorContexMenu.Items.Add(mi1);

            foreach (PluginInterface plugin in plugins)
            {
                MenuItem mi = new MenuItem();
                mi.Header = plugin.getName();
                mi.Click += new RoutedEventHandler(mi_Click);
                ColorContexMenu.Items.Add(mi);
            }
        }

        void mi_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = (MenuItem)sender; // That's the sepcific item that has been clicked.
            String name = (sender as MenuItem).Header.ToString();
            if (name.Equals("Black"))
            {
                InkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
            } else
            {
                foreach (PluginInterface plugin in plugins)
                {
                    if(name.Equals(plugin.getName()))
                    {
                        InkCanvas.DefaultDrawingAttributes.Color = plugin.getColor();
                    }
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (handle)
            {
                _added = e.Added;
                _removed = e.Removed;
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            handle = false;
            InkCanvas.Strokes.Remove(_added);
            InkCanvas.Strokes.Add(_removed);
            handle = true;
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            handle = false;
            InkCanvas.Strokes.Add(_added);
            InkCanvas.Strokes.Remove(_removed);
            handle = true;
        }
    }
}
