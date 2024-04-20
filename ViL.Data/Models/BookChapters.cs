﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ViL.Common.Enums;

namespace ViL.Data.Models
{
    [Table("BookChapters")]
    public class BookChapters : EntityBase
    {
        [Key]        
        public string ChapterId { get; set; }
        [Required(ErrorMessage = "BookId is required")]
        public string BookId { get; set; }
        public string? ChapterTitle { get; set; }
        public int? ChapterNum { get; set; }
        public string? FileName { get; set; }
        public string? Images { get; set; }
        public string? Url { get; set; }
        public int? Views { get; set; }
        public int? Comments { get; set; }
        public int? Likes { get; set; }
        public string? UploaderId { get; set; }

        public BookChapters() : base()
        {
            ChapterId = Guid.NewGuid().ToString();
            BookId = string.Empty;
        }

        public BookChapters(string bookId, int? chapterNo, string? uploaderId, string? title = null) : base(status: (int)ChapterStatus.Draft)
        {
            ChapterId = Guid.NewGuid().ToString();
            BookId = bookId;
            ChapterTitle = $"Chương {chapterNo}" + (title!= null && title!="" ? $": {title}" : "");
            ChapterNum = chapterNo;
            Images = string.Empty;
            FileName = $"..\\Data\\{bookId}\\text\\{chapterNo.ToString()?.PadLeft(5,'0')}.txt";
            Url = bookId;
            UploaderId = uploaderId;
            Views = 0;
            Comments = 0;
            Likes = 0;
        }
    }
}
