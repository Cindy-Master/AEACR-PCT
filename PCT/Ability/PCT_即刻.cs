using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.GCD;

namespace Cindy_Master.PCT.Ability
{
    public class PCT_即刻 : ISlotResolver
    {
        public int Check()
        {
            if (!PictomancerRotationEntry.QT.GetQt(QTKey.即刻))
            {
                return -3;
            }
            if (!(PCT_Data.Spells.即刻).GetSpell().IsReadyWithCanCast())
            {
                return -6;
            }
            if (Core.Me.HasAura(PCT_Data.Buffs.加速) || Core.Me.HasAura(PCT_Data.Buffs.星空))
            {
                return -7;
            }

            var 动物彩绘Checker = new PCT_动物彩绘();
            var 武器彩绘Checker = new PCT_武器彩绘();
            var 风景彩绘Checker = new PCT_风景彩绘();

            int 动物彩绘Check结果 = 动物彩绘Checker.Check();
            int 武器彩绘Check结果 = 武器彩绘Checker.Check();
            int 风景彩绘Check结果 = 风景彩绘Checker.Check();

            if (动物彩绘Check结果 < 0 && 风景彩绘Check结果 < 0 && 武器彩绘Check结果 < 0)
            {
                return -2;
            }


            return 0;


        }
        public void Build(Slot slot)
        {

            slot.Add(PCT_Data.Spells.即刻.GetSpell());


        }
    }
}