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
        public string Xulydulieu( string text){
            String s = text.Replace(",", "&44;");
            s = s.Replace("\"","&34;");
            s = s.Replace("'", "&39;");
            s = s.Replace("\r", "");
            s = s.Replace("\n", "");
            return s;
        }

    }
}