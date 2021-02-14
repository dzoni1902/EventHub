using System.ComponentModel.DataAnnotations;

namespace EventHub.Core.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}