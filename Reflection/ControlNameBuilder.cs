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
            return propertyName;


        }

    }
}
