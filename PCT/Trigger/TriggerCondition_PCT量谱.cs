using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.JobApi;
using ImGuiNET;


namespace Cindy_Master.PCT.Trigger
{
    public class TriggerCondition_PCT量谱 : ITriggerCond
    {
        public string DisplayName { get; } = "Cindy-Master-PCT/检测职业量谱";

        public string Remark { get; set; }

        public int BloodUserInput;

        public bool Larger;

        public bool Draw()
        {
            ImGui.Text("能量");
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
            ImGuiHelper.LeftInputInt("能量", ref BloodUserInput, 10);

            ImGui.Checkbox("和指定的能量比较(真:大于,否:等于)", ref Larger);

            return true;
        }

        public bool Handle(ITriggerCondParams triggerCondParams)
        {
            uint realBlood = Core.Resolve<JobApi_Pictomancer>().能量;
            if (Larger)
            {
                if (realBlood > BloodUserInput)
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
                if (realBlood == BloodUserInput)
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
