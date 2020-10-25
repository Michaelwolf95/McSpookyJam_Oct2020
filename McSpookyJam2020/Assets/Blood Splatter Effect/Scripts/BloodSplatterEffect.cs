using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodSplatterEffect : MonoBehaviour
{
    //Instance of the script
    public static BloodSplatterEffect instance;

    //Array of all the blood splatter sprites
    public Sprite[] bloodSplatterSprites;

    //The duration of the splatter
    public float duration = 1;

    //The Image used to display the sprites
    private Image bloodSplatterImage;

    private bool isRoutineRunning;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        //Get the image
        bloodSplatterImage = GetComponentInChildren<Image>();
    }


    public void BloodSplatter()
    {
        //Return if there is no image
        if(bloodSplatterImage == null)
        {
            Debug.LogError("No blood splatter image found!");
            return;
        }

        //Return if there are no elements in the array
        if(bloodSplatterSprites.Length == 0)
        {
            Debug.LogError("Assign one or more sprites to the array!");
            return;
        }

        //Start the coroutine provided that it is not already running
        if (!isRoutineRunning)
        {
            StartCoroutine(BloodSplatterRoutine());
        }
    }


    IEnumerator BloodSplatterRoutine()
    {
        isRoutineRunning = true;

        //Select a random sprite from the array
        bloodSplatterImage.sprite = bloodSplatterSprites[Random.Range(0, bloodSplatterSprites.Length)];

        Color col = bloodSplatterImage.color;

        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            col.a = Mathf.Lerp(1, 0, timer / duration);
            bloodSplatterImage.color = col;

            yield return null;
        }

        isRoutineRunning = false;
    }
}
