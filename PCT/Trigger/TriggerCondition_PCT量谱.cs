using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.JobApi;
using ImGuiNET;
using System.Numerics;


namespace Cindy_Master.PCT.Trigger
{
    public class TriggerCondition_PCT量谱 : ITriggerCond
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/颜料监控";

        public string Remark { get; set; }

        public int BloodUserInput;

        public bool Larger;

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.8f, 0.4f, 0.9f, 1.0f);
        private readonly Vector4 能量颜色 = new Vector4(1.0f, 0.6f, 0.1f, 1.0f);
        private readonly Vector4 比较符号颜色 = new Vector4(0.2f, 0.9f, 0.5f, 1.0f);
        private readonly Vector4 模式颜色 = new Vector4(0.4f, 0.7f, 1.0f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);

        private bool 显示设置 = true;

        public bool Draw()
        {
            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 8.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(5, 5));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("颜料监控");
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
                // 美化能量输入部分
                ImGui.BeginGroup();

                // 使用彩色文本显示"能量"
                ImGui.PushStyleColor(ImGuiCol.Text, 能量颜色);
                ImGui.Text("能量");
                ImGui.PopStyleColor();
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置能量值监测条件");
                    ImGui.EndTooltip();
                }

                ImGui.SameLine();

                // 使用彩色文本显示比较符号
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

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.2f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.3f, 0.3f, 0.6f, 0.8f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.4f, 0.4f, 0.7f, 0.9f));

                ImGui.SetNextItemWidth(输入框宽度);
                ImGuiHelper.LeftInputInt("能量值", ref BloodUserInput, 10);

                ImGui.PopStyleColor(3);
                ImGui.EndGroup();

                ImGui.Spacing();

                // 美化比较模式选择
                ImGui.BeginGroup();
                ImGui.PushStyleColor(ImGuiCol.Text, 模式颜色);
                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.9f, 0.4f, 0.8f, 1.0f));
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.6f));

                ImGui.Checkbox("和指定的能量比较(勾选:大于, 不勾选:等于)", ref Larger);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("勾选时为大于关系(>)，不勾选时为等于关系(=)");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(3);
                ImGui.EndGroup();

                // 添加说明
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.PushTextWrapPos(ImGui.GetContentRegionAvail().X);
                ImGui.TextWrapped("此触发器用于监测职业能量值，可以设置在能量达到特定值或大于特定值时触发动作。");
                ImGui.PopTextWrapPos();
            }

            ImGui.PopStyleVar(2);

            return true;
        }

        public bool Handle(ITriggerCondParams triggerCondParams)
        {
            uint realBlood = Core.Resolve<JobApi_Pictomancer>().能量;
            if (Larger)
            {
                if (realBlood > BloodUserInput)
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
                if (realBlood == BloodUserInput)
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
