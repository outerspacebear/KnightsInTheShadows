using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class LevelEvents
{
    public class ObjectiveCompletedEvent : UnityEvent<LevelObjective> { }
    public class LevelCompletedEvent : UnityEvent { }

    public static ObjectiveCompletedEvent objectiveCompleteEvent { get; } = new ObjectiveCompletedEvent();
    public static LevelCompletedEvent levelCompletedEvent { get; } = new LevelCompletedEvent();
}
