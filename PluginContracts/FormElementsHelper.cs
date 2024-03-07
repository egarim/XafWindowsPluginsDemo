using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PluginContracts
{
   

    public class FormElementsHelper
    {
        private readonly Form targetForm;
        private readonly List<string> fieldNames;
        private readonly int startX = 10;
        private readonly int startY = 10;
        private readonly int spaceBetween = 30;
        private readonly int textBoxWidth = 200;

        public FormElementsHelper(Form form, List<string> fieldNames)
        {
            this.targetForm = form ?? throw new ArgumentNullException(nameof(form));
            this.fieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));
        }

        public void CreateFormElements()
        {
            int currentY = startY;

            // Dynamically add labels and textboxes based on fieldNames
            foreach (var fieldName in fieldNames)
            {
                // Create and add the label
                Label label = new Label
                {
                    Text = fieldName,
                    Location = new System.Drawing.Point(startX, currentY),
                    AutoSize = true
                };
                targetForm.Controls.Add(label);

                // Create and add the textbox
                TextBox textBox = new TextBox
                {
                    Location = new System.Drawing.Point(startX + label.Width + 10, currentY),
                    Width = textBoxWidth
                };
                
                targetForm.Controls.Add(textBox);

                currentY += spaceBetween;
            }

            // Add the Save button at the end
            Button saveButton = new Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(startX, currentY)
            };
            saveButton.Click += SaveButton_Click;
            targetForm.Controls.Add(saveButton);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Save button clicked!");
            // Implement your save logic here
        }
    }

}
