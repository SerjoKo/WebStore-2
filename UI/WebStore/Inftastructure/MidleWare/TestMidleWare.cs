using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebStore.Inftastructure.MidleWare
{
    public class TestMidleWare
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<TestMidleWare> _Logger;

        public TestMidleWare(RequestDelegate next, ILogger<TestMidleWare> logger)
        {
            _Next = next;
            _Logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var processing = _Next(context);
            await processing;

        }
    }
}
