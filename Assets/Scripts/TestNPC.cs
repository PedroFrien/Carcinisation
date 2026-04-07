using UnityEngine;
using Yarn.Unity;

public class TestNPC : BaseNPC
{
    public override void AddYarnFunc()
    {
        DialogueRunner runner = FindFirstObjectByType<DialogueRunner>();

        try
        {
            runner.AddCommandHandler("TestFunc", TestYarnFunc);
        }
        catch (System.ArgumentException)
        {
            // Already registered by another instance, skip
        }
    }

    public void TestYarnFunc()
    {        
        Debug.Log("Kills you aaahhhhh!");
    }
}
