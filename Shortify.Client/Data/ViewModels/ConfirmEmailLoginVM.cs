using Shortify.Client.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shortify.Client.Data.ViewModels
{
    public class ConfirmEmailLoginVM
    {
        [Required(ErrorMessage ="Email address is required")]
        [CustomEmailValidator(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }
    }
}
