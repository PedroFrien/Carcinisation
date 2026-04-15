using CsvHelper.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn;
using Yarn.Unity;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class Conversation
{
    public List<BaseNPC> speakingNPCS;
    public string dialogueName;
}

[System.Serializable]
public class MovementPos
{
    public string posName;
    public Vector3 position;
    public Quaternion endRot;
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


    private NavMeshAgent agent;

    [SerializeField] public List<MovementPos> movementPositions;

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

        agent =  GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position);
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

    public void MoveToPos(string posName)
    {
        MovementPos targetPos = null;
        foreach (MovementPos pos in movementPositions)
        {
            if (pos.posName == posName)
            {
                targetPos = pos;
            }
        }

        if (targetPos != null)
        {
            agent.SetDestination(targetPos.position);
        }
        else
        {
            Debug.Log("Couldn't find a pos from that string! You might've mispelled it");
        }


        
    }
}
