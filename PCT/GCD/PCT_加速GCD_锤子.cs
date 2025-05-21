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
    public class PCT_加速GCD_锤子 : ISlotResolver
    {
        public int Check()
        {

            if (!PictomancerRotationEntry.QT.GetQt(QTKey.锤子))
            {
                return -1;
            }


            if (!Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
            {
                return -3;
            }
            if (!Core.Me.IsMoving() && Helper.自身存在Buff(PCT_Data.Buffs.加速装置) && Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速) >= PCTSettings.Instance.多少层打锤子 && Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速) != 0 && Helper.自身存在Buff大于时间(PCT_Data.Buffs.锤子预备, 10000))
            {
                return -4;
            }

            if (!Core.Me.HasAura(PCT_Data.Buffs.星空))
            {
                return -5;
            }

            if (!Core.Resolve<JobApi_Pictomancer>().生物画 &&
                Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘) != PCT_Data.Spells.动物彩绘 &&
        PictomancerRotationEntry.QT.GetQt(QTKey.动物彩绘) &&
        PictomancerRotationEntry.QT.GetQt(QTKey.死都得画))
            {
                return -7;
            }

            // 风景画判断


            // 武器画相关判断
            if (!Core.Resolve<JobApi_Pictomancer>().武器画 &&
                Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器彩绘) != PCT_Data.Spells.武器彩绘 &&
                PictomancerRotationEntry.QT.GetQt(QTKey.武器彩绘) &&
                PictomancerRotationEntry.QT.GetQt(QTKey.死都得画))
            {
                return -9;
            }



            return 0;
        }

        public void Build(Slot slot)
        {


            var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.锤1));
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
}
