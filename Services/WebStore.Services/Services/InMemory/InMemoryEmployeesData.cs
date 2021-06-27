using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Domain.Entitys;
using WebStore.Inerfaces.Services;

namespace WebStore.Services.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {

        private int _MaxId;

        public InMemoryEmployeesData()
        {
            _MaxId = TestData.Employees.Max(i => i.Id);
        }

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Contains(employee)) return employee.Id;

            employee.Id = ++_MaxId;

            TestData.Employees.Add(employee);

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var db_item = Get(id);

            if (db_item is null) return false;

            return TestData.Employees.Remove(db_item);
        }

        public Employee Get(int id) => TestData.Employees.SingleOrDefault(employee => employee.Id == id);

        public IEnumerable<Employee> GetAll() => TestData.Employees;

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Contains(employee)) return;

            var db_item = Get(employee.Id);

            if (db_item is null) return;

            db_item.Name = employee.Name; // и т.д
            db_item.SurName = employee.SurName;
            db_item.MiddleName = employee.MiddleName;
            db_item.Age = employee.Age;
        }
    }
}
