using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoMartDB
{
    class DBConnect
    {
        private SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-S91CI5I\MSSQLSERVER01;Initial Catalog=GoMartDB1;Integrated Security=True;");


        public SqlConnection Getcon()
        {
            return con;
        }

        public void Opencon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
        public void Closecon()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}

