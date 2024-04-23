using ViL.Common.Enums;
using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookChaptersService : IServices<BookChapters>
    {
        public void ProcessAudioAsync(string txtPath, string txtFileName, string? uploaderId);
    }

    public class BookChaptersService : ServiceBase<BookChapters>, IBookChaptersService
    {
        protected INotificationsService _notiService;

        public BookChaptersService(IBookChaptersRepository chapterRepository, ViLDbContext dbContext, INotificationsService notificationsService) : base(chapterRepository, dbContext) 
        {
            _notiService = notificationsService;
        }

        protected override bool validate(BookChapters entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity);
            }
            var isValid = true;
            var query = _repository.GetById(entity.ChapterId);
            if (query != null)
            {
                if (query.BookId != entity.BookId)
                {
                    isValid = false;
                    listErrorMsgs.Add("BookId không được phép thay đổi");
                }
            } else
            {
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }

        public void ProcessAudioAsync(string txtPath, string txtFileName, string? uploaderId)
        {
            var flaskApiUrl = "http://127.0.0.1:5000/";

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(15);
                // Đọc nội dung từ file .txt
                var txtFilePath = Path.Combine(txtPath, txtFileName);
                var textInput = System.IO.File.ReadAllText(txtFilePath);

                // Gửi yêu cầu tới Flask API
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(textInput), "text_file", txtFileName);

                try
                {
                    var response = client.PostAsync(flaskApiUrl, formData).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Lưu file .wav xuống máy chủ ASP.NET
                        string folder = Path.GetDirectoryName(txtPath) ?? "..\\Data\\tmp";
                        var path = Path.Combine(folder, "audio");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(txtFilePath);
                        var outputPath = Path.Combine(path, $"{fileName}.wav");
                        using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                        {
                            using (var fileStream = new FileStream(outputPath, FileMode.Create))
                            {
                                responseStream.CopyToAsync(fileStream).Wait();
                            }
                        }
                        if (uploaderId != null)
                        {
                            var successNotification = new Notifications((int)NotiType.Success, uploaderId, "Audio convertion success!");
                        }
                    }
                    else
                    {
                        // Handle error if needed
                    }
                } catch(Exception)
                {
                    
                }              
                
            }
        }
    }
}
