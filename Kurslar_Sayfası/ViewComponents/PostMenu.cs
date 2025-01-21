using System.Reflection.Metadata;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents{
    public class PostMenu:ViewComponent{
         BlobContext _context;
         public PostMenu(BlobContext context)
         {
            _context=context;
         }

         public IViewComponentResult Invoke(){
            return View(_context.Posts.OrderByDescending(p=>p.Publishedon).Take(5).ToList());
         }
    }
}