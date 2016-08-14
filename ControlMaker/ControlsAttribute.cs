using System;

namespace Reflection
{
    public class ControlsAttribute : Attribute
    {
        public enum ControlTypes
        {
            Label, TextBox, LabelTextbox, DropDownList, LabelDropDownList, Checkbox, LabelCheckBox,
            ListView, FilterableListView, DateTimePicker, LabelDateTimePicker, TimeRangePicker
        }

        public ControlTypes ControlType { get; set; }

    }
}
