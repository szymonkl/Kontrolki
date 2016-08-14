using System.Windows.Forms;

namespace ControlMaker
{
    public interface IViewUpdater
    {
        void UpdateView(object sourceObject, GroupBox groupBox);
    }
}
