namespace ViL.Api.Models
{
    public class BanedInfo
    {
        //public DateTime ExpriedDate { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }

        public BanedInfo()
        {
            //ExpriedDate = DateTime.Now.AddDays(1);
            Duration = 1;
            Reason = string.Empty;
        }

        public BanedInfo(int duration, string reason)
        {
            //ExpriedDate = expriedDate;
            Duration = duration;
            Reason = reason;
        }
    }
}
