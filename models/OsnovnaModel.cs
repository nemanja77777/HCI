using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace ProjekatA.models
{
    public abstract class OsnovnaModel
    {
        public MySqlConnection connection;
        public string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"]?.ConnectionString;

        public OsnovnaModel()
        {
            
             connection = new MySqlConnection(connectionString);

            
            
        }

        // Otvori konekciju
        public void Open()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        // Zatvori konekciju
        public void Close()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        // Apstraktne CRUD metode
        public abstract int Create();
        public abstract object Read(int id);
        public abstract void Update();
        public abstract void Delete(int id);
        public abstract List<object> ReadAll();
    }
}
