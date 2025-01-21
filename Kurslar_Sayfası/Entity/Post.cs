using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entity
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }
        public String? Content { get; set; }
        public DateTime Publishedon { get; set; }
        public bool isactive { get; set; }
        public int UserId { get; set; }
        public User user { get; set; } = null!;
        public List<Comment> comments { get; set; } = new List<Comment>();  
        public List<Tag> tags { get; set; } = new List<Tag>();

    }
}