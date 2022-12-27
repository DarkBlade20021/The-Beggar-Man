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
    public int CoinsInBag;
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
        AddCoins(CoinsInBag);
        if(Coins >= maxCoins)
            coinCounterText.text = maxCoins.ToString();
        else if(Coins < maxCoins)
            coinCounterText.text = CoinsUI.ToString();
    }

    public void SubtractCoins()
    {
        if(Coins >= CoinsInBag)
            StartCoroutine(SubtractCoinsRoutine());
    }

    IEnumerator SubtractCoinsRoutine()
    {
        GameObject coinSubtracterText = Instantiate(coinSubtracterObj, coinChangement);
        coinSubtracterText.GetComponent<TMP_Text>().text = "-" + CoinsInBag;
        Coins -= CoinsInBag;
        yield return new WaitForSeconds(waitWhat);
        Destroy(coinSubtracterText);
        CoinsUI -= CoinsInBag;
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

    void AddCoins(int addedCoins)
    {
        additionnedCoins = addedCoins;
        if (Input.GetMouseButtonDown(0) && Coins <= additionnedCoins)
            StartCoroutine(AddCoinsRoutine());
    }

    IEnumerator AddCoinsRoutine()
    {
        GameObject coinAdditionnerText = Instantiate(coinAdditionnerObj, coinChangement);
        coinAdditionnerText.GetComponent<TMP_Text>().text = "+" + additionnedCoins;
        Coins += additionnedCoins;
        yield return new WaitForSeconds(waitWhat);
        Destroy(coinAdditionnerText);
        CoinsUI += additionnedCoins;
    }

}
