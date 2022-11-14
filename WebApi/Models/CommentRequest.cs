using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CommentRequest
    {
        [Required]
        public string Comment { get; set; }

        [Required]
        public int IssueId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
