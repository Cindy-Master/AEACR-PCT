using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using Cindy_Master.PCT.Data;
using ImGuiNET;
using PCT.utils.Helper;

namespace Cindy_Master.Trigger
{
    public class TriggerCondition_PCT团辅层数 : ITriggerCond
    {
        public string DisplayName { get; } = "Cindy-Master-BuffStack/检测目标BUFF层数";

        public string Remark { get; set; }

        public int StackCountUserInput;

        public bool Larger;

        public bool Draw()
        {
            ImGui.Text("BUFF: 加速 层数");
            ImGui.SameLine();
            if (Larger)
            {
                ImGui.Text(">");
            }
            else
            {
                ImGui.Text("=");
            }
            ImGui.SameLine();
            ImGuiHelper.LeftInputInt("BUFF层数", ref StackCountUserInput, 1);

            ImGui.Checkbox("和指定的BUFF层数比较(真:大于, 否:等于)", ref Larger);

            return true;
        }

        public bool Handle(ITriggerCondParams triggerCondParams)
        {
            // Access the target's buff stack count for the "加速" buff

            int buffStackCount = Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速); // Get stack count for "加速"

            if (Larger)
            {
                if (buffStackCount > StackCountUserInput)
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
                if (buffStackCount == StackCountUserInput)
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
