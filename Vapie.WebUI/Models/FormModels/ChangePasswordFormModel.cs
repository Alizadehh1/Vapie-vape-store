using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vapie.WebUI.Models.FormModels
{
    public class ChangePasswordFormModel
    {
        [Required,DataType(DataType.Password),Display(Name ="Current Password")]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirm new Password")]
        [Compare("NewPassword",ErrorMessage ="Confirm new password does not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
