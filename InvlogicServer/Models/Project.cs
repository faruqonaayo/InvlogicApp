using System.ComponentModel.DataAnnotations;

namespace InvlogicServer.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDescription { get; set; }
        [Required]
        public bool ProjectStatus { get; set; } = false;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now; 
        public DateTime ClosedAt { get; set; }
    }
}