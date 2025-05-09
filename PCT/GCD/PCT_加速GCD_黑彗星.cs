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

            int buffStacks = Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.锤子预备);

            int timeThreshold = 10000;

            if (buffStacks == 3)
            {
                timeThreshold = 10000; // 3层时，阈值为 10000ms
            }
            else if (buffStacks == 2)
            {
                timeThreshold = 7000;  // 2层时，阈值为 7000ms (假设 7 代表 7 秒)

            }
            else if (buffStacks == 1)
            {
                timeThreshold = 5000;  // 1层时，阈值为 5000ms (假设 5 代表 5 秒)
            }

            // 如果满足特定层数条件，并且自身存在Buff大于对应的时间阈值
            if (!Helper.自身存在Buff大于时间(PCT_Data.Buffs.锤子预备, timeThreshold) && Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
            {
                return -14;
            }

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

            if (!Core.Me.HasAura(PCT_Data.Buffs.加速装置) && Helper.自身存在Buff大于时间(PCT_Data.Buffs.星空, 7000))
            {
                return -2;
            }
            if ((PCT_Data.Spells.黑彗星).GetSpell().IsReadyWithCanCast() && (PCT_Data.Spells.反转).GetSpell().IsReadyWithCanCast() && !Core.Me.HasAura(PCT_Data.Buffs.长Buff) && PictomancerRotationEntry.QT.GetQt(QTKey.反转))
            {
                return 4;
            }
            if (!Core.Me.IsMoving() && Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速) > 2 && Helper.自身存在Buff大于时间(PCT_Data.Buffs.星空, 5000))
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
