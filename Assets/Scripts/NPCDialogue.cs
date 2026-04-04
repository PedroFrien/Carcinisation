using UnityEngine;
using Yarn;
using Yarn.Unity;

public abstract class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueRunner dialogueRunner;
    public GameManager gameManager;

    [SerializeField] private string dialogueName;

    public virtual void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public virtual void OnInteract()
    {
        gameManager.SetDialogue(true);
        dialogueRunner.StartDialogue(dialogueName);
    }
}
