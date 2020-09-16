using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Xml.Linq;

public static class TeamEvents
{
    public class TeamTurnStartedEvent : UnityEvent<TeamBase> { }
    public class TeamTurnEndedEvent : UnityEvent<TeamBase> { }
    public class TeamEliminatedEvent : UnityEvent<TeamBase> { }
    public class LoadTeamStateFromXMLEvent : UnityEvent<XDocument> { }

    public static TeamTurnStartedEvent teamTurnStartedEvent { get; } = new TeamTurnStartedEvent();
    public static TeamTurnEndedEvent teamTurnEndedEvent { get; } = new TeamTurnEndedEvent();
    public static TeamEliminatedEvent teamEliminatedEvent { get; } = new TeamEliminatedEvent();
    public static LoadTeamStateFromXMLEvent loadTeamStateFromXMLEvent { get; } = new LoadTeamStateFromXMLEvent();
}
