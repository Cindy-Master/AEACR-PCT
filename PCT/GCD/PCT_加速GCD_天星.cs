using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using PCT.utils.Helper;
using Shiyuvi3._0;

namespace Cindy_Master.PCT.GCD;

public class PCT_加速GCD_天星 : ISlotResolver
{
    public int Check()
    {

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.天星))
        {
            return -8;
        }

        if (!Core.Me.HasAura(PCT_Data.Buffs.星空))
        {
            return -2;
        }
        if (!Core.Me.HasAura(PCT_Data.Buffs.天星预备))
        {
            return -1;
        }
        if (!Core.Me.HasAura(PCT_Data.Buffs.加速装置) && (Core.Me.HasAura(PCT_Data.Buffs.锤子预备) || Core.Resolve<JobApi_Pictomancer>().武器画) && Core.Me.HasAura(PCT_Data.Buffs.加速))
        {
            return -4;

            //秒天星的检测卡住  千万别改
        }
        if (Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速) >= 3 && (Core.Me.HasAura(PCT_Data.Buffs.锤子预备) || Core.Resolve<JobApi_Pictomancer>().武器画))
        {
            return -5;
        }
        if (Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速) == 0)
        {
            return -6;
        }



        return 0;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.天星));
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.天星), canTargetObjects));
        }
        else
        {
            slot.Add(PCT_Data.Spells.天星.GetSpell());
        }

        if (PCTSettings.Instance.聊天框提示瞬发)
        {
            LogHelper.Print("要瞬发啦");
        }
    }

}