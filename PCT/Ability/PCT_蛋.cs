
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;

namespace Cindy_Master.PCT.Ability
{
    public class PCT_蛋 : ISlotResolver
    {
        public int Check()
        {
            if (!PictomancerRotationEntry.QT.GetQt(QTKey.自动减伤))
            {
                return -3;
            }
            if (!(PCT_Data.Spells.蛋).GetSpell().IsReadyWithCanCast())
            {
                return -2;
            }
            if (!TargetHelper.targetCastingIsBossAOE(Core.Me.GetCurrTarget(), 1000) && !PCTSettings.Instance.fate模式)
            {
                return -1;
            }
            if (!TargetHelper.targetCastingIsBossAOE(Core.Me.GetCurrTarget(), 1000) && Core.Me.CurrentHpPercent() >= 0.4)
            {
                return -1;
            }

            return 0;


        }
        public void Build(Slot slot)
        {
            if (Core.Resolve<MemApiDuty>().InBossBattle)
            {
                Core.Resolve<MemApiChatMessage>().Toast2("AOE要来啦, 启动自动减伤!!", 1, 3000);
            }
            slot.Add(PCT_Data.Spells.蛋.GetSpell());


        }
    }
}