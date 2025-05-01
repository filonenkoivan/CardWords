using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public string? Name { get; set; }

        public int Id { get; set; }

        public List<CardCollection>? Collections { get; set; }

        public string? Password { get; set; }
        public UserStats? Stats { get; set; }
    }
}
