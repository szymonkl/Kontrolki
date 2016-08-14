using System.Windows.Forms;

namespace ControlMaker
{
    public interface IModelControlUpdater
    {
        void UpdateObjectFromControl(object targetObject, GroupBox groupBox);
    }
}
