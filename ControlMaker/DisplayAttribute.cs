using System;

namespace Reflection
{
    public class DisplayAttribute : Attribute
    {
        private bool _isDisplayed;

        public DisplayAttribute(bool isDisplayed)
        {
            _isDisplayed = isDisplayed;
        }

        public bool IsDisplayed
        {
            get { return _isDisplayed; }
            set { _isDisplayed = value; }
        }
    }
}
