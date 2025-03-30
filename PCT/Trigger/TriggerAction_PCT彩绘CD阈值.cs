using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT.Setting;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT彩绘CD阈值 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/彩绘CD阈值设置";

        public string Remark { get; set; }

        public float 风景彩绘CD阈值;
        public float 动物彩绘CD阈值;
        public float 武器彩绘CD阈值;

        public bool 初始化完成 = false;
        private readonly Vector4 标题颜色 = new Vector4(0.9f, 0.5f, 0.1f, 1.0f);
        private readonly Vector4 当前设置颜色 = new Vector4(0.2f, 0.8f, 0.8f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);

        private bool 展开设置 = true;

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

            // 使用样式和颜色
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(5, 5));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("彩绘CD阈值高级设置");
            ImGui.PopStyleColor();
            ImGui.Separator();

            // 折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 悬停颜色);
            if (ImGui.Button(展开设置 ? "收起设置" : "展开设置"))
            {
                展开设置 = !展开设置;
            }
            ImGui.PopStyleColor(2);

            if (展开设置)
            {
                float spacing = ImGui.GetStyle().ItemInnerSpacing.X;
                float width = ImGui.GetContentRegionAvail().X * 0.6f;

                // 风景彩绘设置
                ImGui.Text("风景彩绘CD阈值");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置风景彩绘的冷却时间阈值，高于此值不会自动使用");
                    ImGui.EndTooltip();
                }
                ImGui.SameLine();
                ImGui.SetNextItemWidth(width);
                ImGuiHelper.LeftInputFloat("风景", ref 风景彩绘CD阈值, 0.5f);

                // 动物彩绘设置
                ImGui.Text("动物彩绘CD阈值");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置动物彩绘的冷却时间阈值，高于此值不会自动使用");
                    ImGui.EndTooltip();
                }
                ImGui.SameLine();
                ImGui.SetNextItemWidth(width);
                ImGuiHelper.LeftInputFloat("动物", ref 动物彩绘CD阈值, 0.5f);

                // 武器彩绘设置
                ImGui.Text("武器彩绘CD阈值");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("设置武器彩绘的冷却时间阈值，高于此值不会自动使用");
                    ImGui.EndTooltip();
                }
                ImGui.SameLine();
                ImGui.SetNextItemWidth(width);
                ImGuiHelper.LeftInputFloat("武器", ref 武器彩绘CD阈值, 0.5f);

                ImGui.Spacing();
                ImGui.Separator();

                // 当前设置显示
                ImGui.PushStyleColor(ImGuiCol.Text, 当前设置颜色);
                ImGui.TextWrapped($"当前设置: 风景={PCTSettings.Instance.风景彩绘CD阈值}秒 | 动物={PCTSettings.Instance.动物彩绘CD阈值}秒 | 武器={PCTSettings.Instance.武器彩绘CD阈值}秒");
                ImGui.PopStyleColor();
            }

            ImGui.PopStyleVar(2);

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