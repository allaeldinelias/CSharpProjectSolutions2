using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcBlog.Models
{
    public class BlogContext: DbContext
    {
        public DbSet<Post> posts { get; set; }

        public BlogContext()
            :base("BlogContext")
        {
        }
    }
}