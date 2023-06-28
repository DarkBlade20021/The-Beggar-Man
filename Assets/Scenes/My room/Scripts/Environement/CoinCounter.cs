using System.Collections;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    [Header("References")]
    public TMP_Text coinCounterText;
    public GameObject coinSubtracterObj;
    public GameObject coinAdditionnerObj;
    public Transform coinChangement;

    [Header("Properties")]
    public int maxCoins;
    int subtractedCoins;
    int additionnedCoins;
    public float waitWhat = 0.5f;
    public int Coins { get; private set; }
    public int CoinsUI { get; private set; }

    private static CoinCounter instance;
    public static CoinCounter Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<CoinCounter>();
            return instance;
        }
    }

    void Start()
    {
        Coins = maxCoins;
        CoinsUI = maxCoins;
    }


    void Update()
    {
        //AddCoins(CoinsInBag);
        if(Coins >= maxCoins)
            coinCounterText.text = maxCoins.ToString();
        else if(Coins < maxCoins)
            coinCounterText.text = CoinsUI.ToString();
    }

    public void SubtractCoins()
    {
        if(Coins >= Inventory.Instance.currentBag.cost)
            StartCoroutine(SubtractCoinsRoutine());
    }

    IEnumerator SubtractCoinsRoutine()
    {
        GameObject coinSubtracterText = Instantiate(coinSubtracterObj, coinChangement);
        coinSubtracterText.GetComponent<TMP_Text>().text = "-" + Inventory.Instance.currentBag.cost;
        Coins -= Inventory.Instance.currentBag.cost;
        yield return new WaitForSeconds(waitWhat);
        Destroy(coinSubtracterText);
        CoinsUI -= Inventory.Instance.currentBag.cost;
    }

    public void SubtractCoinsPercentage(int percentage)
    {
        StartCoroutine(SubtractCoinsPercentRoutine(percentage));
    }

    IEnumerator SubtractCoinsPercentRoutine(int percentage)
    {
        GameObject coinSubtracterText = Instantiate(coinSubtracterObj, coinChangement);
        int subtractedCoins = percentage * Coins / 100;
        coinSubtracterText.GetComponent<TMP_Text>().text = "-" + subtractedCoins;
        Coins -= subtractedCoins;
        yield return new WaitForSeconds(waitWhat);
        Destroy(coinSubtracterText);
        CoinsUI -= subtractedCoins;
    }

    public void AddCoins(int addedCoins)
    {
        additionnedCoins = addedCoins + Coins;
        if (additionnedCoins < maxCoins)
            StartCoroutine(AddCoinsRoutine(addedCoins));
        if(additionnedCoins >= maxCoins)
            StartCoroutine(AddCoinsRoutine(maxCoins - addedCoins));
    }

    IEnumerator AddCoinsRoutine(int addedCoins)
    {
        GameObject coinAdditionnerText = Instantiate(coinAdditionnerObj, coinChangement);
        coinAdditionnerText.GetComponent<TMP_Text>().text = "+" + addedCoins;
        Coins = additionnedCoins;
        yield return new WaitForSeconds(waitWhat);
        Destroy(coinAdditionnerText);
        CoinsUI = additionnedCoins;
    }

}
