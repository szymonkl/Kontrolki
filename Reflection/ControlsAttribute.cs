using System;

namespace Reflection
{
    public class ControlsAttribute : Attribute
    {
        public enum ControlTypes
        {
            Label, TextBox, LabelTextbox, DropDownList, LabelDropDownList, Checkbox, LabelCheckBox, ListView
        }

        public ControlTypes ControlType { get; set; }

    }
}
