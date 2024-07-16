using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Tenancy.ModelReg
{
    public class RegLessor
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Вы не указали логин!")]
        public string Login { get; set; }//
        [Required(ErrorMessage = "Поставьте пароль")]
        [DataType(DataType.Password)]
        
        public string Password { get; set; }//
        [Required(ErrorMessage ="Введите фамилию!")]
        public string Surname { get; set; }
        [Required(ErrorMessage ="Введите имя!")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Укажите фамилию!")]
        public string SecondName { get; set; }

    }
}