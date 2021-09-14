using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class DatabaseService : IDatabaseService
    {
        private static readonly string DB_FORMAT_CON = "Server={0};Database={1};User Id=sa;Password={2};MultipleActiveResultSets=true";

        //private string _formatConnection = string.Empty;

        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            //_formatConnection = _configuration["ConnectionStrings:FormatConnection"];
        }

        public async Task<CompanyModel> GetDetailByBienBanXoaBoIdAsync(string bienBanId)
        {
            try
            {
                string cusManConnection = string.Format(_configuration["ConnectionStrings:FormatConnection"], "CusMan");
                List<CompanyModel> companyModels = new List<CompanyModel>();
                using (SqlConnection connection = new SqlConnection(cusManConnection))
                {
                    using (SqlCommand command = new SqlCommand($"select * from Companys where Type = 0 and TypeDetail = 0 and TypeNewDetailInvoice = 1 order by DataBaseName", connection))
                    {
                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CompanyModel companyModel = new CompanyModel
                                {
                                    DataBaseName = reader["DataBaseName"].ToString(),
                                    TaxCode = reader["TaxCode"].ToString(),
                                    Type = int.Parse(reader["Type"].ToString()),
                                    ConnectionString = string.Format(_configuration["ConnectionStrings:FormatConnection"], reader["DataBaseName"].ToString())
                                };
                                companyModels.Add(companyModel);
                            }
                        }
                    }
                }

                foreach (var item in companyModels)
                {
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand($"select COUNT(*) from BienBanXoaBos where BienBanXoaBoId = '{bienBanId}'", connection))
                        {
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
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CompanyModel> GetDetailByHoaDonIdAsync(string hoaDonId)
        {
            try
            {
                string cusManConnection = string.Format(_configuration["ConnectionStrings:FormatConnection"], "CusMan");
                List<CompanyModel> companyModels = new List<CompanyModel>();
                using (SqlConnection connection = new SqlConnection(cusManConnection))
                {
                    using (SqlCommand command = new SqlCommand($"select * from Companys where Type = 0 and TypeDetail = 0 and TypeNewDetailInvoice = 1 order by DataBaseName", connection))
                    {
                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CompanyModel companyModel = new CompanyModel
                                {
                                    DataBaseName = reader["DataBaseName"].ToString(),
                                    TaxCode = reader["TaxCode"].ToString(),
                                    Type = int.Parse(reader["Type"].ToString()),
                                    ConnectionString = string.Format(_configuration["ConnectionStrings:FormatConnection"], reader["DataBaseName"].ToString())
                                };
                                companyModels.Add(companyModel);
                            }
                        }
                    }
                }

                foreach (var item in companyModels)
                {
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand($"select COUNT(*) from HoaDonDienTus where HoaDonDienTuId = '{hoaDonId}'", connection))
                        {
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
            catch (Exception e)
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
                    string query = $"SELECT * FROM Companys WHERE TaxCode = '{key}' AND [Type] = 0";
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

                                return companyModel;
                            }
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CompanyModel> GetDetailByLookupCodeAsync(string lookupCode)
        {
            try
            {
                string cusManConnection = string.Format(_configuration["ConnectionStrings:FormatConnection"], "CusMan");
                List<CompanyModel> companyModels = new List<CompanyModel>();
                using (SqlConnection connection = new SqlConnection(cusManConnection))
                {
                    using (SqlCommand command = new SqlCommand($"select * from Companys where Type = 0 and TypeDetail = 0 and TypeNewDetailInvoice = 1 order by DataBaseName", connection))
                    {
                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                CompanyModel companyModel = new CompanyModel
                                {
                                    DataBaseName = reader["DataBaseName"].ToString(),
                                    TaxCode = reader["TaxCode"].ToString(),
                                    Type = int.Parse(reader["Type"].ToString()),
                                    ConnectionString = string.Format(_configuration["ConnectionStrings:FormatConnection"], reader["DataBaseName"].ToString())
                                };
                                companyModels.Add(companyModel);
                            }
                        }
                    }
                }

                foreach (var item in companyModels)
                {
                    if(item.TaxCode == "0109205608")
                    {
                        var a = 1;
                    }
                    using (SqlConnection connection = new SqlConnection(item.ConnectionString))
                    {
                        using (SqlCommand command = new SqlCommand($"select COUNT(*) from HoaDonDienTus where TrangThaiPhatHanh = 3 and MaTraCuu = '{lookupCode}'", connection))
                        {
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
            catch (Exception e)
            {
                return null;
            }
        }


    }
}
