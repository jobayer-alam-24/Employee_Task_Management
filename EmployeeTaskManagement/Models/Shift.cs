using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class Shift
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProjectItem> ProjectItems { get; set; }
    }
}
