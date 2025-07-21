using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;


namespace DoAnCNPM.Models
{
    public class DataModel
    {
        private string connecttionStrings = "workstation id=VOVBACSI.mssql.somee.com;packet size=4096;user id=LuongDat_SQLLogin_1;pwd=123456789;data source=VOVBACSI.mssql.somee.com;persist security info=False;initial catalog=VOVBACSI;TrustServerCertificate=True";
        public ArrayList get(String sql)
        {
            ArrayList datalist = new ArrayList();
            SqlConnection connection = new SqlConnection(connecttionStrings);
            
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            using (SqlDataReader r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    ArrayList row = new ArrayList();
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        row.Add(Xulydulieu(r.GetValue(i).ToString()));
                    }
                    datalist.Add(row);

                }
            }
            connection.Close();

            return datalist;
        }
        // Method mới để thực thi stored procedures với tham số
        public ArrayList ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            ArrayList datalist = new ArrayList();
            SqlConnection connection = new SqlConnection(connecttionStrings);
            
            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            // Thêm tham số nếu có
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            
            connection.Open();
            using (SqlDataReader r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    ArrayList row = new ArrayList();
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        row.Add(Xulydulieu(r.GetValue(i).ToString()));
                    }
                    datalist.Add(row);
                }
            }
            connection.Close();

            return datalist;
        }
        
        // Method để thực thi stored procedure không trả về dữ liệu (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters = null)
        {
            SqlConnection connection = new SqlConnection(connecttionStrings);
            
            SqlCommand command = new SqlCommand(procedureName, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            // Thêm tham số nếu có
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();

            return rowsAffected;
        }
        public string Xulydulieu(string text)
        {
            String s = text.Replace(",", "&44;");
            s = s.Replace("\"", "&34;");
            s = s.Replace("'", "&39;");
            s = s.Replace("\r", "");
            s = s.Replace("\n", "");
            return s;
        }

    }
}