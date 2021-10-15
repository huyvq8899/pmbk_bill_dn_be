using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace BKSOFT_KYSO
{
    class CertificateUtil
    {
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        public static X509Certificate2 GetAllCertificateFromStore(string mst)
        {
            X509Certificate2 cert = null;
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

                // Push certificate has MST the first
                DateTime curDate = DateTime.Now;
                X509Certificate2Collection x509Certificates = new X509Certificate2Collection();
                List<X509Certificate2> lstCerts = new List<X509Certificate2>();
                foreach (X509Certificate2 x509 in collection)
                {
                    string mstToken = Utils.GetMaSoThueFromSubject(x509.Subject);
                    if (mst.Equals(mstToken))
                    {
                        lstCerts.Add(x509);
                    }
                    else if (!string.IsNullOrEmpty(x509.SerialNumber))
                    {
                        x509Certificates.Insert(0, x509);
                    }
                    else if (!string.IsNullOrEmpty(x509.Subject))
                    {
                        x509Certificates.Insert(0, x509);
                    }
                }

                // Add to list
                lstCerts = lstCerts.OrderBy(x => x.NotAfter).ToList();
                foreach (var x509 in lstCerts)
                {
                    x509Certificates.Add(x509);
                }

                //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(x509Certificates, "Hóa đơn Bách Khoa", "Chọn chứng thư số", X509SelectionFlag.SingleSelection, GetForegroundWindow());

                foreach (X509Certificate2 x509 in scollection)
                {
                    cert = x509;
                    break;
                }

                store.Close();
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return cert;
        }

        public static X509Certificate2 GetAllCertificateFromStore()
        {
            X509Certificate2 cert = null;
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection, "Hóa đơn Bách Khoa", "Chọn chứng thư số", X509SelectionFlag.SingleSelection, GetForegroundWindow());

                foreach (X509Certificate2 x509 in scollection)
                {
                    cert = x509;
                    break;
                }

                store.Close();
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return cert;
        }
    }
}
