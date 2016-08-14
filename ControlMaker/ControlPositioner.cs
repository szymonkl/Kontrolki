using System.Windows.Forms;

namespace Reflection
{
    public class ControlPositioner
    {
        public static int LeftMargin { get; set; } = 10;
        public static int RightMargin { get; set; } = 10;
        public static int TopMargin { get; set; } = 20;
        public static int BottomMargin { get; set; } = 20;

        public static int HorizontalSpace { get; set; } = 10;
        public static int VerticalSpace { get; set; }


        public static void ToLeft(params Control[] controls)
        {

            if (controls.Length > 0)
            {
                int leftPosition = LeftMargin;
                int prevControlSize = controls[0].Size.Width;
                for (int i = 0; i < controls.Length; i++)
                {
                    Control control = controls[i];
                    control.Top = TopMargin;

                    if (i == 0)
                    {
                        control.Left = leftPosition;
                    }
                    else
                    {
                        control.Left = leftPosition + prevControlSize + HorizontalSpace;
                        prevControlSize = control.Size.Width;
                        leftPosition = control.Left;
                    }
                }
            }
        }

        public static void ToRight(params Control[] controls)
        {

            int lastIndex = controls.Length - 1;
            int leftPosition = controls[0].Parent.ClientSize.Width - RightMargin - controls[lastIndex].Size.Width;
            for (int i = lastIndex; i >= 0; i--)
            {
                Control control = controls[i];
                control.Top = TopMargin;
                if (i == lastIndex)
                {
                    control.Left = leftPosition;
                }
                else
                {
                    control.Left = leftPosition - control.Size.Width - HorizontalSpace;
                    leftPosition = control.Left;
                }
            }
        }

        public static void SetControlSize(Control control)
        {
            control.Width = control.Parent.ClientSize.Width - LeftMargin - RightMargin;
            control.Height = control.Parent.ClientSize.Height - TopMargin - BottomMargin;
        }

        private static void ControlsAutosize(bool isAutosized, params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.AutoSize = isAutosized;
            }
        }
    }

}
