
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;

namespace Cindy_Master.PCT.Ability

{
    public class PCT_醒梦 : ISlotResolver
    {
        public int Check()
        {

            if (Core.Me.CurrentMp > 6800)
            {
                return -1;
            }
            if (!(PCT_Data.Spells.醒梦).GetSpell().IsReadyWithCanCast())
            {
                return -2;
            }
            return 0;


        }
        public void Build(Slot slot)
        {
            slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.醒梦).GetSpell());
        }
    }
}