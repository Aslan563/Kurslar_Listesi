using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;
namespace BlogApp.Models
{
    public class PostCreateView
    {

        [Required(ErrorMessage = "PostId boş bırakılamaz")]
        [Display(Name = "PostId")]
        public int PostId { get; set; }


        [Required(ErrorMessage = "Başlık boş bırakılamaz")]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Açıklama boş bırakılamaz")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "İçerik boş bırakılamaz")]
        [Display(Name = "İçerik")]
        public string? Content { get; set; }



        [Required(ErrorMessage = "Url boş bırakılamaz")]
        [Display(Name = "Url")]
        public string? Url { get; set; }

        [Required(ErrorMessage ="isactive boş bırakılamaz")]
        [Display(Name ="isactive")]
        public bool isactive { get; set; }

        [Display(Name ="Tags")]
        public List<Tag> tags { get; set; } = new List<Tag>();



    }
}

