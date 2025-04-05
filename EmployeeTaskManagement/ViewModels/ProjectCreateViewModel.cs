using EmployeeTaskManagement.Models;

namespace EmployeeTaskManagement.ViewModels
{
    public class ProjectCreateViewModel
    {
        public Project Project { get; set; }

        public List<MemberViewModel> Members { get; set; }
    }
}
