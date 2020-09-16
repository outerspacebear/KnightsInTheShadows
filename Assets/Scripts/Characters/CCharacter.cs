using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class CCharacter : MonoBehaviour
{
    public ECharacterAction[] availableActions { get; } = { ECharacterAction.MOVE, ECharacterAction.ATTACK };

    public void ResetActionPoints()
    {
        currentActionPoints = baseActionPoints;
    }

    public bool CanTakeAction(ECharacterAction action)
    {
        if(!availableActions.Contains(action))
        {
            //This character class cannot perform this action
            return false;
        }

        if(currentActionPoints >= CharacterActions.actionCostMap[action])
        {
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        MapLoadedEvent.Get().AddListener(OnMapLoaded);

        if(!loadedState)
        {
            currentActionPoints = baseActionPoints;
        }

        if (!(playerHighlilghter = GameObject.Find("PlayerHighlighter")))
        {
            Debug.LogError("Coudln't find the player highlighter!");
            return;
        }
        playerHighlighterDefaultPosition = playerHighlilghter.transform.position;

        LevelEvents.levelIsMultiplayerEvent.AddListener(LevelIsMultiplayer);
    }

    private void OnDestroy()
    {
        LevelEvents.levelIsMultiplayerEvent.RemoveListener(LevelIsMultiplayer);
    }

    void LevelIsMultiplayer() => isMultiplayerLevel = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Character " + gameObject.name + "clicked on!");
        CharacterEvents.characterClickedEvent.Invoke(this);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Character " + name + " right-clicked on!");
            CharacterEvents.characterRightClickedEvent.Invoke(this);
        }
    }

    public void OnSelected()
    {
        if (playerHighlilghter)
        {
            playerHighlilghter.transform.position = gameObject.transform.position;
        }

        Debug.Log("Character " + name + " selected!");
        CharacterEvents.characterSelectedEvent.Invoke(this);
    }

    public void OnDeselected()
    {
        if (playerHighlilghter)
        {
            playerHighlilghter.transform.position = playerHighlighterDefaultPosition;
        }

        Debug.Log("Character " + name + " de-selected!");
        CharacterEvents.characterDeselectedEvent.Invoke(this);
    }

    [PunRPC]
    public void MoveToTileAtPosition(Vector3 tilePosition)
    {
        CTile targetTile = map.TryGetTileAt(tilePosition);
        if(!targetTile)
        {
            Debug.LogError("Could not find tile at " + tilePosition.ToString() + "; something might have gone wrong!");
            return;
        }

        MoveTo(targetTile);
    }

    public void MoveTo(CTile tile)
    {
        transform.position = tile.transform.position;
        occupyingTile = tile;
        currentActionPoints -= CharacterActions.actionCostMap[ECharacterAction.MOVE];

        Debug.Log("Character " + name + " moved to " + tile.transform.position.ToString());
        CharacterEvents.actionTakenEvent.Invoke(this, ECharacterAction.MOVE);
    }

    public void Attack(CCharacter character)
    {
        Debug.Log("Character " + name + " is attacking character " + character.name + "!");
        currentActionPoints -= CharacterActions.actionCostMap[ECharacterAction.MOVE];

        if(!isMultiplayerLevel)
        {
            character.OnAttacked(attackDamage);
        }
        else
        {
            PhotonView photonView = character.GetComponent<PhotonView>();
            photonView.RPC("OnAttacked", RpcTarget.All, attackDamage);
        }

        CharacterEvents.actionTakenEvent.Invoke(this, ECharacterAction.ATTACK);
    }

    [PunRPC]
    public void OnAttacked(int damage)
    {
        Debug.Log("Character " + name + " attacked for " + damage.ToString() + " damage! Remaining HP: " + hitPoints);
        CharacterEvents.characterAttackedEvent.Invoke(this);

        hitPoints -= damage;
        if(hitPoints <= 0)
        {
            CharacterEvents.characterDeathEvent.Invoke(this);
        }
    }

    public int GetMovementPerAction() => movementPerAction;

    public int GetAttackRange() => attackRange;

    public int GetHitPoints() => hitPoints;

    public void SetHitPoints(int hitPoints) => this.hitPoints = hitPoints;

    public void RefreshOccupyingTile()
    {
        occupyingTile = null;
        if (!(occupyingTile = map.TryGetTileAt(transform.position)))
        {
            Debug.LogError("Character " + gameObject.name + " is not placed on a valid tile");
            return;
        }
    }

    public void SetStartingPosition(Vector3 newPosition) => startingPosition = newPosition;

    public void LoadedState() => loadedState = true;

    void OnMapLoaded(TileMap map)
    {
        this.map = map;
        transform.position = startingPosition;

        RefreshOccupyingTile();
    }

    public int currentActionPoints { get; set; }
    public CTile occupyingTile { get; set; }
    [SerializeField]
    public Color tileHighlightColor = Color.cyan;

    [SerializeField]
    int movementPerAction;
    [SerializeField]
    private int baseActionPoints;
    [SerializeField]
    int attackRange = 1;
    [SerializeField]
    int attackDamage = 1;
    [SerializeField]
    int hitPoints;
    [SerializeField]
    Vector3 startingPosition;

    static GameObject playerHighlilghter;
    static Vector3 playerHighlighterDefaultPosition;

    TileMap map;
    bool isMultiplayerLevel = false;

    bool loadedState = false;
}
