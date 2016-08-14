using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class CheckBoxUpdater : IControlUpdater
    {
        public void UpdateControl(object sourceObject, GroupBox groupBox)
        {
            foreach (var property in sourceObject.GetType().GetProperties())
            {
                if (ValidatePropertyCheckBox(property))
                {
                    UpdateCheckBoxBoxWithValues(sourceObject, property, groupBox);
                }
            }
        }

        private void UpdateCheckBoxBoxWithValues(object sourceObject, PropertyInfo property, GroupBox groupBox)
        {
            var controls = groupBox.Controls.Find(ControlNameBuilder<CheckBox>.BuildName(property.Name), true);
            if (controls.Length > 0)
            {
                ((CheckBox)controls[0]).Checked = (bool)property.GetValue(sourceObject);
            }

        }

        private bool ValidatePropertyCheckBox(PropertyInfo property)
        {
            return property.GetCustomAttribute<ControlsAttribute>().ControlType ==
                   ControlsAttribute.ControlTypes.Checkbox ||
                   property.GetCustomAttribute<ControlsAttribute>().ControlType ==
                   ControlsAttribute.ControlTypes.LabelCheckBox;
        }
    }
}
