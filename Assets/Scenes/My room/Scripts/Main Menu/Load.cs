using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Menu");
    }
}
