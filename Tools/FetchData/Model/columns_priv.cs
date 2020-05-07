using System;

namespace Entities
{
    public class columns_priv
    {
		public string Host { get; set; }
		public string Db { get; set; }
		public string User { get; set; }
		public string Table_name { get; set; }
		public string Column_name { get; set; }
		public DateTime? Timestamp { get; set; }

    }
}
