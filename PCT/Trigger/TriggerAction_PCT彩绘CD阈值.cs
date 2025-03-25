using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using Cindy_Master.PCT.Setting;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT彩绘CD阈值 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/设置彩绘CD阈值";

        public string Remark { get; set; }

        public float 风景彩绘CD阈值;
        public float 动物彩绘CD阈值;
        public float 武器彩绘CD阈值;
        
        public bool 初始化完成 = false;

        public bool Draw()
        {
            // 从PCTSettings获取当前值作为默认值
            if (!初始化完成)
            {
                风景彩绘CD阈值 = (float)PCTSettings.Instance.风景彩绘CD阈值;
                动物彩绘CD阈值 = (float)PCTSettings.Instance.动物彩绘CD阈值;
                武器彩绘CD阈值 = (float)PCTSettings.Instance.武器彩绘CD阈值;
                初始化完成 = true;
            }
            
            ImGui.Text("彩绘CD阈值设置");
            
            ImGui.Text("风景彩绘CD阈值");
            ImGui.SameLine();
            ImGuiHelper.LeftInputFloat("秒##风景", ref 风景彩绘CD阈值, 0.5f);
            
            ImGui.Text("动物彩绘CD阈值");
            ImGui.SameLine();
            ImGuiHelper.LeftInputFloat("秒##动物", ref 动物彩绘CD阈值, 0.5f);
            
            ImGui.Text("武器彩绘CD阈值");
            ImGui.SameLine();
            ImGuiHelper.LeftInputFloat("秒##武器", ref 武器彩绘CD阈值, 0.5f);
            
            ImGui.Text($"当前设置: 风景={PCTSettings.Instance.风景彩绘CD阈值}秒, 动物={PCTSettings.Instance.动物彩绘CD阈值}秒, 武器={PCTSettings.Instance.武器彩绘CD阈值}秒");

            return true;
        }

        public bool Handle()
        {
            // 更新PCTSettings中的彩绘CD阈值设置
            PCTSettings.Instance.风景彩绘CD阈值 = 风景彩绘CD阈值;
            PCTSettings.Instance.动物彩绘CD阈值 = 动物彩绘CD阈值;
            PCTSettings.Instance.武器彩绘CD阈值 = 武器彩绘CD阈值;
            PCTSettings.Instance.Save();
            
            return true;
        }
    }
} 