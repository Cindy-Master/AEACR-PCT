using AEAssist.CombatRoutine.Trigger;
using AEAssist.Helper;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.Trigger
{
    public class TriggerCondition_爆发药状态 : ITriggerCond
    {
        public string DisplayName { get; } = "爆发药状态检测";

        public string Remark { get; set; }

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.3f, 0.7f, 0.9f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);

        private bool 显示设置 = true;

        public bool Draw()
        {
            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(5, 5));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("爆发药状态监控");
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
                // 获取当前爆发药状态用于显示
                bool currentPotionState = ItemHelper.CheckCurrJobPotion();

                ImGui.PushTextWrapPos(ImGui.GetContentRegionAvail().X);
                ImGui.TextWrapped($"当前爆发药状态: {(currentPotionState ? "可用" : "不可用")}");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("实时显示当前爆发药是否可用");
                    ImGui.EndTooltip();
                }

                ImGui.TextWrapped("此触发器用于监测爆发药是否可用，当爆发药可用时触发行动。");
                ImGui.PopTextWrapPos();
            }

            ImGui.PopStyleVar(2);

            return true;
        }

        public bool Handle(ITriggerCondParams triggerCondParams)
        {
            // 检查爆发药是否可用
            return ItemHelper.CheckCurrJobPotion();
        }
    }
}