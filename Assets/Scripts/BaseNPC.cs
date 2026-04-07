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

    public Animator animator;
    public Transform playerPos;

    public virtual void Start()
    {
        
        animator = GetComponent<Animator>();
        playerPos = FindFirstObjectByType<FPController>().transform;

        if (animator != null) animator.SetBool("Idle", true);

        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.endDialogue.AddListener(ResetInteract);
            Debug.Log($"{gameObject.name} subscribed to endDialogue"); // confirm it's firing
        }
        else
        {
            Debug.Log("GameManager not found!");
        }

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

    public void ResetInteract()
    {
        Debug.Log("ResetInteract Called");
        interactable = true;
    }
}
