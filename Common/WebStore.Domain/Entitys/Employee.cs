using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entitys.Base;

namespace WebStore.Domain.Entitys
{
    public class Employee : Entity
    {
        //public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string SurName { get; set; }

        public string MiddleName { get; set; }

        public int Age { get; set; }
    }
}
