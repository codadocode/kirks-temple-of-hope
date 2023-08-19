using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEvents
{
    private List<TimedEventData> timedEventsData = new List<TimedEventData>();

    public void AddTimedEvent(string eventId, float delay, Action eventAction, bool loop = false)
    {
        TimedEventData nextEvent = new TimedEventData();
        nextEvent.eventId = eventId;
        nextEvent.actionEvent = eventAction;
        nextEvent.loop = loop;
        nextEvent.startedTime = Time.time;
        nextEvent.delay = delay;
        this.timedEventsData.Add(nextEvent);
    }

    public void RemoveTimedEvent(string eventId)
    {
        for (int i = 0; i < this.timedEventsData.Count; i++)
        {
            TimedEventData actualEventData = this.timedEventsData[i];
            if (actualEventData.eventId.Equals(eventId))
            {
                this.timedEventsData.RemoveAt(i);
                break;
            }
        }
    }

    public void ProcessEvents()
    {
        for (int i = 0; i < this.timedEventsData.Count; i++)
        {
            TimedEventData timedEvent = this.timedEventsData[i];

            if (Time.time < (timedEvent.startedTime + timedEvent.delay)) continue;
            
            timedEvent.actionEvent?.Invoke();

            if (timedEvent.loop)
            {
                timedEvent.startedTime = Time.time;
                this.timedEventsData[i] = timedEvent;
            }else if (this.timedEventsData.Count > 0)
            {
                this.timedEventsData.RemoveAt(i);
                i--;
            }
        }
    }

    private void ClearAllEvents()
    {
        this.timedEventsData.Clear();
    }
}

public struct TimedEventData
{
    public string eventId;
    public float startedTime;
    public float delay;
    public bool loop;
    public Action actionEvent;
}
