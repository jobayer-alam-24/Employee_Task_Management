using EmployeeTaskManagement.Models; using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace EmployeeTaskManagement.Models
{
    public class ApplicationMember  : IdentityUser
    {
        public string Name {  get; set; }

        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }

        public virtual ICollection<ProjectItemMember> ProjectItems { get; set; }

        public ApplicationMember()
        {
            ProjectMembers = new List<ProjectMember>();
            ProjectItems = new List<ProjectItemMember>();
        }


    }
}
