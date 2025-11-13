using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
    public class PostLike : IEntity
    {
        public int PostLikeId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.Now;
        public Post? Post { get; set; }
        public User? User { get; set; }
    }
}
