using DLL.Entity.QuyDinhKyThuat;
using Services.Repositories.Implimentations.ESignCloud;
using Services.ViewModels.Pos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.ESignCloud
{
    public interface IESignCloudService
    {
        Task<int> PrepareFileForSignCloud(string agreementUUID, SignCloudMetaData signCloudMetaData, string passCode,
            string mimeType,
            string fileName,
            byte[] fileData, string fullXmlFilePath);
        Task<int> ForgetPasscodeForSignCloud(string agreementUUID);
        Task<ChungThuSoSuDung> GetInfoSignCloud(string agreementUUID);
        Task<MessageObj> SignCloudFile(MessageObj dataJson);
        Task ChangePasscodeForSignCloud(string agreementUUID, string currentPassCode, string newPassCode);
    }
}
