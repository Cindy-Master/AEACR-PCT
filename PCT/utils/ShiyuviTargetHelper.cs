using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Common.Math;

namespace Shiyuvi3._0;

public class ShiyuviTargetHelper
{
    // public static bool CheckAllNearbyEnemiesSkills(float radius, int[] skillIds, IBattleChara center = null)
    // {
    //     // 如果没有指定中心点，默认使用玩家
    //     if (center == null)
    //     {
    //         center = Core.Me;
    //     }
    //
    //     // 安全检查：确保中心点有效
    //     if (center == null || !center.IsValid())
    //     {
    //         return false;
    //     }
    //
    //     // 创建一个 HashSet 来存储技能ID，提高查找效率
    //     var skillIdSet = new HashSet<int>(skillIds);
    //
    //     // 获取指定范围内的所有敌对目标
    //     var nearbyEnemies = Data.AllHostileTargets?
    //         .Where(enemy =>
    //             enemy != null && enemy.IsValid() && Vector3.Distance(enemy.Position, center.Position) <= radius)
    //         .ToList() ?? new List<IBattleChara>();
    //
    //     // 遍历所有附近的敌人
    //     foreach (var enemy in nearbyEnemies)
    //     {
    //         // 检查敌人当前正在释放的技能
    //         var castingSpellId = enemy.CastActionId;
    //         if (castingSpellId != 0 && skillIdSet.Contains((int)castingSpellId))
    //         {
    //             return true; // 如果任何敌人正在释放指定的技能之一，立即返回true
    //         }
    //
    //         // 如果需要检查最近使用的技能，可以在这里添加额外的逻辑
    //     }
    //
    //     return false; // 如果没有找到任何敌人释放指定的技能，返回false
    // }

    public static unsafe IBattleChara FindDispelTargetForward()
    {
        AgentHUD* agentHud = AgentHUD.Instance();
        if (agentHud == null)
        {
            //LogHelper.PrintError("AgentHUD不可用。");
            return null;
        }

        int partyMemberCount = agentHud->PartyMemberCount;

        if (partyMemberCount < 0 || partyMemberCount > 10)
        {
            //LogHelper.PrintError($"无效的partyMemberCount：{partyMemberCount}");
            return null;
        }

        if (Core.Me.HasCanDispel())
            return Core.Me;


        for (int i = 0; i < partyMemberCount; i++)
        {
            HudPartyMember member;
            fixed (HudPartyMember* memberPtr = &agentHud->PartyMembers[0])
            {
                member = memberPtr[i];
            }

            var obj = Svc.Objects.SearchByEntityId(member.EntityId);

            if (obj is IBattleChara BattleChara && obj != null && obj.IsValid() && obj.IsTargetable && !obj.IsDead && BattleChara.HasCanDispel())
            {
                return BattleChara;
            }
        }

        return null;
    }

    public static unsafe IBattleChara FindResurrectTargetForward()
    {
        AgentHUD* agentHud = AgentHUD.Instance();
        if (agentHud == null)
        {
            //LogHelper.PrintError("AgentHUD不可用。");
            return null;
        }

        int partyMemberCount = agentHud->PartyMemberCount;

        if (partyMemberCount < 0 || partyMemberCount > 10)
        {
            //LogHelper.PrintError($"无效的partyMemberCount：{partyMemberCount}");
            return null;
        }

        if (Core.Me.IsDead)
            return null;


        for (int i = 0; i < partyMemberCount; i++)
        {
            HudPartyMember member;
            fixed (HudPartyMember* memberPtr = &agentHud->PartyMembers[0])
            {
                member = memberPtr[i];
            }

            var obj = Svc.Objects.SearchByEntityId(member.EntityId);

            if (obj is IBattleChara BattleChara && obj != null && obj.IsValid() && obj.IsTargetable && obj.IsDead && !BattleChara.HasAura(148))
            {
                return BattleChara;
            }
        }

        return null;
    }

    public static unsafe IBattleChara FindDispelTarget()
    {
        AgentHUD* agentHud = AgentHUD.Instance();
        if (agentHud == null)
        {
            //LogHelper.PrintError("AgentHUD不可用。");
            return null;
        }

        int partyMemberCount = agentHud->PartyMemberCount;

        if (partyMemberCount < 0 || partyMemberCount > 10)
        {
            //LogHelper.PrintError($"无效的partyMemberCount：{partyMemberCount}");
            return null;
        }

        if (Core.Me.HasCanDispel())
            return Core.Me;


        for (int i = partyMemberCount - 1; i >= 0; i--)
        {
            HudPartyMember member;
            fixed (HudPartyMember* memberPtr = &agentHud->PartyMembers[0])
            {
                member = memberPtr[i];
            }

            var obj = Svc.Objects.SearchByEntityId(member.EntityId);

            if (obj is IBattleChara BattleChara && obj != null && obj.IsValid() && obj.IsTargetable && !obj.IsDead && BattleChara.HasCanDispel())
            {
                return BattleChara;
            }
        }

        return null;
    }

    public static unsafe IBattleChara FindResurrectTarget()
    {
        AgentHUD* agentHud = AgentHUD.Instance();
        if (agentHud == null)
        {
            //LogHelper.PrintError("AgentHUD不可用。");
            return null;
        }

        int partyMemberCount = agentHud->PartyMemberCount;

        if (partyMemberCount < 0 || partyMemberCount > 10)
        {
            //LogHelper.PrintError($"无效的partyMemberCount：{partyMemberCount}");
            return null;
        }

        if (Core.Me.IsDead)
            return null;


        for (int i = partyMemberCount - 1; i >= 0; i--)
        {
            HudPartyMember member;
            fixed (HudPartyMember* memberPtr = &agentHud->PartyMembers[0])
            {
                member = memberPtr[i];
            }

            var obj = Svc.Objects.SearchByEntityId(member.EntityId);

            if (obj is IBattleChara BattleChara && obj != null && obj.IsValid() && obj.IsTargetable && obj.IsDead && !BattleChara.HasAura(148))
            {
                return BattleChara;
            }
        }

        return null;
    }



    public static bool CheckAllNearbyEnemiesSkills(float radius, HashSet<uint> skillIds, IBattleChara center = null)
    {
        if (center == null)
        {
            center = Core.Me;
        }

        if (center == null || !center.IsValid())
        {
            return false;
        }

        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null && enemy.IsValid() && Vector3.Distance(enemy.Position, center.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        foreach (var enemy in nearbyEnemies)
        {
            var castingSpellId = enemy.CastActionId;
            if (castingSpellId != 0 && skillIds.Contains(castingSpellId))
            {
                return true;
            }
        }

        return false;
    }

    // 在类中声明一个全局字典，用于存储目标的上一次位置
    private static Dictionary<IBattleChara, Vector3> previousPositions = new Dictionary<IBattleChara, Vector3>();

    public static bool IsTargetMoving(IBattleChara target)
    {
        if (target == null || !target.IsValid())
            return false;

        // 获取目标的当前位置
        Vector3 currentPosition = target.Position;

        // 如果之前没有记录这个目标的位置，说明是第一次检测，初始化它的位置
        if (!previousPositions.ContainsKey(target))
        {
            previousPositions[target] = currentPosition;
            return false; // 第一次检测时，无法判断是否在移动，返回false
        }

        // 获取之前记录的位置
        Vector3 previousPosition = previousPositions[target];

        // 更新记录的位置
        previousPositions[target] = currentPosition;

        // 计算当前位置和之前位置的差距
        float distanceMoved = Vector3.Distance(currentPosition, previousPosition);

        // 如果距离大于某个阈值（例如0.01），我们认为它在移动
        return distanceMoved > 0.01f;
    }

    /*
        public static bool 血仇与减速数量大于等于X(int enemycount = 1)
        {
            var tanktarget = PartyHelper.CastableAlliesWithin30.Where(e => e.HasAnyAura(Buff.SpecialBuffs))
                .FirstOrDefault(); //坦克目标
            if (tanktarget != null && tanktarget.IsValid())
            {
                if (!tanktarget.GetCurrTarget().IsBoss())
                {
                    if (ShiyuviTargetHelper.DoNearbyEnemiesHaveBuff(tanktarget, 1193) ||
                        ShiyuviTargetHelper.DoNearbyEnemiesHaveBuff(tanktarget, 9)) //血仇和减速
                    {
                        if ((GetNearbyEnemiesWithBuffCount(tanktarget, 1193) +
                             GetNearbyEnemiesWithBuffCount(tanktarget, 9)) >= enemycount)
                            return true;
                    }
                }
            }

            return false;
        }
    */
    /// <summary>
    /// 获取指定目标附近一定范围内的敌对目标数量。
    /// </summary>
    /// <param name="target">要检查其周围敌人的目标（例如玩家角色或某个怪物）。</param>
    /// <param name="radius">检查半径，即目标周围多远范围内的敌人会被检查。默认为 7f。</param>
    /// <returns>附近拥有指定buff的敌对目标的数量。</returns>
    public static int GetNearbyEnemiesCount(IBattleChara target, float radius = 7f)
    {
        // 安全检查：确保目标有效
        if (target == null || !target.IsValid())
        {
            return 0;
        }

        // 获取附近的敌对目标
        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null && enemy.IsValid() && Vector3.Distance(enemy.Position, target.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        // 计数拥有指定buff的敌人数量
        int count = 0;
        foreach (var enemy in nearbyEnemies)
        {
            // 检查敌人是否拥有指定的buff
            if (enemy.StatusList != null && enemy.IsValid())
            {
                count++;
            }
        }

        return count;
    }


    /// <summary>
    /// 获取指定目标附近一定范围内的拥有指定buff的敌对目标数量。
    /// </summary>
    /// <param name="target">要检查其周围敌人的目标（例如玩家角色或某个怪物）。</param>
    /// <param name="buffId">要检查的buff的ID。</param>
    /// <param name="radius">检查半径，即目标周围多远范围内的敌人会被检查。默认为 7f。</param>
    /// <returns>附近拥有指定buff的敌对目标的数量。</returns>
    public static int GetNearbyEnemiesWithBuffCount(IBattleChara target, uint buffId, float radius = 7f)
    {
        // 安全检查：确保目标有效
        if (target == null || !target.IsValid())
        {
            return 0;
        }

        // 获取附近的敌对目标
        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null && enemy.IsValid() && Vector3.Distance(enemy.Position, target.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        // 计数拥有指定buff的敌人数量
        int count = 0;
        foreach (var enemy in nearbyEnemies)
        {
            // 检查敌人是否拥有指定的buff
            if (enemy.StatusList != null && enemy.StatusList.Any(status => status.StatusId == buffId))
            {
                count++;
            }
        }

        return count;
    }


    /// <summary>
    /// 检查指定目标附近一定范围内的敌对目标是否拥有指定的buff。
    /// </summary>
    /// <param name="target">要检查其周围敌人的目标（例如玩家角色或某个怪物）。</param>
    /// <param name="buffId">要检查的buff的ID。</param>
    /// <param name="radius">检查半径，即目标周围多远范围内的敌人会被检查。默认为 7f。</param>
    /// <returns>如果至少有一个附近敌对目标拥有指定的buff，则返回 true；否则返回 false。</returns>
    public static bool DoNearbyEnemiesHaveBuff(IBattleChara target, uint buffId, float radius = 7f)
    {
        // 安全检查：确保目标有效
        if (target == null || !target.IsValid())
        {
            return false;
        }

        // 获取附近的敌对目标
        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null && enemy.IsValid() && Vector3.Distance(enemy.Position, target.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        // 遍历附近的敌人
        foreach (var enemy in nearbyEnemies)
        {
            // 检查敌人是否拥有指定的buff
            if (enemy.StatusList != null && enemy.StatusList.Any(status => status.StatusId == buffId))
            {
                return true; // 找到一个敌人拥有指定的buff，立即返回true
            }
        }

        return false; // 没有找到任何敌人拥有指定的buff
    }

    private static Dictionary<uint, DateTime> deathTimes = new Dictionary<uint, DateTime>();

    public static bool AoeCount5m(int count)
    {
        var player = Svc.ClientState.LocalPlayer;
        if (player == null) return false;

        const float aoeRadius = 5f; // 5米的AOE范围

        var enemies = Data.AllHostileTargets?
            .Where(e => e != null && e.IsValid() && e.DistanceToPlayer() <= aoeRadius * 2) // 预筛选，减少后续计算
            .ToList() ?? new List<IBattleChara>();

        if (enemies.Count < count) return false; // 如果敌人总数少于所需数量，直接返回false

        int targetsInRange = enemies.Count(e =>
            Vector3.Distance(e.Position, player.Position) <= aoeRadius + e.HitboxRadius
        );

        return targetsInRange >= count;
    }

    public static bool IsTargetDeadLongEnough(IBattleChara target, double secondsThreshold = 1)
    {
        if (target == null || !target.IsDead)
        {
            deathTimes.Remove(target.NameId);
            return false;
        }

        if (!deathTimes.ContainsKey(target.NameId))
        {
            deathTimes[target.NameId] = DateTime.Now;
            return false;
        }

        return (DateTime.Now - deathTimes[target.NameId]).TotalSeconds >= secondsThreshold;
    }

    public static void UpdateDeathStatus(IBattleChara target)
    {
        if (target == null) return;

        if (target.IsDead)
        {
            if (!deathTimes.ContainsKey(target.NameId))
            {
                deathTimes[target.NameId] = DateTime.Now;
            }
        }
        else
        {
            deathTimes.Remove(target.NameId);
        }
    }

    public static void CleanupDeathTimes(double cleanupThresholdSeconds = 5)
    {
        var currentTime = DateTime.Now;
        var keysToRemove = deathTimes.Where(kvp => (currentTime - kvp.Value).TotalSeconds > cleanupThresholdSeconds)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            deathTimes.Remove(key);
        }
    }

    public static IBattleChara HasCanDispelTargetSelector() //逆顺位优先康复逻辑
    {
        List<IBattleChara> targets = new List<IBattleChara>();

        // 添加当前玩家
        targets.Add(Core.Me);

        // 添加DPS，保持特定顺序，但只添加实际存在的
        for (int i = Math.Min(8, PartyHelper.CastableDps.Count - 1); i >= 0; i--)
        {
            if (PartyHelper.CastableDps[i] != null)
            {
                targets.Add(PartyHelper.CastableDps[i]);
            }
        }

        // 添加坦克，保持特定顺序，但只添加实际存在的
        for (int i = Math.Min(8, PartyHelper.CastableTanks.Count - 1); i >= 0; i--)
        {
            if (PartyHelper.CastableTanks[i] != null)
            {
                targets.Add(PartyHelper.CastableTanks[i]);
            }
        }

        // 添加治疗者，如果存在的话
        for (int i = Math.Min(8, PartyHelper.CastableHealers.Count - 1); i >= 0; i--)
        {
            if (PartyHelper.CastableHealers[i] != null)
            {
                targets.Add(PartyHelper.CastableHealers[i]);
            }
        }

        // 查找并返回第一个可驱散的目标
        foreach (IBattleChara target in targets)
        {
            if (target.HasCanDispel())
                return target;
        }

        // 如果没有找到可驱散的目标，返回当前玩家
        return Core.Me;
    }

    public static Vector3
        GetNearestPositionForStance(float targetRotation, string desiredStance) //获取目标最近的最优身位解（TP身位最短最优位置)
    {
        const float STANCE_ANGLE = (float)(Math.PI / 2); // 90 degrees in radians for stance zones
        const float BUFFER_ANGLE = 0.05f; // Small buffer angle to avoid edge cases

        Vector3 myPosition = Core.Me.Position;
        var target = Core.Me.GetCurrTarget();
        if (target == null)
        {
            throw new InvalidOperationException("No target selected.");
        }

        Vector3 targetPosition = target.Position;
        float targetRadius = target.HitboxRadius;

        // Calculate target's forward vector
        Vector3 targetForward = new Vector3((float)Math.Sin(targetRotation), 0, (float)Math.Cos(targetRotation));

        // Calculate target's right vector
        Vector3 targetRight = new Vector3(targetForward.Z, 0, -targetForward.X);

        // Calculate direction from target to player
        Vector3 directionToMe = Vector3.Normalize(myPosition - targetPosition);

        Vector3 desiredDirection;
        switch (desiredStance.ToLower())
        {
            case "front":
                desiredDirection = targetForward;
                break;
            case "back":
                desiredDirection = -targetForward;
                break;
            case "side":
                // Choose the side closest to the player
                desiredDirection = Vector3.Dot(directionToMe, targetRight) > 0 ? targetRight : -targetRight;
                break;
            default:
                throw new ArgumentException("Invalid stance. Use 'front', 'back', or 'side'.");
        }

        // Calculate the projection of the direction to the player (ignoring Y axis)
        Vector3 projectedDirection = new Vector3(directionToMe.X, 0, directionToMe.Z);
        projectedDirection = Vector3.Normalize(projectedDirection);

        // Calculate the signed angle between the projected direction and the desired direction
        float signedAngle = SignedAngle(projectedDirection, desiredDirection, new Vector3(0, 1, 0));

        Vector3 nearestPosition;
        if (Math.Abs(signedAngle) < (STANCE_ANGLE / 2) - BUFFER_ANGLE)
        {
            // If the player is already within the stance zone (with buffer), return the nearest point on the target's hitbox
            nearestPosition = targetPosition + directionToMe * targetRadius;
        }
        else
        {
            // If the player is outside the stance zone, calculate the closest point within the stance zone
            float safeAngle = (STANCE_ANGLE / 2) - BUFFER_ANGLE;

            // Calculate the boundary edges of the stance zone
            Vector3 leftEdgeDirection = RotateVector(desiredDirection, new Vector3(0, 1, 0), -safeAngle);
            Vector3 rightEdgeDirection = RotateVector(desiredDirection, new Vector3(0, 1, 0), safeAngle);
            // Calculate the positions of the left and right edge points
            Vector3 leftEdgePoint = targetPosition + leftEdgeDirection * targetRadius;
            Vector3 rightEdgePoint = targetPosition + rightEdgeDirection * targetRadius;

            // Calculate the distances from the player to the left and right edge points
            float distanceToLeftEdge = Vector3.Distance(myPosition, leftEdgePoint);
            float distanceToRightEdge = Vector3.Distance(myPosition, rightEdgePoint);

            // Choose the edge point that is closest to the player
            Vector3 closestEdgePoint = (distanceToLeftEdge < distanceToRightEdge) ? leftEdgePoint : rightEdgePoint;

            // Calculate the direction from the player to the closest edge point
            Vector3 playerToEdgeDirection = Vector3.Normalize(closestEdgePoint - myPosition);

            // Calculate the angle between the player's direction to the target and the direction to the closest edge
            float angleToEdge =
                (float)Math.Acos(Math.Clamp(Vector3.Dot(directionToMe, playerToEdgeDirection), -1f, 1f));

            // If the angle to the edge is small enough (within the safe angle), project the player's position onto the target's hitbox
            if (angleToEdge < safeAngle)
            {
                // Project the player's position directly onto the target's hitbox along their current direction
                Vector3 projectedPlayerPosition =
                    targetPosition + Vector3.Normalize(myPosition - targetPosition) * targetRadius;
                nearestPosition = projectedPlayerPosition;
            }
            else
            {
                // Otherwise, use the calculated closest edge point
                nearestPosition = closestEdgePoint;
            }
        }

        return nearestPosition;
    }

    private static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
        // 计算两个向量的点积，得到夹角的余弦值
        float dot = Vector3.Dot(Vector3.Normalize(from), Vector3.Normalize(to));
        dot = Math.Clamp(dot, -1f, 1f); // 夹角的余弦值需要在[-1, 1]区间内

        // 通过反余弦函数计算无符号角度
        float unsignedAngle = (float)Math.Acos(dot);

        // 使用叉积计算角度的符号（正负）
        Vector3 cross = Vector3.Cross(from, to);
        float sign = Math.Sign(Vector3.Dot(axis, cross));

        // 返回有符号角度
        return unsignedAngle * sign;
    }

    // 绕轴旋转向量
    private static Vector3 RotateVector(Vector3 vector, Vector3 axis, float angle)
    {
        // 归一化轴向量
        axis = Vector3.Normalize(axis);

        // 计算旋转矩阵的分量
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);
        float oneMinusCos = 1.0f - cos;

        // 计算旋转后的向量
        return vector * cos
               + Vector3.Cross(axis, vector) * sin
               + axis * Vector3.Dot(axis, vector) * oneMinusCos;
    }


    public static bool IsTargetVisibleOrInRange(uint actionId, IBattleChara? target) //目标在技能攻击范围内且可被技能命中
    {
        unsafe
        {
            if (Svc.ClientState.LocalPlayer != null && target != null && target.IsTargetable)
            {
                var skilltarget = (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)target.Address;
                var me = (FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject*)Svc.ClientState.LocalPlayer.Address;
                if (ActionManager.GetActionInRangeOrLoS == null)
                {
                    return false;
                }

                return ActionManager.GetActionInRangeOrLoS(actionId, me, skilltarget) is not (566 or 562);
            }
        }

        return false;
    }


    public static IBattleChara? SmartTargetCircleAOE(int minTargetCount, IBattleChara? currentTarget,
        float maxCastRange, float aoeRadius, uint actionId) //目标为中心的圆形AOE伤害，选择最优目标（同血量选血最多的）
    {
        var player = Svc.ClientState.LocalPlayer;
        var enemies = Data.AllHostileTargets?
            .Where(e => e != null && e.IsValid() && e.DistanceToPlayer() <= maxCastRange + aoeRadius &&
                        IsTargetVisibleOrInRange(actionId, e))
            .ToList() ?? new List<IBattleChara>();



        List<IBattleChara> bestTargets = new List<IBattleChara>();
        int maxHitCount = 0;

        foreach (var potentialTarget in enemies)
        {
            // 获取潜在目标的范围圈大小
            float potentialTargetRadius = potentialTarget.HitboxRadius;

            // 先判断潜在目标是否在施法距离内
            if (potentialTarget.DistanceToPlayer() > maxCastRange + potentialTargetRadius) continue;

            // 计算这个潜在目标可以击中的敌人数
            int hitCount = enemies.Count(e =>
                Vector3.Distance(e.Position, potentialTarget.Position) <= aoeRadius + e.HitboxRadius
            );

            // 更新最佳目标列表
            if (hitCount > maxHitCount)
            {
                maxHitCount = hitCount;
                bestTargets.Clear();
                bestTargets.Add(potentialTarget);
            }
            else if (hitCount == maxHitCount)
            {
                bestTargets.Add(potentialTarget);
            }
        }

        // 如果有击中足够多敌人的目标
        if (bestTargets.Any() && maxHitCount >= minTargetCount)
        {
            // 从最佳目标中选择血量最多的
            return bestTargets.OrderByDescending(t => t.CurrentHp).FirstOrDefault();
        }

        // 如果没有找到满足条件的目标，返回当前选中的目标
        return null;
    }


    public static IBattleChara? SmartTargetLineAOE(int minCount, IBattleChara currentTarget, float length, float width,
        uint actionId, int spellCastRange = 10)
    {
        var player = Svc.ClientState.LocalPlayer;
        if (player == null) return null;

        // 获取范围内的所有敌人
        var enemies = Data.AllHostileTargets
            .Where(e => e.DistanceToPlayer() <= spellCastRange && IsTargetVisibleOrInRange(actionId, e))
            .ToList();





        IBattleChara? bestTarget = null;
        int maxHitCount = 0;
        float highestHp = 0;

        // 遍历每个敌人作为潜在目标
        foreach (var potentialTarget in enemies)
        {
            // ... (计算命中数量的代码不变)
            // 计算以这个目标为方向时能命中多少敌人
            Vector3 direction = potentialTarget.Position - player.Position;
            direction.Y = 0;
            direction = Vector3.Normalize(direction);

            int currentHitCount = 0;
            foreach (var enemy in enemies)
            {
                if (IsInRectangle(player.Position, direction, length, width, enemy.Position, enemy.HitboxRadius))
                {
                    currentHitCount++;
                }
            }

            // 更新最佳目标
            if (currentHitCount > maxHitCount)
            {
                maxHitCount = currentHitCount;
                bestTarget = potentialTarget;
                highestHp = potentialTarget.CurrentHp;
            }
            else if (currentHitCount == maxHitCount && potentialTarget.CurrentHp > highestHp)
            {
                bestTarget = potentialTarget;
                highestHp = potentialTarget.CurrentHp;
            }
        }

        if (bestTarget != null && maxHitCount >= minCount) // 只有当命中数量大于等于 minCount 时才切换目标
        {
            return bestTarget;
        }

        return null; // 如果没有找到更好的目标，或者命中数量小于 minCount，则返回当前目标
    }

    private static IBattleChara? GetOptimalTargetForLinearAOE(float length, float width, int spellCastRange)
    {
        var player = Svc.ClientState.LocalPlayer;
        if (player == null) return null;

        // 获取所有可能的敌人
        var enemies = Data.AllHostileTargets
            .Where(e => e.DistanceToPlayer() <= spellCastRange) // 考虑敌人目标圈的距离
            .ToList();

        if (!enemies.Any()) return null;

        List<IBattleChara> optimalTargets = new List<IBattleChara>();
        int maxHitCount = 0;

        foreach (var potentialTarget in enemies)
        {
            if (potentialTarget.DistanceToPlayer() > spellCastRange) continue;

            // 计算方向向量
            Vector3 direction = potentialTarget.Position - player.Position;
            direction.Y = 0; // 忽略高度差
            direction = Vector3.Normalize(direction);

            int hitCount = 0;
            foreach (var enemy in enemies)
            {
                // 判断敌人是否在直线AOE范围内，考虑敌人的目标圈
                if (IsInRectangle(player.Position, direction, length, width, enemy.Position, enemy.HitboxRadius))
                {
                    hitCount++;
                }
            }

            // 更新最佳目标
            if (hitCount > maxHitCount)
            {
                maxHitCount = hitCount;
                optimalTargets.Clear();
                optimalTargets.Add(potentialTarget);
            }
            else if (hitCount == maxHitCount)
            {
                optimalTargets.Add(potentialTarget);
            }
        }

        // 从最佳目标中选择血量最多的
        return optimalTargets.OrderByDescending(t => t.CurrentHp).FirstOrDefault();
    }

    private static bool IsInRectangle(Vector3 start, Vector3 direction, float length, float width, Vector3 point,
        float hitboxRadius)
    {
        Vector3 toPoint = point - start;
        float dotProduct = Vector3.Dot(toPoint, direction);

        // 判断点是否在直线AOE的长度范围内
        if (dotProduct < 0 || dotProduct > length)
            return false;

        // 计算点到直线的距离，并考虑敌人目标圈的半径
        Vector3 projection = start + direction * dotProduct;
        float distanceToLine = Vector3.Distance(point, projection);

        // 判断点是否在直线AOE的宽度范围内，并考虑目标圈大小
        return distanceToLine <= (width / 2) + hitboxRadius;
    }


    public static IBattleChara? SmartTargetFanAOE(float angle, float maxDistance, int targetCount,
        IBattleChara currentTarget, uint actionId) //对目标的扇形AOE，选择最优目标（同血量选血最多的）
    {
        var player = Core.Me;
        if (player == null) return null;

        // 获取所有在最大距离内的敌人
        var enemies = Data.AllHostileTargets
            .Where(e => e.DistanceToPlayer() <= maxDistance && IsTargetVisibleOrInRange(actionId, e)) // 考虑目标圈
            .ToList();

        if (!enemies.Any()) return null;

        List<IBattleChara> optimalTargets = new List<IBattleChara>();
        int maxHitCount = 0;

        foreach (var potentialTarget in enemies)
        {
            Vector3 direction = potentialTarget.Position - player.Position;
            direction.Y = 0; // 忽略高度差
            direction = direction.Normalized;

            int hitCount = 0;
            foreach (var enemy in enemies)
            {
                // 判断敌人是否在扇形范围内，考虑目标圈
                if (IsInFanShape(player.Position, direction, angle, maxDistance, enemy.Position, enemy.HitboxRadius))
                {
                    hitCount++;
                }
            }

            // 更新最佳目标
            if (hitCount > maxHitCount)
            {
                maxHitCount = hitCount;
                optimalTargets.Clear();
                optimalTargets.Add(potentialTarget);
            }
            else if (hitCount == maxHitCount)
            {
                optimalTargets.Add(potentialTarget);
            }
        }

        // 如果找到的最佳目标命中数量不小于指定的目标数量，返回血量最多的目标
        if (optimalTargets.Any() && maxHitCount >= targetCount)
        {
            return optimalTargets.OrderByDescending(t => t.CurrentHp).FirstOrDefault();
        }

        // 如果当前目标在范围内且命中数量足够，返回当前目标
        if (currentTarget != null && currentTarget.DistanceToPlayer() <= maxDistance)
        {
            Vector3 currentDirection = currentTarget.Position - player.Position;
            currentDirection.Y = 0;
            currentDirection = currentDirection.Normalized;

            int currentHitCount = enemies.Count(e =>
                IsInFanShape(player.Position, currentDirection, angle, maxDistance, e.Position, e.HitboxRadius)
            );

            if (currentHitCount >= targetCount)
            {
                return currentTarget;
            }
        }

        // 如果没有找到合适的目标，返回当前目标
        return currentTarget;
    }

    private static bool IsInFanShape(Vector3 origin, Vector3 direction, float angle, float maxDistance, Vector3 point,
        float hitboxRadius)
    {
        Vector3 toPoint = point - origin;
        toPoint.Y = 0; // 忽略高度差

        // 考虑目标圈的半径
        float distance = toPoint.Magnitude;
        if (distance > maxDistance + hitboxRadius)
            return false;

        toPoint = toPoint.Normalized;
        float dotProduct = Vector3.Dot(direction, toPoint);
        double angleRadians = angle * Math.PI / 360f; // 将角度转换为弧度，并除以2（因为是半角）

        // 判断是否在扇形角度范围内
        return Math.Acos(dotProduct) <= angleRadians;
    }

    /// <summary>
    /// 获取周围敌人的目标列表
    /// </summary>
    /// <param name="radius">检查半径，默认30米</param>
    /// <param name="center">检查中心点，默认为玩家</param>
    /// <param name="includeDeadTargets">是否包含死亡目标，默认false</param>
    /// <param name="removeDuplicates">是否去除重复目标，默认true</param>
    /// <returns>敌人目标的列表</returns>
    public static List<IBattleChara> GetNearbyEnemiesTargets(float radius = 30f, IBattleChara center = null,
        bool includeDeadTargets = false, bool removeDuplicates = true)
    {
        // 如果没有指定中心点，默认使用玩家
        if (center == null)
        {
            center = Core.Me;
        }

        // 安全检查：确保中心点有效
        if (center == null || !center.IsValid())
        {
            return new List<IBattleChara>();
        }

        // 获取指定范围内的所有敌对目标
        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null &&
                enemy.IsValid() &&
                Vector3.Distance(enemy.Position, center.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        // 存储敌人的目标
        var enemyTargets = new List<IBattleChara>();

        // 遍历所有附近的敌人，获取它们的目标
        foreach (var enemy in nearbyEnemies)
        {
            var target = enemy.GetCurrTarget();

            // 检查目标是否有效
            if (target != null && target.IsValid() && target.IsTargetable)
            {
                // 根据参数决定是否包含死亡目标
                if (includeDeadTargets || !target.IsDead)
                {
                    enemyTargets.Add(target);
                }
            }
        }

        // 根据参数决定是否去除重复目标
        if (removeDuplicates)
        {
            enemyTargets = enemyTargets.Distinct().ToList();
        }

        return enemyTargets;
    }

    /// <summary>
    /// 获取周围敌人攻击的队友列表
    /// </summary>
    /// <param name="radius">检查半径，默认30米</param>
    /// <param name="center">检查中心点，默认为玩家</param>
    /// <returns>被敌人攻击的队友列表</returns>
    public static List<IBattleChara> GetNearbyEnemiesTargetingAllies(float radius = 30f, IBattleChara center = null)
    {
        var enemyTargets = GetNearbyEnemiesTargets(radius, center, false, true);

        // 获取所有队友
        var allAllies = new List<IBattleChara> { Core.Me };
        allAllies.AddRange(PartyHelper.CastableAlliesWithin30);

        // 过滤出被攻击的队友
        return enemyTargets.Where(target => allAllies.Contains(target)).ToList();
    }

    /// <summary>
    /// 统计周围敌人攻击指定目标的数量
    /// </summary>
    /// <param name="targetToCheck">要检查的目标</param>
    /// <param name="radius">检查半径，默认30米</param>
    /// <param name="center">检查中心点，默认为玩家</param>
    /// <returns>攻击指定目标的敌人数量</returns>
    public static int CountEnemiesTargeting(IBattleChara targetToCheck, float radius = 30f, IBattleChara center = null)
    {
        if (targetToCheck == null || !targetToCheck.IsValid())
        {
            return 0;
        }

        // 如果没有指定中心点，默认使用玩家
        if (center == null)
        {
            center = Core.Me;
        }

        // 安全检查：确保中心点有效
        if (center == null || !center.IsValid())
        {
            return 0;
        }

        // 获取指定范围内的所有敌对目标
        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null &&
                enemy.IsValid() &&
                Vector3.Distance(enemy.Position, center.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        // 统计攻击指定目标的敌人数量
        int count = 0;
        foreach (var enemy in nearbyEnemies)
        {
            var target = enemy.GetCurrTarget();
            if (target != null && target.IsValid() && target.NameId == targetToCheck.NameId)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// 获取被最多敌人攻击的目标
    /// </summary>
    /// <param name="radius">检查半径，默认30米</param>
    /// <param name="center">检查中心点，默认为玩家</param>
    /// <returns>被最多敌人攻击的目标，如果没有则返回null</returns>
    public static IBattleChara GetMostTargetedAlly(float radius = 30f, IBattleChara center = null)
    {
        var enemyTargets = GetNearbyEnemiesTargetingAllies(radius, center);

        if (!enemyTargets.Any())
        {
            return null;
        }

        // 统计每个目标被攻击的次数
        var targetCounts = enemyTargets
            .GroupBy(target => target.NameId)
            .Select(group => new { Target = group.First(), Count = group.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        return targetCounts.FirstOrDefault()?.Target;
    }

    /// <summary>
    /// 检查指定目标是否被敌人攻击
    /// </summary>
    /// <param name="targetToCheck">要检查的目标</param>
    /// <param name="radius">检查半径，默认30米</param>
    /// <param name="center">检查中心点，默认为玩家</param>
    /// <returns>如果被攻击返回true，否则返回false</returns>
    public static bool IsTargetBeingAttacked(IBattleChara targetToCheck, float radius = 30f, IBattleChara center = null)
    {
        return CountEnemiesTargeting(targetToCheck, radius, center) > 0;
    }

    /// <summary>
    /// 如果周围指定距离的敌人都没有目标，则返回最近的敌人
    /// </summary>
    /// <param name="radius">检查半径，默认15米</param>
    /// <param name="center">检查中心点，默认为玩家</param>
    /// <param name="excludeBoss">是否排除Boss，默认false</param>
    /// <param name="onlyTargetable">是否只考虑可目标化的敌人，默认true</param>
    /// <returns>如果所有敌人都没有目标则返回最近的敌人，否则返回null</returns>
    public static IBattleChara GetNearestEnemyIfNoTargets(float radius = 25f, IBattleChara center = null,
      bool excludeBoss = false, bool onlyTargetable = true)
    {
        // 如果没有指定中心点，默认使用玩家
        if (center == null)
        {
            center = Core.Me;
        }

        // 安全检查：确保中心点有效
        if (center == null || !center.IsValid())
        {
            return null;
        }

        // 获取指定范围内的所有敌对目标
        var nearbyEnemies = Data.AllHostileTargets?
            .Where(enemy =>
                enemy != null &&
                enemy.IsValid() &&
                !enemy.IsDead &&
                Vector3.Distance(enemy.Position, center.Position) <= radius)
            .ToList() ?? new List<IBattleChara>();

        // 根据参数过滤敌人
        if (onlyTargetable)
        {
            nearbyEnemies = nearbyEnemies.Where(e => e.IsTargetable).ToList();
        }

        if (excludeBoss)
        {
            nearbyEnemies = nearbyEnemies.Where(e => !e.IsBoss()).ToList();
        }

        // 如果没有附近的敌人，返回null
        if (!nearbyEnemies.Any())
        {
            return null;
        }

        // 筛选出没有目标的敌人
        var enemiesWithNoTarget = nearbyEnemies.Where(enemy =>
        {
            var target = enemy.GetCurrTarget();
            return target == null || !target.IsValid() || target.IsDead;
        }).ToList();

        // 返回无目标敌人中最近的一个
        if (enemiesWithNoTarget.Any())
        {
            return enemiesWithNoTarget
                .OrderBy(enemy => Vector3.Distance(enemy.Position, center.Position))
                .FirstOrDefault();
        }

        return null; // 如果所有敌人都有目标，返回null
    }
}