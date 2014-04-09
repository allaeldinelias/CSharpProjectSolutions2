using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcBlog.Models
{
    [Table ("Posts")]
    public class Post
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}