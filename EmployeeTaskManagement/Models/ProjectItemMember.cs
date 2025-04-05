using Microsoft.Identity.Client;

namespace EmployeeTaskManagement.Models
{
    public class ProjectItemMember
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public ApplicationMember Member { get; set; }

        public int ProjectItemId { get; set; }
        public ProjectItem ProjectItem { get; set; }

        public DateTime AssignedDate { get; set; }

        public string Description { get; set; } 

        public ProjectItemAssignStatus Status {  get; set; }

        public ProjectItemMember()
        {
            AssignedDate = DateTime.UtcNow;
        }
    }

    public enum ProjectItemAssignStatus : int
    {
        Assigned = 1,
        Accepted = 2,
        Questionable = 3,
        Done = 4,
        Reviewing = 5,
        Published = 6
    }
}
