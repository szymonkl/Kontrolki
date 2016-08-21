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
    public class TimeRangeBuilder
    {
        public static int CheckBoxWidth { get; set; } = 25;
        public static int CheckBoxHeight { get; set; } = 25;
        public static int StartPickerWidth { get; set; } = 80;
        public static int StartPickerHeight { get; set; } = 30;
        public static int StopPickerWidth { get; set; } = 80;
        public static int StopPickerHeight { get; set; } = 30;
        
        private static  PropertyInfo _boolProperty;
        private static PropertyInfo _startTimeProperty;
        private static PropertyInfo _stopTimeProperty;

        public static GroupBox CreateTimeRangeControl(object controlObject, PropertyInfo property)
        {
            GroupBox groupBox = new GroupBox();
            if (IsValid(property))
            {
                //Getting values
                bool isChecked = GetBoolValue(property, controlObject);
                DateTime startTime = GetStartTime(property, controlObject);
                DateTime stopTime = GetStopTime(property, controlObject);
                
                CreateSimpleGroupBox(property, groupBox);
                
                var checkBox = CreateCheckBox(isChecked);
                
                var startTimePicker = CreateStartTimePicker(startTime);
                
                var stopTimePicker = CreateStopTimePicker(stopTime);
                
                groupBox.Controls.Add(checkBox);
                groupBox.Controls.Add(startTimePicker);
                groupBox.Controls.Add(stopTimePicker);
                
                //Setting control position

                Control[] controls = new Control[3];
                controls[0] = checkBox;
                controls[1] = startTimePicker;
                controls[2] = stopTimePicker;

                ControlPositioner.LeftMargin = 10;
                ControlPositioner.TopMargin = 20;
                ControlPositioner.HorizontalSpace = 10;
                ControlPositioner.VerticalSpace = 20;
                
                ControlPositioner.ToLeft(controls);
                
            }

            return groupBox;



        }

        private static void CreateSimpleGroupBox(PropertyInfo property, GroupBox groupBox)
        {
            Attribute attribute = property.GetCustomAttribute<NazwaAttribute>();
            groupBox.Text = attribute != null ? ((NazwaAttribute) attribute).DisplayName : property.Name;
            groupBox.Name = ControlNameBuilder<GroupBox>.BuildName(property.Name);
            groupBox.Height = ControlPositioner.TopMargin + CheckBoxHeight + ControlPositioner.BottomMargin;
            groupBox.Width = ControlPositioner.LeftMargin + CheckBoxWidth + 2*ControlPositioner.HorizontalSpace +
                             StartPickerWidth + StopPickerWidth + ControlPositioner.RightMargin;
        }

        private static CheckBox CreateCheckBox(bool isChecked)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Name = ControlNameBuilder<CheckBox>.BuildName(_boolProperty.Name);
            checkBox.Checked = isChecked;
            checkBox.Height = CheckBoxHeight;
            checkBox.Width = CheckBoxWidth;
            return checkBox;
        }

        private static DateTimePicker CreateStartTimePicker(DateTime startTime)
        {
            DateTimePicker startTimePicker = new DateTimePicker();
            startTimePicker.Name = ControlNameBuilder<DateTimePicker>.BuildName(_startTimeProperty.Name);
            startTimePicker.Value = startTime;
            startTimePicker.Format = DateTimePickerFormat.Time;
            startTimePicker.ShowUpDown = true;
            startTimePicker.Height = StartPickerHeight;
            startTimePicker.Width = StartPickerWidth;
            return startTimePicker;
        }

        private static DateTimePicker CreateStopTimePicker(DateTime stopTime)
        {
            DateTimePicker stopTimePicker = new DateTimePicker();
            stopTimePicker.Name = ControlNameBuilder<DateTimePicker>.BuildName(_stopTimeProperty.Name);
            stopTimePicker.Value = stopTime;
            stopTimePicker.Format = DateTimePickerFormat.Time;
            stopTimePicker.ShowUpDown = true;
            stopTimePicker.Height = StopPickerHeight;
            stopTimePicker.Width = StopPickerWidth;
            return stopTimePicker;
        }

        private static bool IsValid(PropertyInfo property)
        {
            return property.PropertyType == typeof(TimeRange);
        }

        private static bool GetBoolValue(PropertyInfo property, object controlObject)
        {
             Type controlObjectTime = controlObject.GetType();
             PropertyInfo[] controlObjectProperties = controlObjectTime.GetProperties();
             PropertyInfo timeRangeProperty = controlObjectProperties.ToList().Find(prop => prop.Name == property.Name);
             object timeRange = Activator.CreateInstance(timeRangeProperty.PropertyType);
             PropertyInfo[] timeRangeProperties = timeRange.GetType().GetProperties();
            _boolProperty = timeRangeProperties.ToList().Find(p => p.PropertyType == typeof(bool));
             object value = GetPropertyValue(GetPropertyValue(controlObject, timeRangeProperty.Name), _boolProperty.Name);
              
             return (bool) value;
        }

        private static DateTime GetStartTime(PropertyInfo property, object controlObject)
        {
            Type controlObjectTime = controlObject.GetType();
            PropertyInfo[] controlObjectProperties = controlObjectTime.GetProperties();
            PropertyInfo timeRangeProperty = controlObjectProperties.ToList().Find(prop => prop.Name == property.Name);
            object timeRange = Activator.CreateInstance(timeRangeProperty.PropertyType);
            PropertyInfo[] timeRangeProperties = timeRange.GetType().GetProperties();
            _startTimeProperty = timeRangeProperties.ToList().Find(p => p.Name == "StartTime");
            object value = GetPropertyValue(GetPropertyValue(controlObject, timeRangeProperty.Name), _startTimeProperty.Name);

            return (DateTime)value;
        }

        private static DateTime GetStopTime(PropertyInfo property, object controlObject)
        {
            Type controlObjectTime = controlObject.GetType();
            PropertyInfo[] controlObjectProperties = controlObjectTime.GetProperties();
            PropertyInfo timeRangeProperty = controlObjectProperties.ToList().Find(prop => prop.Name == property.Name);
            object timeRange = Activator.CreateInstance(timeRangeProperty.PropertyType);
            PropertyInfo[] timeRangeProperties = timeRange.GetType().GetProperties();
            _stopTimeProperty = timeRangeProperties.ToList().Find(p => p.Name == "StopTime");
            object value = GetPropertyValue(GetPropertyValue(controlObject, timeRangeProperty.Name), _stopTimeProperty.Name);

            return (DateTime)value;
        }

        private static object GetPropertyValue(object obj, string propertyName)
        {
            var objType = obj.GetType();
            var prop = objType.GetProperty(propertyName);

            return prop.GetValue(obj, null);
        }
    }
}
