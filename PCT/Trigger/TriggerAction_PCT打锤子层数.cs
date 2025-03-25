using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT.Setting;
using ImGuiNET;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT打锤子层数 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/设置打锤子层数";

        public string Remark { get; set; }

        public int 打锤子层数;

        public bool 初始化完成 = false;

        public bool Draw()
        {
            // 从PCTSettings获取当前值作为默认值
            if (!初始化完成)
            {
                打锤子层数 = PCTSettings.Instance.多少层打锤子;
                初始化完成 = true;
            }

            ImGui.Text("加速剩多少层打锤子设置");
            ImGui.SameLine();

            ImGuiHelper.LeftInputInt("层数", ref 打锤子层数, 1);

            ImGui.Text($"当前设置: {PCTSettings.Instance.多少层打锤子}层");
            ImGui.Text("设置为0表示不使用此功能");

            return true;
        }

        public bool Handle()
        {
            // 更新PCTSettings中的打锤子层数设置
            PCTSettings.Instance.多少层打锤子 = 打锤子层数;
            PCTSettings.Instance.Save();

            return true;
        }
    }
}