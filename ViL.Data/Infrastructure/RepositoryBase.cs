using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using ViL.Common.Commons;
using ViL.Data.Models;

namespace ViL.Data.Infrastructure
{
    public class RepositoryBase<T> where T : EntityBase
    {
        protected ViLDbContext _context;
        protected DbSet<T> dbset;

        public RepositoryBase(ViLDbContext context)
        {
            _context = context;
            this.dbset = _context.Set<T>();
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.dbset;
            }
        }

        public virtual void Add(T entity)
        {
            if (!typeof(T).IsDefined(typeof(HasNoKey), false))
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        dbset.Add(entity);
                        _context.SaveChanges();
                        transaction.Commit();
                    } catch 
                    {
                        transaction.Rollback();
                    }
                    
                }
            } else
            {
                var storeName = "GenericInsert";
                var tableName = typeof(T).Name;
                List<string> cols = new List<string>();
                List<string> values = new List<string>();
                foreach(var prop in entity.GetType().GetProperties())
                {
                    cols.Add(prop.Name);
                    if (prop.PropertyType == typeof(string))
                    {
                        values.Add($"N'{prop.GetValue(entity)}'");
                    } else
                    {
                        values.Add($"'{prop.GetValue(entity)}'");
                    }
                }
                var dataCols = string.Join(",", cols);
                var dataValues = string.Join(",", values);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var commandString = $"EXEC {storeName} @tablename, @datacol, @data";
                        var tableNameParam = new SqlParameter("@tablename", SqlDbType.VarChar) { Value = tableName };
                        var dataColsParam = new SqlParameter("@datacol", SqlDbType.NVarChar) { Value = dataCols };
                        var dataValuesParam = new SqlParameter("@data", SqlDbType.NVarChar) { Value = dataValues };

                        _context.Database.ExecuteSqlRaw(commandString, tableNameParam, dataColsParam, dataValuesParam);
                        _context.SaveChanges();
                        transaction.Commit();
                    } catch 
                    {
                        transaction.Rollback();
                    }
                    
                }
            }
        }

        public virtual void Update(T entity)
        {
            if (!typeof(T).IsDefined(typeof(HasNoKey), false))
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    
                    try
                    {
                        _context.ChangeTracker.Clear();
                        dbset.Attach(entity);
                        _context.Entry(entity).State = EntityState.Modified;
                        _context.SaveChanges();
                        transaction.Commit();
                    } catch
                    {
                        transaction.Rollback();
                    }
                    
                }
            } else
            {
                var storename = "GenericUpdate";
                var tableName = typeof(T).Name;
                List<string> data = new List<string>();
                List<string> ids = new List<string>();
                entity.UpdateDate = DateTime.Now;
                foreach (var prop in entity.GetType().GetProperties())
                {
                    if (prop.Name.Contains("Id"))
                    {
                        ids.Add($"{prop.Name} = N'{prop.GetValue(entity)}'");
                    } else if (!prop.PropertyType.IsDefined(typeof(VilUnchanged), false))
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            data.Add($"{prop.Name}=N'{prop.GetValue(entity)}'");
                        } else
                        {
                            data.Add($"{prop.Name}='{prop.GetValue(entity)}'");
                        }
                    }
                }

                var dataString = string.Join(",", data);
                var condition = string.Join(" AND ", ids);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var commandString = $"EXEC {storename} @tablename, @data, @where";
                        var tableNameParam = new SqlParameter("@tablename", SqlDbType.VarChar) { Value = tableName };
                        var dataParam = new SqlParameter("@data", SqlDbType.NVarChar) { Value = dataString };
                        var whereParam = new SqlParameter("@where", SqlDbType.NVarChar) { Value = condition };

                        _context.Database.ExecuteSqlRaw(commandString, tableNameParam, dataParam, whereParam);
                        _context.SaveChanges();
                        transaction.Commit();
                    } catch 
                    {
                        transaction.Rollback();
                    }
                    
                }
            }
        }

        public virtual void Delete(T entity)
        {

            if (!typeof(T).IsDefined(typeof(HasNoKey), false))
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        dbset.Remove(entity);
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            else
            {
                var storename = "GenericDelete";
                var tableName = typeof(T).Name;

                var whereList = new List<string>();
                var propasKey = entity.GetKeys();
                foreach (var prop in propasKey)
                {
                    whereList.Add($"{prop.Name} = {prop.GetValue(entity)}");
                }
                var where = string.Join(" AND ", whereList);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var commandString = $"EXEC {storename} @tablename, @where";
                        var tableNameParam = new SqlParameter("@tablename", SqlDbType.VarChar) { Value = tableName };
                        var whereParam = new SqlParameter("@where", SqlDbType.NVarChar) { Value = where };

                        _context.Database.ExecuteSqlRaw(commandString, tableNameParam, whereParam);
                        _context.SaveChanges();
                        transaction.Commit();
                    } catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }  
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            if (!typeof(T).IsDefined(typeof(HasNoKey), false))
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var objects = dbset.Where<T>(where);
                        foreach (T entity in objects)
                        {
                            dbset.Remove(entity);
                        }
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }

                }
            }  else
            {
                var objects = dbset.Where<T>(where);
                var storename = "GenericDelete";
                var tableName = typeof(T).Name;

                var conditionList = new List<string>();
                
                foreach (var entity in objects)
                {
                    var whereList = new List<string>();
                    var keys = entity.GetKeys();
                    foreach (var prop in keys)
                    {
                        whereList.Add($"({prop.Name} = N'{prop.GetValue(entity)}')");
                    }
                    var newCondition = $"{string.Join(" AND ", whereList)}";
                    conditionList.Add(newCondition);
                }
                    var condition = string.Join(" OR ", conditionList);

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var commandString = $"EXEC {storename} @tablename, @where";
                        var tableNameParam = new SqlParameter("@tablename", SqlDbType.VarChar) { Value = tableName };
                        var whereParam = new SqlParameter("@where", SqlDbType.NVarChar) { Value = condition };

                        _context.Database.ExecuteSqlRaw(commandString, tableNameParam, whereParam);
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public virtual T? GetById(string id)
        {
            return dbset.Find(id);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
