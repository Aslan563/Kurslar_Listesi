using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BlogApp.Entity;
using BlogApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace BlogApp.Controllers
{
    public class PostController : Controller
    {
        public readonly BlobContext _context;
        public PostController(BlobContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? tagurl)
        {
            var post = _context.Posts.Where(p => p.isactive).AsQueryable();
            if (!string.IsNullOrEmpty(tagurl))
            {
                post = post.Where(p => p.tags.Any(t => t.Url == tagurl));
            }
            var PostTagLists = new PostViewTag
            {
                posts = await post
            .Include(p => p.tags)
            .ToListAsync()
            };
            return View(PostTagLists);
        }
         public async Task<IActionResult> Course()
        {
            var post = _context.Posts.AsQueryable();
            
            var PostTagLists = new PostViewTag
            {
                posts = await post
            .Include(p => p.tags)
            .ToListAsync()
            };
            return View(PostTagLists);
        }
        public async Task<IActionResult> Details(string? url)
        {
            if (url == null)
            {
                return NotFound();
            }
            var model = await _context.Posts
            .Include(p => p.tags)
            .Include(p=>p.user)
            .Include(c => c.comments)
            .ThenInclude(u => u.user)
            .FirstOrDefaultAsync(p => p.Url == url);

            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> AddComment(string? text, int postid, string? url)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var image = User.FindFirstValue(ClaimTypes.UserData);
            if (!@User.Identity!.IsAuthenticated)
            {
                return Json(new { success = false, message = "404 found" });
            }

            if (url == null)
            {
                return Json(new { success = false, message = "url boş" });
            }


            var models = new Comment
            {
                Text = text,
                PostId = postid,
                Publishedon = DateTime.Now,
                UserId = int.Parse(userid ?? "")
            };


            _context.Comments.Add(models);
            await _context.SaveChangesAsync();


            return Json(new
            {
                username,
                text,
                publishedon = models.Publishedon,
                image1 = Url.Content(image),

            });
        }

        [Authorize]
        public IActionResult Create()
        {

            return View();


        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateView model,IFormFile postimagefile)
        {
               var allowexcepsion = new[] { ".jpg", ".png", ".jpeg" };
            var randomfilename = "";
            var path = "";
            var excension = "";

            if (postimagefile != null)
            {
                excension = Path.GetExtension(postimagefile.FileName).ToLower();  // Dosya uzantısını almak için .jpg gibi
                randomfilename = $"{Guid.NewGuid()}{excension}";

               
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", randomfilename);

               
                if (!allowexcepsion.Contains(excension))
                {
                    ModelState.AddModelError("", "Geçersiz dosya formatı. Lütfen bir resim yükleyin.");
                }
                else
                {
                  
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await postimagefile.CopyToAsync(stream);
                    }
                }
            }
            else
            {
               
                ModelState.AddModelError("", "Lütfen bir resim yükleyin.");
              
            }

            if (ModelState.IsValid)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _context.Posts.AddAsync(new Post
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    Image = "~/img/" + randomfilename,
                    Publishedon = DateTime.Now,
                    UserId = int.Parse(userid ?? ""),
                    isactive = false
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Post");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var model = await _context.Posts.ToListAsync();
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(role))
            {
                model = await _context.Posts.Include(p => p.user).Where(p => p.UserId == userid).ToListAsync();
            }




            return View(model);


        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Posts.Include(p=>p.tags).FirstOrDefaultAsync(p => p.PostId == id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.tags= await _context.Tags.ToListAsync();
            return View(new PostCreateView
            {
                PostId = model.PostId,
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                Url = model.Url,
                isactive = model.isactive,
                tags=model.tags
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, PostCreateView model,int[] tagIds)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var post = await _context.Posts.Include(p=>p.tags).FirstOrDefaultAsync(p=>p.PostId==model.PostId);

                if (post == null)
                {
                    return NotFound();
                }


                post.Title = model.Title;
                post.Description = model.Description;
                post.Content = model.Content;
                post.Url = model.Url;
                post.isactive = model.isactive;
                post.tags=_context.Tags.Where(t=>tagIds.Contains(t.TagId)).ToList();
                
                _context.Posts.Update(post);
                
                await _context.SaveChangesAsync();

                return RedirectToAction("List");
            }
               ViewBag.tags= await _context.Tags.ToListAsync();
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(new PostCreateView
            {
                PostId = model.PostId,
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                Url = model.Url,
                isactive = model.isactive
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id, PostCreateView model)
        {
            if (id == null)
            {
                return NotFound();
            }
            if(id!=model.PostId){
                return NotFound();
            }
            var removemodel=await _context.Posts.FindAsync(id);
            if(removemodel!=null){
                    _context.Posts.Remove(removemodel );
                    await _context.SaveChangesAsync();
                    return RedirectToAction("List");
            }
         
            return View(model);
            
        }

    }
}