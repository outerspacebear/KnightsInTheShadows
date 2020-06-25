using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TeamEvents
{
    public class TeamTurnStartedEvent : UnityEvent<CTeam> { }
    public class TeamTurnEndedEvent : UnityEvent<CTeam> { }

    public static TeamTurnStartedEvent teamTurnStartedEvent { get; } = new TeamTurnStartedEvent();
    public static TeamTurnEndedEvent teamTurnEndedEvent { get; } = new TeamTurnEndedEvent();
}
