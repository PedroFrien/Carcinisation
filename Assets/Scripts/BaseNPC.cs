using UnityEngine;
using Yarn;
using Yarn.Unity;

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

    [SerializeField] private string dialogueName;

    private Animator animator;
    private Transform playerPos;

    public virtual void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        animator = GetComponent<Animator>();
        playerPos = FindFirstObjectByType<FPController>().transform;

        if (animator != null) animator.SetBool("Idle", true); 

    }

    public virtual void OnInteract()
    {

        if (dialogueRunner != null && interactable == true)
        {
            gameManager.SetDialogue(true);
            dialogueRunner.StartDialogue(dialogueName);
            interactable = false;
        }
        
    }
}
