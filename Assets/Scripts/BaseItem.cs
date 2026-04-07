using UnityEngine;

public abstract class BaseItem : MonoBehaviour, IInteractable
{
    public float damage;

    public Vector3 offsetPos;
    public Vector3 rotateOffsetPos;
    public float useInterval;
    public bool canUse = true;

    public bool equipped;

    public bool Interactable { get; set; } = true;


    public virtual void Use()
    {
        if (!equipped || !canUse) return;
        canUse = false;

        ItemAction();

        Invoke("CooldownReset", useInterval);
    }

    public void CooldownReset()
    {
        canUse = true;
    }

    public abstract void ItemAction();

    public void OnInteract()
    {
        FindFirstObjectByType<ItemManager>().PickupItem(this);
    }
}
