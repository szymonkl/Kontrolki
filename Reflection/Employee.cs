namespace Reflection
{
    public class Employee
    {
        [Nazwa(nameof(Dictionary.Imie))]
        public string Name { get; set; }
        [Nazwa(nameof(Dictionary.Nazwisko))]
        public string LastName { get; set; }
        [Nazwa(nameof(Dictionary.Department))]
        public string Job { get; set; }
        [Nazwa(nameof(Dictionary.Miasto))]
        public string City { get; set; }

    }
}
