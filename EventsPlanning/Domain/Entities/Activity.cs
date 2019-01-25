using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Activity
    {
        [HiddenInput(DisplayValue = false)]
        public int ActivityId { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, укажите название мероприятия")]
        public string NameEvent { get; set; }
        [Required(ErrorMessage = "Пожалуйста, укажите дату и время проведения")]
        [Display(Name = "Дата проведения")]
        public DateTime DataTime { get; set; }
        [Display(Name = "Количество мест")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение")]
        public int NumberSeats { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [HiddenInput(DisplayValue = false)]
        public List<Subscription> Subscriptions { get; set; }
    }
}