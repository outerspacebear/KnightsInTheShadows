using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TeamEvents
{
    public class TeamTurnEndedEvent : UnityEvent<CTeam> { }

    public static TeamTurnEndedEvent teamTurnEndedEvent { get; } = new TeamTurnEndedEvent();
}
