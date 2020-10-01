using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterActionAudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!moveSound || !attackSound || !deathSound)
        {
            Debug.LogError("One or more of the sounds have not been asigned to CharacterActionAudioPlayer! It will not function.");
            return;
        }

        audioSource = GetComponent<AudioSource>();

        CharacterEvents.actionTakenEvent.AddListener(OnCharacterActionTaken);
        CharacterEvents.characterDeathEvent.AddListener(OnCharacterDeath);
    }

    private void OnDestroy()
    {
        CharacterEvents.actionTakenEvent.RemoveListener(OnCharacterActionTaken);
        CharacterEvents.characterDeathEvent.RemoveListener(OnCharacterDeath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCharacterActionTaken(CCharacter character, ECharacterAction action)
    {
        AudioClip clipToPlay = null;
        switch(action)
        {
            case ECharacterAction.MOVE:
            case ECharacterAction.SPRINT:
                clipToPlay = moveSound;
                break;
            case ECharacterAction.ATTACK:
                clipToPlay = attackSound;
                break;
            case ECharacterAction.SLASH:
                clipToPlay = slashSound;
                break;
            default:
                Debug.LogWarning("This should never happen! Case for a character action not covered in switch!");
                break;
        }

        PlayClip(clipToPlay);  
    }

    void OnCharacterDeath(CCharacter character)
    {
        PlayClip(deathSound);
    }

    void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    AudioSource audioSource;

    [SerializeField]
    AudioClip moveSound = null;
    [SerializeField]
    AudioClip attackSound = null;
    [SerializeField]
    AudioClip deathSound = null;
    [SerializeField]
    AudioClip slashSound = null;
}
