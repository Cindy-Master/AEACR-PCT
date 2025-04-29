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

namespace Cindy_Master.PCT.GCD;

public class PCT_黑彗星 : ISlotResolver
{
    public int Check()
    {


        if (!PictomancerRotationEntry.QT.GetQt(QTKey.黑彗星))
        {
            return -8;
        }

        if (!Core.Me.HasAura(PCT_Data.Buffs.黑彗星buff))
        {
            return -1;
        }
        // 动物彩绘相关判断
        if (!Core.Resolve<JobApi_Pictomancer>().生物画 &&
            PictomancerRotationEntry.QT.GetQt(QTKey.动物彩绘) &&
            PictomancerRotationEntry.QT.GetQt(QTKey.优先画画))
        {
            return -7;
        }

        // 风景画判断
        if (!Core.Resolve<JobApi_Pictomancer>().风景画 &&
            PictomancerRotationEntry.QT.GetQt(QTKey.风景彩绘) &&
            PictomancerRotationEntry.QT.GetQt(QTKey.优先画画))
        {
            return -8;
        }

        // 武器画相关判断
        if (!Core.Resolve<JobApi_Pictomancer>().武器画 &&
            PictomancerRotationEntry.QT.GetQt(QTKey.武器彩绘) &&
            PictomancerRotationEntry.QT.GetQt(QTKey.优先画画))
        {
            return -9;
        }
        if (Core.Resolve<JobApi_Pictomancer>().豆子 == 0)
        {
            return -2;
        }
        if (PictomancerRotationEntry.QT.GetQt(QTKey.倾泻资源) && (PCT_Data.Spells.黑彗星).GetSpell().IsReadyWithCanCast())
        {
            return 6;
        }
        if ((Core.Resolve<JobApi_Pictomancer>().能量 == 100) && (PCT_Data.Spells.黑彗星).GetSpell().IsReadyWithCanCast() && (PCT_Data.Spells.反转).GetSpell().IsReadyWithCanCast() && PictomancerRotationEntry.QT.GetQt(QTKey.反转))
        {
            return 4;
        }
        if ((Core.Resolve<JobApi_Pictomancer>().能量 == 100) && ((PCT_Data.Spells.流水).GetSpell().IsReadyWithCanCast()) && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
        {
            return 3;
        }
        if (!Core.Me.IsMoving() && PictomancerRotationEntry.QT.GetQt(QTKey.基础连) && !PictomancerRotationEntry.QT.GetQt(QTKey.测112))
        {
            return -3;
        }


        return 0;
    }

    public void Build(Slot slot)
    {
        var canTargetObjects = ShiyuviTargetHelper.SmartTargetCircleAOE(2, Core.Me.GetCurrTarget(), 25f, 5f, PCT_Data.Spells.黑彗星);
        if (canTargetObjects != null && PictomancerRotationEntry.QT.GetQt(QTKey.智能AOE))
        {
            slot.Add(new Spell(Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.黑彗星), canTargetObjects));
        }
        else
        {
            slot.Add(PCT_Data.Spells.黑彗星.GetSpell());
        }

        if (PCTSettings.Instance.聊天框提示瞬发)
        {
            LogHelper.Print("要瞬发啦");
        }

    }

}