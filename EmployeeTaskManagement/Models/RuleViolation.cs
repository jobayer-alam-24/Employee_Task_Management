namespace EmployeeTaskManagement.Models
{
    public class RuleViolation
    {
        public int Id { get; set; }
        public string MemberId {  get; set; }
        public DateTime IncidentDate{ get; set; }
        public int RuleId { get;set; }
        public int Point { get; set;}
        public string Note {  get; set; }   
        public string MemberNote { get; set; }
        public RuleViolationReplyStatus ReplyStatus { get; set; }
        public RuleViolationStatus Status { get; set; }
        public ApplicationMember Member { get;set;}
        public Rule Rule { get; set; }


    }

    public enum RuleViolationStatus : int
    {
        Default = 0,
        Good = 1,
        Bad = 2,
    }

    public enum RuleViolationReplyStatus : int
    {
        NoReply= 1,
        Replied=2

    }
}
