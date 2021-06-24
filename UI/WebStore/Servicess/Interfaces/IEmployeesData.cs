using System.Collections.Generic;
using WebStore.Domain.Entitys;

namespace WebStore.Servicess.Interfaces
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();

        Employee Get(int id);

        int Add(Employee employee);

        void Update(Employee employee);

        bool Delete(int id);
    }
}
