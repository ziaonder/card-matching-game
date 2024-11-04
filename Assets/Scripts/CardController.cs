using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour, IClickable
{
    [HideInInspector] public SO_CardData cardData;
    private int row, column;
    public float timeForStateChange = 0f;
    public enum State { GettingHidden, RotatingBack, RotatingFront, Front, Back, FlashingRed, FlashingGreen}
    public State currentState = State.GettingHidden;
    public State CurrentState => currentState;
    public int instanceID;
    [SerializeField] private AudioSource cardFlipSound;
    private bool isFlippedToFrontLast = true;

    private void OnEnable()
    {
        GameManager.OnMatch += CheckMatch;
    }

    private void OnDisable()
    {
        GameManager.OnMatch -= CheckMatch;
    }

    private void Start()
    {
        StartCoroutine(SetImage());
        instanceID = gameObject.GetInstanceID();
    }

    private void Update()
    {
        if(Time.time > 3 && currentState == State.GettingHidden)
        {
            if (isFlippedToFrontLast)
            {
                cardFlipSound.Play();
                isFlippedToFrontLast = false;
            }
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), 
                Quaternion.Euler(0, 180, 0), timeForStateChange);

            if(timeForStateChange < 1)
                timeForStateChange += Time.deltaTime / 2;
            else
            {
                timeForStateChange = 1;
                currentState = State.Back;
            }
        }

        switch (currentState)
        {
            case State.RotatingFront:
                if (!isFlippedToFrontLast)
                {
                    cardFlipSound.Play();
                    isFlippedToFrontLast = true;
                }
                if (timeForStateChange < 1)
                    timeForStateChange += Time.deltaTime * 2;
                else
                {
                    timeForStateChange = 1;
                    currentState = State.Front;
                }

                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 180, 0),
                    Quaternion.Euler(0, 0, 0), timeForStateChange);
                break;
            case State.RotatingBack:
                if (isFlippedToFrontLast)
                {
                    cardFlipSound.Play();
                    isFlippedToFrontLast = false;
                }
                if (timeForStateChange < 1)
                {
                    timeForStateChange += Time.deltaTime * 2;
                    float angleDifference = Quaternion.Angle(Quaternion.Euler(0, 0, 0),
                        Quaternion.Euler(0, 180, 0));
                    float fraction = timeForStateChange / 1;
                    float stepAngle = angleDifference * fraction;
                    transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(0, 0, 0), 
                        Quaternion.Euler(0, 180, 0), -stepAngle);
                }else
                    currentState = State.Back;
                break;
            case State.FlashingGreen:
                Material mat = GameManager.Instance.whiteMat;
                if (timeForStateChange < .1f)
                    mat = GameManager.Instance.greenMat;
                else if(timeForStateChange < .4f)
                    mat = GameManager.Instance.whiteMat;
                else if(timeForStateChange < .5f)
                    mat = GameManager.Instance.greenMat;
                else if (timeForStateChange < .8f)
                    mat = GameManager.Instance.whiteMat;
                else if(timeForStateChange > .9f)
                    Destroy(gameObject);

                transform.GetChild(2).GetComponent<MeshRenderer>().material = mat;
                timeForStateChange += Time.deltaTime;
                break;
            case State.FlashingRed:
                Material mat2 = GameManager.Instance.whiteMat;
                if (timeForStateChange < .1)
                    mat2 = GameManager.Instance.redMat;
                else if (timeForStateChange < .4f)
                    mat2 = GameManager.Instance.whiteMat;
                else if (timeForStateChange < .5f)
                    mat2 = GameManager.Instance.redMat;
                else if (timeForStateChange < .8f)
                    mat2 = GameManager.Instance.whiteMat;
                else if(timeForStateChange > .9f)
                {
                    timeForStateChange = 0f;
                    currentState = State.RotatingBack;
                }

                transform.GetChild(2).GetComponent<MeshRenderer>().material = mat2;
                timeForStateChange += Time.deltaTime;
                break;
        }
    }
    
    private IEnumerator SetImage()
    {
        while (cardData.cardImage == null)
        {
            yield return null;
        }

        transform.GetChild(0).GetComponent<MeshRenderer>().material = cardData.material;
    }
    
    public void SetPosition(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public void OnClick()
    {
        // We do not want to flip the card if it's already flipping or used by another process.
        if (currentState == State.Back)
        {
            timeForStateChange = 0f;
            currentState = State.RotatingFront;
        }
    }

    public void CheckMatch(int instanceID1, int instanceID2, bool isItMatch)
    {
        // Check whether this card is checked.
        if (instanceID1 == gameObject.GetInstanceID() || instanceID2 == gameObject.GetInstanceID())
        {
            timeForStateChange = 0f;
            currentState = isItMatch ? State.FlashingGreen : State.FlashingRed;
        }

    }
}
