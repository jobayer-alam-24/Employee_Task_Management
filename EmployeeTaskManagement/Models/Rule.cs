using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class Rule
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

  
}
