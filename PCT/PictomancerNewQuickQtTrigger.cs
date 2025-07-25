using AEAssist.CombatRoutine.Trigger;
using Cindy_Master.PCT;
using ImGuiNET;
using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Cindy_Master.Trigger;

/// <summary>
/// 新的QT触发器，提供一个经过美化的快捷开关面板来管理所有QT技能的状态。
/// </summary>
public class PictomancerQuickQtTrigger : ITriggerAction
{
    public string DisplayName { get; } = "PCT/QT快捷开关";

    public string Remark { get; set; }

    public Dictionary<string, bool> QtStates = new();

    private readonly string[] _qtArray;

    public PictomancerQuickQtTrigger()
    {
        _qtArray = PictomancerRotationEntry.QT.GetQtArray();
        foreach (var qtName in _qtArray)
        {
            if (!QtStates.ContainsKey(qtName))
            {
                QtStates[qtName] = false;
            }
        }
    }

    /// <summary>
    /// 绘制经过美化的用户界面（UI）。
    /// </summary>
    public bool Draw()
    {
        // --- 主容器 ---
        ImGui.BeginChild("###TriggerPCT_QuickToggle_Styled", new Vector2(0f, 0f), false, ImGuiWindowFlags.NoScrollbar);

        // --- 整体样式压栈 ---
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 8.0f); // 圆角
        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1.0f); // 边框
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8, 8));

        // --- 1. 顶部控制面板 ---
        DrawControlPanel();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        // --- 2. QT技能开关网格 ---
        DrawQtGrid();

        // --- 样式出栈 ---
        ImGui.PopStyleVar(3);
        ImGui.EndChild();
        return true;
    }

    /// <summary>
    /// 绘制顶部的“一键全开/全关”控制面板
    /// </summary>
    private void DrawControlPanel()
    {
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 5.0f);

        // “一键全开”按钮
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0.4f, 0.2f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.3f, 0.5f, 0.3f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0.4f, 0.6f, 0.4f, 1.0f));
        if (ImGui.Button("  一键全开  ##EnableAll", new Vector2(100, 25)))
        {
            // 使用 ToList() 创建一个键的副本，以安全地修改字典
            foreach (var key in QtStates.Keys.ToList())
            {
                QtStates[key] = true;
            }
        }
        ImGui.PopStyleColor(3);

        ImGui.SameLine();

        // “一键全关”按钮
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.5f, 0.2f, 0.2f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.6f, 0.3f, 0.3f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0.7f, 0.4f, 0.4f, 1.0f));
        if (ImGui.Button("  一键全关  ##DisableAll", new Vector2(100, 25)))
        {
            foreach (var key in QtStates.Keys.ToList())
            {
                QtStates[key] = false;
            }
        }
        ImGui.PopStyleColor(3);

        ImGui.PopStyleVar();
    }

    /// <summary>
    /// 绘制QT技能的网格布局
    /// </summary>
    private void DrawQtGrid()
    {
        int columns = 3; // 3列布局，更紧凑
        ImGui.Columns(columns, "QtButtonsGrid", false);

        // --- 定义按钮颜色 ---
        // 中性背景色
        var buttonBg = new Vector4(0.25f, 0.25f, 0.3f, 0.4f);
        var buttonBgHover = new Vector4(0.35f, 0.35f, 0.4f, 0.6f);
        var buttonBgActive = new Vector4(0.45f, 0.45f, 0.5f, 0.7f);
        // 状态颜色
        var textOn = new Vector4(0.4f, 1.0f, 0.4f, 1.0f); // 开启状态文字颜色 (亮绿色)
        var textOff = new Vector4(0.6f, 0.6f, 0.6f, 1.0f); // 关闭状态文字颜色 (灰色)
        var borderOn = new Vector4(0.4f, 1.0f, 0.4f, 0.7f); // 开启状态边框颜色
        var borderOff = new Vector4(0.4f, 0.4f, 0.4f, 0.5f); // 关闭状态边框颜色

        foreach (var qtName in _qtArray)
        {
            ImGui.PushID(qtName); // 为每个按钮提供独立ID

            bool currentState = QtStates[qtName];
            string statusText = currentState ? "[开]" : "[关]";

            // --- 根据状态选择颜色 ---
            var textColor = currentState ? textOn : textOff;
            var borderColor = currentState ? borderOn : borderOff;

            // --- 压入样式 ---
            ImGui.PushStyleColor(ImGuiCol.Button, buttonBg);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, buttonBgHover);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, buttonBgActive);
            ImGui.PushStyleColor(ImGuiCol.Text, textColor);
            ImGui.PushStyleColor(ImGuiCol.Border, borderColor);

            // 创建按钮
            if (ImGui.Button($"{statusText} {qtName}", new Vector2(ImGui.GetColumnWidth() - 5, 32)))
            {
                QtStates[qtName] = !currentState;
            }

            // --- 弹出样式 ---
            ImGui.PopStyleColor(5);
            ImGui.PopID();

            ImGui.NextColumn();
        }

        ImGui.Columns(1);
    }

    /// <summary>
    /// 处理触发器逻辑
    /// </summary>
    public bool Handle()
    {
        foreach (var qtState in QtStates)
        {
            PictomancerRotationEntry.QT.SetQt(qtState.Key, qtState.Value);
        }
        return true;
    }
}