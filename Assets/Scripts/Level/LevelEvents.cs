using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class LevelEvents
{
    public class ObjectiveCompletedEvent : UnityEvent<LevelObjective> { }
    public class LevelWonEvent : UnityEvent<TeamBase> { }

    public static ObjectiveCompletedEvent objectiveCompleteEvent { get; } = new ObjectiveCompletedEvent();
    public static LevelWonEvent levelWonEvent { get; } = new LevelWonEvent();
}
