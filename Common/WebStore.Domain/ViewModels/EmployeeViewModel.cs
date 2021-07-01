using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"[А-ЯЁ][а-яё]+")] // @"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage
        public string SurName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Возраст")]
        [Range(18, 80)]
        public int Age { get; set; }
    }
}
