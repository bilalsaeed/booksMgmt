using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class DrawingFiles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileId { get; set; }
        public string Type { get; set; } //P-> Photos, V-> Videos, A -> Audios
        public string ContentType { get; set; }
        public int Size { get; set; }
        public virtual DrawingOrder DrawingOrder { get; set; }
        public int DrawingOrderId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public long CreatedAtTicks { get; set; } = DateTime.UtcNow.Ticks;
    }
}