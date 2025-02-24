using System.ComponentModel.DataAnnotations;

namespace InvlogicServer.Models
{
    public class Todo
    {
        [Key]
        public int TodoId { get; set; }
        [Required]
        public string TodoName { get; set; }
        public string TodoSummary { get; set; }
        [Required]
        public bool TodoStatus { get; set; } = false;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CompletedAt { get; set; }
        // foreign key
        public int ProjectId { get; set; }
        // navigation property
        public Project Project { get; set; }
    }
}