using UnityEngine;
using Yarn.Unity;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private bool oneTimeUse = true;

    [SerializeField] private string conversation;

    [SerializeField] private ConversationManager conversationManager;

    private void Awake()
    {
        conversationManager = FindFirstObjectByType<ConversationManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerCutscene();
    }

    private void TriggerCutscene()
    {
        conversationManager.StartDialogue(conversation);

        if (oneTimeUse) Destroy(gameObject);
    }
}
