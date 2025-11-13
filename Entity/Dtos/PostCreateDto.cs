using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos
{
    public class PostCreateDto : IDto
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
    }
}
