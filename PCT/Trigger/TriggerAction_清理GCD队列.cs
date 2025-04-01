using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.Helper;
using ImGuiNET;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_清理GCD队列 : ITriggerAction
    {
        public string DisplayName { get; } = "清理GCD队列";

        public string Remark { get; set; }

        // 这个行为需要绘制界面元素
        public bool Draw()
        {
            // 添加说明文字
            ImGui.TextWrapped("此触发器行为将在执行时清空高优先级的GCD技能队列。");
            ImGui.TextWrapped("通常用于在特定战斗阶段或需要重置技能序列时使用。");
            return true;
        }

        // 执行清理GCD队列的操作
        public bool Handle()
        {
            LogHelper.Print("TriggerAction: 清理GCD队列执行");

            if (AI.Instance.BattleData != null)
            {
                if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
                {
                    LogHelper.Print("准备清理GCD高优先级队列，包含以下技能：");
                    try
                    {
                        // 使用 SelectMany 获取所有技能实体并记录名称
                        var skillsToClear = AI.Instance.BattleData.HighPrioritySlots_GCD
                            .Where(slot => slot?.Actions != null)
                            .SelectMany(slot => slot.Actions)
                            .Where(action => action?.Spell != null)
                            .Select(action => action.Spell.Name)
                            .ToList();

                        if (skillsToClear.Any())
                        {
                            foreach (var skillName in skillsToClear)
                            {
                                LogHelper.Print($"- {skillName}");
                            }
                        }
                        else
                        {
                            LogHelper.Print("队列中未找到有效的技能实体来记录。");
                        }

                        // 清理队列
                        AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
                        LogHelper.Print("GCD高优先级队列已清理。");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"清理GCD队列并记录技能时发生错误: {ex.Message}");
                        // 即使记录出错，也尝试清理
                        AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
                        LogHelper.Print("尝试清理GCD队列（可能记录不完整）。");
                    }
                }
                else
                {
                    LogHelper.Print("GCD高优先级队列已为空，无需清理。");
                }
                return true;
            }
            else
            {
                LogHelper.Error("无法访问 BattleData，清理GCD队列失败。");
                return false;
            }
        }
    }
}