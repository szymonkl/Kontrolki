using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class ModelCheckBoxUpdater : IModelControlUpdater
    {
        public void UpdateObjectFromControl(object targetObject, GroupBox groupBox)
        {
            var properies = targetObject.GetType().GetProperties();
            var filteredProperties =
                properies.ToList()
                    .FindAll(
                        p =>
                            p.GetCustomAttribute<ControlsAttribute>().ControlType ==
                            ControlsAttribute.ControlTypes.Checkbox ||
                            p.GetCustomAttribute<ControlsAttribute>().ControlType ==
                            ControlsAttribute.ControlTypes.LabelCheckBox);

            if (filteredProperties.Count > 0)
            {
                var allGroupBoxesControls = new List<Control>();
                foreach (Control control in groupBox.Controls)
                {
                    allGroupBoxesControls.AddRange(control.Controls.OfType<Control>());
                }

                foreach (Control control in allGroupBoxesControls)
                {
                    if (control is CheckBox)
                    {
                        var property = filteredProperties.First(
                            p => p.Name == ControlNameBuilder<CheckBox>.GetPropertyNameFromControlName(control.Name));
                        if (property != null)
                        {
                            if (property.PropertyType == typeof(bool))
                            {
                                property.SetValue(targetObject, ((CheckBox)control).Checked);
                            }
                        }
                    }
                }
            }
        }
    }
}
