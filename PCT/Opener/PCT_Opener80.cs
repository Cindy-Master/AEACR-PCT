using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using PCT.utils.Helper;

namespace Cindy_Master.PCT.Opener
{
    public class PCT_Opener80 : IOpener
    {
        public int StartCheck()
        {
            if (PCT_Data.Spells.风景构想.GetSpell().Charges < 1)
                return -2;

            if (AI.Instance.BattleData.CurrBattleTimeInMs > 3000)
                return -5;
            if (Core.Me.HasAura(PCT_Data.Buffs.锤子预备))
                return -6;
            return 0;
        }

        public int StopCheck(int index)
        {
            return -1;
        }

        private readonly List<OpenerStep> _steps;

        public List<Action<Slot>> Sequence { get; }

        public PCT_Opener80()
        {
            _steps = new List<OpenerStep>();

            // 条件添加步骤
            if (PictomancerRotationEntry.QT.GetQt(QTKey.爆发药))
            {
                _steps.Add(new OpenerStep(Spell.CreatePotion()));
            }

            // 添加其他步骤
            _steps.AddRange(new List<OpenerStep>
            {
                new OpenerStep(
                    new Spell(PCT_Data.Spells.疾风, SpellTargetType.Target),
                    new Spell(PCT_Data.Spells.武器画构想, SpellTargetType.Target)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.流水, SpellTargetType.Target),
                    new Spell(PCT_Data.Spells.星空, SpellTargetType.Self) { WaitServerAcq = false }
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.锤1, SpellTargetType.Target),
                    new Spell(PCT_Data.Spells.动物构想1, SpellTargetType.Target),
                    new Spell(PCT_Data.Spells.反转, SpellTargetType.Target)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.冰结, SpellTargetType.Target)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.飞石, SpellTargetType.Target)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.闪雷, SpellTargetType.Target)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.锤1, SpellTargetType.Target)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.锤1, SpellTargetType.Target),
                    new Spell(PCT_Data.Spells.即刻, SpellTargetType.Self)
                ),
                new OpenerStep(
                    new Spell(PCT_Data.Spells.动物彩绘2, SpellTargetType.Self),
                    new Spell(PCT_Data.Spells.动物构想2, SpellTargetType.Target),
                    new Spell(PCT_Data.Spells.莫古, SpellTargetType.Target)
                ),
            });

            // 初始化序列
            Sequence = _steps.Select(step => new Action<Slot>(slot => step.Execute(slot))).ToList();
        }

        public Action CompeltedAction { get; set; }

        public void InitCountDown(CountDownHandler countDownHandler)
        {
            Helper.三画(countDownHandler);
            Helper.自动锁目标();
            countDownHandler.AddAction(1800, PCT_Data.Spells.火炎, SpellTargetType.Target);
            Core.Resolve<MemApiChatMessage>().Toast2("80级 起手", 1, 3000);
        }
    }
}
