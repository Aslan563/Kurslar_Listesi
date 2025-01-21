using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class RegisterView{
        [Required(ErrorMessage ="Username boş bırakılamaz")]
        [Display(Name ="Username")]
        public string? UserName { get; set; }

         [Required(ErrorMessage ="Name boş bırakılamaz")]
        [Display(Name ="Name")]
        public string? Name { get; set; }
       
       

        [Required(ErrorMessage ="Eposta boş bırakılamaz")]
        [Display(Name ="Eposta")]
        [EmailAddress(ErrorMessage ="geçerli Eposta giriniz")]
        
        public string? Email{get;set;}

        [Required(ErrorMessage ="Şifre boş bırakılamaz")]
        [Display(Name ="Şifre")]
        [StringLength(10,ErrorMessage ="{0} alanı   {2} karakterden az olamaz",MinimumLength =6)]    
        [DataType(DataType.Password)]
        public string? Password{get;set;}

          

       
    }
}

