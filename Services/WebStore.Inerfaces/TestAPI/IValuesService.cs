using System.Collections.Generic;

namespace WebStore.Inerfaces.TestAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetAll();

        string GetByIndex(int index);

        void Add(string value);

        void Edit(int index, string str);

        bool Delete(int index);
    }
}
