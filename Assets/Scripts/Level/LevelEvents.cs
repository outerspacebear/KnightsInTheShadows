﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class LevelEvents
{
    public enum PauseActions { PAUSE, PLAY }

    public class ObjectiveCompletedEvent : UnityEvent<LevelObjective> { }
    public class LevelWonEvent : UnityEvent<TeamBase> { }
    public class LevelIsMultiplayerEvent : UnityEvent { }
    public class PauseLevelEvent : UnityEvent<PauseActions> { }

    public static ObjectiveCompletedEvent objectiveCompleteEvent { get; } = new ObjectiveCompletedEvent();
    public static LevelWonEvent levelWonEvent { get; } = new LevelWonEvent();
    public static LevelIsMultiplayerEvent levelIsMultiplayerEvent { get; } = new LevelIsMultiplayerEvent();
    public static PauseLevelEvent pauseLevelEvent { get; } = new PauseLevelEvent();
}
