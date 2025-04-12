using AEAssist.CombatRoutine.Trigger;
using Cindy_Master.PCT.Setting;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT快死不画阈值 : ITriggerAction
    {
        public string DisplayName { get; } = "PCT/快死不画TTK阈值";

        public string Remark { get; set; }

        public int TTK阈值;
        public bool 初始化完成 = false;

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.9f, 0.5f, 0.1f, 1.0f);
        private readonly Vector4 提示颜色 = new Vector4(0.2f, 0.8f, 0.5f, 1.0f);
        private readonly Vector4 默认阈值颜色 = new Vector4(0.4f, 0.6f, 0.9f, 1.0f);
        private readonly Vector4 滑块颜色 = new Vector4(0.2f, 0.7f, 0.4f, 1.0f);
        private readonly Vector4 重置按钮颜色 = new Vector4(0.9f, 0.4f, 0.2f, 0.7f);
        private readonly Vector4 重置按钮悬停颜色 = new Vector4(1.0f, 0.5f, 0.3f, 0.9f);

        public bool Draw()
        {
            // 从PCTSettings获取当前TTK阈值作为默认值
            if (!初始化完成)
            {
                TTK阈值 = PCTSettings.Instance.TTK阈值;
                初始化完成 = true;
            }

            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(5, 5));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("快死不画TTK阈值设置");
            ImGui.PopStyleColor();
            ImGui.Separator();

            // 说明信息
            ImGui.TextWrapped("此设置控制当目标血量即将耗尽时是否停止使用彩绘技能的判断阈值。");
            ImGui.TextWrapped("TTK（Time To Kill）是指预计击杀目标所需的剩余时间（毫秒）。");

            ImGui.Spacing();

            // 当前默认值提示
            ImGui.PushStyleColor(ImGuiCol.Text, 默认阈值颜色);
            ImGui.TextWrapped($"默认阈值: 15000毫秒（15秒）");
            ImGui.PopStyleColor();

            ImGui.Spacing();

            // TTK阈值滑动条
            ImGui.PushStyleColor(ImGuiCol.SliderGrab, 滑块颜色);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.2f, 0.3f, 0.6f));
            ImGui.PushItemWidth(300);
            ImGui.SliderInt("阈值（毫秒）", ref TTK阈值, 5000, 20000, "%d ms");
            ImGui.PopItemWidth();
            ImGui.PopStyleColor(2);

            ImGui.Spacing();

            // 重置按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 重置按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 重置按钮悬停颜色);
            if (ImGui.Button("重置为默认值"))
            {
                TTK阈值 = 15000;
            }
            ImGui.PopStyleColor(2);

            ImGui.Spacing();
            ImGui.Separator();

            // 添加提示信息
            ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
            ImGui.TextWrapped("当目标的TTK低于设定阈值时，如果启用了 快死不画 QT，将会停止对该目标使用彩绘技能。");
            ImGui.TextWrapped("适当调整这个值可以避免在BOSS即将死亡时浪费彩绘时间");
            ImGui.PopStyleColor();

            ImGui.PopStyleVar(2);

            return true;
        }

        public bool Handle()
        {
            // 更新PCTSettings中的TTK阈值设置
            PCTSettings.Instance.TTK阈值 = TTK阈值;
            PCTSettings.Instance.Save();

            return true;
        }
    }
}
