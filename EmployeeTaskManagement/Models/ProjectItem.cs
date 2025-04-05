using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class ProjectItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 

        public DateTime AssignDate { get; set; }
        public int WorkingHours {  get; set; }
        [Display(Name = "Select project")]
        public int ProjectId { get; set; }

        [Display(Name ="Select shift..")]
        public int ShiftId { get; set; }

        public virtual Project Project { get; set; }

        public virtual Shift Shift { get; set; }

        public virtual ICollection<ProjectItemMember> Members { get; set; }

        public ProjectItem()
        {
            Members = new List<ProjectItemMember>();
        }
    }
}
