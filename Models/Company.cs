namespace JobApplicationTracker.Models
{
    public enum ComppanyStatus
    {
        Pending, 
        Approved, 
        Rejected
    }
    public class  Company:IEntity
    {
        public int Id { get; set; }

        public  required string Name { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public ComppanyStatus Status { get; set; } = ComppanyStatus.Pending;
    }
}