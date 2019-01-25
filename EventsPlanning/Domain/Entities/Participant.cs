using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        [Required(ErrorMessage = "Укажите ваше Имя")]
        [Display(Name = "User name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите вашу Фамилию")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Укажите ваш Email")]
        public string Email { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(16, 100, ErrorMessage = "Пожалуйста, введите возрост от 16 до 100")]
        public int Age { get; set; }

        public List<Subscription> Subscriptions { get; set; }
    }
}
