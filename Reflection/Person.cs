using System;
using System.Collections.Generic;

namespace Reflection
{
    public class Person
    {
        [Nazwa(nameof(Dictionary.Imie))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string Imie { get; set; }

        [Nazwa(nameof(Dictionary.Nazwisko))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string Nazwisko { get; set; }

        [Nazwa(nameof(Dictionary.Wiek))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public int Wiek { get; set; }

        [Nazwa(nameof(Dictionary.Panstwo))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string Panstwo { get; set; }

        [Nazwa(nameof(Dictionary.Miasto))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string Miasto { get; set; }

        [Nazwa(nameof(Dictionary.Ulica))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string Ulica { get; set; }

        [Nazwa(nameof(Dictionary.KodPocztowy))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string KodPocztowy { get; set; }

        [Nazwa(nameof(Dictionary.Department))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelDropDownList)]
        public List<string> DepartmentsList { get; set; }

        [Nazwa(nameof(Dictionary.Boss))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelCheckBox)]
        public bool Director { get; set; }

        [Nazwa(nameof(Dictionary.Employees))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.ListView)]
        public List<Employee> Employees { get; set; }


    }
}
