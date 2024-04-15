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

    void Update()
    {
        if (!countingDown)
        {
            text.text = "";
        }
        else if (timeLeft > 10)
        {
            // Don't display the countdown when timeLeft is greater than 10
            text.text = "";
            timeLeft -= Time.deltaTime;
        }
        else if (timeLeft > 0)
        {
            // Start displaying the countdown when timeLeft is 10 or less
            text.text = $"HIT EACH OTHER IN {timeLeft:0}s OR DIE";
            timeLeft -= Time.deltaTime;
        }
        else if (timeLeft <= 0)
        {
            text.text = "";
            player1.GetComponent<PlayerController>().KnockOut();
            player2.GetComponent<PlayerController>().KnockOut();
        }
    }
    public void ResetTimer()
    {
        countingDown = true; // Start the countdown again
        timeLeft = 20f;
        text.text = "";
    }
}
