using System.Windows.Forms;

namespace Reflection
{
    public class ControlPositioner
    {
        public static int LeftMargin { get; set; } = 10;
        public static int RightMargin { get; set; } = 10;
        public static int TopMargin { get; set; } = 20;
        public int BottomMargin { get; set; } = 20;

        public static int HorizontalSpace { get; set; } = 10;
        public static int VerticalSpace { get; set; }


        public static void ToLeft(params Control[] controls)
        {

            if (controls.Length>1)
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

        //public static void ToLeft(Control control1, Control control2, Control control3) 
        //{
        //    ControlsAutosize(control1, control2, control3, true);

        //    control1.Left = LeftMargin;
        //    control1.Top = TopMargin;
        //    control2.Left = LeftMargin + control1.Size.Width + HorizontalSpace;
        //    control2.Top = TopMargin;
        //    control3.Left = control2.Left + control2.Size.Width + HorizontalSpace;
        //    control3.Top = TopMargin;

            
        //}

        //public static void ToRight(Control control1, Control control2, Control control3)
        //{
        //    control3.Left = control3.Parent.Right - RightMargin - control3.Size.Width;
        //    control3.Top = TopMargin;

        //    control2.Left = control3.Left - HorizontalSpace - control2.Width;
        //    control2.Top = TopMargin;

        //    control1.Left = control2.Left - HorizontalSpace - control1.Width;
        //    control1.Top = TopMargin;
        //}

        private static void ControlsAutosize(Control control1, Control control2,
            Control control3, bool isAutosized)
        {
            control1.AutoSize = isAutosized;
            control2.AutoSize = isAutosized;
            control3.AutoSize = isAutosized;
        }
    }

}
