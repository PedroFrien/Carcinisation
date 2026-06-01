using CsvHelper.Configuration;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Yarn;
using Yarn.Unity;
using static UnityEditor.PlayerSettings;

//[System.Serializable]
//public class Conversation
//{
//    public List<BaseNPC> speakingNPCS;
//    public string dialogueName;
//}

//[System.Serializable]
//public class MovementPos
//{
//    public string posName;
//    public Vector3 position;
//    public Quaternion endRot;
//}

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

    public string currentConversation;
    public string[] conversations;
    public int conversationIndex = 0;


    public Animator animator;
    public PanCamera pCamera;
    public Transform playerPos;
    private Quaternion lookDir;
    public bool inConversation = false;
    public bool talking = false;
    public bool moving = false;




    private NavMeshAgent agent;

    //[SerializeField] public List<MovementPos> movementPositions;

    public virtual void Start()
    {
        currentConversation = conversations[conversationIndex];
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("no animator");
        }

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



    [YarnCommand("SetTalking")]
    public void SetTalking(bool isTalking)
    {
        animator = GetComponent<Animator>();
        if (isTalking)
        {
            talking = true;
            animator.SetBool("Talking", true);
        }
        else
        {
            talking = false;
            if (animator != null)
            {
                Debug.Log("aniamtor is assigned. W ragebait");
            }
            animator.SetBool("Talking", false);
        }
    }

    [YarnCommand("TalkToPlayer")]
    public void TalkToPlayer()
    {
        foreach (BaseNPC npc in conversationManager.npcs)
        {
            npc.SetTalking(false);
        }
        PanTo();
        SetTalking(true);
        FacePlayer();
    }

    [YarnCommand("SetAnimation")]
    public void SetAnimation(string animationName)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }


        animator.SetBool(animationName, true);
    }

    [YarnCommand("FacePlayer")]
    public void FacePlayer()
    {
        transform.LookAt(playerPos.position);
    }

    [YarnCommand("LookAt")]
    public void LookAt(float x, float y, float z)
    {
        Vector3 pos = new Vector3(x, y, z);

        transform.LookAt(pos);
    }


    public virtual void OnInteract()
    {

        if (dialogueRunner != null && interactable == true)
        {
            conversationManager.StartDialogue(currentConversation);
            interactable = false;

            //pCamera.PanTo(transform.position);

            FacePlayer();
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

  

    

    public void ResetLook()
    {
        transform.rotation = lookDir;
    }

    [YarnCommand("MoveTo")]
    public IEnumerator MoveTo(float x, float y, float z)
    {
        Vector3 movementPos = new Vector3(x, y, z);

        moving = true;
        agent.SetDestination(movementPos);   
        
        while (moving)
        {
            yield return null;
        }
    }

    [YarnCommand("PanTo")]
    public void PanTo()
    {
        pCamera.PanTo(transform.position);
    }

    [YarnCommand("TeleportTo")]
    public void TeleportTo(float x, float y, float z)
    {
        agent.enabled = false;
        Vector3 movementPos = new Vector3(x, y, z);

        transform.position = movementPos;
        agent.enabled = true;
    }

    [YarnCommand("SetCameraTarget")]
    public void SetCameraTarget()
    {
        pCamera.SetTarget(transform);
    }

    [YarnCommand("IncrementConversation")]
    public void IncrementConversation()
    {
        if (conversations.Length > conversationIndex - 1)
        {
            conversationIndex++;
            currentConversation = conversations[conversationIndex];
        }
        
    }

    [YarnCommand("SetConversationIndex")]
    public void SetConversationIndex(int index)
    {
        if (conversations.Length > index - 1)
        {
            conversationIndex = index;
            currentConversation = conversations[conversationIndex];
        }
        
            
    }

    
    
}
