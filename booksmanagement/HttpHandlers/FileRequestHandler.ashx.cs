using booksmanagement.Models;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace booksmanagement.HttpHandlers
{
    /// <summary>
    /// Summary description for FileRequestHandler
    /// </summary>
    public class FileRequestHandler : IHttpHandler
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        string width = string.Empty;
        string height = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString.Count > 0)
            {

                string size = context.Request.QueryString["size"];

                if (!string.IsNullOrEmpty(size))
                {
                    try
                    {
                        width = size.Split('x')[0];
                        height = size.Split('x')[1];
                    }
                    catch
                    {
                        width = string.Empty;
                        height = string.Empty;
                    }
                }
                else
                {
                    width = "40";
                    height = "50";
                }

                var storedImage = new byte[0];

                if (!string.IsNullOrEmpty(context.Request.QueryString["Type"].ToString()))
                {
                    if (context.Request.QueryString["Type"].ToString() == "UploadBookPartCode")
                    {
                        int bookId = int.Parse(context.Request.QueryString["BookId"].ToString());
                        string sessionId = context.Request.QueryString["SessionId"];
                        HttpPostedFile file = context.Request.Files[0];
                        BinaryReader b = new BinaryReader(file.InputStream);
                        byte[] binData = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));

                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;

                        var oldFiles = db.BookMediaFiles.Where(m => m.BookId == bookId && m.Type == "P").ToList();
                        db.BookMediaFiles.RemoveRange(oldFiles);

                        BookMediaFiles bookFile = new BookMediaFiles();
                        bookFile.File = binData;
                        bookFile.FileName = fileName;
                        bookFile.FileType = fileExtension;
                        bookFile.BookId = bookId;
                        bookFile.Type = "P";
                        bookFile.FileSize = file.ContentLength;
                        bookFile.SessionId = sessionId;
                        db.BookMediaFiles.Add(bookFile);

                        var book = db.Books.Where(bk => bk.Id == bookId).FirstOrDefault();
                        if(book != null)
                        {
                            book.PartCodeAvailable = true;
                        }

                        db.SaveChanges();

                        string json = "{\"success\":\"true\"}";
                        context.Response.Clear();
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.Write(json);
                        context.Response.End();
                    }
                    if (context.Request.QueryString["Type"].ToString() == "UploadBookSoftCopy")
                    {
                        int bookId = int.Parse(context.Request.QueryString["BookId"].ToString());
                        string sessionId = context.Request.QueryString["SessionId"];
                        HttpPostedFile file = context.Request.Files[0];
                        BinaryReader b = new BinaryReader(file.InputStream);
                        byte[] binData = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));

                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;

                        var oldFiles = db.BookMediaFiles.Where(m => m.BookId == bookId && m.Type == "S").ToList();
                        db.BookMediaFiles.RemoveRange(oldFiles);

                        BookMediaFiles bookFile = new BookMediaFiles();
                        bookFile.File = binData;
                        bookFile.FileName = fileName;
                        bookFile.FileType = fileExtension;
                        bookFile.BookId = bookId;
                        bookFile.Type = "S";
                        bookFile.FileSize = file.ContentLength;
                        bookFile.SessionId = sessionId;
                        db.BookMediaFiles.Add(bookFile);


                        var book = db.Books.Where(bk => bk.Id == bookId).FirstOrDefault();
                        if (book != null)
                        {
                            book.SoftCopyAvailable = true;
                        }

                        db.SaveChanges();

                        string json = "{\"success\":\"true\"}";
                        context.Response.Clear();
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.Write(json);
                        context.Response.End();

                    }
                    if (context.Request.QueryString["Type"].ToString() == "GetBookPartCode")
                    {
                        int bookId = int.Parse(context.Request.QueryString["BookId"].ToString());
                        int softBookId = int.Parse(context.Request.QueryString["SoftBookId"].ToString());
                        var fileObj = db.BookMediaFiles.Where(b => b.BookId == bookId && b.Type == "P").FirstOrDefault();
                        if (fileObj == null)
                        {
                            fileObj = db.BookMediaFiles.Where(b => b.BookId == softBookId && b.Type == "P").FirstOrDefault();
                        }

                        if (fileObj == null)
                        {

                            string json = "{\"success\":\"false\"}";
                            context.Response.Clear();
                            context.Response.ContentType = "application/json; charset=utf-8";
                            context.Response.Write(json);
                            context.Response.End();
                        }
                        else
                        {
                            HttpResponse resp = context.Response;
                            resp.ClearHeaders();
                            resp.ClearContent();
                            resp.AddHeader("Content-Disposition", "attachment; filename=" + fileObj.FileName);
                            resp.AddHeader("Content-Length", fileObj.FileSize.ToString());
                            resp.ContentType = fileObj.FileType;
                            resp.OutputStream.Write(fileObj.File, 0, fileObj.File.Length);
                        }
                    }
                    if (context.Request.QueryString["Type"].ToString() == "GetBookSoftCopy")
                    {
                        int bookId = int.Parse(context.Request.QueryString["BookId"].ToString());
                        var fileObj = db.BookMediaFiles.Where(b => b.BookId == bookId && b.Type == "S").FirstOrDefault();

                        if (fileObj == null)
                        {

                            string json = "{\"success\":\"false\"}";
                            context.Response.Clear();
                            context.Response.ContentType = "application/json; charset=utf-8";
                            context.Response.Write(json);
                            context.Response.End();
                        }
                        else
                        {
                            HttpResponse resp = context.Response;
                            resp.ClearHeaders();
                            resp.ClearContent();
                            //resp.AddHeader("Content-Disposition", "filename=" + fileObj.FileName);
                            resp.AddHeader("Content-Type", fileObj.FileType);
                            resp.AddHeader("Content-Length", fileObj.FileSize.ToString());
                            resp.BinaryWrite(fileObj.File);
                            resp.End();
                            //resp.ContentType = fileObj.FileType;
                            //resp.OutputStream.Write(fileObj.File, 0, fileObj.File.Length);
                        }
                    }
                    if (context.Request.QueryString["Type"].ToString() == "GetMediaFile")
                    {
                        int fileId = int.Parse(context.Request.QueryString["FileId"].ToString());
                        string downloadAble = "";
                        downloadAble = context.Request.QueryString["IsDownload"]?.ToString();
                        var fileObj = db.GeneralMediaFiles.Find(fileId);
                        HttpResponse resp = context.Response;
                        resp.ClearHeaders();
                        resp.ClearContent();
                        if (downloadAble == "Y")
                            resp.AddHeader("Content-Disposition", "attachment; filename=" + fileObj.FileName);
                        resp.AddHeader("Content-Type", fileObj.FileType);
                        resp.AddHeader("Content-Length", fileObj.FileSize.ToString());
                        resp.BinaryWrite(fileObj.File);
                        resp.End();
                    }
                    if (context.Request.QueryString["Type"].ToString() == "UploadDrawingImage")
                    {
                        string id = context.Request.QueryString["DrawingOrderId"].ToString();
                        int drawingOrderId = 0;
                        if (string.IsNullOrEmpty(id))
                        {
                            drawingOrderId = int.Parse(id);
                        }
                        
                        string sessionId = context.Request.QueryString["SessionId"];
                        HttpPostedFile file = context.Request.Files[0];
                        BinaryReader b = new BinaryReader(file.InputStream);
                        byte[] binData = b.ReadBytes(int.Parse(file.InputStream.Length.ToString()));

                        string fileName = file.FileName;
                        string fileExtension = file.ContentType;

                        //var oldFiles = db.DrawingFiles.Where(m => m.DrawingOrderId == drawingOrderId && m.Type == "P").ToList();
                        //db.DrawingFiles.RemoveRange(oldFiles);


                        DrawingFiles drawingFile = new DrawingFiles();
                        drawingFile.File = binData;
                        drawingFile.FileName = fileName;
                        drawingFile.FileType = fileExtension;
                        if(drawingOrderId != 0)
                            drawingFile.DrawingOrderId = drawingOrderId;
                        drawingFile.Type = "P";
                        drawingFile.FileSize = file.ContentLength;
                        drawingFile.SessionId = sessionId;
                        db.DrawingFiles.Add(drawingFile);
                        db.SaveChanges();

                        string json = "{\"success\":\"true\",\"FileId\":\""+ drawingFile.Id + "\"}";
                        context.Response.Clear();
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.Write(json);
                        context.Response.End();
                    }
                    if (context.Request.QueryString["Type"].ToString() == "GetDrawingFile")
                    {
                        int fileId = int.Parse(context.Request.QueryString["FileId"].ToString());
                        string downloadAble = "";
                        downloadAble = context.Request.QueryString["IsDownload"]?.ToString();
                        var fileObj = db.DrawingFiles.Find(fileId);
                        HttpResponse resp = context.Response;
                        resp.ClearHeaders();
                        resp.ClearContent();
                        if(downloadAble == "Y")
                            resp.AddHeader("Content-Disposition", "attachment; filename=" + fileObj.FileName);
                        resp.AddHeader("Content-Type", fileObj.FileType);
                        resp.AddHeader("Content-Length", fileObj.FileSize.ToString());
                        resp.BinaryWrite(fileObj.File);
                        resp.End();
                    }
                    if (context.Request.QueryString["Type"].ToString() == "GetDrawingFileThumbnail")
                    {
                        int fileId = int.Parse(context.Request.QueryString["FileId"].ToString());
                        var fileObj = db.DrawingFiles.Find(fileId);
                        HttpResponse resp = context.Response;

                        resp.ClearHeaders();
                        resp.ClearContent();

                        if (fileObj.File == null)
                        {
                            resp.ContentType = "image/png";
                            resp.OutputStream.Write(null, 0, 0);
                        }
                        else
                        {
                            resp.ContentType = "image/png";
                            var bytes = resizeImage(Convert.ToInt32(width), Convert.ToInt32(height), fileObj.File);
                            resp.OutputStream.Write(bytes, 0, bytes.Length);
                        }
                    }

                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("Hello World");
            }
        }

        private Image GetImage(byte[] storedImage)
        {
            var stream = new MemoryStream(storedImage);
            return Image.FromStream(stream);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public byte[] resizeImage(int newWidth, int newHeight, byte[] byteArrayIn)
        {

            ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            Size size = new Size(newWidth, newHeight);
            using (MemoryStream inStream = new MemoryStream(byteArrayIn))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Save(outStream);
                    }
                    BinaryReader b1 = new BinaryReader(outStream);
                    byte[] binData1 = b1.ReadBytes(int.Parse(outStream.Length.ToString()));
                    return binData1;
                    // Do something with the stream.
                }
            }
            //try
            //{
            //    Image imgPhoto = GetImage(byteArrayIn);

            //    int sourceWidth = imgPhoto.Width;
            //    int sourceHeight = imgPhoto.Height;

            //    if (sourceWidth < newWidth || sourceHeight < newHeight)
            //        return byteArrayIn;

            //    //Consider vertical pics
            //    //if (sourceWidth < sourceHeight)
            //    //{
            //    //    int buff = newWidth;

            //    //    newWidth = newHeight;
            //    //    newHeight = buff;
            //    //}

            //    int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            //    float nPercent = 0, nPercentW = 0, nPercentH = 0;

            //    nPercentW = ((float)newWidth / (float)sourceWidth);
            //    nPercentH = ((float)newHeight / (float)sourceHeight);
            //    if (nPercentH < nPercentW)
            //    {
            //        nPercent = nPercentH;
            //        destX = System.Convert.ToInt16((newWidth -
            //                  (sourceWidth * nPercent)) / 2);
            //    }
            //    else
            //    {
            //        nPercent = nPercentW;
            //        destY = System.Convert.ToInt16((newHeight -
            //                  (sourceHeight * nPercent)) / 2);
            //    }

            //    int destWidth = (int)(sourceWidth * nPercent);
            //    int destHeight = (int)(sourceHeight * nPercent);


            //    Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
            //                  PixelFormat.Format24bppRgb);

            //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
            //                 imgPhoto.VerticalResolution);
            //    bmPhoto.MakeTransparent();

            //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //    grPhoto.Clear(Color.Transparent);
            //    grPhoto.InterpolationMode =
            //        System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;



            //    grPhoto.DrawImage(imgPhoto,
            //        new Rectangle(destX, destY, destWidth, destHeight),
            //        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            //        GraphicsUnit.Pixel);

            //    grPhoto.Dispose();
            //    imgPhoto.Dispose();
            //    MemoryStream ms = new MemoryStream();
            //    bmPhoto.Save(ms, ImageFormat.Png);
            //    return ms.ToArray();
            //}
            //catch
            //{
            //    return null;
            //}
        }
    }
}