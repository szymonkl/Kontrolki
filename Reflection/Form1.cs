using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reflection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        private Person _person = new Person();

        private void btnPobierz_Click(object sender, EventArgs e)
        {
            List<Employee> employeesList = new List<Employee>()
            {

                new Employee() {Name = "Adam", LastName = "Kliś", City = "Warszawa", Job = "IT"},
                new Employee() {Name = "Joe", LastName = "Jones", City = "London", Job = "Payroll"},
                new Employee() {Name = "Mark", LastName = "Johnson", City = "Los Angeles", Job = "HR"},
                new Employee() {Name = "Jane", LastName = "Miles", City = "Chicago", Job = "IT"},
                new Employee() {Name = "Lisa", LastName = "Mona", City = "Tokyo", Job = "HR"}
            };
            Person p1 = new Person
            {
                Imie = "Szymon",
                Miasto = "Kraków",
                Nazwisko = "Kliś",
                Panstwo = "Polska",
                Wiek = 24,
                Ulica = "Mysłowicka",
                KodPocztowy = "43-100",
                Employees = employeesList,
                DepartmentsList = new List<string> { "HR", "IT", "Helpdesk", "Płace" },
                Director = true
                

            };

            FinancialData financialData = new FinancialData
            {
                AccoountNumber = "2131-1234-1234",
                Bonus = 250,
                BonusPercentage = "15%",
                Salary = 2200
            };
            Favourites favourites = new Favourites
            {
                FavouriteColor = "Pomarańczowy",
                FavouriteFood = "Pizza",
                Hobby = "Muzyka"
            };
            
           
            var objectControlBuilder = new ObjectControlBuilder();
            objectControlBuilder.BuildControlForObjectInGroupBox(p1, gbTextBoxes);
            objectControlBuilder.BuildControlForObjectInGroupBox(financialData, gbFinanse);
            objectControlBuilder.BuildControlForObjectInGroupBox(favourites, gbUlubione);
            var controlBuilder = new ControlBuilder();
            controlBuilder.CreateListViewInGroupBox(p1, gbPracownicy);
           
            _person = p1;

        }

        private void button1_Click(object sender, EventArgs e)
        {

           
            _person.Imie = "Adam1";
            _person.Nazwisko = "Kowalski";
            var controlBuilder = new ControlBuilder();
            controlBuilder.UpdateTextBoxes(_person, gbTextBoxes);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ControlBuilder controlBuilder = new ControlBuilder();
            controlBuilder.UpdateObjectFromTextBoxes(_person, gbTextBoxes);
        }
    }

    
}
