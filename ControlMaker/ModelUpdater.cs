using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class ModelUpdater : IModelUpdater
    {
        public  void UpdateObject(object targetObject, GroupBox groupBox)
        {
            UpdateObjectFromTextBoxes(targetObject,groupBox);
            UpdateObjectFromCheckboxes(targetObject, groupBox);
            UpdateObjectFromTimeRange(targetObject, groupBox);
        }

        public  void UpdateObjectFromTextBoxes(object targetObject, GroupBox groupBox)
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
                        else if(property.PropertyType == typeof(int))
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

        public  void UpdateObjectFromCheckboxes(object targetObject, GroupBox groupBox)
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

            if (filteredProperties.Count>0)
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

        public  void UpdateObjectFromTimeRange(object targetObject, GroupBox groupBox)
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
                            CheckBox checkBox = (CheckBox) FindGropBoxControl<CheckBox>(simpleGroupBox, property.Name);
                            property.SetValue(timeRange,checkBox.Checked);
                            objectProperty.SetValue(targetObject, timeRange);
                        }
                        else if (property.Name == "StartTime")
                        {
                            DateTimePicker startTimePicker =
                                (DateTimePicker) FindGropBoxControl<DateTimePicker>(simpleGroupBox, property.Name);
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

        private  object CreateTimeRangeObject(PropertyInfo objectProperty)
        {
            return Activator.CreateInstance(objectProperty.PropertyType);
        }

        private  GroupBox FindControl(GroupBox groupBox, string propertyName)
        {
            string groupBoxName = ControlNameBuilder<GroupBox>.BuildName(propertyName);
            return (GroupBox) groupBox.Controls.Find(groupBoxName, false)[0];
        }

        private  bool ValidateProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<ControlsAttribute>().ControlType ==
                   ControlsAttribute.ControlTypes.TimeRangePicker;
        }

        private  Control FindGropBoxControl<TType>(GroupBox groupBox, string propertyName)
        {
            string controlName = ControlNameBuilder<TType>.BuildName(propertyName);
            return groupBox.Controls.Find(controlName, false)[0];
        }


        public List<IModelControlUpdater> ModelControlUpdaters;

        public ModelUpdater()
        {
            ModelControlUpdaters = new List<IModelControlUpdater>();
            RegisterUpdaters();
        }

        private void RegisterUpdaters()
        {
            foreach (Type controlUpdaterType in Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IModelControlUpdater))))
            {
                object controlUpdaer = Activator.CreateInstance(controlUpdaterType);
                ModelControlUpdaters.Add(controlUpdaer as IModelControlUpdater);
            }
        }

        public void UpdateModel(object targetObject, GroupBox groupBox)
        {
            foreach (IModelControlUpdater updater in ModelControlUpdaters)
            {
                updater.UpdateObjectFromControl(targetObject,groupBox);
            }
        }



    }
}