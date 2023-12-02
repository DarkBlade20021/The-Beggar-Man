using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [Header("Properties")]
    int current = 0;
    int max;
    public int maxOpenned = 0;
    [Header("Inventory")]
    public CoinBagItem[] coinBags;
    public GameObject[] coinBagsUi;
    public GameObject[] coinBagsUiItems;
    public CoinBagItem currentBag;

    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Inventory>();
            return instance;
        }
    }

    void Start()
    {
        max = coinBags.Length - 1;
    }

    void Update()
    {
        foreach(GameObject ui in coinBagsUiItems)
            ui.SetActive(false);
        for (int i = 0; i <= maxOpenned; i++)
            coinBagsUiItems[i].SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(MyPlayer.Instance.nextAction.WasReleasedThisFrame())
        {
            if(current == maxOpenned)
                current = -1;
            current++;
        }
        else if(MyPlayer.Instance.previousAction.WasReleasedThisFrame())
        {
            if(current == 0)
                current = maxOpenned + 1;
            current--;
        }
        currentBag = coinBags[current];
        foreach(GameObject ui in coinBagsUi)
            ui.SetActive(false);
        coinBagsUi[current].SetActive(true);
   }
}
