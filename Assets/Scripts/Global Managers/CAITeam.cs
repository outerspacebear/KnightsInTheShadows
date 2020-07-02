using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAITeam : TeamBase
{
    public override void BeginTurn()
    {
        Debug.Log("Beginning turn for AI team " + name);

        foreach (var character in characters)
        {
            character.ResetActionPoints();
        }

        if (HaveAllCharactersEndedTurn())
        {
            shouldEndTurnNextUpdate = true;
        }
    }

    public override void OnEndTurn()
    {
        Debug.Log("Ending turn for AI team " + name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldEndTurnNextUpdate)
        {
            TeamEvents.teamTurnEndedEvent.Invoke(this);
            shouldEndTurnNextUpdate = false;
        }
    }

    bool shouldEndTurnNextUpdate { get; set; } = false;
}
