using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;
using IBLL;
using System.Reflection;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {

        public void Add(T item)
        {
            using (var db = new DBEntities())
            {
                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "Timestamp")
                    {
                        property.SetValue(item, DateTime.Now);
                    }

                }
                db.Set<T>().Add(item);
                db.SaveChanges();
            
            }
        }
       
        public void Update(T item)
        {
            using (var db = new DBEntities())
            {
                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "Timestamp")
                    {
                        property.SetValue(item, DateTime.Now);
                    }
                    if (property.Name == "IsDeleted")
                    {
                        string flag = property.GetValue(item).ToString();
                    }
                }

                db.Entry(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void Delete(Guid guid)
        {
            using (var db = new DBEntities())
            {

                T item = Find(guid);
                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "IsDeleted")
                    {
                        property.SetValue(item, true);
                    }
                    if (property.Name == "Timestamp")
                    {
                        property.SetValue(item, DateTime.Now);
                    }
                }

                Update(item);
                db.SaveChanges();
            
            }
        }

        public void Delete(T item)
        {
            using (var db = new DBEntities())
            {

                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "IsDeleted")
                    {
                        property.SetValue(item, true);
                    }
                    if (property.Name == "Timestamp")
                    {
                        property.SetValue(item, DateTime.Now);
                    }
                }

                Update(item);
                db.SaveChanges();
            }
        }
        public IEnumerable<T> SqlQuery(string sql)
        {
            using (var db = new DBEntities())
            {
                return db.Database.SqlQuery<T>(sql).ToList();
            }
        }

        public void SqlCommand(string sql)
        {
            using (var db = new DBEntities())
            {
                db.Database.ExecuteSqlCommand(sql);
            }
        }

        public T Find(Guid guid)
        {
            using (var db = new DBEntities())
            {
                return db.Set<T>().Find(guid);
            }
        }


        public DataTable SqlTable(Database database, string sql)
        {

            SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            conn.ConnectionString = database.Connection.ConnectionString;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
           
            conn.Close();//连接需要关闭  
            conn.Dispose();
            return table;
        }



    }
}
