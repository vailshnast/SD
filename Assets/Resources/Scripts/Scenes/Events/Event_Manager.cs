using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event
{
    public int chance { get; private set; }

    private int min_Range { get; set; }
    private int max_Range { get; set; }

    public delegate string Behaviour();
    private Behaviour behaviour;


    public Event(int chance, Behaviour behaviour)
    {
        this.chance = chance;
        this.behaviour = behaviour;
    }
    public bool Play_Event(ref Text text_Label, float chance)
    {
        if (chance > min_Range && chance < max_Range)
        {
            text_Label.text = behaviour();
            return true;
        }

        return false;
    }
    public void SetRange(int min_Range, int max_Range)
    {
        this.min_Range = min_Range;
        this.max_Range = max_Range;
    }
}
public class Event_Manager
{

    private List<Event> events = new List<Event>();
    private int total_Length = 0;
    private Text text_Label;

    public Event_Manager(Text text_Label, params Event[] events)
    {
        this.text_Label = text_Label;

        for (int i = 0; i < events.Length; i++)
        {
            this.events.Add(events[i]);
        }
        int current_Length = 0;
        for (int i = 0; i < this.events.Count; i++)
        {
            this.events[i].SetRange(current_Length, current_Length + this.events[i].chance);
            current_Length += this.events[i].chance;
        }

        for (int i = 0; i < this.events.Count; i++)
        {
            total_Length += this.events[i].chance;
        }
    }

    public void Create_Event()
    {
        float random_Point = Random.value * total_Length;
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].Play_Event(ref text_Label, random_Point))
                return;
        }
    }
}