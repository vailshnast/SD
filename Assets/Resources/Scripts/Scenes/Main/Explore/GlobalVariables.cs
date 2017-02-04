using UnityEngine;
using System.Collections;

public enum NodeState
{
    Null,
    Constructed,
    Disabled,
}
public enum NodeEvent
{
    Battle,
    Exit,
    Empty_Path,

    Encounter_Sack,
    Encounter_Chest,
    Encounter_Camp,
    Encounter_Fountain,
    Encounter_Altar,
    Encounter_Mushrooms,
    Encounter_Trap
}
public enum NodeExploreState
{
    Hidden,
    Revealed,
    Explored
}
public enum NeighbourDirection
{
    Top,
    Bottom,
    Left,
    Right
}
public enum ImageDirection
{
    Main,
    Left,
    Right,
    Null,
}
public enum TargetState
{
    Static,
    Clicked,
    Inactive
}