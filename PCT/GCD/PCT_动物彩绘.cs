using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;

using Cindy_Master.PCT.Setting;

namespace Cindy_Master.PCT.GCD
{
    public class PCT_动物彩绘 : ISlotResolver
    {
        public int Check()
        {
            if (!(PCT_Data.Spells.动物彩绘).IsUnlock())
            {
                return -9;
            }


            if (!PictomancerRotationEntry.QT.GetQt(QTKey.动物彩绘))
            {
                return -7;
            }

            if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), PCTSettings.Instance.TTK阈值, false) && PictomancerRotationEntry.QT.GetQt(QTKey.快死不画))
            {
                return -6;
            }
            if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), PCTSettings.Instance.TTK阈值, true) && PictomancerRotationEntry.QT.GetQt(QTKey.快死不画))
            {
                return -6;
            }
            if (!PCTSettings.Instance.奔放模式)
            {
                if (Core.Me.IsMoving() && !Core.Me.HasAura(167))
                    return -1;
            }
            if (!Core.Me.HasAura(PCT_Data.Buffs.锤子预备, 5000) && Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
            {
                return -9;
            }
            if (PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
            {
                return -13;
            }

            if (Core.Resolve<JobApi_Pictomancer>().生物画)
            {
                return -2;
            }


            var 生物构想Spell = PCT_Data.Spells.动物构想.GetSpell();
            int 生物构想MaxCharges = Core.Resolve<MemApiSpell>().GetMaxCharges(生物构想Spell.Id);
            float 生物构想Charges = Core.Resolve<MemApiSpell>().GetCharges(生物构想Spell.Id);
            double 生物构想Cooldown = 生物构想Spell.Cooldown.TotalSeconds;

            if (生物构想MaxCharges == 2)
            {
                生物构想Cooldown -= 40;
            }
            else if (生物构想MaxCharges == 3)
            {
                生物构想Cooldown -= 80;
            }
            if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘) == PCT_Data.Spells.动物彩绘)
            {
                return -4;
            }
            if (Core.Me.HasAura(PCT_Data.Buffs.星空))
            {
                return -5;
            }
            if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell().Cooldown.TotalSeconds <= 30)
            {
                return 3;
            }
            double userCooldownThreshold = PCTSettings.Instance.动物彩绘CD阈值;
            if (!PictomancerRotationEntry.QT.GetQt(QTKey.优先画画) && 生物构想Cooldown >= userCooldownThreshold && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
            {
                return -7;
            }



            return 0;
        }

        public void Build(Slot slot)
        {


            var spell = GetSpell();
            slot.Add(spell);
            if (PCTSettings.Instance.聊天框提示读条 && !Core.Me.HasAura(167))
            {
                LogHelper.Print("要读条啦");
            }
            if (PCTSettings.Instance.聊天框提示瞬发 && Core.Me.HasAura(167))
            {
                LogHelper.Print("要瞬发啦");
            }
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;

        }

        private Spell GetSpell()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘).GetSpell();
        }
    }
}
