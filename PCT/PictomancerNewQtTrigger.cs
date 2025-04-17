using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT;
using ImGuiNET;
using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Cindy_Master.Trigger;

public class PictomancerNewQtTrigger : ITriggerAction
{
    public string DisplayName { get; } = "PCT/QT设置(新)";

    public string Remark { get; set; }

    public List<PictomancerQtSetting> QTList = new();

    public bool Draw()
    {
        ImGui.BeginChild("###TriggerPCT", new Vector2(0f, 0f));

        // 美化左侧菜单区域
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(10, 6));
        ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8, 10));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(12, 12));
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarSize, 14.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarRounding, 9.0f);

        // 左侧菜单栏样式
        ImGui.PushStyleColor(ImGuiCol.Header, new Vector4(0.25f, 0.35f, 0.55f, 0.9f));
        ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new Vector4(0.35f, 0.45f, 0.65f, 0.9f));
        ImGui.PushStyleColor(ImGuiCol.HeaderActive, new Vector4(0.45f, 0.55f, 0.75f, 1.0f));

        // 菜单分割线
        ImGui.PushStyleColor(ImGuiCol.Separator, new Vector4(0.4f, 0.5f, 0.6f, 0.5f));

        // 滚动条样式
        ImGui.PushStyleColor(ImGuiCol.ScrollbarBg, new Vector4(0.15f, 0.15f, 0.2f, 0.6f));
        ImGui.PushStyleColor(ImGuiCol.ScrollbarGrab, new Vector4(0.3f, 0.4f, 0.6f, 0.8f));
        ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabHovered, new Vector4(0.4f, 0.5f, 0.7f, 0.8f));
        ImGui.PushStyleColor(ImGuiCol.ScrollbarGrabActive, new Vector4(0.5f, 0.6f, 0.8f, 1.0f));

        ImGuiHelper.DrawSplitList("QT开关", QTList, DrawHeader, AddCallBack, DrawCallback);

        ImGui.PopStyleColor(8);
        ImGui.PopStyleVar(6);
        ImGui.EndChild();
        return true;
    }

    public bool Handle()
    {
        foreach (var qtSetting in QTList)
        {
            qtSetting.action();
        }
        return true;
    }

    private PictomancerQtSetting DrawCallback(PictomancerQtSetting arg)
    {
        arg.draw();
        return arg;
    }

    private string DrawHeader(PictomancerQtSetting arg)
    {
        string statusText = arg.Value ? "[开]" : "[关]";
        Vector4 statusColor = arg.Value ? new Vector4(0.2f, 0.8f, 0.2f, 1.0f) : new Vector4(0.8f, 0.2f, 0.2f, 1.0f);

        ImGui.PushStyleColor(ImGuiCol.Text, statusColor);
        string result = $"{statusText} {arg.Key}";
        ImGui.PopStyleColor();

        return result;
    }

    private PictomancerQtSetting AddCallBack()
    {
        return new PictomancerQtSetting();
    }
}

public class PictomancerQtSetting
{
    public string Key = "默认";
    public bool Value = false;
    private int combo;
    private bool toggleValue;

    // 图绘世界使的QT列表
    private readonly string[] _qtArray = PictomancerRotationEntry.QT.GetQtArray();

    public PictomancerQtSetting()
    {
    }

    // 自定义滑动开关控件
    private bool ToggleSwitch(string label, ref bool value, float width = 60.0f, float height = 24.0f)
    {
        bool changed = false;
        float radius = height * 0.5f;

        Vector2 pos = ImGui.GetCursorScreenPos();
        ImDrawListPtr drawList = ImGui.GetWindowDrawList();

        float switchWidth = width;
        float switchHeight = height;

        // 样式定义
        Vector4 bgColor = value
            ? new Vector4(0.2f, 0.6f, 0.2f, 1.0f)  // 开启背景色
            : new Vector4(0.65f, 0.2f, 0.2f, 1.0f); // 关闭背景色

        Vector4 knobColor = new Vector4(0.95f, 0.95f, 0.95f, 1.0f); // 滑块颜色
        Vector4 frameColor = new Vector4(0.2f, 0.2f, 0.2f, 0.5f);   // 边框颜色

        // 计算滑块位置
        float knobRadius = radius - 2;
        float knobX = value ? pos.X + width - radius : pos.X + radius;

        // 交互区域
        ImGui.InvisibleButton(label, new Vector2(switchWidth, switchHeight));
        bool hovered = ImGui.IsItemHovered();

        // 记录状态变化
        if (ImGui.IsItemClicked())
        {
            value = !value;
            changed = true;
        }

        // 绘制背景框
        uint bgColorU = ImGui.ColorConvertFloat4ToU32(hovered
            ? new Vector4(bgColor.X * 1.1f, bgColor.Y * 1.1f, bgColor.Z * 1.1f, bgColor.W)
            : bgColor);

        // 绘制圆角背景
        drawList.AddRectFilled(pos, new Vector2(pos.X + width, pos.Y + height),
            bgColorU, radius);

        // 绘制滑块
        uint knobColorU = ImGui.ColorConvertFloat4ToU32(knobColor);
        drawList.AddCircleFilled(new Vector2(knobX, pos.Y + radius), knobRadius, knobColorU);

        // 绘制文字


        // 重置光标位置
        ImGui.SetCursorScreenPos(new Vector2(pos.X, pos.Y + switchHeight + 2));

        return changed;
    }

    public void draw()
    {
        combo = Array.IndexOf(_qtArray, Key);
        if (combo == -1)
        {
            combo = 0;
        }

        toggleValue = Value;

        // 美化布局
        ImGui.BeginGroup();
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(10, 6));
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 6.0f);

        // 删除内部嵌套框架，直接在外框显示内容
        float comboWidth = ImGui.GetContentRegionMax().X - 32;

        // 标题
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0.8f, 0.85f, 1.0f, 1.0f));
        ImGui.PushFont(ImGui.GetFont());
        ImGui.Text("QT 技能设置");
        ImGui.PopFont();
        ImGui.PopStyleColor();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        // 下拉框样式
        ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.22f, 0.3f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new Vector4(0.25f, 0.27f, 0.35f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new Vector4(0.3f, 0.32f, 0.4f, 1.0f));
        ImGui.PushStyleColor(ImGuiCol.PopupBg, new Vector4(0.18f, 0.18f, 0.25f, 0.95f));

        // 控件布局
        ImGui.Text("选择技能:");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(comboWidth * 0.7f);
        ImGui.Combo("##QtSelect", ref combo, _qtArray, _qtArray.Length);

        ImGui.PopStyleColor(4);
        ImGui.Spacing();
        ImGui.Spacing();

        // 添加滑动开关
        ImGui.Text("技能状态:");
        ImGui.SameLine(90);

        // 使用自定义滑动开关，缩短宽度
        if (ToggleSwitch("##QtToggle", ref toggleValue, 60, 30))
        {
            // 开关状态改变时的操作
        }

        ImGui.Spacing();

        // 添加分隔线和状态说明
        ImGui.Separator();
        ImGui.Spacing();

        // 状态指示文本
        ImGui.PushStyleColor(ImGuiCol.Text, toggleValue
            ? new Vector4(0.2f, 0.8f, 0.2f, 1.0f)
            : new Vector4(0.8f, 0.2f, 0.2f, 1.0f));

        string statusInfo = toggleValue
            ? "当前状态: 启用"
            : "当前状态: 禁用";

        ImGui.Text(statusInfo);
        ImGui.PopStyleColor();

        ImGui.PopStyleVar(2);
        ImGui.EndGroup();

        Key = _qtArray[combo];
        Value = toggleValue;
    }

    public void action()
    {
        // 设置图绘世界使的QT
        PictomancerRotationEntry.QT.SetQt(Key, Value);
    }
}