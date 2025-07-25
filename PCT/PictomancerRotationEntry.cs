using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using Cindy_Master.PCT.Ability;
using Cindy_Master.PCT.Data;
using Cindy_Master.PCT.GCD;
using Cindy_Master.PCT.Opener;
using Cindy_Master.PCT.Setting;
using Cindy_Master.PCT.Trigger;
using Cindy_Master.PCT.Ui;
using Cindy_Master.Trigger;
using Cinndy_Master.PCT;
using PatchouliTC.Common;

namespace Cindy_Master.PCT
{
    public class PictomancerRotationEntry : IRotationEntry
    {

        public void Dispose()
        {
        }

        public string AuthorName { get; set; } = "Cindy_Master";

        private List<SlotResolverData> SlotResolvers = new()
        {



            new SlotResolverData(new PCT_加速GCD_天星(), SlotMode.Gcd),
            new SlotResolverData(new PCT_即刻(),SlotMode.OffGcd),
            new SlotResolverData(new PCT_爆发药(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_加速GCD_黑彗星(), SlotMode.Gcd),
            new SlotResolverData(new PCT_加速GCD_锤子(), SlotMode.Gcd),
            new SlotResolverData(new PCT_彩虹(), SlotMode.Gcd),

            new SlotResolverData(new PCT_加速GCD_基础连长(), SlotMode.Gcd),
            new SlotResolverData(new PCT_加速GCD_基础连短(), SlotMode.Gcd),

            new SlotResolverData(new PCT_黑彗星(), SlotMode.Gcd),
            new SlotResolverData(new PCT_动物彩绘(), SlotMode.Gcd),

            new SlotResolverData(new PCT_锤子(), SlotMode.Gcd),
            new SlotResolverData(new PCT_白神圣(), SlotMode.Gcd),
            new SlotResolverData(new PCT_风景彩绘(), SlotMode.Gcd),
            new SlotResolverData(new PCT_武器彩绘(), SlotMode.Gcd),
            new SlotResolverData(new PCT_BaseGCD_长(), SlotMode.Gcd),
            new SlotResolverData(new PCT_BaseGCD_短(), SlotMode.Gcd),
            new SlotResolverData(new PCT_即刻(), SlotMode.Gcd),
            new SlotResolverData(new PCT_爆发药(), SlotMode.Gcd),
            new SlotResolverData(new PCT_扩散蛋(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_蛋(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_昏乱(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_反转(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_莫古炮(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_马蒂恩(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_风景构想(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_武器构想(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_动物构想(), SlotMode.OffGcd),
            new SlotResolverData(new PCT_风景构想(), SlotMode.Gcd),
            new SlotResolverData(new PCT_醒梦(), SlotMode.OffGcd),

        };
        public string 下一个GCD()
        {
            // 尝试找到第一个槽模式为全局冷却且可用的技能解析器
            var firstGCDSkill =
                SlotResolvers.FirstOrDefault(srd => srd.SlotMode == SlotMode.Gcd && srd.SlotResolver.Check() >= 0);
            // 如果找到了，则返回该技能解析器的类型名称；否则，返回"无技能"
            return firstGCDSkill != null ? firstGCDSkill.SlotResolver.GetType().Name : "无技能";
        }

        /// <summary>
        /// 检查并返回第一个可用的非全局冷却（Off-GCD）技能的名称。
        /// </summary>
        /// <returns>返回第一个可用的Off-GCD技能的名称，如果没有可用的Off-GCD技能，则返回"无技能"。</returns>
        public string 下一个OGCD()
        {
            // 尝试找到第一个槽模式为非全局冷却且可用的技能解析器
            var firstoffGCDSkill =
                SlotResolvers.FirstOrDefault(srd => srd.SlotMode == SlotMode.OffGcd && srd.SlotResolver.Check() >= 0);
            // 如果找到了，则返回该技能解析器的类型名称；否则，返回"无技能"
            return firstoffGCDSkill != null ? firstoffGCDSkill.SlotResolver.GetType().Name : "无技能";
        }
        public Rotation Build(string settingFolder)
        {
            PCTSettings.Build(settingFolder);
            BuildQT();
            var rot = new Rotation(SlotResolvers)
            {
                TargetJob = Jobs.Pictomancer,
                AcrType = AcrType.Both,
                MinLevel = 0,
                MaxLevel = 100,
                Description = "高贵的画爷需要打4目标aoe才能赚威力 对...对吗?",
            };
            rot.AddOpener(GetOpener);
            rot.SetRotationEventHandler(new PictomancerRotationEventHandler());
            rot.AddTriggerAction(new PictomancerQtTrigger());
            rot.AddTriggerAction(new PictomancerNewQtTrigger());
            rot.AddTriggerAction(new PictomancerQuickQtTrigger());
            rot.AddTriggerCondition(new TriggerCondition_PCT量谱());
            rot.AddTriggerCondition(new TriggerCondition_PCT团辅层数());
            rot.AddTriggerCondition(new TriggerCondition_爆发药状态());
            rot.AddTriggerAction(new TriggerAction_PCT彩绘CD阈值());
            rot.AddTriggerAction(new TriggerAction_PCT打锤子层数());
            rot.AddTriggerAction(new TriggerAction_PCT动物层数());
            rot.AddTriggerAction(new TriggerAction_PCT模式开关());
            rot.AddTriggerAction(new TriggerAction_清理GCD队列());
            rot.AddTriggerAction(new TriggerAction_清理能力技队列());

            //rot.AddTriggerAction(new TriggerAction_PCT起手技开关());
            return rot;
        }

        IOpener? GetOpener(uint level)
        {
            if (PCTSettings.Instance.日随模式 || PCTSettings.Instance.fate模式)
            {
                return null;
            }


            if (level == 100)
            {
                // 根据用户选择的 3GCD 或 2GCD 来返回不同的起手
                if (PCTSettings.Instance.Enable3GCDOpener)
                {
                    return new PCT_Opener100_3();  // 3GCD 起手
                }
                else if (PCTSettings.Instance.Enable2GCDOpener)
                {
                    return new PCT_Opener100_2();  // 2GCD 起手
                }
                else if (PCTSettings.Instance.Enable100EdenOpener)
                {
                    return new PCT_OpenerEden();  // 2GCD 起手
                }
                else if (PCTSettings.Instance.Enable100轴EdenOpener)
                {
                    return new PCT_OpenerEden轴();  // 2GCD 起手
                }
                else if (PCTSettings.Instance.Enable100FastOpener)
                {
                    return new PCT_Opener100Fast();  // 3GCD速泄 起手
                }
            }
            if (level == 90)
            {
                // 根据用户选择的 3GCD 或 2GCD 来返回不同的起手
                if (PCTSettings.Instance.Enable90Opener)
                {
                    return new PCT_Opener90();  // 3GCD 起手
                }
                if (PCTSettings.Instance.Enable90OmegaOpener)
                {
                    return new PCT_OpenerOmegatest();  // 2GCD 起手
                }
                /* if (PCTSettings.Instance.Enable90OmegaOpenerTest)
                 {
                     return new PCT_OpenerOmegatest();  // 2GCD 起手
                 }
                */
                if (PCTSettings.Instance.Enable90DragonOpener)
                {
                    return new PCT_OpenerDragon();
                }


            }

            return level switch
            {


                80 => new PCT_Opener80(),
                70 => new PCT_Opener70(),
                _ => null
            };
        }


        public static JobViewWindow QT { get; private set; }



        public IRotationUI GetRotationUI()
        {
            return QT;
        }

        public void BuildQT()
        {


            QT = new JobViewWindow(PCTSettings.Instance.JobViewSave, PCTSettings.Instance.Save, "Cindy_Master PCT");
            QT.SetUpdateAction(OnUIUpdate); // 设置QT中的Update回调 不需要就不设置
            QT.AddTab("通用", PCT_UI.DrawQtGeneral);

            // Add QT hotkeys for each skill
            PictomancerRotationEntry.QT.AddQt(QTKey.爆发药, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.白神圣, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.黑彗星, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.天星, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.彩虹, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.锤子, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.爆发, true);

            PictomancerRotationEntry.QT.AddQt(QTKey.动物彩绘, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.武器彩绘, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.风景彩绘, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.动物构想, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.武器构想, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.风景构想, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.反转, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.基础连, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.自动绘画, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.即刻, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.马蒂恩, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.莫古, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.自动减伤, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.快死不画, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.自动停手, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.智能AOE, true);
            PictomancerRotationEntry.QT.AddQt(QTKey.测112, false);
            PictomancerRotationEntry.QT.AddQt(QTKey.倾泻资源, false);
            PictomancerRotationEntry.QT.AddQt(QTKey.优先画画, false, "团辅期内不生效");
            PictomancerRotationEntry.QT.AddQt(QTKey.死都得画, false, "团辅期内生效");
            PictomancerRotationEntry.QT.AddQt(QTKey.快死不爆, true, "影响团辅和锤子构想");




            PictomancerRotationEntry.QT.AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(SpellsDefine.Surecast, SpellTargetType.Self, false));
            PictomancerRotationEntry.QT.AddHotkey("昏乱", new HotKeyResolver_NormalSpell(SpellsDefine.Addle, SpellTargetType.Target, false));
            PictomancerRotationEntry.QT.AddHotkey("疾跑", new HotKeyResolver_疾跑());
            PictomancerRotationEntry.QT.AddHotkey("爆发药", new HotKeyResolver_Potion());
            PictomancerRotationEntry.QT.AddHotkey("蛋盾", new HotKeyResolver_NormalSpell(PCT_Data.Spells.蛋, SpellTargetType.Self, false));
            PictomancerRotationEntry.QT.AddHotkey("扩散蛋盾", new HotKeyResolver_NormalSpell(PCT_Data.Spells.扩散蛋, SpellTargetType.Self, false));
            PictomancerRotationEntry.QT.AddHotkey("LB", new HotKeyResolver_法系LB());


            PCTSettings.Instance.LoadQtStates(QT);


        }

        private void OnUIUpdate()
        {

        }

        public void OnDrawSetting()
        {
        }


    }
}