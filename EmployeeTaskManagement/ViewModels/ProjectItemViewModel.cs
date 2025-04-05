using EmployeeTaskManagement.Models;

namespace EmployeeTaskManagement.ViewModels
{
    public class ProjectItemViewModel
    {
        public ProjectItem Item { get; set; } = new ProjectItem();
        public List<MemberViewModel> Members { get; set; } = new List<MemberViewModel>();

    }
}
