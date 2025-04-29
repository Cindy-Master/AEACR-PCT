using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Cindy_Master.PCT;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using PCT.utils.Helper;
namespace Cinndy_Master.PCT
{
    public class PictomancerRotationEventHandler : IRotationEventHandler
    {
        private readonly HashSet<uint> _targetSpellIds = new HashSet<uint>
        {
            PCT_Data.Spells.动物彩绘1,
            PCT_Data.Spells.动物彩绘2,
            PCT_Data.Spells.动物彩绘4,
            PCT_Data.Spells.动物彩绘3,
            PCT_Data.Spells.风景画,
            PCT_Data.Spells.武器画,
            PCT_Data.Spells.火炎,
            PCT_Data.Spells.疾风,
            PCT_Data.Spells.流水,
            PCT_Data.Spells.烈风,
            PCT_Data.Spells.烈水,
            PCT_Data.Spells.烈炎,
            PCT_Data.Spells.冰结,
            PCT_Data.Spells.震雷,
            PCT_Data.Spells.坚石,
            PCT_Data.Spells.冰冻,
            PCT_Data.Spells.闪雷,
            PCT_Data.Spells.飞石,

        };

        private static bool lastStopValue = false;
        public static bool manualOverride = false;
        private int originalCastingSpellSuccessRemainTiming;
        private bool originalNoClipGCD3; // 新增字段用于存储原始 NoClipGCD3 状态

        public static void Stop()
        {
            if (StopBuffCheck)
            {
                PlayerOptions.Instance.Stop = true;
                manualOverride = false;
                Core.Resolve<MemApiSpell>().CancelCast();
            }
            else
            {
                if (PlayerOptions.Instance.Stop != lastStopValue)
                {
                    manualOverride = true;
                }

                if (!manualOverride)
                {
                    PlayerOptions.Instance.Stop = false;
                }
            }

            lastStopValue = PlayerOptions.Instance.Stop;
        }

        public static bool StopBuffCheck => !PictomancerRotationEntry.QT.GetQt(QTKey.自动停手) ||
                                            Helper.自身存在其中Buff(PCT_Data.Buffs.InvincibleBuff) ||
                                            Helper.目标是否拥有其中的BUFF(PCT_Data.Buffs.UnattackableBuff);

        public void OnResetBattle()
        {
            Stop();

            if (PCTSettings.Instance.QT重置)
            {
                Helper.Reset();
            }
            PCTSettings.Instance.SaveQtStates(PictomancerRotationEntry.QT);
        }

        public async Task OnNoTarget()
        {
            if (!Helper.自身是否在移动())
            {
                if (!Core.Me.HasAura(PCT_Data.Buffs.星空))
                {
                    if (PictomancerRotationEntry.QT.GetQt(QTKey.自动绘画) && Helper.是否在副本中() && !Core.Resolve<MemApiDuty>().IsOver)
                    {
                        if (!Core.Resolve<JobApi_Pictomancer>().武器画 && (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器彩绘).GetSpell().IsReadyWithCanCast()) && !Core.Me.IsMoving())
                        {
                            await Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器彩绘).GetSpell().Cast();
                            LogHelper.Info("无目标武器画");
                        }

                        if (!Core.Resolve<JobApi_Pictomancer>().生物画 && (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘).GetSpell().IsReadyWithCanCast()) && !Core.Me.IsMoving())
                        {
                            await Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘).GetSpell().Cast();
                            LogHelper.Info("无目标动物画");
                        }

                        if (!Core.Resolve<JobApi_Pictomancer>().风景画 && (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景彩绘).GetSpell().IsReadyWithCanCast()) && !Core.Me.IsMoving())
                        {
                            await Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景彩绘).GetSpell().Cast();
                            LogHelper.Info("无目标风景画");
                        }

                        await Task.CompletedTask;
                    }
                }
            }
        }

        public void OnSpellCastSuccess(Slot slot, Spell spell)
        {
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;
        }

        public async Task OnPreCombat()
        {

            if (PictomancerRotationEntry.QT.GetQt(QTKey.自动绘画) && Helper.是否在副本中() && !Core.Resolve<MemApiDuty>().IsOver)
            {
                if (!Core.Resolve<JobApi_Pictomancer>().武器画 && (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器彩绘)).GetSpell().IsReadyWithCanCast())
                {
                    await Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.武器彩绘).GetSpell().Cast();
                    LogHelper.Info("脱战武器画");
                }

                if (!Core.Resolve<JobApi_Pictomancer>().生物画 && (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘)).GetSpell().IsReadyWithCanCast())
                {
                    await Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.动物彩绘).GetSpell().Cast();
                    LogHelper.Info("脱战动物画");
                }

                if (!Core.Resolve<JobApi_Pictomancer>().风景画 && (Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景彩绘)).GetSpell().IsReadyWithCanCast())
                {
                    await Core.Resolve<MemApiSpell>().CheckActionChange(PCT_Data.Spells.风景彩绘).GetSpell().Cast();
                    LogHelper.Info("脱战风景画");
                }

                await Task.CompletedTask;
            }

        }

        public void AfterSpell(Slot slot, Spell spell)
        {
            if (_targetSpellIds.Contains(spell.Id))
            {
                AI.Instance.BattleData.CurrGcdAbilityCount = 1;
            }
            else
            {
                LogHelper.Info($"施放的技能 {spell.Name} ({spell.Id}) 不在目标列表中");
            }
        }

        public void OnBattleUpdate(int currTime)
        {
            if (SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming > 250)
            {
                //originalCastingSpellSuccessRemainTiming = SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming;
                //SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming = 250;
                //LogHelper.PrintError("[WARNING]\n检测到 读条技能成功判定的剩余时间大于250 , 请调整至250, 否则会严重影响能力技插入。 已帮您停手");
                //PlayerOptions.Instance.Stop = true;
            }
            if (SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3 == true)
            {
                // 保存原始状态
                //originalNoClipGCD3 = SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3;

                // 设置为 false
                //SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3 = false;
                //LogHelper.PrintError("[WARNING]\n检测到打开了全局不卡GCD, 请关闭, 不关闭会严重卡顿 RGB 的能力技插入。已帮您停手");
                //PlayerOptions.Instance.Stop = true;

            }
            if (Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.长Buff) == 1 && PictomancerRotationEntry.QT.GetQt(QTKey.测112) && Core.Me.HasAura(PCT_Data.Buffs.蓝红))
            {
                PictomancerRotationEntry.QT.SetQt(QTKey.测112, false);
            }
        }

        public void OnEnterRotation()
        {
            LogHelper.Print("[Cindy-Master]\n画家ACR 1.6.1 版本");
            Core.Resolve<MemApiChatMessage>().Toast2("[Cindy-Master]\n画家ACR 1.6.1 版本\n高难模式请勿打开优化GCD偏移选项!!!", 1, 3000);

            if (PCTSettings.Instance.日随模式)
            {
                LogHelper.Print("[WARNING]\n您当前使用的是 日随模式 请确认使用场景无问题\n日随模式下将不会 [执行起手]");
            }
            if (PCTSettings.Instance.高难模式)
            {
                LogHelper.Print("[WARNING]\n您当前使用的是 高难模式 请确认使用场景无问题");
            }

            // 修正条件判断中的赋值操作符错误
            if (SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3 == true)
            {
                // 保存原始状态
                //originalNoClipGCD3 = SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3;

                // 设置为 false
                //SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3 = false;
                //LogHelper.PrintError("[WARNING]\n检测到打开了全局不卡GCD, 请关闭, 不关闭会严重卡顿 RGB 的能力技插入。已帮您停手");
                //PlayerOptions.Instance.Stop = true;

            }

            if (PCTSettings.Instance.武器彩绘CD阈值 < 20)
            {
                PCTSettings.Instance.武器彩绘CD阈值 = 30;
            }

            if (PCTSettings.Instance.高难模式 && SettingMgr.GetSetting<GeneralSettings>().OptimizeGcd == true)
            {
                //originalCastingSpellSuccessRemainTiming = SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming;
                //SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming = 250;
                LogHelper.PrintError("[WARNING]\n检测到 您当前处于高难模式 且 启用了优化GCD偏移  , 请前往 ACR 首页 设置 中关闭, 否则会严重影响起手。 已帮您停手");
                PlayerOptions.Instance.Stop = true;


            }
            if (SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming > 250)
            {
                //originalCastingSpellSuccessRemainTiming = SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming;
                //SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming = 250;
                //LogHelper.PrintError("[WARNING]\n检测到 读条技能成功判定的剩余时间大于250 , 请调整至250, 否则会严重影响能力技插入。 已帮您停手");
                //PlayerOptions.Instance.Stop = true;


            }

        }

        public void OnExitRotation()
        {
            // 恢复 CastingSpellSuccessRemainTiming
            //SettingMgr.GetSetting<GeneralSettings>().CastingSpellSuccessRemainTiming = originalCastingSpellSuccessRemainTiming;
            //LogHelper.Print($"[WARNING]\n[Cindy-Master]画家ACR关闭 还原读条技能成功判定的剩余时间为{originalCastingSpellSuccessRemainTiming}");

            // 恢复 NoClipGCD3
            //SettingMgr.GetSetting<GeneralSettings>().NoClipGCD3 = originalNoClipGCD3;
            //LogHelper.Print("[WARNING]\n[Cindy-Master]画家ACR关闭 还原 NoClipGCD3 的状态");
            PCTSettings.Instance.SaveQtStates(PictomancerRotationEntry.QT);
        }

        public void OnTerritoryChanged()
        {
            PCTSettings.Instance.SaveQtStates(PictomancerRotationEntry.QT);
        }
    }
}
