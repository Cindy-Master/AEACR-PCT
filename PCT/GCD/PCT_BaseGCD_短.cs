﻿using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using PCT.utils.Helper;
using Shiyuvi3._0;


namespace Cindy_Master.PCT.GCD;

public class PCT_BaseGCD_短 : ISlotResolver
{
    public int Check()
    {
        if (!(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.火炎)).GetSpell().IsReadyWithCanCast())
        {
            return -2;
        }

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.基础连))
        {
            return -8;
        }
        if (!PCTSettings.Instance.奔放模式)
        {
            if (Core.Me.IsMoving() && !Core.Me.HasAura(167))
            {
                return -1;
            }
        }

        if (Helper.上一个能力技能() == PCT_Data.Spells.星空 && SpellExtension.IsUnlock(PCT_Data.Spells.风景构想) && PCTSettings.Instance.高难模式)
        {
            return -3;
        }

        return 0;
    }


    private Spell GetSpell()
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(4, Core.Me.GetCurrTarget(), 25f, 5f, Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.烈炎));
        if (canTargetObjects != null && PCT_Data.Spells.烈炎.IsUnlock() && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            return new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.烈炎), canTargetObjects);

        }
        else
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.火炎).GetSpell();
        }



    }
    public void Build(Slot slot)
    {
        var spell = GetSpell();
        slot.Add(spell);
        if (PCTSettings.Instance.聊天框提示读条)
        {
            LogHelper.Print("要读条啦");
        }

    }

}
