using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using PCT.utils.Helper;
using Shiyuvi3._0;

namespace Cindy_Master.PCT.GCD
{
    public class PCT_加速GCD_基础连长 : ISlotResolver
    {
        public int Check()
        {

            if (!PictomancerRotationEntry.QT.GetQt(QTKey.基础连))
            {
                return -1;
            }
            if (PictomancerRotationEntry.QT.GetQt(QTKey.团辅期乱打))
            {
                return -2;
            }

            if (!Core.Me.HasAura(PCT_Data.Buffs.长Buff))
            {
                return -3;
            }
            if (!Core.Me.HasAura(PCT_Data.Buffs.加速) || !Core.Me.HasAura(PCT_Data.Buffs.星空))
            {
                return -5;
            }
            if (!PCTSettings.Instance.奔放模式)
            {
                if (Core.Me.IsMoving() && !Core.Me.HasAura(167))
                {
                    return -13;
                }
            }
            if (Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.长Buff) <= 2 && PictomancerRotationEntry.QT.GetQt(QTKey.测112))
            {
                return -3;
            }

            return 0;
        }

        public void Build(Slot slot)
        {

            var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.冰冻));
            if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
            {
                slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.冰冻), canTargetObjects));
            }
            else
            {
                slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.冰结).GetSpell());
            }

            if (PCTSettings.Instance.聊天框提示读条)
            {
                LogHelper.Print("要读条啦");
            }

        }

    }
}
