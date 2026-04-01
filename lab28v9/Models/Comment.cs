using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab28v9.Models;
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; } = "";
        public string Text { get; set; } = "";
    }