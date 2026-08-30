using System;
using System.Collections.Generic;

namespace LabelPlus
{
    // 原子操作对应的撤销重做方法组
    public class AtomActionHandler
    {
        public Action<NestedLabelItem> Undo { get; private set; }
        public Action<NestedLabelItem> Redo { get; private set; }
        
        public AtomActionHandler(Action<NestedLabelItem> undo, Action<NestedLabelItem> redo)
        {
            Undo = undo;
            Redo = redo;
        }
    }

    static class UndoRedoManager
    {
        private const int DefaultCapacity = 200;
        private static AtomActionList actions =
            new AtomActionList(DefaultCapacity);
        private static Dictionary<AtomActionType, AtomActionHandler> handlers =
            new Dictionary<AtomActionType, AtomActionHandler>();

        public static void RegisterHandler(AtomActionType actionType, Action<NestedLabelItem> undo, Action<NestedLabelItem> redo)
        {
            handlers[actionType] = new AtomActionHandler(undo, redo);
        }

        public static AtomAction RegisterAction(
            AtomActionType actionType,
            NestedLabelItem anchor)
        {
            AtomActionHandler handler;
            if (!handlers.TryGetValue(actionType, out handler))
                throw new InvalidOperationException("未注册该操作类型的撤销/重做处理器。");

            AtomAction action = new AtomAction(actionType, anchor, handler);
            actions.Register(action);
            return action;
        }

        public static void UndoAction()
        {
            actions.Undo();
        }

        public static void RedoAction()
        {
            actions.Redo();
        }

        public static void Clear()
        {
            actions.Clear();
        }
    }
}
