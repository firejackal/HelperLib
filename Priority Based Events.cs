using System.Collections.Generic;

namespace HelperLib.PriorityBasedEvents
{
    public class PriorityEventsCollection : SortedList<PriorityKey, BaseEvent>
    {
        private List<PriorityKey> mToRemove = new List<PriorityKey>();

        public void Push(BasePriorityKey key, BaseEvent evnt)
        {
            base.Add(new PriorityKey(key), evnt);
        } //Push

        public void Update(BasePriorityKey currentPriority)
        {
            if(base.Count == 0) return;

            for(int index = 0; index < base.Keys.Count; index++) {
                if(base.Keys[index].IsExpired(currentPriority)) {
                    base[base.Keys[index]].Run();
                    this.mToRemove.Add(base.Keys[index]);
                } else {
                    break; //no reason to continue if the item is not expired yet.
                }
            } //next index

            if(this.mToRemove.Count > 0) {
                for(int index = (this.mToRemove.Count - 1); index >= 0; index--) {
                    base.Remove(this.mToRemove[index]);
                } //next index
            }
        } //Update
    } //PriorityEventsCollection class

    public class PriorityKey : System.IComparable
    {
        public BasePriorityKey Key;

        public PriorityKey() { }

        public PriorityKey(BasePriorityKey key) { this.Key = key; }

        public bool IsExpired(BasePriorityKey key) { return key.CompareTo(this.Key) >= 0; }

        public int CompareTo(object obj)
        {
            if(obj == null || !(obj is PriorityKey)) throw new System.ArgumentException("Object is not the correct type.");
            PriorityKey key = (PriorityKey)obj;
            return this.Key.CompareTo(key.Key);
        } //CompareTo
    } //PriorityKey class

    public interface BaseEvent
    {
        void Run();
    } //BaseEvent interface

    public abstract class BasePriorityKey : System.IComparable
    {
        public abstract int CompareTo(object obj);
    } //BasePriorityKey class
} //HelperLib.PriorityBasedEvents namespace
