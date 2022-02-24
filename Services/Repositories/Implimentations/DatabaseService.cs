using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class DatabaseService : IDatabaseService
    {
        private static readonly string DB_FORMAT_CON = "Server={0};Database={1};User Id=sa;Password={2};MultipleActiveResultSets=true";

        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CompanyModel> GetDetailByBienBanXoaBoIdAsync(string bienBanId)
        {
            try
            {
                List<CompanyModel> companyModels = await GetCompanies();

                foreach (var item in companyModels)
                {
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        string query = $"SELECT COUNT(*) FROM BienBanXoaBos WHERE Id = @bienBanId";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@bienBanId", SqlDbType.NVarChar);
                            command.Parameters["@bienBanId"].Value = bienBanId;

                            await connection.OpenAsync();
                            object result = await command.ExecuteScalarAsync();
                            if ((int)result > 0)
                            {
                                return item;
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CompanyModel> GetDetailByHoaDonIdAsync(string hoaDonId)
        {
            try
            {
                List<CompanyModel> companyModels = await GetCompanies();

                foreach (var item in companyModels)
                {
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        string query = $"SELECT COUNT(*) FROM HoaDonDienTus WHERE HoaDonDienTuId = @hoaDonId";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@hoaDonId", SqlDbType.NVarChar);
                            command.Parameters["@hoaDonId"].Value = hoaDonId;

                            await connection.OpenAsync();
                            object result = await command.ExecuteScalarAsync();
                            if ((int)result > 0)
                            {
                                return item;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<CompanyModel> GetDetailByLookupCodeAsync(string lookupCode)
        {
            try
            {
                List<CompanyModel> companyModels = await GetCompanies();

                foreach (var item in companyModels)
                {
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        string query = $"SELECT COUNT(*) FROM HoaDonDienTus WHERE MaTraCuu = @MaTraCuu";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@MaTraCuu", SqlDbType.NVarChar);
                            command.Parameters["@MaTraCuu"].Value = lookupCode;

                            await connection.OpenAsync();
                            object result = await command.ExecuteScalarAsync();
                            if ((int)result > 0)
                            {
                                return item;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CompanyModel> GetDetailBySoHoaDonAsync(KetQuaTraCuuXML input)
        {
            try
            {
                List<CompanyModel> companyModels = await GetCompanies();

                foreach (var item in companyModels)
                {
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        string query = $@"SELECT COUNT(*) FROM HoaDonDienTus hd
                                            INNER JOIN BoKyHieuHoaDons kh ON hd.BoKyHieuHoaDonId = kh.BoKyHieuHoaDonId
                                        WHERE hd.SoHoaDon = @SoHoaDon AND kh.KyHieuHoaDon = @KyHieu";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@SoHoaDon", SqlDbType.NVarChar);
                            command.Parameters["@SoHoaDon"].Value = input.SoHoaDon.ToString();
                            command.Parameters.Add("@KyHieu", SqlDbType.NVarChar);
                            command.Parameters["@KyHieu"].Value = input.KyHieuHoaDon;

                            await connection.OpenAsync();
                            object result = await command.ExecuteScalarAsync();
                            if ((int)result > 0)
                            {
                                return item;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CompanyModel> GetDetailByKeyAsync(string key)
        {
            try
            {
                string qlkhConnection = string.Format(_configuration["ConnectionStrings:FormatConnection"], "CusMan");
                using (SqlConnection connection = new SqlConnection(qlkhConnection))
                {
                    string query = $"SELECT * FROM Companys WHERE TaxCode = @TaxCode AND [Type] = @type AND TypeDetail = @TypeDetail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@TaxCode", SqlDbType.NVarChar);
                        command.Parameters["@TaxCode"].Value = key;

                        command.Parameters.Add("@type", SqlDbType.Int);
                        command.Parameters["@type"].Value = 0;

                        command.Parameters.Add("@TypeDetail", SqlDbType.Int);
                        command.Parameters["@TypeDetail"].Value = 2;            // Hóa đơn theo TT78

                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CompanyModel companyModel = new CompanyModel
                                {
                                    Server = reader["Server"].ToString(),
                                    DataBaseName = reader["DataBaseName"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    TaxCode = reader["TaxCode"].ToString(),
                                    Type = int.Parse(reader["Type"].ToString()),
                                    TypeDetail = int.Parse(reader["TypeDetail"].ToString()),
                                    UrlInvoice = reader["WebsiteInvoice"].ToString()
                                };
                                companyModel.ConnectionString = string.Format(DB_FORMAT_CON, companyModel.Server, companyModel.DataBaseName, companyModel.Password);

                                return companyModel;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CompanyModel>> GetCompanies()
        {
            string cusManConnection = string.Format(_configuration["ConnectionStrings:FormatConnection"], "CusMan");
            List<CompanyModel> companyModels = new List<CompanyModel>();
            using (SqlConnection connection = new SqlConnection(cusManConnection))
            {
                string query = "SELECT * FROM Companys WHERE Type = 0 and TypeDetail = 2 ORDER BY DataBaseName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CompanyModel companyModel = new CompanyModel
                            {
                                Server = reader["Server"].ToString(),
                                DataBaseName = reader["DataBaseName"].ToString(),
                                Password = reader["Password"].ToString(),
                                TaxCode = reader["TaxCode"].ToString(),
                                Type = int.Parse(reader["Type"].ToString()),
                                TypeDetail = int.Parse(reader["TypeDetail"].ToString()),
                                UrlInvoice = reader["WebsiteInvoice"].ToString()
                            };
                            companyModel.ConnectionString = string.Format(DB_FORMAT_CON, companyModel.Server, companyModel.DataBaseName, companyModel.Password);

                            companyModels.Add(companyModel);
                        }
                    }
                }
            }

            return companyModels;
        }
    }
}
