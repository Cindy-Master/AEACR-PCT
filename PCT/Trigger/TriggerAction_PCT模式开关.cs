using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using Cindy_Master.PCT.Setting;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_PCT模式开关 : ITriggerAction
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/设置模式开关";

        public string Remark { get; set; }

        public bool 奔放模式;
        public bool 日随模式;
        public bool 高难模式;
        public bool 聊天框提示读条;
        public bool 聊天框提示瞬发;
        public bool 高难起手自动锁目标;
        public bool QT重置;
        
        public bool 初始化完成 = false;

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
            
            ImGui.Text("模式开关设置");
            
            ImGui.Checkbox("奔放模式", ref 奔放模式);
            
            // 日随模式的Checkbox，选中时自动取消高难模式
            bool 原日随模式 = 日随模式;
            ImGui.Checkbox("日随模式", ref 日随模式);
            if (原日随模式 != 日随模式 && 日随模式)
            {
                // 如果日随模式从关闭变为开启，则关闭高难模式
                高难模式 = false;
            }
            
            // 高难模式的Checkbox，选中时自动取消日随模式
            bool 原高难模式 = 高难模式;
            ImGui.Checkbox("高难模式", ref 高难模式);
            if (原高难模式 != 高难模式 && 高难模式)
            {
                // 如果高难模式从关闭变为开启，则关闭日随模式
                日随模式 = false;
            }
            
            ImGui.Checkbox("聊天框提示读条", ref 聊天框提示读条);
            ImGui.Checkbox("聊天框提示瞬发", ref 聊天框提示瞬发);
            ImGui.Checkbox("高难起手自动锁目标", ref 高难起手自动锁目标);
            ImGui.Checkbox("QT重置", ref QT重置);
            
            // 添加提示信息
            ImGui.TextColored(new System.Numerics.Vector4(1, 1, 0, 1), "注意：日随模式和高难模式不能同时开启");
            ImGui.Text("点击应用按钮保存所有更改");

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