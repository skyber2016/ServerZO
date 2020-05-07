namespace Entities
{
    public class logon
    {
		public string time_stamp { get; set; }
		public string account { get; set; }
		public int? account_id { get; set; }
		public int? fee_type { get; set; }
		public string server { get; set; }
		public int? authen { get; set; }
		public string client_ip { get; set; }
		public string fee_account { get; set; }
		public string notify { get; set; }
		public int? rejoin { get; set; }

    }
}
