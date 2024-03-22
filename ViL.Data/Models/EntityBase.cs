using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ViL.Data.Models
{
    public abstract class EntityBase
    {
        public int Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public EntityBase(int status = 1, string creaeteBy = "", string updateBy = "")
        {
            Status = status;
            CreateDate = DateTime.Now;
            CreateBy = creaeteBy;
            UpdateDate = DateTime.Now;
            UpdateBy = updateBy;
        }

        public virtual PropertyInfo[] GetKeys()
        {
            var type = this.GetType();
            var properties = type.GetProperties()
                                .Where(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)))
                                .ToArray();
            return properties;
        }

        public virtual PropertyInfo[] GetProperties()
        {
            var type = this.GetType();
            var properties = type.GetProperties().ToArray();
            return properties;
        }
    }
}
