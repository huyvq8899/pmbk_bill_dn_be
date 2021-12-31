using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT.TVAN
{
    public class SQLHelper
    {
        public static List<int> GetInvoiceDetailType(string con, string mst)
        {
            List<int> lstTypeDetails = new List<int>();
            try
            {
                SqlConnection conn = new SqlConnection(con);

                conn.Open();

                string query = string.Format(@"SELECT TypeDetail FROM Companys WHERE [Type] = 0 AND TaxCode = '{0}'", mst);
                SqlCommand command = new SqlCommand(query, conn);

                // Get value
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Ms_Thue
                            if (!reader.IsDBNull(0))
                            {
                                int iTypeDetail= reader.GetInt32(0);

                                lstTypeDetails.Add(iTypeDetail);
                            }
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return lstTypeDetails;
        }
    }
}
