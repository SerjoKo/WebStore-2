using System.Collections.Generic;

namespace WebStore.Inerfaces.TestAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetAll();

        string GetByIndex(int index);

        string Add(string value);

        void Edit(int index, string str);

        void Delete(int index);
    }
}
