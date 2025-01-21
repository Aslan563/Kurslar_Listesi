using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.ViewComponents{
    public class TagsMenu:ViewComponent{
        BlobContext _context;
        public TagsMenu(BlobContext context)
        {
            _context=context;
        }

        public IViewComponentResult Invoke(){
            return View(_context.Tags.ToList());
        }
    }
}