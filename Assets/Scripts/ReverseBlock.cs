using UnityEngine;

public class ReverseBlock : Block
{
    public new KeyEnum BoundKeyEnum
    {
        get => m_BoundKeyEnum;
        set
        {
            m_BoundKeyEnum = value;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Status Process(KeyEnum visibleKeyEnum)
    {
        KeyEnum reversedKeyEnum = visibleKeyEnum;
        switch (visibleKeyEnum)
        {
        case KeyEnum.Up:
        {
            reversedKeyEnum = KeyEnum.ReverseDown;
            break;
        }
        case KeyEnum.Down:
        {
            reversedKeyEnum = KeyEnum.ReverseUp;
            break;
        }
        case KeyEnum.Left:
        {
            reversedKeyEnum = KeyEnum.ReverseRight;
            break;
        }
        case KeyEnum.Right:
        {
            reversedKeyEnum = KeyEnum.ReverseLeft;
            break;
        }
        }
        return m_BoundKeyEnum == reversedKeyEnum ? Status.Broken : Status.NotInteracted;
    }
}
