using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class TextBoxUpdater : IControlUpdater
    {
        public void UpdateControl(object sourceObject, GroupBox groupBox)
        {
            foreach (var property in sourceObject.GetType().GetProperties())
            {
                if (ValidatePropertyTextBox(property))
                {
                    UpdateTextBoxWithValues(sourceObject, property, groupBox);
                }
            }
        }

        private void UpdateTextBoxWithValues(object sourceObject, PropertyInfo property, GroupBox groupBox)
        {
            var controls = groupBox.Controls.Find(ControlNameBuilder<TextBox>.BuildName(property.Name), true);
            if (controls.Length > 0)
            {
                ((TextBox)controls[0]).Text = property.GetValue(sourceObject).ToString();
            }
        }

        private bool ValidatePropertyTextBox(PropertyInfo property)
        {
            return property.GetCustomAttribute<ControlsAttribute>().ControlType ==
                   ControlsAttribute.ControlTypes.TextBox ||
                   property.GetCustomAttribute<ControlsAttribute>().ControlType ==
                   ControlsAttribute.ControlTypes.LabelTextbox;
        }
    }
}
