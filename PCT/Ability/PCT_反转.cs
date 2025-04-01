using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Cindy_Master.PCT.Data;

namespace Cindy_Master.PCT.Ability;

public class PCT_反转 : ISlotResolver
{
    public int Check()
    {
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }

        if (GCDHelper.GetGCDCooldown() < 300)
        {
            return -2;
        }
        if (Core.Resolve<JobApi_Pictomancer>().能量 < 50 && !Core.Me.HasAura(PCT_Data.Buffs.反转预备))
        {
            return -1;
        }
        if ((PCT_Data.Spells.黑彗星).GetSpell().IsReadyWithCanCast() || Core.Me.HasAura(PCT_Data.Buffs.黑彗星buff))
        {
            return -9;
        }
        if (Core.Me.HasAura(PCT_Data.Buffs.长Buff))
        {
            return -3;
        }
        if (GCDHelper.GetGCDCooldown() < 300)
        {
            return -11;
        }
        if (!(PCT_Data.Spells.反转).GetSpell().IsReadyWithCanCast())
        {
            return -7;
        }
        if (PCT_Data.Spells.黑彗星.IsUnlock() && Core.Resolve<JobApi_Pictomancer>().能量 == 100 && !Core.Me.HasAura(PCT_Data.Buffs.星空))
        {
            if ((PCT_Data.Spells.黑彗星).GetSpell().IsReadyWithCanCast())
            {
                return -4;
            }
        }
        if (Core.Resolve<JobApi_Pictomancer>().豆子 == 0 && PCT_Data.Spells.白神圣.IsUnlock())
        {
            return -9;
        }


        if (Core.Me.Level < 60)
        {
            return -4;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.反转))
        {
            return -8;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(PCT_Data.Spells.反转.GetSpell());
    }
}