using AEAssist.CombatRoutine.Trigger;
using Cindy_Master.PCT;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.Trigger;

public class PictomancerQtTrigger : ITriggerAction
{


    private int 当前combo = 0;
    private int radioCheck;

    private int radioType;


    public string ValueName { get; set; } = new("");
    public bool Value { get; set; } = new();
    public string DisplayName => "PCT/QT设置";

    public string Remark { get; set; }

    public bool Draw()
    {
        var qtArray = PictomancerRotationEntry.QT.GetQtArray();
        当前combo = Array.IndexOf(qtArray, ValueName);
        if (当前combo == -1)
        {
            当前combo = 0;
        }

        radioCheck = Value ? 0 : 1;
        //return false;
        if (ImGui.BeginTabBar("###TriggerTab"))
        {
            if (ImGui.BeginTabItem("Reaper"))
            {
                ImGui.BeginChild("###TriggerReaper", new Vector2(0, 0));

                //选择类型
                //ImGui.SetCursorPos(new Vector2(40,10));
                /*ImGui.RadioButton("Qt", ref radioType, 0);
                ImGui.NewLine();

                ImGui.SetCursorPos(new Vector2(0, 40));*/
                if (radioType == 0)
                {
                    ImGui.Combo("Qt开关", ref 当前combo, qtArray, qtArray.Length);
                    ValueName = qtArray[当前combo];
                    ImGui.RadioButton("开", ref radioCheck, 0);
                    ImGui.SameLine();
                    ImGui.RadioButton("关", ref radioCheck, 1);
                    Value = radioCheck == 0;
                }


                ImGui.EndChild();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        return true;
    }

    public bool Handle()
    {
        PictomancerRotationEntry.QT.SetQt(ValueName, Value);
        return true;
    }

    public void Check()
    {
    }
}