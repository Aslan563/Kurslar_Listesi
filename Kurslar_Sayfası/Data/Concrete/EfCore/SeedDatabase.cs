using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        //  bu verilerin her zaman ilk olarak ekler 
        public static void verilerigetir(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlobContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "wep programlama", Url = "wep-programlama",Color=TagColor.danger },
                        new Tag { Text = "frontend", Url = "frontend",Color=TagColor.info },
                        new Tag { Text = "backend", Url = "backend",Color=TagColor.primary },
                         new Tag { Text = "fullstack", Url = "fullstack",Color=TagColor.secondary }

                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "Ali",Image="~/img/p1.jpg",Name="Ali",Password="ali123",Email="ali@gmail.com" },
                        new User { UserName = "Kasım",Image="~/img/p2.jpg",Name="Kasım",Password="kasim123",Email="kasim@gmail.com" }
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "Asp",
                            Description = "ASP.NET, Microsoft tarafından geliştirilen ve web tabanlı uygulamalar geliştirmek için kullanılan bir web framework’üdür.",
                            Image = "~/img/asp.jpg",
                            Url = "Asp-kursu",
                            Content = "Asp kursu",
                            Publishedon = DateTime.Now.AddDays(-10),
                            tags = context.Tags.Take(4).ToList(),
                            isactive = true,
                            UserId = 1,
                            comments=new List<Comment>{
                                new Comment{
                                    Text="iyi bir kurs",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=1
                                },
                                new Comment{
                                    Text="kaliteli bir kurs",
                                    Publishedon = DateTime.Now.AddHours(-1),
                                    UserId=2
                                },
                                new Comment{
                                    Text="amazing",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=2
                                },
                                new Comment{
                                    Text="muhteşem",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=1
                                }
                            }

                        },
                         new Post
                         {
                             Title = "php",
                             Image = "~/img/php.jpg",
                             Description="PHP (Hypertext Preprocessor), internet için üretilmiş, sunucu taraflı, çok geniş kullanımlı, genel amaçlı, içerisine HTML gömülebilen betik ve programlama dilidir.",
                             Url = "php-kursu",
                             Content = "php kursu",
                             Publishedon = DateTime.Now.AddDays(-20),
                             tags = context.Tags.Take(2).ToList(),
                             isactive = true,
                             UserId = 1,
                             comments=new List<Comment>{
                                new Comment{
                                    Text="güzel bir kurs",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=1
                                }
                            }


                         },
                        new Post
                        {
                            Title = "React",
                            Description="React, 2011 yılında Facebook tarafından geliştirilen ve Mayıs 2013'te ABD'de düzenlenen JSConf'da açık kaynaklı olarak tanıtılan bir JavaScript kütüphanesidir.",
                            Image = "~/img/react.jpg",
                            Url = "React-kursu",
                            Content = "React kursu",
                            Publishedon = DateTime.Now.AddDays(-60),
                            tags = context.Tags.Take(3).ToList(),
                            isactive = true,
                            UserId = 1,
                            comments=new List<Comment>{
                                new Comment{
                                    Text="günce bir kurs",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=2
                                }
                            }


                        },
                           new Post
                           {
                               Title = "Javascript",
                               Description="JavaScript, web tarayıcılarında çalışan ve dinamik içerikler oluşturmanıza olanak sağlayan bir programlama dilidir.",
                               Image = "~/img/js.jpg",
                               Url = "Javascript-kursu",
                               Content = "Javascript kursu",
                               Publishedon = DateTime.Now.AddDays(-20),
                               tags = context.Tags.Take(1).ToList(),
                               isactive = true,
                               UserId = 1,
                               comments=new List<Comment>{
                                new Comment{
                                    Text="çok iyi bir kurs",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=1
                                }
                            }


                           },
                              new Post
                              {
                                  Title = "Django",
                                  Description="Django, Python Programlama Dili için hazırlanmış ve BSD lisansı ile lisanslanmış yüksek seviyeli bir web çatısıdır.",
                                  Image = "~/img/django.jpg",
                                  Url = "Django-kursu",
                                  Content = "Django kursu",
                                  Publishedon = DateTime.Now.AddDays(-40),
                                  tags = context.Tags.Take(2).ToList(),
                                  isactive = true,
                                  UserId = 1,
                                  comments=new List<Comment>{
                                new Comment{
                                    Text="amazing",
                                    Publishedon = DateTime.Now.AddHours(-10),
                                    UserId=2
                                }
                            }


                              }
                        );




                    context.SaveChanges();
                }
            }
        }
    }
}