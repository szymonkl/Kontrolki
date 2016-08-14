﻿using System;
using System.Windows.Forms;

namespace Reflection
{
    public class ControlNameBuilder<T>
    {
        public static string BuildName(string propertyName)
        {
            string controlName = propertyName;
            if (typeof(T) == typeof(TextBox))
            {
                controlName = string.Concat("tb", propertyName);
            }
            if (typeof(T) == typeof(Label))
            {
                controlName = string.Concat("lbl", propertyName);
            }
            if (typeof(T) == typeof(ComboBox))
            {
                controlName = string.Concat("cb", propertyName);
            }
            if (typeof(T) == typeof(GroupBox))
            {
                controlName =string.Concat("gb", propertyName);
            }
            if (typeof(T) == typeof(CheckBox))
            {
                controlName = string.Concat("cbx", propertyName);
            }
            if (typeof(T) == typeof(ListView))
            {
                controlName = string.Concat("lv", propertyName);
            }
            if (typeof(T) == typeof(Button))
            {
                controlName = string.Concat("btn", propertyName);
            }
            if (typeof(T) == typeof(DateTimePicker))
            {
                controlName = String.Concat("dtp", propertyName);
            }


            return controlName;
        }

        public static string GetPropertyNameFromControlName(string controlName)
        {
            string propertyName = controlName;
            if (typeof(T) == typeof(TextBox))
            {
                propertyName = propertyName.Replace("tb", string.Empty);
            }
            if (typeof(T) == typeof(Label))
            {
                propertyName = propertyName.Replace("lbl", string.Empty);
            }
            if (typeof(T) == typeof(ComboBox))
            {
                propertyName = propertyName.Replace("cb", string.Empty);
            }
            if (typeof(T) == typeof(GroupBox))
            {
                propertyName = propertyName.Replace("gb", string.Empty);
            }
            if (typeof(T) == typeof(CheckBox))
            {
                propertyName = propertyName.Replace("cbx", string.Empty);
            }
            if (typeof(T) == typeof(ListView))
            {
                propertyName = propertyName.Replace("cbx", string.Empty);
            }
            if (typeof(T) == typeof(Button))
            {
                propertyName = propertyName.Replace("btn", string.Empty);
            }
            if (typeof(T) == typeof(DateTimePicker))
            {
                propertyName = propertyName.Replace("dtp", string.Empty);
            }


            return propertyName;

        }

        public static string BuildFilterName(string propertyName)
        {
            return string.Concat(BuildName(propertyName), "FilterName");
        }
        public static string BuildFilterComparerName(string propertyName)
        {
            return string.Concat(BuildName(propertyName), "FilterComparer");
        }
        public static string BuildFilterValueName(string propertyName)
        {
            return string.Concat(BuildName(propertyName), "FilterValue");
        }

        public static string BuildFilterButtonName(string propertyName)
        {
            return string.Concat(BuildName(propertyName), "FilterButton");
        }

        


    }
}
