using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class GeneralMediaFile
    {
        public int Id { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string SessionId { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
    }
}