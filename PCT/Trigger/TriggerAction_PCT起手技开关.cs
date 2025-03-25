using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using Cindy_Master.PCT.Setting;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT起手技开关 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/设置起手技开关";

        public string Remark { get; set; }

        public bool Enable3GCDOpener;
        public bool Enable2GCDOpener;
        public bool Enable90Opener;
        public bool Enable90OmegaOpener;
        public bool Enable90DragonOpener;
        public bool Enable90OmegaOpenerTest;
        public bool Enable100EdenOpener;
        public bool Enable100轴EdenOpener;
        
        public bool 初始化完成 = false;

        public bool Draw()
        {
            // 从PCTSettings获取当前值作为默认值
            if (!初始化完成)
            {
                Enable3GCDOpener = PCTSettings.Instance.Enable3GCDOpener;
                Enable2GCDOpener = PCTSettings.Instance.Enable2GCDOpener;
                Enable90Opener = PCTSettings.Instance.Enable90Opener;
                Enable90OmegaOpener = PCTSettings.Instance.Enable90OmegaOpener;
                Enable90DragonOpener = PCTSettings.Instance.Enable90DragonOpener;
                Enable90OmegaOpenerTest = PCTSettings.Instance.Enable90OmegaOpenerTest;
                Enable100EdenOpener = PCTSettings.Instance.Enable100EdenOpener;
                Enable100轴EdenOpener = PCTSettings.Instance.Enable100轴EdenOpener;
                初始化完成 = true;
            }
            
            ImGui.Text("起手技开关设置");
            
            ImGui.Checkbox("启用3GCD起手", ref Enable3GCDOpener);
            ImGui.Checkbox("启用2GCD起手", ref Enable2GCDOpener);
            ImGui.Checkbox("启用90版本起手", ref Enable90Opener);
            ImGui.Checkbox("启用90版本欧米茄起手", ref Enable90OmegaOpener);
            ImGui.Checkbox("启用90版本龙起手", ref Enable90DragonOpener);
            ImGui.Checkbox("启用90版本欧米茄测试起手", ref Enable90OmegaOpenerTest);
            ImGui.Checkbox("启用100版本伊甸起手", ref Enable100EdenOpener);
            ImGui.Checkbox("启用100轴伊甸起手", ref Enable100轴EdenOpener);
            
            ImGui.Text("点击应用按钮保存所有更改");

            return true;
        }

        public bool Handle()
        {
            // 更新PCTSettings中的起手技开关设置
            PCTSettings.Instance.Enable3GCDOpener = Enable3GCDOpener;
            PCTSettings.Instance.Enable2GCDOpener = Enable2GCDOpener;
            PCTSettings.Instance.Enable90Opener = Enable90Opener;
            PCTSettings.Instance.Enable90OmegaOpener = Enable90OmegaOpener;
            PCTSettings.Instance.Enable90DragonOpener = Enable90DragonOpener;
            PCTSettings.Instance.Enable90OmegaOpenerTest = Enable90OmegaOpenerTest;
            PCTSettings.Instance.Enable100EdenOpener = Enable100EdenOpener;
            PCTSettings.Instance.Enable100轴EdenOpener = Enable100轴EdenOpener;
            PCTSettings.Instance.Save();
            
            return true;
        }
    }
} 