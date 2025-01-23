namespace Infrastructures.Domains.Models.CheckInOut;

public class CheckInOutRecord
{
    public int UserId { get; set; }
    public DateTime CheckTime { get; set; }
    public string CheckType { get; set; }
    public int VerifyCode { get; set; }
    public int SensorId { get; set; }
    public string MemoInfo { get; set; }
    public string WorkCode { get; set; }
    public string Sn { get; set; }
    public int UserExtFmt { get; set; }
}