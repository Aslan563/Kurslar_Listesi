using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entity
{     public enum TagColor{
     primary,danger,success,warning,secondary,info
}
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string? Text { get; set; }
        public TagColor? Color { get; set; }
        public string? Url { get; set; }
      
        public List<Post> posts { get; set; }=new List<Post>();

    }
}