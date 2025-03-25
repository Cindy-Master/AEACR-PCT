using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using Cindy_Master.PCT.Setting;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT动物层数 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/设置动物层数";

        public string Remark { get; set; }

        public int 层数设置;

        public bool Draw()
        {
            ImGui.Text("动物层数设置");
            ImGui.SameLine();
            
            // 从PCTSettings获取当前值作为默认值
            if (层数设置 == 0)
            {
                层数设置 = PCTSettings.Instance.动物层数;
            }
            
            ImGuiHelper.LeftInputInt("层数", ref 层数设置, 1);
            
            ImGui.Text($"当前动物层数设置: {PCTSettings.Instance.动物层数}");

            return true;
        }

        public bool Handle()
        {
            // 更新PCTSettings中的动物层数设置
            PCTSettings.Instance.动物层数 = 层数设置;
            PCTSettings.Instance.Save();
            
            return true;
        }
    }
} 