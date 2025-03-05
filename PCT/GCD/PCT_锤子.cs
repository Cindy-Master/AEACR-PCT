﻿using AEAssist;
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

public class PCT_锤子 : ISlotResolver
{
    public int Check()
    {
        var 风景构想Spell = Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell();
        double 风景构想Cooldown = 风景构想Spell.Cooldown.TotalSeconds;

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.锤子))
        {
            return -8;
        }



        if (!Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
        {
            return -1;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && !Core.Me.IsMoving() && Helper.自身存在Buff大于时间(PCT_Data.Buffs.锤子预备, 15000) && 风景构想Cooldown >= 20 && !Core.Me.HasAura(PCT_Data.Buffs.星空) && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
        {
            return -6;
        }

        if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), 10000, true) && TargetHelper.IsBoss(GameObjectExtension.GetCurrTarget(Core.Me)) && PictomancerRotationEntry.QT.GetQt(QTKey.快死不画))
        {
            return 1;
        }


        if ((!Core.Resolve<JobApi_Pictomancer>().生物画 || !Core.Resolve<JobApi_Pictomancer>().风景画 || (!Core.Resolve<JobApi_Pictomancer>().武器画 && !Core.Me.HasAura(PCT_Data.Buffs.锤子预备))) && PictomancerRotationEntry.QT.GetQt(QTKey.优先画画))
        {
            return -2;
        }

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && 风景构想Cooldown <= 15 && SpellExtension.IsUnlock(PCT_Data.Spells.风景构想) && Core.Me.HasAura(PCT_Data.Buffs.锤子预备, 10000) && PictomancerRotationEntry.QT.GetQt(QTKey.风景构想) && PictomancerRotationEntry.QT.GetQt(QTKey.爆发) && Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.锤子预备) == 3 && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
        {
            return -3;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.锤1)));
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.锤1), canTargetObjects));
        }
        else
        {
            slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.锤1).GetSpell());
        }
        if (PCTSettings.Instance.聊天框提示瞬发)
        {
            LogHelper.Print("要瞬发啦");
        }
    }

}