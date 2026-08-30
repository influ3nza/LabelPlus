using System;
namespace LabelPlus
{
    public enum AtomActionType
    {
        ADD_LABEL,
        DELETE_LABEL,
        MOVE_LABEL,
        EDIT_CATEGORY,
        EDIT_TEXT,
    }

    public class NestedLabelItem
    {
        public LabelItem Before { get; private set; }
        public LabelItem After { get; private set; }
        public string Filename { get; private set; }
        public int Index { get; private set; }

        public NestedLabelItem(LabelItem before, LabelItem after, string filename, int index)
        {
            Before = before;
            After = after;
            Filename = filename;
            Index = index;
        }
    }

    public class AtomAction
    {
        private NestedLabelItem anchor;
        private AtomActionHandler handler;

        public AtomActionType ActionType { get; private set; }

        public AtomAction(
            AtomActionType actionType,
            NestedLabelItem anchor,
            AtomActionHandler handler)
        {
            ActionType = actionType;
            this.anchor = anchor;
            this.handler = handler;
        }

        public void Undo()
        {
            handler.Undo(anchor);
        }

        public void Redo()
        {
            handler.Redo(anchor);
        }

        public void Execute(NestedLabelItem item)
        {
            handler.Redo(item);
        }
    }
}
