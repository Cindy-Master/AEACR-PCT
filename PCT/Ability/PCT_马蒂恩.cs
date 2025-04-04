using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Shiyuvi3._0;
namespace Cindy_Master.PCT.Ability;

public class PCT_马蒂恩 : ISlotResolver
{
    public int Check()
    {
        if (!Core.Resolve<JobApi_Pictomancer>().蔬菜准备)
        {
            return -1;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.马蒂恩))
        {
            return -4;
        }
        if (GCDHelper.GetGCDCooldown() < 300)
        {
            return -2;
        }


        if (Core.Me.HasAura(PCT_Data.Buffs.星空, 15000) && PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源))
        {
            return -1;
        }


        if (!PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell().Cooldown.TotalSeconds <= 40 && (PCT_Data.Spells.风景构想).GetSpell().IsReadyWithCanCast() && PictomancerRotationEntry.QT.GetQt(QTKey.风景构想))
        {
            return -3;
        }
        if (!Core.Me.HasAura(PCT_Data.Buffs.星空) && !PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && PictomancerRotationEntry.QT.GetQt(QTKey.风景构想))
        {
            return -3;
        }
        if (!PCT_Data.Spells.马蒂恩.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }
        if (PCT_Data.Spells.马蒂恩.GetSpell().Charges < 1)
        {
            return -3;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetLineAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, PCT_Data.Spells.马蒂恩);
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.马蒂恩), canTargetObjects));
        }
        else
        {
            slot.Add(PCT_Data.Spells.马蒂恩.GetSpell());
        }

    }
}