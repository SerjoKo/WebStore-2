using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Inerfaces.TestAPI;
using WebStore.WebApi.lients.Base;

namespace WebStore.WebApi.lients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient Client) : base(Client, "api/[controller]")
        {}

        public void Add(string value)
        {
            var response = Http.PostAsJsonAsync(Address, value).Result;
            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int index)
        {
            var response=Http.DeleteAsync($"{Address}/{index}").Result;
            return response.IsSuccessStatusCode;
        }

        public void Edit(int index, string str)
        {
            var response=Http.PutAsJsonAsync($"{Address}/{index}", str).Result;
            response.EnsureSuccessStatusCode();
        }

        public IEnumerable<string> GetAll()
        {
            var response = Http.GetAsync(Address).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<IEnumerable<string>>().Result;
            return Enumerable.Empty<string>();
        }

        public string GetByIndex(int index)
        {
            var response = Http.GetAsync($"{Address}/{index}").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadFromJsonAsync<string>().Result;
            return "";
        }
    }
}
