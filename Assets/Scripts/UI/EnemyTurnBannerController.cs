using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnBannerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        banner.SetActive(false);

        TeamEvents.teamTurnStartedEvent.AddListener(OnTeamTurnStarted);
    }

    void OnTeamTurnStarted(TeamBase team)
    {
        if(team.GetType() == typeof(CAITeam))
        {
            banner.SetActive(true);
        }
        else
        {
            banner.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    GameObject banner = null;
}
