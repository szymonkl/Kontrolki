using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Reflection
{
    public class ObjectControlBuilder
    {
        public  int VerticalDistanceBetweenControls
        {
            get { return _verticalDistanceBetweenControls; }
            set { _verticalDistanceBetweenControls = value; }
        }

        private int _verticalDistanceBetweenControls = 50;


        public void BuildControlForObjectInGroupBox<TObject>(TObject controlObject, GroupBox groupBox )
        {
            var t = typeof(TObject);

            groupBox.AutoSize = true;
            groupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            var properties = t.GetProperties();
            int xPosition = 20;
            int yPosition = 20;
            foreach (var property in properties)
            {
                Attribute attribute = Attribute.GetCustomAttribute(property, typeof(DisplayAttribute));
                if (CheckDisplayAttribute(attribute))
                {
                    var point = new Point(xPosition, yPosition);
                    var controlBuilder = new ControlBuilder();
                    var control = controlBuilder.BuildControlForProperty(controlObject, property);
                    groupBox.Controls.Add(control);
                    control.Location = point;
                    yPosition += control.Size.Height;
                }
            }
        }

        private static bool CheckDisplayAttribute(Attribute attribute)
        {
            return attribute == null || ((DisplayAttribute) attribute).IsDisplayed;
        }
    }
}
