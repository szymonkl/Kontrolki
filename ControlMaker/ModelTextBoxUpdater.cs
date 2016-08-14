using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class ModelTextBoxUpdater : IModelControlUpdater
    {
        public void UpdateObjectFromControl(object targetObject, GroupBox groupBox)
        {
            var properies = targetObject.GetType().GetProperties();
            var allGroupBoxesControls = new List<Control>();
            foreach (Control control in groupBox.Controls)
            {
                allGroupBoxesControls.AddRange(control.Controls.OfType<Control>());
            }

            foreach (Control control in allGroupBoxesControls)
            {
                if (control is TextBox)
                {
                    var property = properies.First(
                        p => p.Name == ControlNameBuilder<TextBox>.GetPropertyNameFromControlName(control.Name));
                    if (property != null)
                    {
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(targetObject, control.Text);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(targetObject, Convert.ToInt32(control.Text));
                        }
                        else if (property.PropertyType == typeof(double))
                        {
                            property.SetValue(targetObject, control.Text);
                        }
                        else if (property.PropertyType == typeof(decimal))
                        {
                            property.SetValue(targetObject, control.Text);
                        }
                        else if (property.PropertyType == typeof(float))
                        {
                            property.SetValue(targetObject, control.Text);
                        }
                    }
                }
            }
        }
    }
}
