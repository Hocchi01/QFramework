/****************************************************************************
 * Copyright (c) 2015 - 2022 liangxiegame UNDER MIT License
 * 
 * http://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;

namespace QFramework
{
#if UNITY_EDITOR
    // v1 No.164
    [ClassAPI("4.ActionKit", "ActionKit", 0, "ActionKit")]
    [APIDescriptionCN("Action 时序动作序列（组合模式 + 命令模式 + 建造者模式）")]
    [APIDescriptionEN("Action Sequence (composite pattern + command pattern + builder pattern)")]
#endif
    public class ActionKit : Architecture<ActionKit>
    {
#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("延时回调")]
        [APIDescriptionEN("delay callback")]
        [APIExampleCode(@"
Debug.Log(""Start Time:"" + Time.time);
 
ActionKit.Delay(1.0f, () =>
{
    Debug.Log(""End Time:"" + Time.time);
             
}).Start(this); // update driven
 
// Start Time: 0.000000
---- after 1 seconds ----
---- 一秒后 ----
// End Time: 1.000728
")]
#endif
        public static IAction Delay(float seconds, Action callback)
        {
            return QFramework.Delay.Allocate(seconds, callback);
        }


#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("动作序列")]
        [APIDescriptionEN("action sequence")]
        [APIExampleCode(@"
Debug.Log(""Sequence Start:"" + Time.time);
 
ActionKit.Sequence()
    .Callback(() => Debug.Log(""Delay Start:"" + Time.time))
    .Delay(1.0f)
    .Callback(() => Debug.Log(""Delay Finish:"" + Time.time))
    .Start(this, _ => { Debug.Log(""Sequence Finish:"" + Time.time); });
 
// Sequence Start: 0
// Delay Start: 0
------ after 1 seconds ------
------ 1 秒后 ------
// Delay Finish: 1.01012
// Sequence Finish: 1.01012
")]
#endif
        public static ISequence Sequence()
        {
            return QFramework.Sequence.Allocate();
        }

#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("延时帧")]
        [APIDescriptionEN("delay by frameCount")]
        [APIExampleCode(@"
Debug.Log(""Delay Frame Start FrameCount:"" + Time.frameCount);
 
ActionKit.DelayFrame(1, () => { Debug.Log(""Delay Frame Finish FrameCount:"" + Time.frameCount); })
        .Start(this);
 
ActionKit.Sequence()
        .DelayFrame(10)
        .Callback(() => Debug.Log(""Sequence Delay FrameCount:"" + Time.frameCount))
        .Start(this);

// Delay Frame Start FrameCount:1
// Delay Frame Finish FrameCount:2
// Sequence Delay FrameCount:11
 
// --- also support nextFrame
// --- 还可以用 NextFrame  
// ActionKit.Sequence()
//      .NextFrame()
//      .Start(this);
//
// ActionKit.NextFrame(() => { }).Start(this);
")]
#endif
        public static IAction DelayFrame(int frameCount, Action onDelayFinish)
        {
            return QFramework.DelayFrame.Allocate(frameCount, onDelayFinish);
        }

        public static IAction NextFrame(Action onNextFrame)
        {
            return QFramework.DelayFrame.Allocate(1, onNextFrame);
        }


#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("条件")]
        [APIDescriptionEN("condition action")]
        [APIExampleCode(@"
ActionKit.Sequence()
        .Callback(() => Debug.Log(""Before Condition""))
        .Condition(() => Input.GetMouseButtonDown(0))
        .Callback(() => Debug.Log(""Mouse Clicked""))
        .Start(this);

// Before Condition
// ---- after left mouse click ----
// ---- 鼠标左键点击之后 ----
// Mouse Clicked
")]
#endif
        void ConditionAPI()
        {
        }

#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("重复动作")]
        [APIDescriptionEN("repeat action")]
        [APIExampleCode(@"
ActionKit.Repeat()
        .Condition(() => Input.GetMouseButtonDown(0))
        .Callback(() => Debug.Log(""Mouse Clicked""))
        .Start(this);
// always Log Mouse Clicked when click left mouse
// 鼠标左键点击时，每次都会输出 Mouse Clicked

ActionKit.Repeat(5) // -1、0 means forever 1 means once  2 means twice
        .Condition(() => Input.GetMouseButtonDown(1))
        .Callback(() => Debug.Log(""Mouse right clicked""))
        .Start(this, () =>
        {
            Debug.Log(""Right click finished"");
        });
// Mouse right clicked
// Mouse right clicked
// Mouse right clicked
// Mouse right clicked
// Mouse right clicked
// Right click finished
    ")]
#endif
        public static IRepeat Repeat(int repeatCount = -1)
        {
            return QFramework.Repeat.Allocate(repeatCount);
        }


#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("并行动作")]
        [APIDescriptionEN("parallel action")]
        [APIExampleCode(@"
Debug.Log(""Parallel Start:"" + Time.time);
 
ActionKit.Parallel()
        .Delay(1.0f, () => { Debug.Log(Time.time); })
        .Delay(2.0f, () => { Debug.Log(Time.time); })
        .Delay(3.0f, () => { Debug.Log(Time.time); })
        .Start(this, () =>
        {
            Debug.Log(""Parallel Finish:"" + Time.time);
        });
// Parallel Start:0
// 1.01
// 2.01
// 3.02
// Parallel Finish:3.02
")]
#endif
        public static IParallel Parallel()
        {
            return QFramework.Parallel.Allocate();
        }

#if UNITY_EDITOR
        [MethodAPI]
        [APIDescriptionCN("复合动作示例")]
        [APIDescriptionEN("Complex action example")]
        [APIExampleCode(@"
ActionKit.Sequence()
        .Callback(() => Debug.Log(""Sequence Start""))
        .Callback(() => Debug.Log(""Parallel Start""))
        .Parallel(p =>
        {
            p.Delay(1.0f, () => Debug.Log(""Delay 1s Finished""))
                .Delay(2.0f, () => Debug.Log(""Delay 2s Finished""));
        })
        .Callback(() => Debug.Log(""Parallel Finished""))
        .Callback(() => Debug.Log(""Check Mouse Clicked""))
        .Sequence(s =>
        {
            s.Condition(() => Input.GetMouseButton(0))
                .Callback(() => Debug.Log(""Mouse Clicked""));
        })
        .Start(this, () =>
        {
            Debug.Log(""Finish"");
        });
// 
// Sequence Start
// Parallel Start
// Delay 1s Finished
// Delay 2s Finished
// Parallel Finished
// Check Mouse Clicked
// ------ After Left Mouse Clicked ------
// ------ 鼠标左键点击后 ------
// Mouse Clicked
// Finish

")]
#endif
        public void ComplexAPI()
        {
        }


        #region Events

        public static EasyEvent OnUpdate => ActionKitMonoBehaviourEvents.Instance.OnUpdate;
        public static EasyEvent OnFixedUpdate => ActionKitMonoBehaviourEvents.Instance.OnFixedUpdate;
        public static EasyEvent OnLateUpdate => ActionKitMonoBehaviourEvents.Instance.OnLateUpdate;
        public static EasyEvent OnGUI => ActionKitMonoBehaviourEvents.Instance.OnGUIEvent;
        public static EasyEvent OnApplicationQuit => ActionKitMonoBehaviourEvents.Instance.OnApplicationQuitEvent;

        protected override void Init()
        {
        }

        #endregion
    }
}