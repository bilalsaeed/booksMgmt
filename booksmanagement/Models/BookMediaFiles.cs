using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class BookMediaFiles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileId { get; set; }
        public string Type { get; set; } //P-> Part code, S-> soft copy
        public string ContentType { get; set; }
        public int Size { get; set; }
        public virtual Book Book { get; set; }
        public int BookId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public long CreatedAtTicks { get; set; } = DateTime.UtcNow.Ticks;
    }
}