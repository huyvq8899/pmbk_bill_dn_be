using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Helper.Constants;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
                    string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";
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
                string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";
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
        public async Task<bool> InsertFileMauHoaDon(MauHoaDonUploadImage model, Datacontext datacontext)
        {
            bool hasSave = false;

            if (model.Logo != null)
            {
                var filename = ContentDispositionHeaderValue
                                       .Parse(model.Logo.ContentDisposition)
                                       .FileName
                                       .Trim('"');
                var indexext = filename.LastIndexOf(".");
                var name = filename.Substring(0, indexext);
                var ext = filename.Substring(indexext);
                string filenameGuid = $"{name}_{Guid.NewGuid()}{ext}";

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";
                string folder = _hostingEnvironment.WebRootPath + rootFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filenameGuid);
                using (FileStream fs = File.Create(filePath))
                {
                    model.Logo.CopyTo(fs);
                    fs.Flush();
                }

                var thietLapLogo = await datacontext.MauHoaDonThietLapMacDinhs
                    .FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId && x.Loai == LoaiThietLapMacDinh.Logo);
                if (thietLapLogo != null)
                {
                    thietLapLogo.GiaTri = filenameGuid;
                }

                FileData fileData = new FileData
                {
                    RefId = model.MauHoaDonId,
                    Type = 4,
                    DateTime = DateTime.Now,
                    Binary = File.ReadAllBytes(filePath),
                    FileName = filenameGuid
                };
                await datacontext.FileDatas.AddAsync(fileData);

                hasSave = true;
            }
            if (!string.IsNullOrEmpty(model.RemovedLogoFileName))
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}\{model.RemovedLogoFileName}";
                string folder = _hostingEnvironment.WebRootPath + rootFolder;
                FileInfo file = new FileInfo(folder);
                if (file.Exists)
                {
                    file.Delete();
                }

                var removedFileData = await datacontext.FileDatas
                    .FirstOrDefaultAsync(x => x.RefId == model.MauHoaDonId && x.FileName == model.RemovedLogoFileName);
                if (removedFileData != null)
                {
                    datacontext.FileDatas.RemoveRange(removedFileData);
                }
            }

            if (model.Background != null)
            {
                var filename = ContentDispositionHeaderValue
                                       .Parse(model.Background.ContentDisposition)
                                       .FileName
                                       .Trim('"');
                var indexext = filename.LastIndexOf(".");
                var name = filename.Substring(0, indexext);
                var ext = filename.Substring(indexext);
                string filenameGuid = $"{name}_{Guid.NewGuid()}{ext}";

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";
                string folder = _hostingEnvironment.WebRootPath + rootFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filenameGuid);
                using (FileStream fs = File.Create(filePath))
                {
                    model.Background.CopyTo(fs);
                    fs.Flush();
                }

                var thietLapHinhNen = await datacontext.MauHoaDonThietLapMacDinhs
                    .FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId && x.Loai == LoaiThietLapMacDinh.HinhNenTaiLen);
                if (thietLapHinhNen != null)
                {
                    thietLapHinhNen.GiaTri = filenameGuid;
                }

                FileData fileData = new FileData
                {
                    RefId = model.MauHoaDonId,
                    Type = 4,
                    DateTime = DateTime.Now,
                    Binary = File.ReadAllBytes(filePath),
                    FileName = filenameGuid
                };
                await datacontext.FileDatas.AddAsync(fileData);

                hasSave = true;
            }
            if (!string.IsNullOrEmpty(model.RemovedBackgroundFileName))
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}\{model.RemovedBackgroundFileName}";
                string folder = _hostingEnvironment.WebRootPath + rootFolder;
                FileInfo file = new FileInfo(folder);
                if (file.Exists)
                {
                    file.Delete();
                }

                var removedFileData = await datacontext.FileDatas
                    .FirstOrDefaultAsync(x => x.RefId == model.MauHoaDonId && x.FileName == model.RemovedBackgroundFileName);
                if (removedFileData != null)
                {
                    datacontext.FileDatas.RemoveRange(removedFileData);
                }
            }

            if (hasSave)
            {
                await datacontext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> DeleteFileRefTypeById(string id, Datacontext datacontext)
        {
            await DeleteInFileDataByRefIdAsync(id, datacontext);

            var taiLieuDinhKems = await datacontext.TaiLieuDinhKems.Where(x => x.NghiepVuId == id).ToListAsync();
            if (taiLieuDinhKems.Count() > 0)
            {
                datacontext.TaiLieuDinhKems.RemoveRange(taiLieuDinhKems);
                await datacontext.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// AnhBH
        /// </summary>
        public async Task<bool> DeleteFileAttach(TaiLieuDinhKemViewModel model, Datacontext datacontext)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string rootFolder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";
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

                var taiLieuDinhKems = await datacontext.TaiLieuDinhKems.Where(x => x.TaiLieuDinhKemId == model.TaiLieuDinhKemId).ToListAsync();
                if (taiLieuDinhKems.Count() > 0)
                {
                    datacontext.TaiLieuDinhKems.RemoveRange(taiLieuDinhKems);
                    await datacontext.SaveChangesAsync();
                }

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
        public bool DeleteFileAvatar(string fileName)
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

        public async Task<bool> DeleteInFileDataByRefIdAsync(string refId, Datacontext datacontext)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}");

            var entities = await datacontext.FileDatas.Where(x => x.RefId == refId).ToListAsync();

            FileHelper.ScanToDeleteFiles(folderPath, entities.Select(x => x.FileName).ToList());

            datacontext.FileDatas.RemoveRange(entities);
            var result = await datacontext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<TaiLieuDinhKemViewModel>> GetFilesById(string nghiepVuId, Datacontext datacontext)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            var query = from tldk in datacontext.TaiLieuDinhKems
                        where tldk.NghiepVuId == nghiepVuId
                        orderby tldk.CreatedDate
                        select new TaiLieuDinhKemViewModel
                        {
                            TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                            NghiepVuId = tldk.NghiepVuId,
                            LoaiNghiepVu = tldk.LoaiNghiepVu,
                            TenGoc = tldk.TenGoc,
                            TenGuid = tldk.TenGuid,
                            CreatedDate = tldk.CreatedDate,
                            Link = _httpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{tldk.LoaiNghiepVu}\{nghiepVuId}\FileAttach", tldk.TenGuid),
                            Status = tldk.Status
                        };
            return await query.ToListAsync();
        }
    }
}
