using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context.WebStore.DAL.Context;
using WebStore.Domain.Entitys;
using WebStore.Inerfaces.Services;

namespace WebStore.Servicess.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _db;

        public SqlEmployeesData(WebStoreDB db) => _db = db;
        
        public int Add(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentException(nameof(employee));
            }

            //_db.Employees.Add(employee);
            //_db.Entry(employee).State = EntityState.Added;
            _db.Add(employee);

            _db.SaveChanges();

            return employee.Id;
        }

        public bool Delete(int id)
        {
            var employee = _db.Employees.Select
                (e => new Employee { Id = e.Id }).FirstOrDefault(e => e.Id == id);
            if (employee is null)
            {
                return false;
            }
            //_db.Database.ExecuteSqlRaw();
            //var employee = Get(id);
            //if (Get(id) is not { } employee)
            //{
            //    return false;
            //}
            //_db.Employees.Remove(employee);
            //_db.Entry(employee).State = EntityState.Deleted;
            _db.Remove(employee);

            _db.SaveChanges();

            return true;
        }

        public Employee Get(int id)
        {
            return _db.Employees.SingleOrDefault(employee => employee.Id == 0);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees.ToArray();
        }

        public void Update(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentException(nameof(employee));
            }

            //_db.Employees.Update(employee);
            //_db.Entry(employee).State = EntityState.Modified;
            _db.Update(employee);
            _db.SaveChanges();
        }
    }
}
