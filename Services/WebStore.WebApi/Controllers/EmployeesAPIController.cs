using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entitys;
using WebStore.Inerfaces;
using WebStore.Inerfaces.Services;

namespace WebStore.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [Route(WebAPIAddress.Employees)]
    [ApiController]
    public class EmployeesAPIController : ControllerBase
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesAPIController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_EmployeesData.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_EmployeesData.Get(id));
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            var id = _EmployeesData.Add(employee);
            return Ok(id);
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            _EmployeesData.Update(employee);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _EmployeesData.Delete(id);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
