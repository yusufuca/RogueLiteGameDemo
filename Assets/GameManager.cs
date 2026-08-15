using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;



    public GameObject player;
    public GameObject goblin;
    public bool isDebugTextOpen = true;
    private void Awake()
    {
        gm = this;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Instantiate(goblin);
        }
    }
}
