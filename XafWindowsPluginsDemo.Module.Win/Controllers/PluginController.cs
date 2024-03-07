using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using PluginContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using XafWindowsPluginsDemo.Module.BusinessObjects;

namespace XafWindowsPluginsDemo.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PluginController : ViewController
    {
        SimpleAction ShowPlugin;
        SimpleAction LoadPlugin;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public PluginController()
        {
            InitializeComponent();
            LoadPlugin = new SimpleAction(this, "Load Plugin", "View");
            LoadPlugin.Execute += LoadPlugin_Execute;


            ShowPlugin = new SimpleAction(this, "Show Plugin", "View");
            ShowPlugin.Execute += ShowPlugin_Execute;
            
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void ShowPlugin_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (DomainSender, args) =>
            {
                string assemblyName = new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName))
                {
                    if (stream == null) return null;
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };

            var CurrentPlugin=this.View.CurrentObject as Plugin;
            string assemblyFile = Path.Combine(CurrentPlugin.Path, CurrentPlugin.Dll);
            Assembly loadedAssembly = Assembly.LoadFrom(assemblyFile);

            var ClassType= loadedAssembly.GetType(CurrentPlugin.ClassName);
            IPlugin plugin= Activator.CreateInstance(ClassType) as IPlugin;

            plugin.ShowWindow(this.View.ObjectSpace);

            Console.WriteLine($"Assembly loaded: {loadedAssembly.FullName}");

            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112737/).
        }
        private void LoadPlugin_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set the file dialog to filter for DLL files
            openFileDialog.Filter = "DLL Files (*.dll)|*.dll|All Files (*.*)|*.*";
            openFileDialog.Title = "Select a DLL File";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the path of specified file
                string filePath = openFileDialog.FileName;
                var CurrentPlugin= this.View.CurrentObject as Plugin;
                CurrentPlugin.Dll = Path.GetFileName(filePath);
                CurrentPlugin.Path = Path.GetDirectoryName(filePath);


                Assembly loadedAssembly = Assembly.LoadFrom(filePath);
                Console.WriteLine($"Assembly loaded: {loadedAssembly.FullName}");
                
                // Get all types in the assembly that implement the IPlugin interface
                Type pluginType = typeof(IPlugin);
                Type[] types = loadedAssembly.GetTypes()
                    .Where(p => pluginType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                    .ToArray();

                CurrentPlugin.ClassName = types[0].FullName;
                this.View.ObjectSpace.CommitChanges();
                // Now you can do something with the selected file path
                //MessageBox.Show($"Selected file: {filePath}", "File Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
