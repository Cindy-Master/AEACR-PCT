using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT.Data;
using ImGuiNET;
using PCT.utils.Helper;
using System.Numerics;

namespace Cindy_Master.Trigger
{
    public class TriggerCondition_PCT团辅层数 : ITriggerCond
    {
        public string DisplayName { get; } = "PCT/团辅层数监控";

        public string Remark { get; set; }

        public int StackCountUserInput;
        public bool Larger;

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.3f, 0.7f, 0.9f, 1.0f);
        private readonly Vector4 BUFF颜色 = new Vector4(0.8f, 0.6f, 0.1f, 1.0f);
        private readonly Vector4 比较符号颜色 = new Vector4(0.2f, 0.8f, 0.2f, 1.0f);
        private readonly Vector4 选择框颜色 = new Vector4(0.5f, 0.3f, 0.8f, 1.0f);
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
            ImGui.TextWrapped("团辅层数监控");
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
                // BUFF输入部分
                ImGui.BeginGroup();

                // 使用彩色文本显示"加速"
                ImGui.PushStyleColor(ImGuiCol.Text, BUFF颜色);
                ImGui.Text("BUFF: 加速 层数");
                ImGui.PopStyleColor();
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("监控加速BUFF层数");
                    ImGui.EndTooltip();
                }

                ImGui.SameLine();

                // 比较符号
                ImGui.PushStyleColor(ImGuiCol.Text, 比较符号颜色);
                if (Larger)
                {
                    ImGui.Text(">");
                }
                else
                {
                    ImGui.Text("=");
                }
                ImGui.PopStyleColor();

                ImGui.SameLine();

                // 设置输入框样式和宽度
                float 输入框宽度 = ImGui.GetContentRegionAvail().X * 0.5f;
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.2f, 0.3f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.3f, 0.3f, 0.5f, 0.8f));

                ImGui.SetNextItemWidth(输入框宽度);
                ImGuiHelper.LeftInputInt("层数", ref StackCountUserInput, 1);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置需要监测的层数值");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(2);
                ImGui.EndGroup();

                ImGui.Spacing();

                // 比较模式选择
                ImGui.BeginGroup();
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.2f, 0.4f, 0.6f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, 选择框颜色);
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.9f, 0.9f, 0.9f, 1.0f));

                ImGui.Checkbox("比较模式: 勾选为大于(>), 不勾选为等于(=)", ref Larger);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("选择比较方式：勾选时为大于关系，不勾选时为等于关系");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(3);
                ImGui.EndGroup();

                // 添加说明和当前状态
                ImGui.Spacing();
                ImGui.Separator();

                // 获取当前BUFF层数用于显示
                int currentBuffStacks = Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速);

                ImGui.PushTextWrapPos(ImGui.GetContentRegionAvail().X);
                ImGui.TextWrapped($"当前加速BUFF层数: {currentBuffStacks}");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("实时显示当前检测到的加速BUFF层数");
                    ImGui.EndTooltip();
                }

                ImGui.TextWrapped("此触发器用于监测加速BUFF的层数，可配置在达到特定层数或超过特定层数时触发行动。");
                ImGui.PopTextWrapPos();
            }

            ImGui.PopStyleVar(2);

            return true;
        }

        public bool Handle(ITriggerCondParams triggerCondParams)
        {
            // Access the target's buff stack count for the "加速" buff
            int buffStackCount = Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速); // Get stack count for "加速"

            if (Larger)
            {
                if (buffStackCount > StackCountUserInput)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (buffStackCount == StackCountUserInput)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}