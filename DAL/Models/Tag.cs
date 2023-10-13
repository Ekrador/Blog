using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public List<Post> Posts { get; set; } = new();
        public Tag() 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
