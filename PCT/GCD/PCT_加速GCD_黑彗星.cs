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

namespace Cindy_Master.PCT.GCD
{
    public class PCT_加速GCD_黑彗星 : ISlotResolver
    {
        public int Check()
        {

            if (!PictomancerRotationEntry.QT.GetQt(QTKey.黑彗星))
            {
                return -8;
            }
            if (PictomancerRotationEntry.QT.GetQt(QTKey.团辅期乱打))
            {
                return -8;
            }

            if (!Core.Me.HasAura(PCT_Data.Buffs.黑彗星buff))
            {
                return -1;
            }
            if (Core.Resolve<JobApi_Pictomancer>().豆子 == 0)
            {
                return -1;
            }
            if (!Core.Me.HasAura(PCT_Data.Buffs.加速) || !Core.Me.HasAura(PCT_Data.Buffs.星空))
            {
                return -2;
            }

            if (!Core.Me.HasAura(PCT_Data.Buffs.加速装置))
            {
                return -2;
            }
            if ((PCT_Data.Spells.黑彗星).GetSpell().IsReadyWithCanCast() && (PCT_Data.Spells.反转).GetSpell().IsReadyWithCanCast() && !Core.Me.HasAura(PCT_Data.Buffs.长Buff) && PictomancerRotationEntry.QT.GetQt(QTKey.反转))
            {
                return 4;
            }
            if (!Core.Me.IsMoving() && Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速) > 2)
            {
                return -13;
            }


            return 0;
        }

        public void Build(Slot slot)
        {


            var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.黑彗星));
            if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
            {
                slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.黑彗星), canTargetObjects));
            }
            else
            {
                slot.Add(PCT_Data.Spells.黑彗星.GetSpell());
            }
            if (PCTSettings.Instance.聊天框提示瞬发)
            {
                LogHelper.Print("要瞬发啦");
            }
        }

    }
}
