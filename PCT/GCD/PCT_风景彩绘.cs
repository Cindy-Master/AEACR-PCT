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
    public class PCT_风景彩绘 : ISlotResolver
    {
        public int Check()
        {
            if (!(PCT_Data.Spells.风景彩绘).IsUnlock())
            {

                return -9;
            }

            if (!PictomancerRotationEntry.QT.GetQt(QTKey.风景彩绘))
            {
                return -7;
            }
            if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), 10000, true) && TargetHelper.IsBoss(GameObjectExtension.GetCurrTarget(Core.Me)) && PictomancerRotationEntry.QT.GetQt(QTKey.快死不画))
            {
                return -6;
            }
            if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget()))
            {
                return -3;
            }

            if (!PCTSettings.Instance.奔放模式)
            {
                if (Core.Me.IsMoving() && !Core.Me.HasAura(167))
                    return -1;
            }
            if (Core.Resolve<JobApi_Pictomancer>().风景画)
            {
                return -2;
            }

            var 风景构想Spell = Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell();
            double 风景构想Cooldown = 风景构想Spell.Cooldown.TotalSeconds;

            double userCooldownThreshold = PCTSettings.Instance.风景彩绘CD阈值;

            if (!PictomancerRotationEntry.QT.GetQt(QTKey.优先画画) && 风景构想Cooldown >= userCooldownThreshold && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
            {
                return -3;
            }
            if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景彩绘) == PCT_Data.Spells.风景彩绘)
            {
                return -4;
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
        }

        private Spell GetSpell()
        {
            return Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景彩绘).GetSpell();
        }
    }
}
