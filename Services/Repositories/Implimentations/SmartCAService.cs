using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.Config;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.Constants;
using Services.Repositories.Interfaces;
using Services.ViewModels.Config;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class SmartCAService : ISmartCAService
    {
        private readonly IConfiguration iConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _IHttpContextAccessor;

        public SmartCAService(IConfiguration IConfiguration,
            IHostingEnvironment httpContextAccessor,
            Datacontext db, IMapper mapper,
            IHttpContextAccessor IHttpContextAccessor)
        {
            iConfiguration = IConfiguration;
            _hostingEnvironment = httpContextAccessor;
            _db = db;
            _mp = mapper;
            _IHttpContextAccessor = IHttpContextAccessor;
        }

        public async Task<TaiKhoanSmartCAViewModel> InsertAsync(TaiKhoanSmartCAViewModel model)
        {
            try
            {
                var entity = _mp.Map<TaiKhoanSmartCA>(model);
                entity.CreatedDate = DateTime.Now;
                entity.ModifyDate = DateTime.Now;
                await _db.TaiKhoanSmartCAs.AddAsync(entity);
                await _db.SaveChangesAsync();

                var result = _mp.Map<TaiKhoanSmartCAViewModel>(entity);
                return result;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckTrungUserNameSmartCA(string UserNameInput)
        {
            bool result = await _db.TaiKhoanSmartCAs
                .AnyAsync(x => String.Compare(x.UserNameSmartCA.Trim(), UserNameInput.Trim(), false) == 0);
            return result;
        }

        public async Task<string> GetDataXMLUnsign(string idThongDiep)
        {
            var fileData = await _db.FileDatas.Where(x => x.RefId == idThongDiep && x.IsSigned == false).FirstOrDefaultAsync();
            if (fileData != null)
            {
                return TextHelper.Base64Encode(fileData.Content);
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> AccessSmartCA(string type, string args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/GatewayServiceTest.exe");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.Arguments = $"{type} {args}";
            process.StartInfo.Verb = "runas";
            process.Start();

            var standardOutput = new StringBuilder();
            // read chunk-wise while process is running.
            while (!process.HasExited)
            {
                standardOutput.Append(process.StandardOutput.ReadToEnd());
            }

            // make sure not to miss out on any remaindings.
            standardOutput.Append(process.StandardOutput.ReadToEnd());

            return standardOutput.ToString();
        }

        public async Task<TaiKhoanSmartCAViewModel> GetChuKiMemMoiNhat()
        {
            var entity = await _db.TaiKhoanSmartCAs.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
            var result = _mp.Map<TaiKhoanSmartCAViewModel>(entity);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetDataXMLHoaDonUnsign(string idHoaDon)
        {
            var fileData = await _db.FileDatas.Where(x => x.RefId == idHoaDon && x.IsSigned == false).FirstOrDefaultAsync();
            if (fileData != null)
            {
                return TextHelper.Base64Encode(fileData.Content);
            }
            else
            {
                return string.Empty;
            }
        }

        public string ReadFileHoaDonThayThe(string url)
        {
            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

                //đường dẫn đến file xml đã ký
                var signedXmlFileFolder = fullFolder + "/" + url;
                // Gửi dữ liệu tới TVan
                var xmlContent = File.ReadAllText(signedXmlFileFolder);
                if (xmlContent != null)
                {
                    return xmlContent;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<string> SignSmartCAXML(TaiKhoanSmartCAViewModel model)
        {
            // Lấy thông tin tài khoản mềm
            var accountCA = await this.GetChuKiMemMoiNhat();

            // Data xml nguyên thủy chưa ký
            string dataXML = TextHelper.Decompress(model.DataUnsign);

            // Ghi xuống file cho tool ký
            var path = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/{Guid.NewGuid()}.xml");
            await File.WriteAllTextAsync(path, dataXML);

            // Tạo param
            var pathXMLSigned = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/{Guid.NewGuid()}.xml");
            string json = JsonConvert.SerializeObject(
                           new
                           {
                               UID = accountCA.UserNameSmartCA,
                               Password = accountCA.PasswordSmartCA,
                               MLTDiep = (int)ViewModels.XML.MLTDiep.TDGHDDTTCQTCapMa,
                               Description = model.Description,
                               //DataXML = model.DataUnsign,
                               //IsCompress = true,
                               PathXML = path,
                               PathXMLSigned = pathXMLSigned
                           }, Newtonsoft.Json.Formatting.Indented);

            string encodeJson = TextHelper.Base64Encode(json);

            // Ký số hóa đơn
            pathXMLSigned = await this.AccessSmartCA("-sign", encodeJson);
            string xmlSigned = string.Empty;
            if (!string.IsNullOrEmpty(pathXMLSigned))
            {
                pathXMLSigned = TextHelper.Base64Decode(pathXMLSigned);
                if (File.Exists(pathXMLSigned))
                {
                    xmlSigned = await File.ReadAllTextAsync(pathXMLSigned);

                    // Delete file da ky
                    //File.Delete(pathXMLSigned);
                }
            }

            return xmlSigned;
        }

        public async Task<string> SignSmartCAXMLToKhai(TaiKhoanSmartCAViewModel model)
        {
            string dataXML = await this.GetDataXMLUnsign(model.DataUnsign);

            // Decode
            dataXML = TextHelper.Base64Decode(dataXML);

            // Lấy thông tin tài khoản mềm
            var accountCA = await this.GetChuKiMemMoiNhat();

            // Ghi xuống file cho tool ký
            var path = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/{Guid.NewGuid()}.xml");
            await File.WriteAllTextAsync(path, dataXML);

            // Tạo param
            var pathXMLSigned = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/{Guid.NewGuid()}.xml");
            string json = JsonConvert.SerializeObject(
                           new
                           {
                               UID = accountCA.UserNameSmartCA,
                               Password = accountCA.PasswordSmartCA,
                               MLTDiep = (int)ViewModels.XML.MLTDiep.TDGToKhai,
                               Description = model.Description,
                               PathXML = path,
                               PathXMLSigned = pathXMLSigned
                           }, Newtonsoft.Json.Formatting.Indented);

            string encodeJson = TextHelper.Base64Encode(json);

            // Ký số hóa đơn
            pathXMLSigned = await this.AccessSmartCA("-sign", encodeJson);
            string xmlSigned = string.Empty;
            if (!string.IsNullOrEmpty(pathXMLSigned))
            {
                pathXMLSigned = TextHelper.Base64Decode(pathXMLSigned);
                if (File.Exists(pathXMLSigned))
                {
                    xmlSigned = await File.ReadAllTextAsync(pathXMLSigned);

                    // Delete file da ky
                    //File.Delete(pathXMLSigned);
                }
            }

            return xmlSigned;
        }

        public async Task<string> SignSmartCAXMLSaiSot(TaiKhoanSmartCAViewModel model)
        {
            string dataXML = this.ReadFileHoaDonThayThe(model.DataUnsign);

            var accountCA = await this.GetChuKiMemMoiNhat();

            var path = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/{Guid.NewGuid()}.xml");
            await File.WriteAllTextAsync(path, dataXML);

            // Tạo param
            var pathXMLSigned = Path.Combine(_hostingEnvironment.WebRootPath, $"tools/SmartCA/{Guid.NewGuid()}.xml");
            string json = JsonConvert.SerializeObject(
                           new
                           {
                               UID = accountCA.UserNameSmartCA,
                               Password = accountCA.PasswordSmartCA,
                               MLTDiep = (int)ViewModels.XML.MLTDiep.TDTBHDDLSSot,
                               Description = model.Description,
                               PathXML = path,
                               PathXMLSigned = pathXMLSigned
                           }, Newtonsoft.Json.Formatting.Indented);

            string encodeJson = TextHelper.Base64Encode(json);

            // Ký số hóa đơn
            pathXMLSigned = await this.AccessSmartCA("-sign", encodeJson);
            string xmlSigned = string.Empty;
            if (!string.IsNullOrEmpty(pathXMLSigned))
            {
                pathXMLSigned = TextHelper.Base64Decode(pathXMLSigned);
                if (File.Exists(pathXMLSigned))
                {
                    xmlSigned = await File.ReadAllTextAsync(pathXMLSigned);

                    // Delete file da ky
                    //File.Delete(pathXMLSigned);
                }
            }

            return xmlSigned;
        }
    }
}
