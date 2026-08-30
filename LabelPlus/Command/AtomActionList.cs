using System;
using System.Collections.Generic;

namespace LabelPlus
{
    public class AtomActionList
    {
        private LinkedList<AtomAction> actions = new LinkedList<AtomAction>();
        private int capacity;
        private LinkedListNode<AtomAction> current;

        public AtomActionList(int capacity)
        {
            this.capacity = capacity;
        }

        public int Count
        {
            get { return actions.Count; }
        }

        public bool CanUndo
        {
            get { return current != null; }
        }

        public bool CanRedo
        {
            get
            {
                return current == null
                    ? actions.First != null
                    : current.Next != null;
            }
        }

        public void Register(AtomAction action)
        {
            LinkedListNode<AtomAction> node =
                current == null ? actions.First : current.Next;
            while (node != null)
            {
                LinkedListNode<AtomAction> next = node.Next;
                actions.Remove(node);
                node = next;
            }

            current = actions.AddLast(action);

            if (actions.Count > capacity)
                actions.RemoveFirst();
        }

        public void Undo()
        {
            if (!CanUndo) return;

            LinkedListNode<AtomAction> action = current;
            action.Value.Undo();
            current = action.Previous;
        }

        public void Redo()
        {
            if (!CanRedo) return;

            LinkedListNode<AtomAction> action =
                current == null ? actions.First : current.Next;
            action.Value.Redo();
            current = action;
        }

        public void Clear()
        {
            actions.Clear();
            current = null;
        }
    }
}
