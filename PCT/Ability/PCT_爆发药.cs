using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
namespace Cindy_Master.PCT.Ability;

public class PCT_爆发药 : ISlotResolver
{
    public int Check()
    {



        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发药))
        {
            return -4;
        }
        if (GCDHelper.GetGCDCooldown() < 600)
        {
            return -2;
        }

        if (!ItemHelper.CheckCurrJobPotion())
        {
            return -9;
        }

        if (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell().Cooldown.TotalSeconds > 2)
        {
            return -6;
        }





        return 0;
    }

    public void Build(Slot slot)
    {

        slot.Add(Spell.CreatePotion());


    }
}