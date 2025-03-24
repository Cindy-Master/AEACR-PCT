using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using PCT.utils.Helper;

namespace Cindy_Master.PCT.Ability;

public class PCT_风景构想 : ISlotResolver
{

    public int Check()
    {

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.风景构想))
        {
            return -8;

        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }
        if (Helper.自身是否在移动() && !TargetHelper.IsBoss(GameObjectExtension.GetCurrTarget(Core.Me)))
        {
            return -9;
        }
        if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget()))
        {
            return -3;
        }
        if (!Core.Resolve<JobApi_Pictomancer>().风景画)
        {
            return -1;
        }
        if (!(PCT_Data.Spells.星空).GetSpell().IsReadyWithCanCast())
        {
            return -7;
        }


        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell().Charges < 1)
        {
            return -3;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        Core.Resolve<MemApiChatMessage>().Toast2("色魔纹!! 开!!!", 1, 3000);


        slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.星空), Core.Me));
    }
}