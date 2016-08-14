using System.Windows.Forms;

namespace ControlMaker
{
    public interface IControlUpdater
    {
        void UpdateControl(object sourceObject, GroupBox groupBox);
    }
}
