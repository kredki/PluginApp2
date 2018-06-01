using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
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
        //System.Windows.Ink.StrokeCollection _added;
        //System.Windows.Ink.StrokeCollection _removed;
        System.Windows.Ink.StrokeCollection addedStrokes;
        System.Windows.Ink.StrokeCollection removedStrokes;
        Stack<System.Windows.Ink.StrokeCollection> addedStrokes2;
        Stack<System.Windows.Ink.StrokeCollection> removedStrokes2;
        ResourceManager rm;
        private bool handle = true;
        List<PluginInterface> plugins = new List<PluginInterface>();
        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl");
            Assembly a = Assembly.Load("PluginApp");
            rm = new ResourceManager("PluginApp.Resources", a);
            colorBtn.Content = rm.GetString("Color");
            uddoBtn.Content = rm.GetString("Undo");
            redoBtn.Content = rm.GetString("Redo");
            changeLngBtn.Content = rm.GetString("Changelanguage");

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
            addedStrokes = new System.Windows.Ink.StrokeCollection();
            removedStrokes = new System.Windows.Ink.StrokeCollection();
            addedStrokes2 = new Stack<System.Windows.Ink.StrokeCollection>();
            removedStrokes2 = new Stack<System.Windows.Ink.StrokeCollection>();

            addMenuItems();
        }

        private void addMenuItems()
        {
            MenuItem mi1 = new MenuItem();
            mi1.Header = rm.GetString("Black");
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
            if (name.Equals(rm.GetString("Black")))
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
            /*if (handle)
            {
                _added = e.Added;
                _removed = e.Removed;
            }*/
            if (handle)
            {
                if (e.Added != null)
                {
                    //addedStrokes.Add(e.Added);
                    //removedStrokes.Clear();

                    addedStrokes2.Push(e.Added);
                    removedStrokes2.Clear();
                }
                if (e.Removed != null)
                {
                    removedStrokes2.Push(e.Removed);
                }
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            /*
            handle = false;
            InkCanvas.Strokes.Remove(_added);
            InkCanvas.Strokes.Add(_removed);
            handle = true;*/

            /*if (addedStrokes.Count != 0)
            {
                removedStrokes.Add(addedStrokes.ElementAt(addedStrokes.Count - 1));
                addedStrokes.RemoveAt(addedStrokes.Count - 1);
            }

            InkCanvas.Strokes = addedStrokes;*/

            /*if(addedStrokes2.Count != 0)
            {
                removedStrokes2.Push(addedStrokes2.Pop());
            }*/

            if (addedStrokes2.Count != 0)
            {
                handle = false;
                var stroke = addedStrokes2.Pop();
                InkCanvas.Strokes.Remove(stroke);
                removedStrokes2.Push(stroke);
                handle = true;
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            /*handle = false;
            InkCanvas.Strokes.Add(_added);
            InkCanvas.Strokes.Remove(_removed);
            handle = true;*/

            /*if (removedStrokes.Count != 0)
            {
                addedStrokes.Add(removedStrokes.ElementAt(removedStrokes.Count - 1));
                removedStrokes.RemoveAt(removedStrokes.Count - 1);
            }

            InkCanvas.Strokes = addedStrokes;*/

            /*if(removedStrokes2.Count != 0)
            {
                addedStrokes2.Push(removedStrokes2.Pop());
            }
            var array = addedStrokes2.ToArray();
            List<InkCanvas> list = array.OfType<InkCanvas>().ToList();
            InkCanvas = list;*/

            if (removedStrokes2.Count != 0)
            {
                handle = false;
                var stroke = removedStrokes2.Pop();
                InkCanvas.Strokes.Add(stroke);
                addedStrokes2.Push(stroke);
                handle = true;
            }
        }

        private void ChangeLng(object sender, RoutedEventArgs e)
        {
            if(Thread.CurrentThread.CurrentUICulture.Equals(new CultureInfo("pl")))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            } else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pl");
            }
            Assembly a = Assembly.Load("PluginApp");
            rm = new ResourceManager("PluginApp.Resources", a);
            colorBtn.Content = rm.GetString("Color");
            uddoBtn.Content = rm.GetString("Undo");
            redoBtn.Content = rm.GetString("Redo");
            changeLngBtn.Content = rm.GetString("Changelanguage");

            var menuItems = ColorContexMenu.Items.Cast<MenuItem>().ToArray();
            foreach (MenuItem menuItem in menuItems)
            {
                menuItem.Click -= new RoutedEventHandler(mi_Click);
            }
            ColorContexMenu.Items.Clear();
            addMenuItems();
        }
    }
}