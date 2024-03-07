using DevExpress.ExpressApp;
using PluginContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPlugin
{
    public class CustomWindowPlugin : IPlugin
    {
        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(IObjectSpace Os)
        {
           
            Form1 form = new Form1(Os);

            List<string> Properties = new List<string>();
            Properties.Add("Name");
            Properties.Add("LastName");



            FormElementsHelper formElementsHelper = new FormElementsHelper(form, Properties);
            formElementsHelper.CreateFormElements();
            form.Show();
        }
    }
}
