using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccelMonitor.App.Models
{
    [Table("DataTable")]
    public class DataTableItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime date_Time { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format("[DataTable: ID={0}, date_Time={1}, Data={2}]", Id, date_Time.ToString(), Data);
        }
    }
}
