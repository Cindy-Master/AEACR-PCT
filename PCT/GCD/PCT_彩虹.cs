using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using Shiyuvi3._0;

namespace Cindy_Master.PCT.GCD;

public class PCT_彩虹 : ISlotResolver
{
    public int Check()
    {

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.彩虹))
        {
            return -8;
        }

        if (!Core.Me.HasAura(PCT_Data.Buffs.彩虹预备))
        {

            return -1;
        }



        return 0;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetLineAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, PCT_Data.Spells.彩虹);
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.彩虹), canTargetObjects));
        }
        else
        {
            slot.Add(PCT_Data.Spells.彩虹.GetSpell());
        }
        if (PCTSettings.Instance.聊天框提示瞬发)
        {
            LogHelper.Print("要瞬发啦");
        }
    }

}