using UnityEngine;
using UnityEngine.UI;

public class CrankedTimer : MonoBehaviour
{
    [SerializeField]
    GameObject player1, player2;
    Text text;
    public bool countingDown = false;
    float timeLeft = 10f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countingDown && timeLeft > 0)
        {
            // Update the timer
            text.text = $"Hit each other in {timeLeft:0} secs or DIE";
            timeLeft -= Time.deltaTime;
        }
        else
        {
            text.text = "";
            timeLeft = 10f;
            countingDown = false;
        }

        if (countingDown && timeLeft <= 0)
        {
            // Both players are knocked out
            Debug.Log("Both players are knocked out");

            player1.GetComponent<PlayerController>().KnockOut();
            player2.GetComponent<PlayerController>().KnockOut();
        }
    }
}
