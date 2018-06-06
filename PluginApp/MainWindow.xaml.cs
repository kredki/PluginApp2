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
        Stack<System.Windows.Ink.StrokeCollection> addedStrokes;
        Stack<System.Windows.Ink.StrokeCollection> removedStrokes;
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

            //String folderPath = @"L:\vcprojekty\PluginApp2\plugins\netstandard2.0";
            String folderPath = @"..\..\..\plugins\netstandard2.0";
            //String folderPath = @"X:\PluginApp2\plugins\netstandard2.0";

            List<Assembly> assemblyList = new List<Assembly>();
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.dll"))
            {
                //if (!file.Equals("PluginInterface.dll"))
                {
                    //var temp = Assembly.LoadFile(file);
                    var temp = Assembly.LoadFile(System.IO.Path.GetFullPath(file));
                    assemblyList.Add(temp);
                }
            }

            foreach (Assembly assembly in assemblyList)
            {
                System.Type t1 = null;
                try
                {
                    foreach (var t in assembly.GetTypes())
                    {
                        if (t.IsClass && t.IsPublic && typeof(PluginInterface).IsAssignableFrom(t))
                        {
                            t1 = t;
                            break;
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Exception exSub in ex.LoaderExceptions)
                    {
                        sb.AppendLine(exSub.Message);
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (exFileNotFound != null)
                        {
                            if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                            {
                                sb.AppendLine("Fusion Log:");
                                sb.AppendLine(exFileNotFound.FusionLog);
                            }
                        }
                        sb.AppendLine();
                    }
                    string errorMessage = sb.ToString();
                    //Display or log the error based on your application.
                    Console.WriteLine(sb);
                }

                if (t1 != null)
                {
                    var o = Activator.CreateInstance(t1);
                    plugins.Add((PluginInterface)o);
                }
            }
            InkCanvas.DefaultDrawingAttributes.Color = Colors.Black;
            InkCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
            addedStrokes = new Stack<System.Windows.Ink.StrokeCollection>();
            removedStrokes = new Stack<System.Windows.Ink.StrokeCollection>();

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
            if (handle)
            {
                addedStrokes.Push(e.Added);
                removedStrokes.Push(e.Removed);
                if (e.Added != null)
                {
                    removedStrokes.Clear();
                }
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (addedStrokes.Count != 0)
            {
                handle = false;
                var stroke = addedStrokes.Pop();
                InkCanvas.Strokes.Remove(stroke);
                removedStrokes.Push(stroke);
                handle = true;
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (removedStrokes.Count != 0)
            {
                handle = false;
                var stroke = removedStrokes.Pop();
                InkCanvas.Strokes.Add(stroke);
                addedStrokes.Push(stroke);
                handle = true;
            }
        }

        private void ChangeLng(object sender, RoutedEventArgs e)
        {
            String lng = "en";
            if(Thread.CurrentThread.CurrentUICulture.Equals(new CultureInfo("pl")))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lng);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lng);
            } else
            {
                lng = "pl";
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lng);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lng);
            }
            Assembly a = Assembly.Load("PluginApp");
            rm = new ResourceManager("PluginApp.Resources", a);
            colorBtn.Content = rm.GetString("Color");
            uddoBtn.Content = rm.GetString("Undo");
            redoBtn.Content = rm.GetString("Redo");
            changeLngBtn.Content = rm.GetString("Changelanguage");

            foreach (PluginInterface plugin in plugins)
            {
                plugin.setLng(lng);
            }

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