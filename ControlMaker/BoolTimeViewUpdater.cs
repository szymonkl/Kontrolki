using System;
using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class BoolTimeViewUpdater : IControlUpdater
    {
        public void UpdateControl(object sourceObject, GroupBox groupBox)
        {
            PropertyInfo[] properties = sourceObject.GetType().GetProperties();
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
                            if (checkBox != null)
                            {
                                object value = GetPropertyValue(GetPropertyValue(sourceObject, objectProperty.Name),
                                    property.Name);
                                checkBox.Checked = (bool)value;
                            }

                        }
                        else if (property.Name == "Time")
                        {
                            DateTimePicker timePicker =
                                (DateTimePicker)FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
                            if (timePicker != null)
                            {
                                object value = GetPropertyValue(GetPropertyValue(sourceObject, objectProperty.Name),
                                    property.Name);
                                timePicker.Value = (DateTime)value;
                            }

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

        private static object GetPropertyValue(object obj, string propertyName)
        {
            var objType = obj.GetType();
            var prop = objType.GetProperty(propertyName);

            return prop.GetValue(obj, null);
        }
    }
}
