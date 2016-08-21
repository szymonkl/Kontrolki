using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class ModelBoolTimeUpdater : IModelControlUpdater
    {
        public void UpdateObjectFromControl(object targetObject, GroupBox groupBox)
        {
            PropertyInfo[] properties = targetObject.GetType().GetProperties();
            foreach (var objectProperty in properties)
            {
                if (ValidateProperty(objectProperty))
                {
                    GroupBox simpleGroupBox = FindControl(groupBox, objectProperty.Name);
                    object boolTime = CreateTimeRangeObject(objectProperty);
                    foreach (PropertyInfo property in boolTime.GetType().GetProperties())
                    {
                        if (property.Name == "Checked")
                        {
                            CheckBox checkBox = (CheckBox)FindGropBoxControl<CheckBox>(simpleGroupBox, property.Name);
                            property.SetValue(boolTime, checkBox.Checked);
                            objectProperty.SetValue(targetObject, boolTime);
                        }
                        else if (property.Name == "Time")
                        {
                            DateTimePicker timePicker =
                                (DateTimePicker)FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
                            property.SetValue(boolTime, timePicker.Value);
                            objectProperty.SetValue(targetObject, boolTime);

                        }
                        
                    }
                }
            }
        }

        private object CreateTimeRangeObject(PropertyInfo objectProperty)
        {
            return Activator.CreateInstance(objectProperty.PropertyType);
        }

        private GroupBox FindControl(GroupBox groupBox, string propertyName)
        {
            string groupBoxName = ControlNameBuilder<GroupBox>.BuildName(propertyName);
            return (GroupBox)groupBox.Controls.Find(groupBoxName, false)[0];
        }

        private bool ValidateProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<ControlsAttribute>().ControlType ==
                   ControlsAttribute.ControlTypes.CheckBoxTimePicker;
        }

        private Control FindGropBoxControl<TType>(GroupBox groupBox, string propertyName)
        {
            string controlName = ControlNameBuilder<TType>.BuildName(propertyName);
            return groupBox.Controls.Find(controlName, false)[0];
        }

    }
}
