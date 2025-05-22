using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Test : MonoBehaviour
{
    [SerializeField] private int lifeSpider = 3;

    public SpriteRenderer spider;

    private void Start()
    {
        spider = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (lifeSpider)
        {
            case 3:
                Debug.Log("damage 3");
                break;

            case 2:
                Debug.Log("damage 2");
                break;

            case 1:
                Debug.Log("damage 1");
                break;

            case 0:
                Debug.Log("damage 0");
                Destroy(GetComponent<SpriteRenderer>());
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D Coll)
    {
        if(Coll.gameObject.tag == "PlayerAttack")
        {
            lifeSpider--;
        }
    }

}
