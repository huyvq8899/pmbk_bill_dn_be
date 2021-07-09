using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<bool> InsertFileAttaches(TaiLieuDinhKemViewModel model, Datacontext datacontext)
        {
            bool hasSave = false;
            if (model.Files != null && model.Files.Count > 0)
            {
                List<TaiLieuDinhKem> result = new List<TaiLieuDinhKem>();

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
                    string loaiNghiepVu = Enum.GetName(typeof(LoaiNghiepVu), model.LoaiNghiepVu);
                    string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{loaiNghiepVu}\{model.NghiepVuId}";
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

                    result.Add(new TaiLieuDinhKem
                    {
                        LoaiNghiepVu = model.LoaiNghiepVu,
                        NghiepVuId = model.NghiepVuId,
                        TenGoc = filename,
                        TenGuid = filenameGuid,
                        CreatedDate = DateTime.Now,
                        Status = true
                    });
                }

                if (result.Any())
                {
                    await datacontext.AddRangeAsync(result);
                    hasSave = true;
                }
            }

            if (model.RemovedFileIds != null && model.RemovedFileIds.Count > 0)
            {
                var removedList = await datacontext.TaiLieuDinhKems.Where(x => model.RemovedFileIds.Contains(x.TaiLieuDinhKemId)).ToListAsync();
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(LoaiNghiepVu), model.LoaiNghiepVu);
                string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{loaiNghiepVu}\{model.NghiepVuId}";
                string folder = _hostingEnvironment.WebRootPath + rootFolder;
                foreach (var item in removedList)
                {
                    var filePath = Path.Combine(folder, item.TenGuid);
                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                datacontext.RemoveRange(removedList);
                hasSave = true;
            }

            if (hasSave)
            {
                await datacontext.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// AnhBH
        /// </summary>
        public bool DeleteFileAttach(TaiLieuDinhKemViewModel model)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(LoaiNghiepVu), model.LoaiNghiepVu);
            string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{loaiNghiepVu}\{model.NghiepVuId}";
            string folder = _hostingEnvironment.WebRootPath + rootFolder;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, model.TenGuid);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }

            return false;
        }

        /// <summary>
        /// AnhBH
        /// </summary>
        public async Task<bool> DeleteAllFileAttaches(TaiLieuDinhKemViewModel model, Datacontext datacontext)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(LoaiNghiepVu), model.LoaiNghiepVu);
            string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{loaiNghiepVu}\{model.NghiepVuId}";
            string folder = _hostingEnvironment.WebRootPath + rootFolder;
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            var taiLieuDinhKems = await datacontext.TaiLieuDinhKems.Where(x => x.NghiepVuId == model.NghiepVuId).ToListAsync();
            datacontext.TaiLieuDinhKems.RemoveRange(taiLieuDinhKems);
            await datacontext.SaveChangesAsync();
            return true;
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
