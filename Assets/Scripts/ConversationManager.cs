using TMPro;
using UnityEngine;
using static Unity.Collections.Unicode;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using System;

namespace Yarn.Unity
{
    public class ConversationManager : MonoBehaviour
    {
        public static ConversationManager Instance { get; private set; }

        

        

        public string currentConversation;

        private PanCamera panCamera;

        private string _lastText;

        private GameManager gameManager;

        private DialogueRunner dialogueRunner;


        [SerializeField] public List<BaseNPC> npcs;

        [SerializeField] private TextMeshProUGUI nameText;
        private BaseNPC currentSpeaker;
        


        
        



       


        public bool inConversation = false;
 

        public bool conversationPaused = false;





        private void Awake()
        {
            // Enforce singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            Debug.Log(currentSpeaker);
        }




        private void Start()
        {

            dialogueRunner = FindFirstObjectByType<DialogueRunner>();

            nameText = GameObject.Find("Character Name").GetComponent<TextMeshProUGUI>();


            _lastText = nameText.text;
            //nameText.OnPreRenderText += UpdateSpeaker;


            panCamera = FindFirstObjectByType<PanCamera>();

            gameManager = FindFirstObjectByType<GameManager>();
            gameManager.endDialogue.AddListener(ResetConvo);


            dialogueRunner.AddCommandHandler("PanToLocation", (float x, float y, float z) => PanToLocation(x, y, z));
            //dialogueRunner.AddCommandHandler("MoveTo", (string npcName, float x, float y, float z) => MoveTo(npcName, x, y, z));
            //dialogueRunner.AddCommandHandler("SetCameraTarget", (string npcName) => SetCameraTarget(npcName));
            dialogueRunner.AddCommandHandler("RemoveCameraTarget", () => RemoveCameraTarget());
            dialogueRunner.AddCommandHandler("PauseConversation", () => PauseConversation());
            dialogueRunner.AddCommandHandler("ResumeConversation", () => ResumeConversation());
            //dialogueRunner.AddCommandHandler("IncrementConversation", (string npcName) => IncrementConversation(npcName));



            npcs = new List<BaseNPC>(FindObjectsByType<BaseNPC>(FindObjectsSortMode.None));
        }





        public void PanToLocation(float x, float y, float z)
        {
            Vector3 location = new Vector3(x, y, z);

            panCamera.PanTo(location);
        }


        //    var target = currentConversation.speakingNPCS.Find(n => n.speakingName == npcName);
        //    if (target != null) panCamera.PanTo(target.transform.position);

            //    currentSpeaker = target;
            //    currentSpeaker.SetTalking(true);
            //    currentSpeaker.LookAtPlayer();
            //}


            //public IEnumerator MoveTo(string npcName, float x, float y, float z)
            //{
            //    var target = currentConversation.speakingNPCS.Find(n => n.speakingName == npcName);
            //    if (target != null)
            //    {
            //        target.MoveToPos(x, y, z);
            //    }

            //    while (target.moving)
            //    {
            //        yield return null;
            //    }

            //    ResumeConversation();
            //}


            //public void SetCameraTarget(string npcName)
            //{
            //    var target = currentConversation.speakingNPCS.Find(n => n.speakingName == npcName);
            //    if (target != null)
            //    {
            //        panCamera.SetTarget(target.transform);
            //    }
            //}


        public void RemoveCameraTarget()
        {
            panCamera.RemoveTarget();
        }


        public void StartDialogue(string conversation)
        {
            currentConversation = conversation;

            dialogueRunner.StartDialogue(conversation);
            gameManager.SetDialogue(true);           
        }

        public IEnumerator PauseConversation()
        {
            conversationPaused = true;

            while (conversationPaused)
            {
                Debug.Log("Conversation Still Paused");
                yield return null;
            }

            Debug.Log("Conversation has resumed");
        }

        public void ResumeConversation()
        {
            Debug.Log("Resuming Conversation");
            conversationPaused = false;
        }

        //public void IncrementConversation(string npcName)
        //{
        //    var target = currentConversation.speakingNPCS.Find(n => n.speakingName == npcName);
        //    if (target != null)
        //    {
        //        target.IncrementConversation();
        //    }
        //}


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
            //foreach (var npc in currentConversation.speakingNPCS)
            //{
            //    npc.SetTalking(false);
            //}

            currentConversation = null;


            foreach (BaseNPC npc in npcs)
            {
                npc.SetTalking(false);
            }
            //speakingNPC = null;
        }
  





    }
}

