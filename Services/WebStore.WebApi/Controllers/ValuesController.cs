using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static List<string> _Values = Enumerable
            .Range(1, 10)
            .Select(i => $"Value{i}")
            .ToList();

        //public IEnumerable<string> Get() => _Values;
        [HttpGet]
        public IActionResult Get() => Ok(_Values);
        //public ActionResult<List<string>> Get() => _Values;

        [HttpGet("count")]
        public IActionResult GetCount() => Ok(_Values.Count);

        [HttpGet("{index}")]
        [HttpGet("index[[{index}]]")]
        public ActionResult<string> GetByIndex(int index)
        {
            if (index < 0)
                return BadRequest();
            if (index >= _Values.Count)
                return NotFound();

            return _Values[index];
        }

        [HttpPost]
        [HttpPost("add")]
        public ActionResult Add(string str)
        {
            _Values.Add(str);
            return Ok();
            //return CreatedAtAction(nameof(GetByIndex), new { index = __Values.Count - 1});
        }

        [HttpPut("{index}")]
        [HttpPut("edit/{index}")]
        public ActionResult Replace(int index, string str)
        {
            if (index < 0)
                return BadRequest();
            if (index >= _Values.Count)
                return NotFound();

            _Values[index] = str;
            return Ok();
        }

        [HttpDelete("{index}")]
        public ActionResult Delete(int index)
        {
            if (index < 0)
                return BadRequest();
            if (index >= _Values.Count)
                return NotFound();

            _Values.RemoveAt(index);

            return Ok();
        }
    }
}
