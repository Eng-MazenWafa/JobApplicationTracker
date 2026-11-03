namespace JobApplicationTracker.Models
{
    public class Jop : IEntity
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary { get; set; } 
        public string Location { get; set; } = string.Empty; 
        public bool IsOpen { get; set; } = true;
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public int CompanyId { get; set; }
        public int RecruiterId { get; set; }
    }
}