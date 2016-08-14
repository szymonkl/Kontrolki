using System;
using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class ModelTimeRangeUpdater : IModelControlUpdater
    {
        public void UpdateObjectFromControl(object targetObject, GroupBox groupBox)
        {
            PropertyInfo[] properties = targetObject.GetType().GetProperties();
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
                            property.SetValue(timeRange, checkBox.Checked);
                            objectProperty.SetValue(targetObject, timeRange);
                        }
                        else if (property.Name == "StartTime")
                        {
                            DateTimePicker startTimePicker =
                                (DateTimePicker)FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
                            property.SetValue(timeRange, startTimePicker.Value);
                            objectProperty.SetValue(targetObject, timeRange);

                        }
                        else if (property.Name == "StopTime")
                        {
                            DateTimePicker stopTimePicker =
                                (DateTimePicker)FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
                            property.SetValue(timeRange, stopTimePicker.Value);
                            objectProperty.SetValue(targetObject, timeRange);
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
                   ControlsAttribute.ControlTypes.TimeRangePicker;
        }

        private Control FindGropBoxControl<TType>(GroupBox groupBox, string propertyName)
        {
            string controlName = ControlNameBuilder<TType>.BuildName(propertyName);
            return groupBox.Controls.Find(controlName, false)[0];
        }
    }
}
