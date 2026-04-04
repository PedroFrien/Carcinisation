using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<BaseItem> startingItems = new List<BaseItem>();
    public List<BaseItem> heldWeapons = new List<BaseItem>();

    public BaseItem currentItem;
    public int currentItemIndex = 0;
    public int maxItems;

    [SerializeField] private Transform ItemHoldPos;

    [SerializeField] private TMP_Text ammoCount;

    //private void Update()
    //{

    //}
    public void Start()
    {
        heldWeapons.Clear();


        if (startingItems.Count > 0 && currentItem == null)
        {            
            foreach (var weapon in startingItems)
            {
                BaseItem spawnedWeapon = Instantiate(weapon, ItemHoldPos.position, ItemHoldPos.rotation);


                spawnedWeapon.transform.parent = ItemHoldPos;
                spawnedWeapon.transform.localPosition = spawnedWeapon.offsetPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.rotateOffsetPos);



                spawnedWeapon.GetComponent<Rigidbody>().isKinematic = true;
                spawnedWeapon.GetComponent<BoxCollider>().enabled = false;

                heldWeapons.Add(spawnedWeapon);
                spawnedWeapon.equipped = true;
                spawnedWeapon.gameObject.SetActive(false);
            }

            currentItem = heldWeapons[0];
            currentItemIndex = 0;



            SwapItem(currentItemIndex);
            
            

        }

    }


    public void SwapItem(int index)
    {
        //if (currentItem != null)
        //{
        //    BaseGun potentialGun = currentWeapon.GetComponent<BaseGun>();
        //    if (potentialGun != null && potentialGun.reloading == true) return;
        //}


        if (heldWeapons.Count >= index + 1 && heldWeapons[index] != null)
        {

            if (currentItem != null)
            {
                currentItem.gameObject.SetActive(false);
            }
            currentItem = heldWeapons[index];
            currentItem.gameObject.SetActive(true);
            currentItem.GetComponent<Rigidbody>().isKinematic = true;
            currentItem.GetComponent<BoxCollider>().enabled = false;


            currentItemIndex = index;
            currentItem = heldWeapons[index];

            //GunUpdate();
        }
    }

    public void UseItem()
    {
        if (currentItem == null)
        {
            Debug.Log("No Weapon Equipped");
        }
        else
        {
            currentItem.Use();
            //GunUpdate();
        }

        
        
    }

    //public void GunUpdate()
    //{
    //    if (currentItem != null)
    //    {
    //        BaseItem gun = currentItem.GetComponent<BaseItem>();
    //        if (gun != null)
    //        {
    //            ammoCount.gameObject.SetActive(true);
    //            ammoCount.text = gun.currentAmmo + " / " + gun.reserveAmmo;
    //        }
    //        else
    //        {
    //            ammoCount.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        ammoCount.gameObject.SetActive(false);
    //    }
        
    //}

    //public void ReloadWeapon()
    //{
    //    if (currentItem == null) return;
    //    BaseGun gun = currentWeapon.GetComponent<BaseGun>();

    //    if (gun != null)
    //    {
    //        gun.StartCoroutine("Reload");
    //    }

    //    GunUpdate();

        
    //}

    public void PickupItem(BaseItem weapon)
    {
        if (heldWeapons.Count == maxItems)
        {
            DropItem();
        }





        weapon.transform.parent = ItemHoldPos;
        weapon.transform.parent = ItemHoldPos;
        weapon.transform.localPosition = Vector3.zero;      
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localPosition = weapon.offsetPos;
        weapon.transform.localRotation = Quaternion.Euler(weapon.rotateOffsetPos);
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().enabled = false;



        heldWeapons.Add(weapon);
        //weapon.gameObject.SetActive(false);

        weapon.equipped = true;
        int weaponIndex = heldWeapons.Count - 1;

        SwapItem(weaponIndex);

    }

    public void DropItem()
    {
        if (currentItem == null) return;

        BaseItem weapon = heldWeapons[currentItemIndex];

        if (weapon != null)
        {
            weapon.gameObject.SetActive(true);
            weapon.transform.parent = null;
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.GetComponent<BoxCollider>().enabled = true;

            weapon.GetComponent<BaseItem>().enabled = false;

            
        }

        currentItem = null;
        
        //weapon.transform.localPosition += weapon.offsetPos;
        //weapon.transform.localRotation = Quaternion.Euler(weapon.rotateOffsetPos);



        weapon.equipped = false;

        heldWeapons.Remove(weapon);
       
        if (heldWeapons.Count < currentItemIndex + 1)
        {
            currentItemIndex -= 1;
            currentItemIndex = Mathf.Clamp(currentItemIndex, 0, heldWeapons.Count + 1);
        }

        SwapItem(currentItemIndex);




    }

    


}
