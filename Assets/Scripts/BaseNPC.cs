using CsvHelper.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Conversation currentConversation;
    public Conversation[] conversations;
    public int conversationIndex = 0;


    public Animator animator;
    public PanCamera pCamera;
    public Transform playerPos;
    private Quaternion lookDir;
    public bool inConversation = false;
    public bool talking = false;
    public bool moving = false;


    private NavMeshAgent agent;

    [SerializeField] public List<MovementPos> movementPositions;

    public virtual void Start()
    {
        currentConversation = conversations[conversationIndex];
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

    private void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(transform.position, agent.destination) < 1f)
            {
                moving = false;
                agent.SetDestination(transform.position);
            }
        }
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
            conversationManager.StartDialogue(currentConversation);
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

    public void MoveToPos(float x, float y, float z)
    {
        Vector3 movementPos = new Vector3(x, y, z);

        moving = true;
        agent.SetDestination(movementPos);        
    }

    public void IncrementConversation()
    {
        conversationIndex++;
        currentConversation = conversations[conversationIndex];
    }
}
