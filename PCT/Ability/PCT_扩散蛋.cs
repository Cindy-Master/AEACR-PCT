
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;

namespace Cindy_Master.PCT.Ability
{
    public class PCT_扩散蛋 : ISlotResolver
    {
        public int Check()
        {
            if (!PictomancerRotationEntry.QT.GetQt(QTKey.自动减伤))
            {
                return -3;
            }
            /* if (!TargetHelper.TargercastingIsbossaoe(Core.Me.GetCurrTarget(), 15))
             {
                 return -1;
             }*/

            if (!Core.Me.HasAura(PCT_Data.Buffs.蛋))
            {
                return -2;
            }
            if (!(PCT_Data.Spells.扩散蛋).GetSpell().IsReadyWithCanCast())
            {
                return -2;
            }
            return 0;


        }
        public void Build(Slot slot)
        {
            Core.Resolve<MemApiChatMessage>().Toast2("AOE要来啦, 尝试扩散减伤!!", 1, 3000);

            slot.Add(PCT_Data.Spells.扩散蛋.GetSpell());


        }
    }
}