using UnityEngine;
using System.Collections;

public class Hero_Choser: Touch_processor {
    Hero_chose choser;

    void Awake()
    {
        init();
        minDistance = 25;
    }

    private void init()
    {
        choser = GameObject.FindObjectOfType<Hero_chose>();
    }

    protected override void action_left()
    {
        choser.call_next_hero();
    }

    protected override void action_right()
    {
        choser.call_previous_hero();
    }
}
