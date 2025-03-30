using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using Cindy_Master.PCT.Setting;
using System.Numerics;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT起手技开关 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/高级起手控制";

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
        
        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.9f, 0.5f, 0.1f, 1.0f);
        private readonly Vector4 版本100颜色 = new Vector4(0.3f, 0.6f, 0.9f, 1.0f);
        private readonly Vector4 版本90颜色 = new Vector4(0.8f, 0.4f, 0.2f, 1.0f);
        private readonly Vector4 提示颜色 = new Vector4(0.2f, 0.8f, 0.5f, 1.0f);
        private readonly Vector4 选中颜色 = new Vector4(0.1f, 0.9f, 0.3f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);
        
        // 折叠控制变量
        private bool 显示100级设置 = true;
        private bool 显示90级设置 = true;

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
            
            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(5, 5));
            
            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("高级起手技能配置中心");
            ImGui.PopStyleColor();
            ImGui.Separator();
            
            // 100级起手技部分
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 悬停颜色);
            if (ImGui.Button(显示100级设置 ? "收起100级起手设置" : "展开100级起手设置"))
            {
                显示100级设置 = !显示100级设置;
            }
            ImGui.PopStyleColor(2);
            
            if (显示100级设置)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 版本100颜色);
                ImGui.TextWrapped("100级起手技设置");
                ImGui.PopStyleColor();
                
                ImGui.BeginGroup();
                // 100级选项
                ImGui.PushStyleColor(ImGuiCol.CheckMark, 选中颜色);
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.6f));
                
                if (ImGui.Checkbox("3GCD起手", ref Enable3GCDOpener) && Enable3GCDOpener)
                {
                    // 互斥逻辑，选中一个时取消其他
                    Enable2GCDOpener = false;
                    Enable100EdenOpener = false;
                    Enable100轴EdenOpener = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用3GCD的起手战术");
                    ImGui.EndTooltip();
                }
                
                if (ImGui.Checkbox("2GCD起手", ref Enable2GCDOpener) && Enable2GCDOpener)
                {
                    Enable3GCDOpener = false;
                    Enable100EdenOpener = false;
                    Enable100轴EdenOpener = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用2GCD的起手战术");
                    ImGui.EndTooltip();
                }
                
                if (ImGui.Checkbox("100级伊甸起手", ref Enable100EdenOpener) && Enable100EdenOpener)
                {
                    Enable3GCDOpener = false;
                    Enable2GCDOpener = false;
                    Enable100轴EdenOpener = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用100级伊甸副本特化的起手战术");
                    ImGui.EndTooltip();
                }
                
                if (ImGui.Checkbox("100级轴伊甸起手", ref Enable100轴EdenOpener) && Enable100轴EdenOpener)
                {
                    Enable3GCDOpener = false;
                    Enable2GCDOpener = false;
                    Enable100EdenOpener = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用按时间轴设计的100级伊甸起手战术");
                    ImGui.EndTooltip();
                }
                
                ImGui.PopStyleColor(2);
                ImGui.EndGroup();
            }
            
            ImGui.Spacing();
            ImGui.Separator();
            
            // 90级起手技部分
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 悬停颜色);
            if (ImGui.Button(显示90级设置 ? "收起90级起手设置" : "展开90级起手设置"))
            {
                显示90级设置 = !显示90级设置;
            }
            ImGui.PopStyleColor(2);
            
            if (显示90级设置)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 版本90颜色);
                ImGui.TextWrapped("90级起手技设置");
                ImGui.PopStyleColor();
                
                ImGui.BeginGroup();
                // 90级选项
                ImGui.PushStyleColor(ImGuiCol.CheckMark, 选中颜色);
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.4f, 0.25f, 0.2f, 0.6f));
                
                if (ImGui.Checkbox("标准90级起手", ref Enable90Opener) && Enable90Opener)
                {
                    // 互斥逻辑，选中一个时取消其他
                    Enable90OmegaOpener = false;
                    Enable90DragonOpener = false;
                    Enable90OmegaOpenerTest = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用标准的90级起手战术");
                    ImGui.EndTooltip();
                }
                
                if (ImGui.Checkbox("90级欧米茄起手", ref Enable90OmegaOpener) && Enable90OmegaOpener)
                {
                    Enable90Opener = false;
                    Enable90DragonOpener = false;
                    Enable90OmegaOpenerTest = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用90级欧米茄副本特化的起手战术");
                    ImGui.EndTooltip();
                }
                
                if (ImGui.Checkbox("90级龙起手", ref Enable90DragonOpener) && Enable90DragonOpener)
                {
                    Enable90Opener = false;
                    Enable90OmegaOpener = false;
                    Enable90OmegaOpenerTest = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用90级龙副本特化的起手战术");
                    ImGui.EndTooltip();
                }
                
                if (ImGui.Checkbox("90级欧米茄测试起手", ref Enable90OmegaOpenerTest) && Enable90OmegaOpenerTest)
                {
                    Enable90Opener = false;
                    Enable90OmegaOpener = false;
                    Enable90DragonOpener = false;
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用90级欧米茄副本测试版的起手战术");
                    ImGui.EndTooltip();
                }
                
                ImGui.PopStyleColor(2);
                ImGui.EndGroup();
            }
            
            ImGui.Spacing();
            ImGui.Separator();
            
            // 添加提示信息
            ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
            ImGui.TextWrapped("请选择一种起手技配置方案，点击应用按钮保存设置");
            ImGui.PopStyleColor();
            
            ImGui.PopStyleVar(2);

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