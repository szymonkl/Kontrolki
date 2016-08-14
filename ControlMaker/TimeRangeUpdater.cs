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
    public class TimeRangeUpdater : IControlUpdater
    {
        public void UpdateControl(object sourceObject, GroupBox groupBox)
        {
            PropertyInfo[] properties = sourceObject.GetType().GetProperties();
            foreach (var objectProperty in properties)
            {
                if (ValidateProperty(objectProperty))
                {
                    GroupBox simpleGroupBox = FindControl(groupBox, objectProperty.Name);
                    object timeRange = CreateTimeRangeObject(objectProperty);
                    foreach (PropertyInfo property in timeRange.GetType().GetProperties())
                    {
                        if (property.Name == "Checked")
                        {
                            CheckBox checkBox = (CheckBox)FindGropBoxControl<CheckBox>(simpleGroupBox, property.Name);
                            if (checkBox != null)
                            {
                                object value = GetPropertyValue(GetPropertyValue(sourceObject, objectProperty.Name),
                                    property.Name);
                                checkBox.Checked = (bool) value;
                            }

                        }
                        else if (property.Name == "StartTime")
                        {
                            DateTimePicker startTimePicker =
                                (DateTimePicker)FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
                            if (startTimePicker != null)
                            {
                                object value = GetPropertyValue(GetPropertyValue(sourceObject, objectProperty.Name),
                                    property.Name);
                                startTimePicker.Value = (DateTime) value;
                            }

                        }
                        else if (property.Name == "StopTime")
                        {
                            DateTimePicker stopTimePicker =
                                (DateTimePicker)FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
                            if (stopTimePicker != null)
                            {
                                object value = GetPropertyValue(GetPropertyValue(sourceObject, objectProperty.Name),
                                    property.Name);
                                stopTimePicker.Value = (DateTime)value;
                            }
                        }
                    }
                }
            }
        }
        public void UpdateObjectFromControl(object sourceObject, GroupBox groupBox)
        {
            
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
                   ControlsAttribute.ControlTypes.TimeRangePicker;
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
