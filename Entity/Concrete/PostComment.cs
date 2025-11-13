using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class PostComment : IEntity
    {
        public int PostCommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Post? Post { get; set; }
        public User? User { get; set; }
    }
}
