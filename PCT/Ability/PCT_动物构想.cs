using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using Shiyuvi3._0;

namespace Cindy_Master.PCT.Ability;

public class PCT_动物构想 : ISlotResolver
{
    public int Check()
    {
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.动物构想))
        {
            return -8;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }
        if (GCDHelper.GetGCDCooldown() < 600)
        {
            return -2;
        }
        var 生物构想Spell = PCT_Data.Spells.动物构想.GetSpell();
        int 生物构想MaxCharges = Core.Resolve<MemApiSpell>().GetMaxCharges(生物构想Spell.Id);
        float 生物构想Charges = Core.Resolve<MemApiSpell>().GetCharges(生物构想Spell.Id);
        double 生物构想Cooldown = 1.7;

        if (生物构想MaxCharges == 2)
        {
            生物构想Cooldown = 1.6;
        }
        else if (生物构想MaxCharges == 3)
        {
            生物构想Cooldown = 2.6;
        }
        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell().Cooldown.TotalSeconds <= 10 && (PCT_Data.Spells.风景构想).GetSpell().IsUnlock() && !PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && PictomancerRotationEntry.QT.GetQt(QTKey.风景构想))
        {
            return -3;
        }

        int requiredLayer = PCTSettings.Instance.动物层数 + 1;
        if (生物构想Charges < requiredLayer - 0.2 && !Core.Me.HasAura(PCT_Data.Buffs.星空) && !PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && PictomancerRotationEntry.QT.GetQt(QTKey.风景构想))
        {
            return -6;
        }
        if (!Core.Resolve<JobApi_Pictomancer>().生物画)
        {
            return -1;
        }
        if (!Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想).GetSpell().IsReadyWithCanCast())
        {
            return -4;
        }

        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想).GetSpell().Charges < 1)
        {
            return -4;
        }

        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想) == PCT_Data.Spells.动物构想2 && (PCT_Data.Spells.马蒂恩).GetSpell().IsReadyWithCanCast())
        {
            return -12;
        }
        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想) == PCT_Data.Spells.动物构想4 && (PCT_Data.Spells.莫古).GetSpell().IsReadyWithCanCast())
        {
            return -11;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想));
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想), canTargetObjects));
        }
        else
        {
            slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物构想).GetSpell());
        }

    }
}