
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;

namespace Cindy_Master.PCT.Ability
{
    public class PCT_昏乱 : ISlotResolver
    {
        public int Check()
        {
            if (!PictomancerRotationEntry.QT.GetQt(QTKey.自动减伤))
            {
                return -3;
            }
            if (!TargetHelper.targetCastingIsBossAOE(Core.Me.GetCurrTarget(), 500))
            {
                return -1;
            }
            if (GCDHelper.GetGCDCooldown() < 150)
            {
                return -2;
            }
            if (!(PCT_Data.Spells.昏乱).GetSpell().IsReadyWithCanCast())
            {
                return -2;
            }
            if (Core.Me.HasAura(PCT_Data.Buffs.扩散蛋))
            {
                return -2;
            }
            return 0;


        }
        public void Build(Slot slot)
        {
            if (Core.Resolve<MemApiDuty>().InBossBattle)
            {
                Core.Resolve<MemApiChatMessage>().Toast2("AOE要来啦, 启动自动减伤!!", 1, 3000);
            }
            slot.Add(PCT_Data.Spells.昏乱.GetSpell());



        }
    }
}