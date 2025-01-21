using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostViewTag
    {
        public List<Post> posts {get;set;}= new List<Post>();
        public List<Tag> tags {get;set;}= new List<Tag>();
    }
}