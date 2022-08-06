using System;
using System.ComponentModel.DataAnnotations;
using Vapie.WebUI.AppCode.Infrastructure;

namespace Vapie.WebUI.Models.Entities
{
    public class Contact : BaseEntity
    {
        [Required(ErrorMessage = "Comment cannot be empty")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage ="Email is not correctly")]
        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; }
        public int? AnsweredById { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string AnswerMessage { get; set; }
    }
}
