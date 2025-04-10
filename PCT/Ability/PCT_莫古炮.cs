using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using Shiyuvi3._0;

namespace Cindy_Master.PCT.Ability;

public class PCT_莫古炮 : ISlotResolver
{
    public int Check()
    {

        if (!Core.Resolve<JobApi_Pictomancer>().莫古准备)
        {
            return -1;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.莫古))
        {
            return -4;
        }
        if (!PictomancerRotationEntry.QT.GetQt(QTKey.爆发))
        {
            return -8;
        }
        if (GCDHelper.GetGCDCooldown() < 300)
        {
            return -2;
        }



        bool isRiyueMode = PCTSettings.Instance.日随模式; // Replace with your actual check for 日随模式
        bool isGaoNanMode = PCTSettings.Instance.高难模式; // Replace with your actual check for 高难模式

        // Set cooldown based on mode
        int cooldownTime = 40; // Default to 40 seconds for 日随模式
        if (isGaoNanMode)
        {
            cooldownTime = 20; // Set to 20 seconds for 高难模式
        }

        if (!PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景构想).GetSpell().Cooldown.TotalSeconds <= cooldownTime && PictomancerRotationEntry.QT.GetQt(QTKey.风景构想) && SpellExtension.IsUnlock(PCT_Data.Spells.风景构想))
        {
            return -3;
        }
        if (Core.Me.HasAura(PCT_Data.Buffs.星空, 15000) && PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源))
        {
            return -1;
        }
        if (PCT_Data.Spells.莫古.GetSpell().Charges < 1)
        {
            return -3;
        }
        if (!PCT_Data.Spells.莫古.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        return 0;
    }




    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetLineAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, PCT_Data.Spells.莫古);
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.莫古), canTargetObjects));
        }
        else
        {
            slot.Add(PCT_Data.Spells.莫古.GetSpell());
        }

    }
}