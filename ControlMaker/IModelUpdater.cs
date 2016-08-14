using System.Windows.Forms;

namespace ControlMaker
{
    public interface IModelUpdater
    {
        void UpdateModel(object targetObject, GroupBox groupBox);
    }
}
