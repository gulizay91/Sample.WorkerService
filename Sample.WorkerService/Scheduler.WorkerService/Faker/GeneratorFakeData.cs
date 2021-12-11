using Bogus;

namespace Scheduler.WorkerService.Faker
{
    public static class GeneratorFakeData
    {

        public static List<Employee> Employees { 
            get { 
                if (_employees == null) return SimpleEmployeeList(100);
                return _employees;
            }
            set { _employees = value; }
        }

        private static List<Employee> _employees;

        #region Properties

        /// <summary>
        /// Gets the EmployeeFaker.
        /// </summary>
        private static Faker<Employee> EmployeeFaker => GenerateFaker();

        #endregion

        #region Methods

        /// <summary>
        /// The GenerateFaker.
        /// </summary>
        /// <returns>The <see cref="Faker{Employee}"/>.</returns>
        private static Faker<Employee> GenerateFaker()
        {
            List<DateTime> dates = new();
            DateTime date;
            for (int i = 1; i <= 10; i++)
            {
                date = i % 10 == 0 ? DateTime.Now.AddYears(-i) : DateTime.Now.AddDays(-i * 10);
                dates.Add(date);
                //Console.WriteLine(date.ToString("s"));
            }
            Faker<Employee> _EmployeeFaker = new Faker<Employee>("tr")
              .RuleFor(r => r.Id, Guid.NewGuid())
              .RuleFor(r => r.Name, r => r.Person.FirstName)
              .RuleFor(r => r.Surname, r => r.Person.LastName)
              .RuleFor(r => r.JoinedDate, r => r.Random.ListItem<DateTime>(dates));
            return _EmployeeFaker;
        }

        /// <summary>
        /// The SimpleEmployee.
        /// </summary>
        /// <returns>The <see cref="Employee"/>.</returns>
        internal static Employee SimpleEmployee()
        {
            return EmployeeFaker.Generate();
        }

        /// <summary>
        /// The SimpleEmployeeList.
        /// </summary>
        /// <param name="count">The count<see cref="int"/>.</param>
        /// <returns>The <see cref="List{Employee}"/>.</returns>
        internal static List<Employee> SimpleEmployeeList(int count)
        {
            return EmployeeFaker.Generate(count);
        }

        /// <summary>
        /// The GenerateSimpleEmployeeList.
        /// </summary>
        /// <param name="count"></param>
        internal static void GenerateSimpleEmployeeList(int count)
        {
            _employees = SimpleEmployeeList(count);
        }

        #endregion
    }
}
