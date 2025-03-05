using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT;
using Cindy_Master.PCT.Data;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;



namespace PCT.utils.Helper;

public static class Helper
{
    public static IPlayerCharacter 自身 => Core.Me;

    public static uint 自身血量 => Core.Me.CurrentHp;

    public static uint 自身蓝量 => Core.Me.CurrentMp;

    public static float 自身血量百分比 => Core.Me.CurrentHpPercent();

    public static float 自身蓝量百分比 => Core.Me.CurrentMpPercent();

    public static int 队伍成员数量 => PartyHelper.Party.Count;

    public static uint 自身当前等级 => Core.Me.Level;


    public static bool 是否在副本中()
    {
        return Core.Resolve<MemApiCondition>().IsBoundByDuty();
    }

    public static uint 当前地图id => Core.Resolve<MemApiZoneInfo>().GetCurrTerrId();

    public static int 副本人数()
    {
        return Core.Resolve<MemApiDuty>().DutyMembersNumber();
    }

    public static bool 自身是否在移动()
    {
        return Core.Resolve<MemApiMove>().IsMoving();
    }

    public static bool 自身是否在读条()
    {
        return Core.Me.IsCasting;
    }

    public static int 自身周围单位数量(int range)
    {
        return TargetHelper.GetNearbyEnemyCount(range);
    }

    public static bool 自身存在Buff(uint id)
    {
        return Core.Me.HasAura(id);
    }

    public static bool 自身存在其中Buff(List<uint> auras, int msLeft = 0)
    {
        return Core.Me.HasAnyAura(auras, msLeft);
    }

    public static uint 自身命中其中Buff(List<uint> auras, int msLeft = 0)
    {
        return Core.Me.HitAnyAura(auras, msLeft);
    }

    public static bool 自身存在Buff大于时间(uint id, int time)
    {
        return Core.Me.HasMyAuraWithTimeleft(id, time);
    }




    /** -----------------他人状态相关----------------- **/
    public static float 目标血量百分比 => Core.Me.GetCurrTarget().CurrentHpPercent();

    public static bool 目标战斗状态(IBattleChara target)
    {
        return target.InCombat();
    }


    public static bool 目标是否可见或在技能范围内(uint actionId)
    {
        return Core.Resolve<MemApiSpell>().GetActionInRangeOrLoS(actionId) is not (566 or 562);
    }

    // public static bool 目标是否可见或在技能范围内(uint spellId, CharacterAgent target)
    // {
    //
    //     Core.Resolve<MemApiSpell>().GetActionInRangeOrLoS()
    //     var localPlayer = (GameObject*)Svc.ClientState.LocalPlayer.Address;
    //     if (ActionManager.GetActionInRangeOrLoS(spellId, localPlayer, SignatureHook.Instance.从ObjectID获取GameObject(target.ObjectId)) is 562 or 566) return false;
    //     return true;
    // }
    //
    // public static bool 是否能对目标使用技能(uint spellId, CharacterAgent target)
    // {
    //     return ActionManager.CanUseActionOnTarget(spellId, SignatureHook.Instance.从ObjectID获取GameObject(target.ObjectId));
    // }


    public static float 目标到自身距离()
    {
        return Core.Me.GetCurrTarget().Distance(Core.Me);
    }

    public static float 到自身近战距离(IBattleChara target)
    {
        return Core.Me.Distance(target);
    }

    public static float 到自身距离(IBattleChara target)
    {
        return Core.Me.Distance(target);
    }

    public static bool 目标是否为假人()
    {
        return Core.Me.GetCurrTarget().IsDummy();
    }

    public static bool 目标的指定buff剩余时间是否大于(uint id, int timeLeft = 0)
    {
        return Core.Me.GetCurrTarget().HasMyAuraWithTimeleft(id, timeLeft);
    }

    public static bool 指定buff剩余时间是否大于(IBattleChara target, uint id, int timeLeft = 0)
    {
        return target.HasMyAuraWithTimeleft(id, timeLeft);
    }

    public static int 目标的指定BUFF层数(IBattleChara target, uint buff)
    {
        return Core.Resolve<MemApiBuff>().GetStack(target, buff);
    }

    public static bool 目标可选状态(IBattleChara target)
    {
        return target.IsTargetable;
    }


    public static bool 目标是否拥有BUFF(uint id)
    {
        return Core.Me.GetCurrTarget().HasAura(id);
    }

    /*public static bool 目标是否准备放aoe(IBattleChara target, int timeLeft)
    {
        //  return TargetHelper.TargercastingIsbossaoe(target, timeLeft);
        return null;
    }
    */
    public static bool 目标是否拥有其中的BUFF(List<uint> auras, int timeLeft = 0)
    {
        return Core.Me.GetCurrTarget().HasAnyAura(auras, timeLeft);
    }

    public static bool 是否拥有其中的BUFF(IBattleChara target, List<uint> auras, int timeLeft = 0)
    {
        return target.HasAnyAura(auras, timeLeft);
    }

    public static bool 目标是否存在于DOT黑名单中(IBattleChara r)
    {
        return DotBlacklistHelper.IsBlackList(r);
    }

    public static bool 队员是否拥有可驱散状态()
    {
        return PartyHelper.CastableAlliesWithin30.Any(
            agent => agent.HasCanDispel() && agent.Distance(Core.Me) <= 30);
    }

    public static bool 队员是否拥有BUFF(uint buff)
    {
        return PartyHelper.CastableAlliesWithin30.Any(agent => agent.HasAura(buff));
    }

    public static int 二十米视线内血量低于设定的队员数量(float hp)
    {
        return PartyHelper.CastableAlliesWithin20.Count(
            r => r.CurrentHp != 0 && r.CurrentHpPercent() <= hp
        );
    }

    public static int 三十米视线内血量低于设定的队员数量(float hp)
    {
        return PartyHelper.CastableAlliesWithin30.Count(
            r => r.CurrentHp != 0 && r.CurrentHpPercent() <= hp
        );
    }

    public static int 十十米视线内血量低于设定的队员数量(float hp)
    {
        return PartyHelper.CastableAlliesWithin10.Count(
            r => r.CurrentHp != 0 && r.CurrentHpPercent() <= hp
        );
    }


    public static void Reset()
    {
        PictomancerRotationEntry.QT.SetQt(QTKey.爆发药, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.白神圣, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.黑彗星, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.天星, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.彩虹, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.锤子, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.爆发, true);

        PictomancerRotationEntry.QT.SetQt(QTKey.动物彩绘, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.武器彩绘, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.风景彩绘, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.动物构想, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.武器构想, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.风景构想, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.反转, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.基础连, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.自动绘画, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.即刻, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.马蒂恩, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.莫古, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.自动减伤, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.快死不画, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.自动停手, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.智能AOE, true);
        PictomancerRotationEntry.QT.SetQt(QTKey.测112, false);
        PictomancerRotationEntry.QT.SetQt(QTKey.倾泻资源, false);
        PictomancerRotationEntry.QT.SetQt(QTKey.优先画画, false);
        PictomancerRotationEntry.QT.SetQt(QTKey.团辅期乱打, false);
    }


    /** -----------------目标相关----------------- **/
    public static IBattleChara 自身目标 => Core.Me.GetCurrTarget();

    public static IBattleChara? 自身目标的目标 => Core.Me.GetCurrTargetsTarget();




    public static IBattleChara 获取可驱散队员()
    {
        return PartyHelper.CastableAlliesWithin30.LastOrDefault(agent =>
            agent.HasCanDispel() && agent.Distance(Core.Me) <= 30);
    }

    public static IBattleChara 获取拥有buff队员(uint buff)
    {
        return PartyHelper.CastableAlliesWithin30.LastOrDefault(agent => agent.HasAura(buff));
    }

    public static IBattleChara 获取血量最低成员()
    {
        if (PartyHelper.CastableAlliesWithin30.Count == 0)
            return Core.Me;
        return PartyHelper.CastableAlliesWithin30
            .Where(r => r.CurrentHp > 0).MinBy(r => r.CurrentHpPercent());
    }

    public static IBattleChara 获取血量最低成员_排除buff(uint buffId)
    {
        if (PartyHelper.CastableAlliesWithin30.Count == 0)
            return Core.Me;
        return PartyHelper.CastableAlliesWithin30
            .Where(r => r.CurrentHp > 0 && !r.HasAura(buffId)).MinBy(r => r.CurrentHpPercent());
    }

    public static IBattleChara 获取最低血量T()
    {
        if (PartyHelper.CastableTanks.Count == 0)
            return Core.Me;
        if (PartyHelper.CastableTanks.Count == 2 && PartyHelper.CastableTanks[1].CurrentHpPercent() <
            PartyHelper.CastableTanks[0].CurrentHpPercent())
            return PartyHelper.CastableTanks[1];
        return PartyHelper.CastableTanks[0];
    }

    public static IBattleChara 获取最低血量T_排除buff(uint buffId)
    {
        if (PartyHelper.CastableTanks.Count == 0)
            return Core.Me;
        if (PartyHelper.CastableTanks.Count == 2 &&
            PartyHelper.CastableTanks[1].CurrentHpPercent() < PartyHelper.CastableTanks[0].CurrentHpPercent() &&
            !PartyHelper.CastableTanks[1].HasAura(buffId))
            return PartyHelper.CastableTanks[1];
        return PartyHelper.CastableTanks[0];
    }

    public static IBattleChara 获取距离最远成员()
    {
        IBattleChara RescueTarget = PartyHelper.CastableAlliesWithin30
            .Where(r => r.CurrentHp > 0).MaxBy(r => r.Distance(PartyHelper.CastableAlliesWithin30.FirstOrDefault()));

        return RescueTarget;
    }

    public static IBattleChara 获取第几号小队列表(int index)
    {
        return PartyHelper.CastableParty[index - 1];
    }


    /** -----------------技能相关----------------- **/
    public static Spell 获取技能(uint id)
    {
        return id.GetSpell();
    }


    public static uint 技能状态码(uint id)
    {
        return Core.Resolve<MemApiSpell>().GetActionState(id);
    }

    public static Spell 获取会变化的技能(uint id)
    {
        return Core.Resolve<MemApiSpell>().CheckActionChange(id).GetSpell();
    }

    public static uint 获取会变化的技能id(uint id)
    {
        return Core.Resolve<MemApiSpell>().CheckActionChange(id);
    }


    public static int 高优先级gcd队列中技能数量()
    {
        return AI.Instance.BattleData.HighPrioritySlots_GCD.Count;
    }


    public static uint 上一个连击技能()
    {
        return Core.Resolve<MemApiSpell>().GetLastComboSpellId();
    }

    public static int GCD剩余时间()
    {
        return GCDHelper.GetGCDCooldown();
    }

    public static bool GCD可用状态()
    {
        return GCDHelper.CanUseGCD();
    }

    public static uint 上一个gcd技能()
    {
        return Core.Resolve<MemApiSpellCastSuccess>().LastGcd;
    }

    public static uint 上一个能力技能()
    {
        return Core.Resolve<MemApiSpellCastSuccess>().LastAbility;
    }


    public static bool 技能是否刚使用过(this uint spellId, int time = 1200)
    {
        return spellId.GetSpell().RecentlyUsed(time);
    }

    public static float 充能层数(this uint spellId)
    {
        return Core.Resolve<MemApiSpell>().GetCharges(spellId);
    }




    /// <summary>
    /// 获取时间戳，13位，毫秒
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamps()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public static Spell CreatePotionIfNeeded()
    {
        if (PictomancerRotationEntry.QT.GetQt(QTKey.爆发药))
        {
            return (Spell.CreatePotion());
        }
        return null;
    }

    public static Spell 白神圣()
    {
        if (Core.Resolve<JobApi_Pictomancer>().豆子 != 0)
        {
            return new Spell(PCT_Data.Spells.白神圣, SpellTargetType.Target);
        }
        return null;
    }


    /* public static IBattleChara? GetMostCanTargetObjectsAroundTarget(uint ActionID, IBattleChara currentTarget, int Count = 3, float searchRadius = 10f)
     {

         // 获取技能的 Action 数据
         Lumina.Excel.GeneratedSheets.Action row = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>().GetRow(ActionID.AdjustActionID());
         if (row == null)
         {
             return null;
         }

         Dictionary<uint, IBattleChara> enemysIn = TargetMgr.Instance.EnemysIn25;
         if (enemysIn.Count == 0)
         {
             return null;
         }

         // 过滤出当前目标周围 searchRadius 范围内的敌人
         var nearbyEnemies = enemysIn.Values.Where(e => e.Distance(currentTarget) <= searchRadius).ToList();
         if (nearbyEnemies.Count == 0)
         {
             return null; // 如果没有符合范围的敌人，返回 null
         }

         IBattleChara? bestTarget = null;
         int maxAffected = 0;


         // 遍历符合条件的敌人，寻找能够影响最多敌人的目标
         foreach (IBattleChara enemy in nearbyEnemies)
         {
             // 检查敌人与施法者之间的距离是否在技能的施法范围内
             if (row.Range <= 0 || !(enemy.DistanceToPlayer() > (float)row.Range))
             {
                 int targetAoECount = TargetHelper.GetTargetAoECount(enemy, enemysIn.Values, row.Range, row.EffectRange, row.CastType);
                 if (targetAoECount >= Count && targetAoECount > maxAffected)
                 {
                     maxAffected = targetAoECount;
                     bestTarget = enemy;
                 }
             }
         }

         return bestTarget; // 返回最佳目标，若无则返回 null
     }
 */
    public static Spell 彩虹()
    {

        if (Core.Me.HasAura(PCT_Data.Buffs.彩虹预备))
        {
            return new Spell(PCT_Data.Spells.彩虹, SpellTargetType.Target);
        }
        return null;
    }

    public static Spell 爆发药()
    {
        if (PictomancerRotationEntry.QT.GetQt(QTKey.爆发药))
        {
            return Spell.CreatePotion();
        }
        return null;
    }
    public static void 三画(CountDownHandler countDownHandler)
    {
        var jobApi = Core.Resolve<JobApi_Pictomancer>();

        var spellsWithStatus = new List<(uint spellId, bool isMissing)>
    {
        (PCT_Data.Spells.动物彩绘1, !jobApi.生物画),
        (PCT_Data.Spells.武器彩绘, !jobApi.武器画),
        (PCT_Data.Spells.风景彩绘, !jobApi.风景画)
    };

        var missingSpellIds = spellsWithStatus
                            .Where(s => s.isMissing)
                            .Select(s => s.spellId)
                            .ToList();

        int missingCount = missingSpellIds.Count;

        if (missingCount == 0)
        {
            return;
        }

        List<int> delays = new List<int>();

        if (missingCount == 3)
        {
            delays.Add(12000);
            delays.Add(10000);
            delays.Add(8000);
        }
        else if (missingCount == 2)
        {
            delays.Add(10000);
            delays.Add(8000);
        }
        else
        {
            delays.Add(8000);
        }

        for (int i = 0; i < missingSpellIds.Count && i < delays.Count; i++)
        {
            countDownHandler.AddAction(delays[i], missingSpellIds[i], SpellTargetType.Self);
        }


    }


}