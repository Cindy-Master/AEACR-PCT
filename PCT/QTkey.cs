namespace Cindy_Master.PCT
{
    public static class QTKey
    {
        // Existing QT Key
        public const string 爆发药 = "爆发药";

        // Add QT Keys for all skills
        public const string 自动绘画 = "自动绘画";
        public const string 基础连 = "基础连";
        public const string 锤子 = "锤子";
        public const string 白神圣 = "白神圣";
        public const string 黑彗星 = "黑彗星";
        public const string 反转 = "反转";
        public const string 莫古 = "莫古";
        public const string 马蒂恩 = "马蒂恩";
        public const string 锤1 = "锤1";
        public const string 锤2 = "锤2";
        public const string 锤3 = "锤3";
        public const string 天星 = "天星";
        public const string 彩虹 = "彩虹";
        public const string 动物彩绘 = "动物彩绘";
        public const string 武器彩绘 = "武器彩绘";
        public const string 风景彩绘 = "风景彩绘";
        public const string 动物构想 = "动物构想";
        public const string 武器构想 = "武器构想";
        public const string 风景构想 = "风景构想";
        public const string 即刻 = "即刻";
        public const string 自动减伤 = "自动减伤";
        public const string 快死不画 = "快死不画";
        public const string 自动停手 = "自动停手";
        public const string 智能AOE = "智能AOE";
        public const string 团辅期乱打 = "团辅期乱打";
        public const string 爆发 = "爆发";
        public const string 测112 = "112(危)";
        public const string 优先画画 = "优先画画";
        public const string 倾泻资源 = "倾泻资源";

    }

    public static class QT
    {
        public static bool QTGET(string qtName) => PictomancerRotationEntry.QT.GetQt(qtName);
        public static bool QTSET(string qtName, bool qtValue) => PictomancerRotationEntry.QT.SetQt(qtName, qtValue);
    }
}
