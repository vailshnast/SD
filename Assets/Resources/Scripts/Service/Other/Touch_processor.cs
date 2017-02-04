using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Touch_processor : MonoBehaviour
{

    protected Vector2 firstClick, secondClick, swipe_direction;
    protected float minDistance = 0;

    public void touch_click(BaseEventData touch_a_data)
    {
        PointerEventData touch_a = touch_a_data as PointerEventData;
        firstClick = touch_a.position;
    }

    public void untouch_unclick(BaseEventData touch_b_data)
    {
        PointerEventData touch_b = touch_b_data as PointerEventData;
        secondClick = touch_b.position;
        processor(firstClick, secondClick);
    }

    protected void processor(Vector2 touch_a, Vector2 touch_b)
    {
        Vector2 swipe_direction;


        swipe_direction = new Vector2(touch_b.x - touch_a.x, touch_b.y - touch_a.y);

        swipe_direction.Normalize();

        //Directions
        if (Vector2.Distance(touch_a, touch_b) >= minDistance)
        {
            if (swipe_direction.x < 0 && swipe_direction.y > -0.5f && swipe_direction.y < 0.5f)
            {
                action_left();
            }

            if (swipe_direction.x > 0 && swipe_direction.y > -0.5f && swipe_direction.y < 0.5f)
            {
                action_right();
            }

            if (swipe_direction.y < 0 && swipe_direction.x > -0.5f && swipe_direction.x < 0.5f)
            {
                action_down();
            }

            if (swipe_direction.y > 0 && swipe_direction.x > -0.5f && swipe_direction.x < 0.5f)
            {
                action_up();
            }
        }
    }

    protected virtual void action_left()
    {

    }

    protected virtual void action_right()
    {

    }
    public virtual void action_down()
    {

    }
    protected virtual void action_up()
    {

    }
}
