using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;

namespace Cindy_Master.PCT.Ability;

public class PCT_武器构想 : ISlotResolver
{
    public int Check()
    {

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.武器构想))
        {
            return -8;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }
        if (!Core.Resolve<JobApi_Pictomancer>().武器画)
        {
            return -1;
        }
        if (Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
        {
            return -6;
        }
        if (GCDHelper.GetGCDCooldown() < 600)
        {
            return -2;
        }
        if (!(PCT_Data.Spells.武器画构想).GetSpell().IsReadyWithCanCast())
        {
            return -7;
        }

        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器构想).GetSpell().Charges < 1)
        {
            return -3;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器构想).GetSpell());
    }
}