using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.ViewModels
{
    public class UpdateViolationViewModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        public string Feedback { get; set; }


    }
}
