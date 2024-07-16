using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tenancy.ModelReg
{
    public class RegLessee
    {

        public int ID { get; set; }
        [Required(ErrorMessage ="Вы не указали логин!")]
        public string Login { get; set; }//
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Поставьте пароль")]
        public string Password { get; set; }//
        [Required(ErrorMessage ="Имя организации не указано!")]
        public string Organization { get; set; }//
        [Required(ErrorMessage ="Вы не указали тип бренда(собств/франшиза)!")]
        public string BrandType { get; set; }
        [Required(ErrorMessage ="Опишите ваш торговый профиль!")]
        public string TradingProfile { get; set; }
        // public string ProductProfile { get; set; }
        [Required(ErrorMessage ="Заполните поле Фамилия!")]
        public string Surname { get; set; }
        [Required(ErrorMessage ="Заполните поле Имя!")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Укажите почту!")]
        [RegularExpression(@"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+", ErrorMessage ="Некорректный ввод данных!")]
        public string Email { get; set; }
    }
}