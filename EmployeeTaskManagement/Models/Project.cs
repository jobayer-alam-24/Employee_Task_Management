using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Title { get; set; }

        public virtual List<ProjectMember> ProjectMembers { get; set; }

        public virtual List<ProjectItem> Items { get; set; }

        public Project()
        {
            ProjectMembers = new List<ProjectMember>();
        }


    }
}
