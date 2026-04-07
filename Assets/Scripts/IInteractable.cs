using UnityEngine;

public interface IInteractable
{
    bool Interactable { get; set; }
    void OnInteract();
}
