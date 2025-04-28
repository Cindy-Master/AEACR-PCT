using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.Setting;
using Cindy_Master.Util;
using ImGuiNET;
using PCT.utils.Helper;
using System.Numerics;

namespace Cindy_Master.PCT.Ui
{
    public static class PCT_UI
    {
        private static readonly ImageLoaderWindow imageLoaderWindow = new();

        // 炫酷颜色定义
        private static readonly Vector4 标题颜色 = new Vector4(0.9f, 0.5f, 0.1f, 1.0f);
        private static readonly Vector4 副标题颜色 = new Vector4(0.3f, 0.7f, 0.9f, 1.0f);
        private static readonly Vector4 警告颜色 = new Vector4(1.0f, 0.5f, 0.0f, 1.0f);
        private static readonly Vector4 提示颜色 = new Vector4(0.0f, 0.8f, 0.8f, 1.0f);
        private static readonly Vector4 版本颜色 = new Vector4(0.8f, 0.8f, 0.2f, 1.0f);
        private static readonly Vector4 模式颜色 = new Vector4(0.6f, 0.3f, 0.9f, 1.0f);
        private static readonly Vector4 按钮颜色 = new Vector4(0.2f, 0.4f, 0.8f, 0.8f);
        private static readonly Vector4 按钮悬停颜色 = new Vector4(0.3f, 0.5f, 0.9f, 0.9f);
        private static readonly Vector4 分隔线颜色 = new Vector4(0.5f, 0.5f, 0.7f, 0.8f);
        private static readonly Vector4 Debug颜色 = new Vector4(0.7f, 0.2f, 0.3f, 1.0f);

        // 折叠控制变量
        private static bool 显示战斗模式 = true;
        private static bool 显示模式说明 = true;
        private static bool 显示100级起手 = true;
        private static bool 显示90级起手 = true;
        private static bool 显示彩绘CD设置 = true;
        private static bool 显示其他设置 = true;
        private static bool 显示调试信息 = false;

        public static void DrawQtGeneral(JobViewWindow jobViewWindow)
        {
            // 设置整体样式
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 8.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(6, 6));
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(8, 8));

            // 版本信息
            DrawSeparator();
            ImGui.PushStyleColor(ImGuiCol.Text, 版本颜色);
            ImGui.TextWrapped("色魔绘师 v1.6.1");
            ImGui.PopStyleColor();
            DrawSeparator();

            // 色魔画画按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0.4f, 0.6f, 1.0f, 1.0f));

            if (ImGui.Button("色魔画廊", new Vector2(ImGui.GetContentRegionAvail().X * 0.8f, 36)))
            {
                imageLoaderWindow.OnButtonClick();
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text("打开色魔画廊，查看和加载图像");
                ImGui.EndTooltip();
            }

            ImGui.PopStyleColor(3);
            imageLoaderWindow.RenderWindow();

            ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
            ImGui.TextWrapped("比较吃网速和流量, 建议不要开着梯子用");
            ImGui.PopStyleColor();

            // 模式选择部分
            DrawSeparator();

            // 战斗模式折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示战斗模式 ? "收起战斗模式设置" : "展开战斗模式设置"))
            {
                显示战斗模式 = !显示战斗模式;
            }
            ImGui.PopStyleColor(2);

            if (显示战斗模式)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 模式颜色);
                ImGui.TextWrapped("战斗模式选择");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.25f, 0.25f, 0.35f, 0.8f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.4f, 0.8f, 1.0f, 1.0f));

                bool 日随模式 = PCTSettings.Instance.日随模式;
                if (ImGui.Checkbox("日随模式", ref 日随模式))
                {
                    if (日随模式)
                    {
                        PCTSettings.Instance.日随模式 = true;
                        PCTSettings.Instance.高难模式 = false;
                        PCTSettings.Instance.Save();
                    }
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("适合日常随机副本的模式，不会执行起手");
                    ImGui.EndTooltip();
                }

                bool 高难模式 = PCTSettings.Instance.高难模式;
                if (ImGui.Checkbox("高难模式", ref 高难模式))
                {
                    if (高难模式)
                    {
                        PCTSettings.Instance.高难模式 = true;
                        PCTSettings.Instance.日随模式 = false;
                        PCTSettings.Instance.Save();
                    }
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("适合高难度副本的模式，会执行精确的起手");
                    ImGui.EndTooltip();
                }

                // 奔放模式
                bool enablelaiyifaMode = PCTSettings.Instance.奔放模式;
                if (ImGui.Checkbox("奔放模式", ref enablelaiyifaMode))
                {
                    PCTSettings.Instance.奔放模式 = enablelaiyifaMode;
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("移动施法时才开启");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(2);

                // 自动锁定目标
                bool autoLockTarget = PCTSettings.Instance.高难起手自动锁目标;
                if (ImGui.Checkbox("自动锁目标", ref autoLockTarget))
                {
                    PCTSettings.Instance.高难起手自动锁目标 = autoLockTarget;
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("高难模式下自动锁定目标");
                    ImGui.EndTooltip();
                }
            }

            DrawSeparator();

            // 模式说明折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示模式说明 ? "收起模式说明" : "展开模式说明"))
            {
                显示模式说明 = !显示模式说明;
            }
            ImGui.PopStyleColor(2);

            if (显示模式说明)
            {
                // 模式说明
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.TextWrapped("日随模式将不执行起手");
                ImGui.TextWrapped("高难模式下:");
                ImGui.TextWrapped("     倒计时开始前 三画全满 倒计时>=5s");
                ImGui.TextWrapped("     倒计时开始前 三画未满 倒计时>=15s");
                ImGui.TextWrapped("     未倒计时/不符合上述要求 电了别找我");
                ImGui.TextWrapped("     用起手的时候 最好打开 fuck 减少动画锁 不然可能会因为动画锁微卡gcd");
                ImGui.PopStyleColor();
            }

            DrawSeparator();

            // 100级起手选择折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示100级起手 ? "收起100级起手设置" : "展开100级起手设置"))
            {
                显示100级起手 = !显示100级起手;
            }
            ImGui.PopStyleColor(2);

            if (显示100级起手)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 副标题颜色);
                ImGui.TextWrapped("选择100级起手");
                ImGui.PopStyleColor();
                ImGui.Text("需要保证战前三画 缺画则不执行起手");

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.5f, 0.9f, 0.5f, 1.0f));

                // 3GCD 起手
                bool enable3GCDOpener = PCTSettings.Instance.Enable3GCDOpener;
                if (ImGui.Checkbox("3gcd 起手", ref enable3GCDOpener))
                {
                    if (enable3GCDOpener)
                    {
                        PCTSettings.Instance.Enable3GCDOpener = true;
                        PCTSettings.Instance.Enable100轴EdenOpener = false;
                        PCTSettings.Instance.Enable2GCDOpener = false;
                        PCTSettings.Instance.Enable100EdenOpener = false;
                        PCTSettings.Instance.Enable100FastOpener = false;
                    }
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用3GCD的起手战术");
                    ImGui.EndTooltip();
                }
                bool enable100FastOpener = PCTSettings.Instance.Enable100FastOpener;
                if (ImGui.Checkbox("3g速泄 起手", ref enable100FastOpener))
                {
                    if (enable100FastOpener)
                    {
                        PCTSettings.Instance.Enable3GCDOpener = false;
                        PCTSettings.Instance.Enable100FastOpener = true;
                        PCTSettings.Instance.Enable100轴EdenOpener = false;
                        PCTSettings.Instance.Enable2GCDOpener = false;
                        PCTSettings.Instance.Enable100EdenOpener = false;
                    }
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用3GCD的速泄彩虹起手战术");
                    ImGui.EndTooltip();
                }
                // 绝伊甸起手
                bool enableEdenOpener = PCTSettings.Instance.Enable100EdenOpener;
                if (ImGui.Checkbox("绝伊甸起手", ref enableEdenOpener))
                {
                    if (enableEdenOpener)
                    {
                        PCTSettings.Instance.Enable100EdenOpener = true;
                        PCTSettings.Instance.Enable100FastOpener = false;
                        PCTSettings.Instance.Enable100轴EdenOpener = false;
                        PCTSettings.Instance.Enable2GCDOpener = false;
                        PCTSettings.Instance.Enable3GCDOpener = false;
                    }
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用100级伊甸副本特化的起手战术");
                    ImGui.EndTooltip();
                }

                // 绝伊甸轴only起手
                bool enable轴EdenOpener = PCTSettings.Instance.Enable100轴EdenOpener;
                if (ImGui.Checkbox("绝伊甸轴only起手(必须用轴 起手打完进112)", ref enable轴EdenOpener))
                {
                    if (enable轴EdenOpener)
                    {
                        PCTSettings.Instance.Enable100EdenOpener = false;
                        PCTSettings.Instance.Enable100FastOpener = false;
                        PCTSettings.Instance.Enable100轴EdenOpener = true;
                        PCTSettings.Instance.Enable2GCDOpener = false;
                        PCTSettings.Instance.Enable3GCDOpener = false;
                    }
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用按时间轴设计的100级伊甸起手战术");
                    ImGui.EndTooltip();
                }

                // 2GCD 起手
                bool enable2GCDOpener = PCTSettings.Instance.Enable2GCDOpener;
                if (ImGui.Checkbox("2gcd 起手", ref enable2GCDOpener))
                {
                    if (enable2GCDOpener)
                    {
                        PCTSettings.Instance.Enable2GCDOpener = true;
                        PCTSettings.Instance.Enable100FastOpener = false;
                        PCTSettings.Instance.Enable3GCDOpener = false;
                        PCTSettings.Instance.Enable100轴EdenOpener = false;
                        PCTSettings.Instance.Enable100EdenOpener = false;
                    }
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用2GCD的起手战术，为了GCD防卡 在打开爆发药的情况下会延后武器构想的插入");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(2);
            }

            DrawSeparator();

            // 90级起手选择折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示90级起手 ? "收起90级起手设置" : "展开90级起手设置"))
            {
                显示90级起手 = !显示90级起手;
            }
            ImGui.PopStyleColor(2);

            if (显示90级起手)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 副标题颜色);
                ImGui.TextWrapped("选择90级起手");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.5f, 0.9f, 0.5f, 1.0f));

                // 90级起手
                bool enable90Opener = PCTSettings.Instance.Enable90Opener;
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
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用90级标准起手战术");
                    ImGui.EndTooltip();
                }

                // 90级Omega起手
                bool enable90OmegaOpener = PCTSettings.Instance.Enable90OmegaOpener;
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
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用90级绝欧米茄副本特化的起手战术");
                    ImGui.EndTooltip();
                }

                // 90级Dragon起手
                bool enable90DragonOpener = PCTSettings.Instance.Enable90DragonOpener;
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
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("使用90级绝龙诗副本特化的起手战术");
                    ImGui.EndTooltip();
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

                ImGui.PopStyleColor(2);
            }

            DrawSeparator();

            // 彩绘CD设置折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示彩绘CD设置 ? "收起彩绘CD设置" : "展开彩绘CD设置"))
            {
                显示彩绘CD设置 = !显示彩绘CD设置;
            }
            ImGui.PopStyleColor(2);

            if (显示彩绘CD设置)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 警告颜色);
                ImGui.TextWrapped("注:在未满足阈值CD的情况下 将不会即刻画画!!!");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.Text, 副标题颜色);
                ImGui.TextWrapped("彩绘CD阈值设置");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.SliderGrab, new Vector4(0.4f, 0.5f, 0.9f, 0.8f));
                ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new Vector4(0.5f, 0.6f, 1.0f, 1.0f));

                // 设置动物彩绘CD阈值
                float width = ImGui.GetContentRegionAvail().X * 0.7f;

                ImGui.Text("动物彩绘CD阈值（秒）：");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("动物彩绘的冷却时间阈值，低于此值不会自动使用");
                    ImGui.EndTooltip();
                }

                double currentShengWuCD = PCTSettings.Instance.动物彩绘CD阈值;
                double newShengWuCD = currentShengWuCD;

                ImGui.SetNextItemWidth(width);
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
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.Text($"当前阈值: {PCTSettings.Instance.动物彩绘CD阈值} 秒");
                ImGui.PopStyleColor();

                // 设置武器彩绘CD阈值
                ImGui.Text("武器彩绘CD阈值（秒）：   (强烈推荐30)  (大于60s就是在没有层数的情况下画)");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("武器彩绘的冷却时间阈值，低于此值不会自动使用");
                    ImGui.EndTooltip();
                }

                double currentWuQiCD = PCTSettings.Instance.武器彩绘CD阈值;
                double newWuQiCD = currentWuQiCD;

                ImGui.SetNextItemWidth(width);
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
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.Text($"当前阈值: {PCTSettings.Instance.武器彩绘CD阈值} 秒");
                ImGui.PopStyleColor();

                // 设置风景彩绘CD阈值
                ImGui.Text("风景彩绘CD阈值（秒）：");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("风景彩绘的冷却时间阈值，低于此值不会自动使用");
                    ImGui.EndTooltip();
                }

                double currentFengJingCD = PCTSettings.Instance.风景彩绘CD阈值;
                double newFengJingCD = currentFengJingCD;

                ImGui.SetNextItemWidth(width);
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
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.Text($"当前阈值: {PCTSettings.Instance.风景彩绘CD阈值} 秒");
                ImGui.PopStyleColor();

                ImGui.PopStyleColor(3);
            }

            DrawSeparator();

            // 其他设置折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示其他设置 ? "收起其他设置" : "展开其他设置"))
            {
                显示其他设置 = !显示其他设置;
            }
            ImGui.PopStyleColor(2);

            if (显示其他设置)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, 副标题颜色);
                ImGui.TextWrapped("其他设置");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.SliderGrab, new Vector4(0.4f, 0.5f, 0.9f, 0.8f));
                ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new Vector4(0.5f, 0.6f, 1.0f, 1.0f));

                // 设置至少保留多少动物层数
                float width = ImGui.GetContentRegionAvail().X * 0.7f;

                ImGui.Text("设置至少保留多少动物层数：");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("自动保留指定层数的动物彩绘，不会使其消耗至零");
                    ImGui.EndTooltip();
                }

                int currentShengWuLayer = PCTSettings.Instance.动物层数;
                int newShengWuLayer = currentShengWuLayer;

                ImGui.SetNextItemWidth(width);
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
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.Text($"当前设置至少保留层数: {PCTSettings.Instance.动物层数}");
                ImGui.PopStyleColor();

                // 设置锤子层数阈值
                ImGui.Text("设置爆发期还剩下多少层加速的情况下打锤子：");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("在爆发期间，当加速层数低于此值时会使用锤子");
                    ImGui.EndTooltip();
                }

                int currentHammerLayers = PCTSettings.Instance.多少层打锤子;
                int newHammerLayers = currentHammerLayers;

                ImGui.SetNextItemWidth(width);
                if (ImGui.InputInt("##多少层打锤子阈值", ref newHammerLayers))
                {
                    if (newHammerLayers >= 0 && newHammerLayers <= 5)
                    {
                        PCTSettings.Instance.多少层打锤子 = newHammerLayers;
                        PCTSettings.Instance.Save();
                    }
                    else
                    {
                        newHammerLayers = currentHammerLayers;
                    }
                }
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.Text($"当前层数阈值: {PCTSettings.Instance.多少层打锤子}");
                ImGui.PopStyleColor();

                // 设置TTK阈值
                ImGui.PushStyleColor(ImGuiCol.Text, 警告颜色);
                ImGui.TextWrapped("TTK（Time To Kill）是预计击杀目标所需的剩余时间，合理设置可避免BOSS即将死亡时浪费彩绘时间");
                ImGui.PopStyleColor();

                ImGui.Text("设置TTK阈值（毫秒）：");
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("当目标剩余击杀时间低于此值时，将停止使用彩绘技能");
                    ImGui.EndTooltip();
                }

                int currentTTK = PCTSettings.Instance.TTK阈值;
                int newTTK = currentTTK;

                ImGui.SetNextItemWidth(width);
                if (ImGui.InputInt("##TTK阈值", ref newTTK, 1000, 5000))
                {
                    if (newTTK >= 5000 && newTTK <= 20000)
                    {
                        PCTSettings.Instance.TTK阈值 = newTTK;
                        PCTSettings.Instance.Save();
                    }
                    else
                    {
                        newTTK = currentTTK;
                    }
                }
                ImGui.PushStyleColor(ImGuiCol.Text, 提示颜色);
                ImGui.Text($"当前TTK阈值: {PCTSettings.Instance.TTK阈值} 毫秒（{PCTSettings.Instance.TTK阈值 / 1000.0:F1}秒）");
                ImGui.PopStyleColor();

                // 重置TTK阈值按钮
                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.9f, 0.4f, 0.2f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1.0f, 0.5f, 0.3f, 0.9f));
                if (ImGui.Button("重置TTK阈值为默认值(15秒)", new Vector2(ImGui.GetContentRegionAvail().X * 0.5f, 25)))
                {
                    PCTSettings.Instance.TTK阈值 = 15000;
                    PCTSettings.Instance.Save();
                    newTTK = 15000;
                }
                ImGui.PopStyleColor(2);



                // 聊天框设置
                ImGui.PushStyleColor(ImGuiCol.Text, 副标题颜色);
                ImGui.TextWrapped("聊天框设置：");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.5f, 0.9f, 0.5f, 1.0f));

                // 聊天框提示读条
                bool enableBuffReadout = PCTSettings.Instance.聊天框提示读条;
                if (ImGui.Checkbox("聊天框提示读条", ref enableBuffReadout))
                {
                    PCTSettings.Instance.聊天框提示读条 = enableBuffReadout;
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("在聊天框中显示读条技能的提示信息");
                    ImGui.EndTooltip();
                }

                // 聊天框提示瞬发
                bool enableInstantCastReadout = PCTSettings.Instance.聊天框提示瞬发;
                if (ImGui.Checkbox("聊天框提示瞬发", ref enableInstantCastReadout))
                {
                    PCTSettings.Instance.聊天框提示瞬发 = enableInstantCastReadout;
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("在聊天框中显示瞬发技能的提示信息");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(2);

                // QT重置设置
                ImGui.PushStyleColor(ImGuiCol.Text, 副标题颜色);
                ImGui.TextWrapped("QT重置设置：");
                ImGui.PopStyleColor();

                ImGui.PushStyleColor(ImGuiCol.FrameBg, new Vector4(0.2f, 0.25f, 0.4f, 0.7f));
                ImGui.PushStyleColor(ImGuiCol.CheckMark, new Vector4(0.5f, 0.9f, 0.5f, 1.0f));
                ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);

                // QT重置复选框
                bool resetQT = PCTSettings.Instance.QT重置;
                if (ImGui.Checkbox("重新进入战斗重置QT", ref resetQT))
                {
                    PCTSettings.Instance.QT重置 = resetQT;
                    PCTSettings.Instance.Save();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("重新进入战斗时自动重置QT计时器");
                    ImGui.EndTooltip();
                }

                // 初始化QT按钮
                if (ImGui.Button("初始化QT", new Vector2(ImGui.GetContentRegionAvail().X * 0.4f, 30)))
                {
                    Helper.Reset();
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("手动重置QT计时器");
                    ImGui.EndTooltip();
                }

                ImGui.PopStyleColor(4);
            }

            DrawSeparator();

            // Debug信息折叠/展开按钮
            ImGui.PushStyleColor(ImGuiCol.Button, 按钮颜色);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, 按钮悬停颜色);
            if (ImGui.Button(显示调试信息 ? "收起调试信息" : "展开调试信息"))
            {
                显示调试信息 = !显示调试信息;
            }
            ImGui.PopStyleColor(2);

            if (显示调试信息)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, Debug颜色);
                ImGui.TextWrapped("Debug信息");
                ImGui.PopStyleColor();

                // Debug信息
                ImGui.Text($"即刻: {PictomancerRotationEntry.QT.GetQt(QTKey.即刻)}");
                ImGui.Text($"即刻技能状态: {PCT_Data.Spells.即刻.GetSpell().IsReadyWithCanCast()}");
                ImGui.Text($"移动判定: {Core.Me.IsMoving() && !Core.Me.HasAura(167) && !PCT_Data.Spells.即刻.GetSpell().IsReadyWithCanCast() && !PictomancerRotationEntry.QT.GetQt(QTKey.即刻)}");

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

                ImGui.Text($"加速层数: {Helper.目标的指定BUFF层数(Core.Me, PCT_Data.Buffs.加速)}");
            }

            // 恢复样式
            ImGui.PopStyleVar(3);

            if (ImGui.TreeNode("插入技能状态"))
            {
                if (ImGui.Button("清除队列"))
                {
                    AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                    AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
                }
                ImGui.SameLine();
                if (ImGui.Button("清除一个"))
                {
                    AI.Instance.BattleData.HighPrioritySlots_OffGCD.Dequeue();
                    AI.Instance.BattleData.HighPrioritySlots_GCD.Dequeue();
                }
                ImGui.Text("-------能力技-------");
                if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
                    foreach (var action in AI.Instance.BattleData.HighPrioritySlots_OffGCD.SelectMany(spell => spell.Actions))
                    {
                        ImGui.Text(action.Spell.Name);
                    }
                ImGui.Text("-------GCD-------");
                if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
                    foreach (var action in AI.Instance.BattleData.HighPrioritySlots_GCD.SelectMany(spell => spell.Actions))
                    {
                        ImGui.Text(action.Spell.Name);
                    }
                ImGui.TreePop();
            }
        }

        private static void DrawSeparator()
        {
            ImGui.PushStyleColor(ImGuiCol.Separator, 分隔线颜色);
            ImGui.Separator();
            ImGui.PopStyleColor();
        }
    }
}