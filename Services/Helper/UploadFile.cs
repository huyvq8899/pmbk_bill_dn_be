using DLL;
using DLL.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace ManagementServices.Helper
{
    public class UploadFile
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadFile(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public string InsertFile(IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\FileAttach";
                //string folder = _hostingEnvironment.WebRootPath;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return (filePath);
            }
            return null;
        }

        public string InsertFileExcel(IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\excels";
                //string folder = _hostingEnvironment.WebRootPath;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return (filePath);
            }
            return null;
        }
        public string InsertFileAvatar(out string fileName, IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                var indexext = filename.LastIndexOf(".");
                var name = filename.Substring(0, indexext);
                var ext = filename.Substring(indexext);
                filename = name + Guid.NewGuid() + ext;

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\Avatar";
                //string folder = _IConfiguration["FolderFileBase:url"] + $@"\FilesUpload\Avatar";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                fileName = filename;
                return (filePath);
            }
            fileName = "";
            return null;
        }
        public string InsertFileAttach(out string fileName, IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                var indexext = filename.LastIndexOf(".");
                var name = filename.Substring(0, indexext);
                var ext = filename.Substring(indexext);
                filename = name + Guid.NewGuid() + ext;

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\FileAttach";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                fileName = filename;
                return (filePath);
            }
            fileName = "";
            return null;
        }

        /// <summary>
        /// AnhBH
        /// </summary>
        public List<UploadFileViewModel> InsertFileAttaches(UploadFileViewModel model)
        {
            List<UploadFileViewModel> result = new List<UploadFileViewModel>();

            if (model.Files != null && model.Files.Count > 0)
            {
                foreach (var file in model.Files)
                {
                    var filename = ContentDispositionHeaderValue
                                       .Parse(file.ContentDisposition)
                                       .FileName
                                       .Trim('"');
                    var indexext = filename.LastIndexOf(".");
                    var name = filename.Substring(0, indexext);
                    var ext = filename.Substring(indexext);
                    string filenameGuid = $"{name}_{Guid.NewGuid()}{ext}";

                    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    string menuName = Enum.GetName(typeof(MenuType), model.MenuType);

                    string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{menuName}\{model.Id}";
                    string folder = _hostingEnvironment.WebRootPath + rootFolder;

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, filenameGuid);
                    using (FileStream fs = File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    result.Add(new UploadFileViewModel
                    {
                        Id = model.Id,
                        Name = filename,
                        NameGuid = filenameGuid,
                        Link = rootFolder + "\\" + filenameGuid
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// AnhBH
        /// </summary>
        public bool DeleteFileAttach(UploadFileViewModel model)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string menuName = Enum.GetName(typeof(MenuType), model.MenuType);
            string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{menuName}\{model.Id}";
            string folder = _hostingEnvironment.WebRootPath + rootFolder;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, model.Name);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }

            return false;
        }

        public string InsertFileAttachDiffirentDomain(out string fileName, IList<IFormFile> files, IConfiguration _IConfiguration)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                var indexext = filename.LastIndexOf(".");
                var name = filename.Substring(0, indexext);
                var ext = filename.Substring(indexext);
                filename = name + Guid.NewGuid() + ext;

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _IConfiguration["FolderFileBase:url"] + $@"\FilesUpload\{databaseName}\FileAttach";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                fileName = filename;
                return (filePath);
            }
            fileName = "";
            return null;
        }
        public string InsertFileImage(string fileName, string exten, IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                //fileServer = fileName;
                //var filename = ContentDispositionHeaderValue
                //                   .Parse(file.ContentDisposition)
                //                   .FileName
                //                   .Trim('"');
                //var tmpfileName = fileName;
                //var indexext = fileName.LastIndexOf(".");
                //var name = fileName.Substring(0, indexext);
                //var ext = fileName.Substring(indexext);
                //fileServer = name + Guid.NewGuid() + ext;
                string fileServer = fileName + exten;

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\Image";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, fileServer);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return (filePath);
            }
            return null;
        }
        public string InsertFileMessage(out string fileName, IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                var indexext = filename.LastIndexOf(".");
                var name = filename.Substring(0, indexext);
                var ext = filename.Substring(indexext);
                filename = name + Guid.NewGuid() + ext;

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\MessageFile";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                fileName = filename;
                return (filePath);
            }
            fileName = "";
            return null;
        }
        public bool DeleteFileAvatar(string fileName, IConfiguration _IConfiguration)
        {
            try
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\Avatar";
                //string folder = _IConfiguration["FolderFileBase:url"] + $@"\FilesUpload\Avatar";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, fileName);
                FileInfo file = new FileInfo(filePath);
                file.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool DeleteFileAttach(string fileName)
        {
            try
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = _hostingEnvironment.WebRootPath + $@"\FilesUpload\{databaseName}\FileAttach";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, fileName);
                FileInfo file = new FileInfo(filePath);
                file.Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
