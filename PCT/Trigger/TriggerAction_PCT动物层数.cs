using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT.Setting;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT动物层数 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/动物层数优化";

        public string Remark { get; set; }

        public int 层数设置;
        public bool 初始化完成 = false;

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.7f, 0.5f, 0.9f, 1.0f);
        private readonly Vector4 数值颜色 = new Vector4(0.2f, 0.9f, 0.6f, 1.0f);
        private readonly Vector4 动物图标颜色 = new Vector4(1.0f, 0.7f, 0.2f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);

        private bool 显示设置 = true;

        public bool Draw()
        {
            // 从PCTSettings获取当前值作为默认值
            if (!初始化完成)
            {
                层数设置 = PCTSettings.Instance.动物层数;
                初始化完成 = true;
            }

            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 8.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(6, 6));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("动物层数智能控制系统");
            ImGui.PopStyleColor();
            ImGui.Separator();

            // 折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 悬停颜色);
            if (ImGui.Button(显示设置 ? "收起设置" : "展开设置"))
            {
                显示设置 = !显示设置;
            }
            ImGui.PopStyleColor(2);

            if (显示设置)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 动物图标颜色);
                ImGui.Text("动物层数优化设置");
                ImGui.PopStyleColor();
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置至少保留多少层动物层数，低于此值不会自动使用，影响战斗策略");
                    ImGui.EndTooltip();
                }
                ImGui.SameLine();

                // 设置面板宽度和样式
                float 面板宽度 = ImGui.GetContentRegionAvail().X * 0.4f;
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.3f, 0.2f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.4f, 0.3f, 0.6f, 0.8f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.5f, 0.4f, 0.7f, 0.9f));

                ImGui.SetNextItemWidth(面板宽度);
                ImGuiHelper.LeftInputInt("层数", ref 层数设置, 1);
                ImGui.PopStyleColor(3);

                ImGui.Spacing();
                ImGui.Separator();

                // 当前设置显示
                ImGui.PushStyleColor(ImGuiCol.Text, 数值颜色);
                ImGui.TextWrapped($"当前动物层数设置: {PCTSettings.Instance.动物层数} 层");
                ImGui.PopStyleColor();

                // 添加描述性说明
                ImGui.PushTextWrapPos(ImGui.GetContentRegionAvail().X);
                ImGui.TextWrapped("动物层数控制会影响您的战斗策略和风格。请根据您的战斗需求调整至最佳值。");
                ImGui.PopTextWrapPos();
            }

            ImGui.PopStyleVar(2);

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