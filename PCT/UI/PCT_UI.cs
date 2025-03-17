using AEAssist;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using Cindy_Master.Util;
using ImGuiNET;
using PCT.utils.Helper;


namespace Cindy_Master.PCT.Ui
{
    public static class PCT_UI
    {
        private static readonly ImageLoaderWindow imageLoaderWindow = new();
        public static void DrawQtGeneral(JobViewWindow jobViewWindow)
        {
            ImGui.Separator();
            ImGui.Text("当前版本v1.6.0");
            ImGui.Separator();

            ImGui.Separator();
            if (ImGui.Button("色魔画画"))
            {
                imageLoaderWindow.OnButtonClick();
            }
            imageLoaderWindow.RenderWindow();
            ImGui.Text("比较吃网速和流量, 建议不要开着梯子用");

            // 增加 3GCD 和 2GCD 起手的互斥选择
            ImGui.Separator();

            // 新增：日随模式和高难模式的互斥选择
            ImGui.Text("选择模式：");

            if (ImGui.Checkbox("日随模式", ref PCTSettings.Instance.日随模式))
            {
                if (PCTSettings.Instance.日随模式)
                {
                    PCTSettings.Instance.高难模式 = false;
                    PCTSettings.Instance.Save();
                }
            }

            if (ImGui.Checkbox("高难模式", ref PCTSettings.Instance.高难模式))
            {
                if (PCTSettings.Instance.高难模式)
                {
                    PCTSettings.Instance.日随模式 = false;
                    PCTSettings.Instance.Save();
                }
            }

            ImGui.Text("日随模式将不执行起手");
            ImGui.Separator();
            ImGui.Text("高难模式: 倒计时开始前 三画全满 倒计时>=5s");
            ImGui.Text("          倒计时开始前 三画未满 倒计时>=15s");
            ImGui.Text("未倒计时/不符合上述要求 电了别找我");
            ImGui.Separator();
            ImGui.Text("选择100级起手：");

            // 3GCD 起手的 Checkbox
            bool enable3GCDOpener = PCTSettings.Instance.Enable3GCDOpener;
            if (ImGui.Checkbox("3gcd 起手", ref enable3GCDOpener))
            {
                if (enable3GCDOpener)
                {
                    PCTSettings.Instance.Enable3GCDOpener = true;
                    PCTSettings.Instance.Enable2GCDOpener = false;
                    PCTSettings.Instance.Enable100EdenOpener = false;
                }
                PCTSettings.Instance.Save();
            }

            bool enableEdenOpener = PCTSettings.Instance.Enable100EdenOpener;
            if (ImGui.Checkbox("绝伊甸起手", ref enableEdenOpener))
            {
                if (enableEdenOpener)
                {
                    PCTSettings.Instance.Enable100EdenOpener = true;
                    PCTSettings.Instance.Enable100轴EdenOpener = false;
                    PCTSettings.Instance.Enable2GCDOpener = false;
                    PCTSettings.Instance.Enable3GCDOpener = false;
                }
                PCTSettings.Instance.Save();
            }

            bool enable轴EdenOpener = PCTSettings.Instance.Enable100轴EdenOpener;
            if (ImGui.Checkbox("绝伊甸轴only起手(必须用轴 起手打完进112)", ref enable轴EdenOpener))
            {
                if (enable轴EdenOpener)
                {
                    PCTSettings.Instance.Enable100EdenOpener = false;
                    PCTSettings.Instance.Enable100轴EdenOpener = true;
                    PCTSettings.Instance.Enable2GCDOpener = false;
                    PCTSettings.Instance.Enable3GCDOpener = false;
                }
                PCTSettings.Instance.Save();
            }

            // 2GCD 起手的 Checkbox
            bool enable2GCDOpener = PCTSettings.Instance.Enable2GCDOpener;
            if (ImGui.Checkbox("2gcd 起手", ref enable2GCDOpener))
            {
                if (enable2GCDOpener)
                {
                    PCTSettings.Instance.Enable2GCDOpener = true;
                    PCTSettings.Instance.Enable3GCDOpener = false;
                    PCTSettings.Instance.Enable100EdenOpener = false;
                }
                PCTSettings.Instance.Save();
            }
            ImGui.Text("需要保证战前三画 缺画则不执行起手");

            ImGui.Separator();
            // 在这里增加90级和90级Omega起手的选项
            ImGui.Text("选择90级起手：");

            // 获取当前设置
            bool enable90Opener = PCTSettings.Instance.Enable90Opener;
            bool enable90OmegaOpener = PCTSettings.Instance.Enable90OmegaOpener;
            bool enable90OmegaOpenerTest = PCTSettings.Instance.Enable90OmegaOpenerTest;
            bool enable90DragonOpener = PCTSettings.Instance.Enable90DragonOpener;

            // 90级起手
            if (ImGui.Checkbox("90级起手", ref enable90Opener))
            {
                if (enable90Opener)
                {
                    PCTSettings.Instance.Enable90Opener = true;
                    PCTSettings.Instance.Enable90OmegaOpener = false;
                    PCTSettings.Instance.Enable90OmegaOpenerTest = false;
                    PCTSettings.Instance.Enable90DragonOpener = false;
                }
                PCTSettings.Instance.Save();
            }

            // 90级Omega起手
            if (ImGui.Checkbox("90级Omega起手", ref enable90OmegaOpener))
            {
                if (enable90OmegaOpener)
                {
                    PCTSettings.Instance.Enable90OmegaOpener = true;
                    PCTSettings.Instance.Enable90OmegaOpenerTest = false;
                    PCTSettings.Instance.Enable90Opener = false;
                    PCTSettings.Instance.Enable90DragonOpener = false;
                }
                PCTSettings.Instance.Save();
            }

            // 90级Dragon起手
            if (ImGui.Checkbox("90级Dragon起手", ref enable90DragonOpener))
            {
                if (enable90DragonOpener)
                {
                    PCTSettings.Instance.Enable90DragonOpener = true;
                    PCTSettings.Instance.Enable90Opener = false;
                    PCTSettings.Instance.Enable90OmegaOpener = false;
                    PCTSettings.Instance.Enable90OmegaOpenerTest = false;
                }
                PCTSettings.Instance.Save();
            }
            /*if (ImGui.Checkbox("90级Omega理论最大威力起手", ref enable90OmegaOpenerTest))
            {

                if (enable90OmegaOpenerTest)
                {
                    PCTSettings.Instance.Enable90DragonOpener = false;
                    PCTSettings.Instance.Enable90Opener = false;
                    PCTSettings.Instance.Enable90OmegaOpener = false;
                    PCTSettings.Instance.Enable90OmegaOpenerTest = true;
                }
                PCTSettings.Instance.Save();
            }*/


            ImGui.Separator();
            ImGui.Text("注:在未满足阈值CD的情况下 将不会即刻画画!!!");
            // 设置动物彩绘CD阈值
            ImGui.Text("设置动物彩绘CD阈值（秒）：");
            double currentShengWuCD = PCTSettings.Instance.动物彩绘CD阈值;
            double newShengWuCD = currentShengWuCD;

            if (ImGui.InputDouble("##动物彩绘CD阈值", ref newShengWuCD, 1.0, 5.0, "%.1f"))
            {
                if (newShengWuCD >= 0 && newShengWuCD <= 40)
                {
                    PCTSettings.Instance.动物彩绘CD阈值 = newShengWuCD;
                    PCTSettings.Instance.Save();
                }
                else
                {
                    newShengWuCD = currentShengWuCD;
                }
            }
            ImGui.Text($"当前阈值: {PCTSettings.Instance.动物彩绘CD阈值} 秒");

            ImGui.Separator();

            // 设置武器彩绘CD阈值
            ImGui.Text("设置武器彩绘CD阈值（秒）：   (强烈推荐30)");
            double currentWuQiCD = PCTSettings.Instance.武器彩绘CD阈值;
            double newWuQiCD = currentWuQiCD;

            if (ImGui.InputDouble("##武器彩绘CD阈值", ref newWuQiCD, 1.0, 5.0, "%.1f"))
            {
                if (newWuQiCD >= 0 && newWuQiCD <= 60)
                {
                    PCTSettings.Instance.武器彩绘CD阈值 = newWuQiCD;
                    PCTSettings.Instance.Save();
                }
                else
                {
                    newWuQiCD = currentWuQiCD;
                }
            }
            ImGui.Text($"当前阈值: {PCTSettings.Instance.武器彩绘CD阈值} 秒");

            ImGui.Separator();

            // 设置风景彩绘CD阈值
            ImGui.Text("设置风景彩绘CD阈值（秒）：");
            double currentFengJingCD = PCTSettings.Instance.风景彩绘CD阈值;
            double newFengJingCD = currentFengJingCD;

            if (ImGui.InputDouble("##风景彩绘CD阈值", ref newFengJingCD, 1.0, 5.0, "%.1f"))
            {
                if (newFengJingCD >= 0 && newFengJingCD <= 120)
                {
                    PCTSettings.Instance.风景彩绘CD阈值 = newFengJingCD;
                    PCTSettings.Instance.Save();
                }
                else
                {
                    newFengJingCD = currentFengJingCD;
                }
            }
            ImGui.Text($"当前阈值: {PCTSettings.Instance.风景彩绘CD阈值} 秒");

            ImGui.Separator();

            // 设置至少保留多少动物层数
            ImGui.Text("设置至少保留多少动物层数：");
            int currentShengWuLayer = PCTSettings.Instance.动物层数;
            int newShengWuLayer = currentShengWuLayer;

            if (ImGui.InputInt("##动物层数", ref newShengWuLayer))
            {
                if (newShengWuLayer >= 0 && newShengWuLayer <= 2)
                {
                    PCTSettings.Instance.动物层数 = newShengWuLayer;
                    PCTSettings.Instance.Save();
                }
                else
                {
                    newShengWuLayer = currentShengWuLayer;
                }
            }
            ImGui.Text($"当前设置至少保留层数: {PCTSettings.Instance.动物层数}");


            ImGui.Separator();
            ImGui.Text("重置QT:");
            ImGui.BeginGroup(); // Start group to keep checkbox and button on the same line

            // Checkbox to enable or disable QT reset
            bool resetQT = PCTSettings.Instance.QT重置;  // Initialize a variable to hold checkbox state
            if (ImGui.Checkbox("重新进入战斗重置QT", ref resetQT))
            {
                PCTSettings.Instance.QT重置 = resetQT;
                PCTSettings.Instance.Save();
            }



            // Button to initialize QT
            if (ImGui.Button("初始化QT"))
            {
                // Call the initialization logic for QT
                Helper.Reset();  // Assuming there is a method to initialize QT

            }


            ImGui.EndGroup(); // End group to stop keeping the checkbox and button on the same line

            ImGui.Separator();


            ImGui.Text("聊天框设置：");

            // Add checkbox for "聊天框提示读条"
            bool enableBuffReadout = PCTSettings.Instance.聊天框提示读条;
            if (ImGui.Checkbox("聊天框提示读条", ref enableBuffReadout))
            {
                PCTSettings.Instance.聊天框提示读条 = enableBuffReadout;
                PCTSettings.Instance.Save();
            }

            // Add checkbox for "聊天框提示瞬发"
            bool enableInstantCastReadout = PCTSettings.Instance.聊天框提示瞬发;
            if (ImGui.Checkbox("聊天框提示瞬发", ref enableInstantCastReadout))
            {
                PCTSettings.Instance.聊天框提示瞬发 = enableInstantCastReadout;
                PCTSettings.Instance.Save();
            }

            ImGui.Separator();
            ImGui.Text("选择模式：");

            bool enablelaiyifaMode = PCTSettings.Instance.奔放模式;

            // 创建一个复选框，用户可以通过它来启用或禁用“来一发”模式
            if (ImGui.Checkbox("奔放模式", ref enablelaiyifaMode))
            {
                PCTSettings.Instance.奔放模式 = enablelaiyifaMode;
                PCTSettings.Instance.Save();
            }
            ImGui.Text("移动施法才开");
            ImGui.Separator();
            ImGui.Separator();
            ImGui.Text("Debug");
            // int buffStacks = Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.锤子预备);
            //ImGui.Text($"锤子预备层数: {buffStacks}");
            //ImGui.Text($"下一个GCD {new PictomancerRotationEntry().下一个GCD()}");
            ImGui.Text($"即刻: {PictomancerRotationEntry.QT.GetQt(QTKey.即刻)}");
            ImGui.Text($"即刻: {PCT_Data.Spells.即刻.GetSpell().IsReadyWithCanCast()}");
            ImGui.Text($"即刻: {Core.Me.IsMoving() && !Core.Me.HasAura(167) && !PCT_Data.Spells.即刻.GetSpell().IsReadyWithCanCast() && !PictomancerRotationEntry.QT.GetQt(QTKey.即刻)}");
            //ImGui.Text($"反即刻: {!PictomancerRotationEntry.QT.GetQt(QTKey.即刻)}");
            ImGui.Separator();


            // 获取当前目标（假设目标是玩家的当前敌人或目标）
            var target = Core.Me.GetCurrTarget();

            // 确保目标有效且非空
            if (target != null && target.IsValid())
            {
                // 打印当前目标的Hitbox大小
                float targetHitboxSize = target.HitboxRadius;
                ImGui.Text($"当前目标的Hitbox大小: {targetHitboxSize:F2}");
            }
            else
            {
                ImGui.Text("当前没有目标或目标无效");
            }

            ImGui.Text($"{(34674u).GetSpell().Cooldown.TotalSeconds}");



        }


    }

}