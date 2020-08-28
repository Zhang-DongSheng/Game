using UnityEngine;

public class SAMCard : MonoBehaviour
{
    enum CardStatus
    {
        Idle,
        Front,
        Back,
    }

    [SerializeField] private Transform card;

    [SerializeField] private GameObject front;

    [SerializeField] private GameObject back;

    [SerializeField] private float speed;

    private float destination;

    private float current;

    private CardStatus status;

    private void Awake()
    {
        if (card == null) card = transform;
    }

    private void Update()
    {
        switch (status)
        {
            case CardStatus.Front:
            case CardStatus.Back:
                current = Mathf.MoveTowards(current, destination, speed * Time.deltaTime);
                if (Mathf.Abs(current - destination) < 0.1f)
                {
                    current = destination;
                    status = CardStatus.Idle;
                }
                Rotate(current);
                break;
        }
    }

    private void Rotate(float angle)
    {
        card.localEulerAngles = Vector3.up * angle;

        SetSide(angle < 90);
    }

    private void SetSide(bool side)
    {
        if (front != null && front.activeSelf != side)
            front.SetActive(side);
        if (back != null && back.activeSelf != !side)
            back.SetActive(!side);
    }

    public void ToFront()
    {
        destination = 0;

        status = CardStatus.Front;
    }

    public void ToBack()
    {
        destination = 180;

        status = CardStatus.Back;
    }

    public void Default()
    {
        card.localEulerAngles = Vector3.zero;

        SetSide(true);

        status = CardStatus.Idle;
    }
}