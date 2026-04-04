using UnityEngine;
using Yarn.Unity;

public class TestNPC : NPCDialogue
{
    public override void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        DialogueRunner runner = FindFirstObjectByType<DialogueRunner>();
        runner.AddCommandHandler("TestFunc", TestYarnFunc);

    }

    public void TestYarnFunc()
    {        
        Debug.Log("Kills you aaahhhhh!");
    }
}
