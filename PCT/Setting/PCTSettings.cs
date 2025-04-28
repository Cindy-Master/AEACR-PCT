using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;
using System.Numerics;

namespace Cindy_Master.PCT.Setting
{
    public class PCTSettings
    {
        public static PCTSettings Instance;

        #region 标准模板代码 可以直接复制后改掉类名即可
        private static string path;
        public static void Build(string settingPath)
        {
            path = Path.Combine(settingPath, nameof(PCTSettings) + ".json");
            if (!File.Exists(path))
            {
                Instance = new PCTSettings();
                Instance.Save();
                return;
            }
            try
            {
                Instance = JsonHelper.FromJson<PCTSettings>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                Instance = new PCTSettings();
                LogHelper.Error(e.ToString());
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonHelper.ToJson(this));
        }
        #endregion
        public Dictionary<string, bool> QtStates { get; set; } = new Dictionary<string, bool>();

        public List<string> QtUnVisibleList = [];

        public void SaveQtStates(JobViewWindow jobViewWindow)
        {
            // 获取所有 Qt 控件的名称列表
            string[] qtArray = jobViewWindow.GetQtArray();

            // 遍历所有控件名称，获取对应的状态，并保存到字典中
            foreach (var qtName in qtArray)
            {
                bool qtState = jobViewWindow.GetQt(qtName);
                QtStates[qtName] = qtState;
            }



            // 保存当前设置到 JSON 文件
            Save();
        }

        public void LoadQtStates(JobViewWindow jobViewWindow)
        {
            // 加载保存的所有Qt状态
            foreach (var qtState in QtStates)
            {
                jobViewWindow.SetQt(qtState.Key, qtState.Value);
            }


            // 根据 QtUnVisibleList 设置对应的QT为不可见
            foreach (var hiddenQt in QtUnVisibleList)
            {
                QtUnVisibleList.Add(hiddenQt);
            }
        }



        public JobViewSave JobViewSave = new() { MainColor = new Vector4(139 / 255f, 0 / 255f, 0 / 255f, 1.0f) }; // QT设置存档
        public bool Enable3GCDOpener { get; set; } = true;
        public bool Enable2GCDOpener { get; set; } = false;
        public bool Enable90Opener { get; set; } = true;
        public bool Enable90OmegaOpener { get; set; } = false;
        public bool Enable90DragonOpener { get; set; } = false;
        public bool Enable90OmegaOpenerTest { get; set; } = false;
        public bool Enable100EdenOpener { get; set; } = false;
        public bool Enable100轴EdenOpener { get; set; } = false;
        public bool Enable100FastOpener { get; set; } = false;
        public bool 聊天框提示读条 { get; set; } = false;
        public bool 聊天框提示瞬发 { get; set; } = false;
        public bool 高难起手自动锁目标 { get; set; } = false;
        public int 多少层打锤子 { get; set; } = 0;
        public bool QT重置 { get; set; } = false;
        public double 风景彩绘CD阈值 { get; set; } = 60.0;
        public double 动物彩绘CD阈值 { get; set; } = 15.0;
        public double 武器彩绘CD阈值 { get; set; } = 30.0;
        public int TTK阈值 { get; set; } = 15000;


        public int 动物层数 { get; set; } = 1;

        public bool 奔放模式 = false;
        public bool 日随模式 = true;  // 日随模式，默认启用
        public bool 高难模式 = false; // 高难模式，默认禁用


    }
}