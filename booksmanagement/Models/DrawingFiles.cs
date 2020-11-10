using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class DrawingFiles
    {
        public int Id { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public DrawingOrder DrawingOrder { get; set; }
        public int? DrawingOrderId { get; set; }
        public string Type { get; set; } //P-> Part code, S-> soft copy
        public string SessionId { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
    }
}