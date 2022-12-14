using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public ICollection<IssueEntity> Issues { get; set; }
        public ICollection<CommentEntity> Comments { get; set; }

    }
}
