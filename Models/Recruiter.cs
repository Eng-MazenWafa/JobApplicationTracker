namespace JobApplicationTracker.Models
{
    public class Recruiter : IEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int CompanyId { get; set; }

    }
}