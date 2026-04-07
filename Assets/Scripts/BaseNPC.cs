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
    private Quaternion lookDir;

    public virtual void Start()
    {
        
        animator = GetComponent<Animator>();
        playerPos = FindFirstObjectByType<FPController>().transform;

        if (animator != null) animator.SetBool("Idle", true);

        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.endDialogue.AddListener(ResetInteract);
        }

        DialogueRunner runner = FindFirstObjectByType<DialogueRunner>();

        AddYarnFunc();

        lookDir = transform.rotation;

    }

    public virtual void AddYarnFunc()
    {

    }

    public virtual void OnInteract()
    {

        if (dialogueRunner != null && interactable == true)
        {
            gameManager.SetDialogue(true);
            dialogueRunner.StartDialogue(dialogueName);
            interactable = false;

            transform.LookAt(playerPos);
            animator.SetBool("Talking", true);

        }
        
    }

    public void ResetInteract()
    {
        if (interactable == true) return; 
        
        Debug.Log("ResetInteract Called");
        interactable = true;
        animator.SetBool("Talking", false);
        transform.rotation = lookDir;
    }
}
