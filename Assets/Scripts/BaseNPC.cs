using CsvHelper.Configuration;
using NUnit.Framework;
using UnityEngine;
using Yarn;
using Yarn.Unity;


using System.Collections.Generic;

[System.Serializable]
public class Conversation
{
    public List<BaseNPC> speakingNPCS;
    public string dialogueName;
}
public abstract class BaseNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private bool interactable = true;
    public bool Interactable
    {
        get => interactable;
        set => interactable = value;
    }


    [SerializeField] private DialogueRunner dialogueRunner;
    public GameManager gameManager;
    public ConversationManager conversationManager;

    public string speakingName;

    public Conversation conversation;
    public Animator animator;
    public PanCamera pCamera;
    public Transform playerPos;
    private Quaternion lookDir;
    public bool inConversation = false;
    public bool talking = false;

    public virtual void Start()
    {

        animator = GetComponent<Animator>();

        FPController player = FindFirstObjectByType<FPController>();
        playerPos = player.transform;
        pCamera = player.GetComponent<PanCamera>();


        if (animator != null) animator.SetBool("Idle", true);

        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.endDialogue.AddListener(ResetInteract);
        }

        dialogueRunner = FindFirstObjectByType<DialogueRunner>();

        AddYarnFunc();

        lookDir = transform.rotation;

        conversationManager = FindFirstObjectByType<ConversationManager>();
    }

    public virtual void AddYarnFunc()
    {

    }

    public void SetTalking(bool isTalking)
    {
        if (isTalking)
        {
            talking = true;
            animator.SetBool("Talking", true);
        }
        else
        {
            talking = false;
            animator.SetBool("Talking", false);
        }
    }

    public virtual void OnInteract()
    {

        if (dialogueRunner != null && interactable == true)
        {
            conversationManager.StartDialogue(conversation);
            interactable = false;

            //pCamera.PanTo(transform.position);

            LookAt(playerPos.position);
            //SetTalking(true);
            inConversation = true;
            
        }

    }

    public void ResetInteract()
    {
        if (!inConversation) return; 
        
        Debug.Log("ResetInteract Called");
        interactable = true;
        //SetTalking(false);
        ResetLook();

        inConversation = false;
        SetTalking(false);
    }

    public void LookAtPlayer()
    {
        LookAt(playerPos.position);
    }

    public void LookAt(Vector3 pos)
    {
        transform.LookAt(playerPos);
    }

    public void ResetLook()
    {
        transform.rotation = lookDir;
    }
}
