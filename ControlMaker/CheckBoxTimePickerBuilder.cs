using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Reflection;

namespace ControlMaker
{
    public class CheckBoxTimePickerBuilder
    {
        public static int CheckBoxWidth { get; set; } = 25;
        public static int CheckBoxHeight { get; set; } = 25;
        public static int PickerWidth { get; set; } = 80;
        public static int PickerHeight { get; set; } = 30;

        private static PropertyInfo _boolProperty;
        private static PropertyInfo _timeProperty;

        public static GroupBox CreateTimeRangeControl(object controlObject, PropertyInfo property)
        {
            GroupBox groupBox = new GroupBox();
            if (IsValid(property))
            {
                //Getting values
                bool isChecked = GetBoolValue(property, controlObject);
                DateTime time = GetStartTime(property, controlObject);


                CreateSimpleGroupBox(property, groupBox);

                var checkBox = CreateCheckBox(isChecked);

                var timePicker = CreateTimePicker(time);

                groupBox.Controls.Add(checkBox);
                groupBox.Controls.Add(timePicker);


                //Setting control position

                Control[] controls = new Control[2];
                controls[0] = checkBox;
                controls[1] = timePicker;


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
            groupBox.Width = ControlPositioner.LeftMargin + CheckBoxWidth + ControlPositioner.HorizontalSpace +
                             PickerWidth + ControlPositioner.RightMargin;
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

        private static DateTimePicker CreateTimePicker(DateTime startTime)
        {
            DateTimePicker startTimePicker = new DateTimePicker();
            startTimePicker.Name = ControlNameBuilder<DateTimePicker>.BuildName(_timeProperty.Name);
            startTimePicker.Value = startTime;
            startTimePicker.Format = DateTimePickerFormat.Time;
            startTimePicker.ShowUpDown = true;
            startTimePicker.Height = PickerHeight;
            startTimePicker.Width = PickerWidth;
            return startTimePicker;
        }

        private static bool IsValid(PropertyInfo property)
        {
            return property.PropertyType == typeof(BoolTime);
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
            _timeProperty = timeRangeProperties.ToList().Find(p => p.Name == "Time");
            object value = GetPropertyValue(GetPropertyValue(controlObject, timeRangeProperty.Name),
                _timeProperty.Name);

            return (DateTime) value;
        }

        private static object GetPropertyValue(object obj, string propertyName)
        {
            var objType = obj.GetType();
            var prop = objType.GetProperty(propertyName);

            return prop.GetValue(obj, null);
        }
    }



}
