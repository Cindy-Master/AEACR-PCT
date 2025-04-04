using AEAssist.CombatRoutine.Trigger;
using Cindy_Master.PCT.Setting;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT模式开关 : ITriggerAction
    {
        public string DisplayName { get; } = "PCT/高级模式控制";

        public string Remark { get; set; }

        public bool 奔放模式;
        public bool 日随模式;
        public bool 高难模式;
        public bool 聊天框提示读条;
        public bool 聊天框提示瞬发;
        public bool 高难起手自动锁目标;
        public bool QT重置;

        public bool 初始化完成 = false;

        // 炫酷颜色定义
        private readonly Vector4 标题颜色 = new Vector4(0.9f, 0.5f, 0.1f, 1.0f);
        private readonly Vector4 警告颜色 = new Vector4(1.0f, 0.8f, 0.0f, 1.0f);
        private readonly Vector4 提示颜色 = new Vector4(0.2f, 0.8f, 0.8f, 1.0f);
        private readonly Vector4 模式一颜色 = new Vector4(0.4f, 0.7f, 1.0f, 1.0f);
        private readonly Vector4 模式二颜色 = new Vector4(1.0f, 0.5f, 0.3f, 1.0f);
        private readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.6f, 1.0f, 0.7f);
        private readonly Vector4 悬停颜色 = new Vector4(0.3f, 0.7f, 1.0f, 0.9f);

        // 折叠控制变量
        private bool 显示战斗模式 = true;
        private bool 显示提示选项 = true;

        public bool Draw()
        {
            // 从PCTSettings获取当前值作为默认值
            if (!初始化完成)
            {
                奔放模式 = PCTSettings.Instance.奔放模式;
                日随模式 = PCTSettings.Instance.日随模式;
                高难模式 = PCTSettings.Instance.高难模式;
                聊天框提示读条 = PCTSettings.Instance.聊天框提示读条;
                聊天框提示瞬发 = PCTSettings.Instance.聊天框提示瞬发;
                高难起手自动锁目标 = PCTSettings.Instance.高难起手自动锁目标;
                QT重置 = PCTSettings.Instance.QT重置;
                初始化完成 = true;
            }

            // 添加样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(5, 5));

            // 标题
            ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
            ImGui.TextWrapped("高级模式控制面板");
            ImGui.PopStyleColor();
            ImGui.Separator();

            // 设置面板宽度
            float panelWidth = ImGui.GetContentRegionAvail().X * 0.9f;

            // 奔放模式
            ImGui.BeginGroup();
            ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.5f, 0.9f, 0.5f, 1.0f));
            ImGui.Checkbox("奔放模式", ref 奔放模式);
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text("启用奔放模式，移动施法才开");
                ImGui.EndTooltip();
            }
            ImGui.PopStyleColor();
            ImGui.EndGroup();

            ImGui.Spacing();

            // 战斗模式折叠区域
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 悬停颜色);
            if (ImGui.Button(显示战斗模式 ? "收起战斗模式设置" : "展开战斗模式设置"))
            {
                显示战斗模式 = !显示战斗模式;
            }
            ImGui.PopStyleColor(2);

            if (显示战斗模式)
            {
                // 模式选择区域
                ImGui.BeginGroup();
                ImGui.PushStyleColor(ImGuiCol.Text, 标题颜色);
                ImGui.TextWrapped("战斗模式选择");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.25f, 0.25f, 0.6f, 0.5f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, 模式一颜色);

                // 日随模式的Checkbox，选中时自动取消高难模式
                bool 原日随模式 = 日随模式;
                ImGui.Checkbox("日随模式", ref 日随模式);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("适合日常随机副本的模式，不会执行起手");
                    ImGui.EndTooltip();
                }

                if (原日随模式 != 日随模式 && 日随模式)
                {
                    // 如果日随模式从关闭变为开启，则关闭高难模式
                    高难模式 = false;
                }

                ImGui.PopStyleColor(2);
                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.6f, 0.25f, 0.25f, 0.5f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, 模式二颜色);

                // 高难模式的Checkbox，选中时自动取消日随模式
                bool 原高难模式 = 高难模式;
                ImGui.Checkbox("高难模式", ref 高难模式);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("适合高难度副本的模式，会执行精确的起手");
                    ImGui.EndTooltip();
                }

                if (原高难模式 != 高难模式 && 高难模式)
                {
                    // 如果高难模式从关闭变为开启，则关闭日随模式
                    日随模式 = false;
                }

                ImGui.PopStyleColor(2);
                ImGui.EndGroup();
            }

            ImGui.Spacing();
            ImGui.Separator();

            // 提示选项区域折叠
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 悬停颜色);
            if (ImGui.Button(显示提示选项 ? "收起提示选项设置" : "展开提示选项设置"))
            {
                显示提示选项 = !显示提示选项;
            }
            ImGui.PopStyleColor(2);

            if (显示提示选项)
            {
                ImGui.BeginGroup();
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.TextWrapped("提示与自动化选项");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.3f, 0.8f, 1.0f, 1.0f));
                ImGui.Checkbox("聊天框提示读条", ref 聊天框提示读条);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("在聊天框显示读条技能的提示");
                    ImGui.EndTooltip();
                }

                ImGui.Checkbox("聊天框提示瞬发", ref 聊天框提示瞬发);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("在聊天框显示瞬发技能的提示");
                    ImGui.EndTooltip();
                }

                ImGui.Checkbox("高难起手自动锁目标", ref 高难起手自动锁目标);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("高难模式下自动锁定目标");
                    ImGui.EndTooltip();
                }

                ImGui.Checkbox("QT重置", ref QT重置);
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("重新进入战斗时重置QT");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor();
                ImGui.EndGroup();
            }

            ImGui.Spacing();
            ImGui.Separator();

            // 添加提示信息
            ImGui.PushStyleColor(ImGuiCol.Text, 警告颜色);
            ImGui.TextWrapped("注意：日随模式和高难模式不能同时开启");
            ImGui.PopStyleColor();

            ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
            ImGui.TextWrapped("点击应用按钮保存所有更改");
            ImGui.PopStyleColor();

            ImGui.PopStyleVar(2);

            return true;
        }

        public bool Handle()
        {
            // 更新PCTSettings中的模式开关设置
            PCTSettings.Instance.奔放模式 = 奔放模式;
            PCTSettings.Instance.日随模式 = 日随模式;
            PCTSettings.Instance.高难模式 = 高难模式;
            PCTSettings.Instance.聊天框提示读条 = 聊天框提示读条;
            PCTSettings.Instance.聊天框提示瞬发 = 聊天框提示瞬发;
            PCTSettings.Instance.高难起手自动锁目标 = 高难起手自动锁目标;
            PCTSettings.Instance.QT重置 = QT重置;
            PCTSettings.Instance.Save();

            return true;
        }
    }
}