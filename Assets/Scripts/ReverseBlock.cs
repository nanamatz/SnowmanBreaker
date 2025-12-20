using UnityEngine;

public class ReverseBlock : Block
{
    private KeyEnum m_VisibleKeyEnum;
    public new KeyEnum BoundKeyEnum
    {
        get => m_VisibleKeyEnum;
        set
        {
            m_VisibleKeyEnum = value;
            int keyEnumCount = (int)KeyEnum.Count;
            m_BoundKeyEnum = (KeyEnum)(((int)value + (keyEnumCount / 2)) % keyEnumCount);
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

    public new IBreakable.Status Process(KeyEnum keyEnum )
    {
        return m_BoundKeyEnum == keyEnum ? IBreakable.Status.Broken : IBreakable.Status.NotInteracted;
    }
}
