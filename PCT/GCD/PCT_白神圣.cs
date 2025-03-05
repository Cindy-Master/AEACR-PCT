using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Shiyuvi3._0;

namespace Cindy_Master.PCT.GCD
{
    public class PCT_白神圣 : ISlotResolver
    {
        public int Check()
        {

            if (!PictomancerRotationEntry.QT.GetQt(QTKey.白神圣))
            {
                return -8;
            }

            if (Core.Resolve<JobApi_Pictomancer>().豆子 == 0)
            {
                return -1;
            }

            if (!Core.Me.IsMoving() && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
            {
                return -2;
            }
            if (Core.Me.HasAura(PCT_Data.Buffs.黑彗星buff))
            {
                return -2;
            }

            return 0;
        }

        public void Build(Slot slot)
        {
            var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, PCT_Data.Spells.白神圣);
            if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
            {
                slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.白神圣), canTargetObjects));
            }
            else
            {
                slot.Add(PCT_Data.Spells.白神圣.GetSpell());
            }
        }

    }
}
