using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entity
{
    public class Comment
    {
        [Key]
        public int Commentid { get; set; }
        public String? Text { get; set; }
        public DateTime Publishedon { get; set; }
         public int UserId { get; set; }
         public User user { get; set; }=null!;
         public int PostId { get; set; }
         public Post post { get; set; }=null!;
    }
}