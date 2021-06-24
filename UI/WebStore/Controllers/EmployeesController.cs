using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entitys;
using WebStore.Domain.Entitys.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Inerfaces.Services;

namespace WebStore.Controllers
{
    //[Route("Staf")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        //[Route("info-id-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);//_Employees.FirstOrDefault(employe => employe.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        //[Route("all")]
        public IActionResult Index()
        {
            return View(_EmployeesData.GetAll());
        }

        public IActionResult Create()
        {
            return View("Edit", new EmployeeViewModel());
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());
            var employee = _EmployeesData.Get((int)id);

            if (employee is null) return NotFound();

            var view_model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                SurName = employee.SurName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
            };
            return View(view_model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            if (Model.Name == "rty")
                ModelState.AddModelError("Name", "rty - фиговое имя");

            var employee = new Employee
            {
                Id = Model.Id,
                Name = Model.Name,
                SurName = Model.SurName,
                MiddleName = Model.MiddleName,
                Age = Model.Age,
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null)
                return NotFound();


            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                SurName = employee.SurName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
            }
            );
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
