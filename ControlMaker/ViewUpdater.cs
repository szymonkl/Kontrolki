using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ControlMaker
{
    public class ViewUpdater : IViewUpdater
    {
        public List<IControlUpdater> ControlUpdaters;
        public ViewUpdater()
        {
            ControlUpdaters  = new List<IControlUpdater>();
            RegisterUpdaters();
        }
        
        private void RegisterUpdaters()
        {
            foreach (Type controlUpdaterType in Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(IControlUpdater))))
            {
                object controlUpdaer =  Activator.CreateInstance(controlUpdaterType);
                ControlUpdaters.Add(controlUpdaer as IControlUpdater);
            }
        }

        public void UpdateView(object sourceObject, GroupBox groupBox)
        {
            foreach (IControlUpdater updater in ControlUpdaters)
            {
                updater.UpdateControl(sourceObject, groupBox);
            }
        }
    }
}