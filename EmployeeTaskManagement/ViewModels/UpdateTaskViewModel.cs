using EmployeeTaskManagement.Models;

namespace EmployeeTaskManagement.ViewModels
{
    public class UpdateTaskViewModel
    {
        public int Id { get; set; }

        public string Name {  get; set; }   
        public  string Description { get; set; }
        public ProjectItemAssignStatus Status { get; set; }
        public int ProjectId { get; set; }  
    }
}
