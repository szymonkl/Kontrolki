namespace Reflection
{
    public class FinancialData
    {

        [Nazwa(nameof(Dictionary.Salary))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public int Salary { get; set; }
        [Nazwa(nameof(Dictionary.AccountNumber))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string AccoountNumber { get; set; }
        [Display(true)]
        [Nazwa(nameof(Dictionary.BonusPercentage))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string BonusPercentage { get; set; }
        [Nazwa(nameof(Dictionary.Bonus))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public int Bonus { get; set; }

    }
}
