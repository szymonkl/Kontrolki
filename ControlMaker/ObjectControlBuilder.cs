using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ControlMaker;

namespace Reflection
{
    public class ObjectControlBuilder
    {
        public int VerticalDistance { get; set; } = 10;
        public int HorizontalDistance { get; set; } = 40;
        public int LeftMargin { get; set; } = 10;
        public int RightMargin { get; set; } = 20;
        public int TopMargin { get; set; } = 20;
        public int BottomMargin { get; set; } = 20;

        public void BuildControlForObjectInGroupBox<TObject>(TObject controlObject, GroupBox groupBox )
        {
            var t = typeof(TObject);

            var properties = t.GetProperties();
            int xPosition = LeftMargin;
            int yPosition = TopMargin;
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
                    yPosition += control.ClientSize.Height + VerticalDistance;
                    if (CheckYPosition(yPosition, control.ClientSize.Height, groupBox))
                    {
                        yPosition = TopMargin;
                        xPosition = LeftMargin + control.ClientSize.Width + HorizontalDistance;
                    }

                }
            }
        }

        private static bool CheckYPosition(int yPosition, int controlHeight, GroupBox groupBox)
        {
            return yPosition + controlHeight >= groupBox.Height;
        }

        

        private static bool CheckDisplayAttribute(Attribute attribute)
        {
            return attribute == null || ((DisplayAttribute) attribute).IsDisplayed;
        }
    }
}
