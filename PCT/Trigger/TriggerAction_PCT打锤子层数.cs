using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT.Setting;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT打锤子层数 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/锤子层数控制";

        public string Remark { get; set; }

        public int 打锤子层数;

        public bool 初始化完成 = false;

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.9f, 0.3f, 0.1f, 1.0f);
        private readonly Vector4 数值颜色 = new Vector4(0.2f, 0.8f, 0.8f, 1.0f);
        private readonly Vector4 提示颜色 = new Vector4(0.7f, 0.7f, 0.2f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);

        private bool 显示设置 = true;

        public bool Draw()
        {
            // 从PCTSettings获取当前值作为默认值
            if (!初始化完成)
            {
                打锤子层数 = PCTSettings.Instance.多少层打锤子;
                初始化完成 = true;
            }

            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 8.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(6, 6));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("加速层数智能控制系统");
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
                // 设置面板宽度
                float 面板宽度 = ImGui.GetContentRegionAvail().X * 0.7f;

                ImGui.BeginGroup();
                ImGui.Text("加速剩余层数打锤子");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置在爆发期间，当加速BUFF剩余多少层时使用锤子");
                    ImGui.EndTooltip();
                }
                ImGui.SameLine();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.2f, 0.3f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.3f, 0.3f, 0.6f, 0.8f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.4f, 0.4f, 0.8f, 0.9f));
                ImGui.SetNextItemWidth(面板宽度 * 0.3f);
                ImGuiHelper.LeftInputInt("层数", ref 打锤子层数, 1);
                ImGui.PopStyleColor(3);
                ImGui.EndGroup();

                ImGui.Spacing();
                ImGui.Separator();

                // 当前设置显示
                ImGui.BeginGroup();
                ImGui.PushStyleColor(ImGuiCol.Text, 数值颜色);
                ImGui.TextWrapped($"当前设置: {PCTSettings.Instance.多少层打锤子}层");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.TextWrapped("设置为0表示没有加速装置的时候打锤子 (此设置对于移动/出团辅圈等情况的自动切锤子无效)");
                ImGui.PopStyleColor();
                ImGui.EndGroup();
            }

            ImGui.PopStyleVar(2);

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