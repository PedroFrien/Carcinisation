using TMPro;
using UnityEngine;
using static Unity.Collections.Unicode;
using System.Collections.Generic;

namespace Yarn.Unity
{
    public class ConversationManager : MonoBehaviour
    {
        public string speakingNPC;

        public BaseNPC currentSpeaker;

        public Conversation currentConversation;

        private PanCamera panCamera;

        [SerializeField]private TextMeshProUGUI nameText;
        private string _lastText;

        private GameManager gameManager;

        private DialogueRunner dialogueRunner;

        [SerializeField] private List<BaseNPC> npcs;





        private void Start()
        {
            dialogueRunner = FindFirstObjectByType<DialogueRunner>();

            nameText = GameObject.Find("Character Name").GetComponent<TextMeshProUGUI>();


            _lastText = nameText.text;
            //nameText.OnPreRenderText += UpdateSpeaker;


            panCamera = FindFirstObjectByType<PanCamera>();

            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.endDialogue.AddListener(ResetConvo);


            try
            {
                dialogueRunner.AddCommandHandler("PanTo", (string npcName) => PanTo(npcName));
            }
            catch (System.ArgumentException)
            {
                // Already registered by another instance, skip
            }



            npcs = new List<BaseNPC>(FindObjectsByType<BaseNPC>(FindObjectsSortMode.None));
        }

        public void PanTo(string npcName)
        {
 
            if (currentSpeaker != null)
            {
                currentSpeaker.SetTalking(false);
                currentSpeaker = null;
            }
            

            var target = currentConversation.speakingNPCS.Find(n => n.speakingName == npcName);
            if (target != null) panCamera.PanTo(target.transform.position);

            currentSpeaker = target;
            currentSpeaker.SetTalking(true);
            currentSpeaker.LookAtPlayer();
        }

        public void StartDialogue(Conversation conversation)
        {
            currentConversation = conversation;

            dialogueRunner.StartDialogue(conversation.dialogueName);
            gameManager.SetDialogue(true);

            
        }



        //public void UpdateSpeaker(TMP_TextInfo textInfo)
        //{
        //    if (currentConversation == null) return;
        //    speakingNPC = textInfo.textComponent.text;

        //    foreach (BaseNPC npc in currentConversation.speakingNPCS)
        //    {
        //        string npcName = npc.name;
        //        if (npcName == speakingNPC && npcName != currentSpeaker.name)
        //        {
        //            SetNewSpeaker(npc);
        //        }
        //    }
        //}

        //public void SetNewSpeaker(BaseNPC speaker)
        //{
        //    currentSpeaker = speaker;
        //    panCamera.PanTo(speaker.transform.position);
        //}

        private void ResetConvo()
        {
            foreach (var npc in currentConversation.speakingNPCS)
            {
                npc.SetTalking(false);
            }

            currentConversation = null;
            currentSpeaker = null;
            speakingNPC = null;
        }

        public void MoveNPC(string npcName, string posName)
        {
            BaseNPC targetNPC = null;
            foreach (BaseNPC npc in npcs)
            {
                if (npc.speakingName == npcName)
                {
                    targetNPC = npc;
                }
            }

            if (targetNPC != null)
            {
                targetNPC.MoveToPos(posName);
            }
        }




    }
}

