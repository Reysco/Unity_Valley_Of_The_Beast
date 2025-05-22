using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    InventoryController inventoryController;
    [SerializeField] ItemContainerPanel itemContainerPanel;
    Transform openedChest;
    [SerializeField] float maxDistance = 0.8f;

    [SerializeField] GameObject chestInventory;
    [SerializeField] Animator chestAnim;

    private void Awake()
    {
        inventoryController= GetComponent<InventoryController>();
    }

    private void Update()
    {
        if(openedChest != null)
        {
            float distance = Vector2.Distance(openedChest.position, transform.position);
            if(distance > maxDistance)
            {
                openedChest.GetComponent<LootContainerInteract>().Close(GetComponent<Character>());
                StartCoroutine("ChestClose");
            }
        }
    }

    public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer;
        itemContainerPanel.inventory = targetItemContainer;
        openedChest = _openedChest;

        StopCoroutine("ChestClose"); //para evitar bug de caso abra o bau mt rapido, fique o inventario do bau na tela
        StartCoroutine("ChestOpen");
    }

    public void Close()
    {
        StopCoroutine("ChestOpen"); //para evitar bug de caso abra o bau mt rapido, fique o inventario do bau na tela
        StartCoroutine("ChestClose");
        openedChest = null;
    }


    IEnumerator ChestOpen()
    {
        chestInventory.SetActive(true);
        chestAnim.SetBool("open", true);
        inventoryController.Open();

        yield return new WaitForSeconds(0.3f);
        itemContainerPanel.gameObject.SetActive(true);
    }

    IEnumerator ChestClose()
    {
        chestAnim.SetBool("open", false);

        yield return new WaitForSeconds(0.1f);
        itemContainerPanel.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        chestInventory.SetActive(false);
        inventoryController.Close();
    }
}
