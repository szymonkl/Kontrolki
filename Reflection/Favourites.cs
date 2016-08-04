namespace Reflection
{
    public class Favourites
    {
        [Nazwa(nameof(Dictionary.FavouriteColor))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string FavouriteColor { get; set; }
        [Nazwa(nameof(Dictionary.FavouriteFood))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string FavouriteFood { get; set; }
        [Nazwa(nameof(Dictionary.Hobby))]
        [Controls(ControlType = ControlsAttribute.ControlTypes.LabelTextbox)]
        public string Hobby { get; set; }
    }
}
