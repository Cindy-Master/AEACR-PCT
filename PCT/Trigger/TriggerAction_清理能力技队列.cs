using AEAssist.CombatRoutine.Module; // 添加 System 用于 Exception
using AEAssist.CombatRoutine.Trigger;
using AEAssist.Helper;
using ImGuiNET;

namespace Cindy_Master.PCT.Trigger
{
    public class TriggerAction_清理能力技队列 : ITriggerAction
    {
        public string DisplayName { get; } = "清理能力技队列";

        public string Remark { get; set; }

        // 这个行为需要绘制界面元素
        public bool Draw()
        {
            // 添加说明文字
            ImGui.TextWrapped("此触发器行为将在执行时清空高优先级的能力技能（oGCD）队列。");
            ImGui.TextWrapped("这有助于确保在需要时不会意外触发队列中的旧能力技。");
            return true;
        }

        // 执行清理能力技队列的操作
        public bool Handle()
        {
            LogHelper.Print("TriggerAction: 清理能力技队列执行");

            if (AI.Instance.BattleData != null)
            {
                if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
                {
                    LogHelper.Print("准备清理能力技高优先级队列，包含以下技能：");
                    try // 添加 try-catch 以防 SelectMany 或访问 Spell.Name 出错
                    {
                        // 使用 SelectMany 获取所有技能实体并记录名称
                        var skillsToClear = AI.Instance.BattleData.HighPrioritySlots_OffGCD
                            .Where(slot => slot?.Actions != null) // 过滤掉空的 slot 或 Actions
                            .SelectMany(slot => slot.Actions)
                            .Where(action => action?.Spell != null) // 过滤掉空的 action 或 spell
                            .Select(action => action.Spell.Name)
                            .ToList(); // 转换成列表以便记录

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
                        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                        LogHelper.Print("能力技高优先级队列已清理。");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error($"清理能力技队列并记录技能时发生错误: {ex.Message}");
                        // 即使记录出错，也尝试清理
                        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                        LogHelper.Print("尝试清理能力技队列（可能记录不完整）。");
                    }
                }
                else
                {
                    LogHelper.Print("能力技高优先级队列已为空，无需清理。");
                }
                return true;
            }
            else
            {
                LogHelper.Error("无法访问 BattleData，清理能力技队列失败。");
                return false;
            }
        }
    }
}