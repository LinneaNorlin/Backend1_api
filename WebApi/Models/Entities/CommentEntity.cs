﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Entities
{
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public int IssueId { get; set; }

        [Required]
        public int UserId { get; set; }

        public IssueEntity Issue { get; set; }
        public UserEntity User { get; set; }

    }
}
