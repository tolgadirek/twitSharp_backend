using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class Post : IEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
    }
}
