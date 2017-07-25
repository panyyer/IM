using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

namespace newQQ_Server
{
    /*
     * TODO:Change to connection pool 
     */
    class DBHelper
    {

        private static String host = "需要自己输入";
        private static String username = "需要自己输入";
        private static String password = "需要自己输入";
        private static String database = "需要自己输入";
        private static String M_str_sqlcon = "server=" + host
            + ";user id=" + username
            + ";password="
            + password
            + ";database="
            + database
            + ";";
        private static MySqlConnection conn = new MySqlConnection(M_str_sqlcon);
        //private static MySqlDataReader read;

        public static MySqlDataReader read(String sql)
        {
            if (null != conn && conn.State.ToString() != "Open")
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                   // MessageBox.Show(ex.Message+"c");
                }
            }
            //new MySqlCommand(sql, conn).ExecuteReader();
            return new MySqlCommand(sql, conn).ExecuteReader();
        }

        public static int nonQuery(String sql)
        {
            if (null != conn && conn.State.ToString() != "Open")
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                  //  MessageBox.Show(ex.Message+"dd");
                }
            }
            return new MySqlCommand(sql, conn).ExecuteNonQuery();
        }
    }
}
