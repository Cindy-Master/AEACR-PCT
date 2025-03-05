using AEAssist.Helper;

public static class Config
{
    //public static string Gateway { get; set; } = "https://setu.kaigua.vip";
    public static string Gateway { get; set; } = "http://154.9.252.182:28390";
    public static string BiosId { get; set; } = MacIdHelper.BiosId();
    public static int SeTuNum { get; set; } = 5;
}
