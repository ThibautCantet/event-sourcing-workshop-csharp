using System.Collections.ObjectModel;
using fr.soat.eventsourcing.api;

namespace fr.soat.eventsourcing.impl;

public class InMemoryEventStore : IEventStore
{
    private Dictionary<String, List<IEvent>> _store = new();
    //private Object lock = new Object();

    public List<IEvent> LoadEvents(IAggregateId aggregateId)
    {
        //synchronized (lock) {
        _store.TryGetValue(aggregateId.ToString(), out var result);
        return result == null ? new List<IEvent>() : result;
        //}
    }

    public void Store(IAggregateId aggregateId, List<IEvent> events)
    {
        //synchronized (lock) {
        List<IEvent> previousEvents = LoadEvents(aggregateId);
        CheckVersion(previousEvents, events);

        for (int i = previousEvents.Count; i < events.Count; i++)
        {
            previousEvents.Add(events[i]);
        }

        _store[aggregateId.ToString()] = previousEvents;
        //}
    }

    private void CheckVersion(List<IEvent> previousEvents, List<IEvent> events)
    {
        return;
        
        if (previousEvents.Count == 0 && events.Count == 0)
        {
            return;
        }

        if (!events.Except(previousEvents).Any())
        {
            String msg = "Failed to save events, version mismatch (there was a concurrent update)";
            //"- In store: " + reverse(previousEvents) + "\n" +
            //"- Trying to save: " + reverse(events);
            throw new EventConcurrentUpdateException(msg);
        }
    }
    
    /*
    private static readonly int INDEXOFSUBLIST_THRESHOLD =   35;
    public static int indexOfSubList(List<Event> source, List<Event> target) {
        int sourceSize = source.Count;
        int targetSize = target.Count;
        int maxCandidate = sourceSize - targetSize;

        if (sourceSize < INDEXOFSUBLIST_THRESHOLD)
            //|| (source instanceof RandomAccess&&target instanceof RandomAccess)) 
        {
            nextCand:
            for (int candidate = 0; candidate <= maxCandidate; candidate++) {
                for (int i=0, j=candidate; i<targetSize; i++, j++)
                    if (!eq(target[i], source[j]))
                        continue nextCand;  // Element mismatch, try next cand
                return candidate;  // All elements of candidate matched target
            }
        } else {  // Iterator version of above algorithm
            var si = source;
            nextCand:
            for (int candidate = 0; candidate <= maxCandidate; candidate++) {
                var ti = target ;
                for (int i=0; i<targetSize; i++) {
                    if (!eq(ti.next(), si.next())) {
                        // Back up source iterator to next candidate
                        for (int j=0; j<i; j++)
                            si.previous();
                        continue nextCand;
                    }
                }
                return candidate;
            }
        }
        return -1;  // No candidate matched the target
    }
    
    static Boolean eq(Object o1, Object o2) {
        return o1==null ? o2==null : o1.Equals(o2);
    }
    */

    public void Clear()
    {
        _store.Clear();
    }
}