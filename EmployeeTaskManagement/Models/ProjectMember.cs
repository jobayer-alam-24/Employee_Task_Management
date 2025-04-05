namespace EmployeeTaskManagement.Models
{

    public class ProjectMember
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public int ProjectId { get; set; }

        public ApplicationMember Member { get; set; }

        public Project Project { get; set; }

    }
}
