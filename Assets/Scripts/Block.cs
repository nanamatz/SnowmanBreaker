using UnityEngine;
public interface IBreakable
{
    enum Status
    {
        Broken = 0,
        Interacted = 1,
        NotInteracted = 2,
    }

    public Status Process(KeyEnum keyEnum);
}

public class Block : MonoBehaviour, IBreakable
{
    protected KeyEnum m_BoundKeyEnum;
    public KeyEnum BoundKeyEnum { get => m_BoundKeyEnum; set => m_BoundKeyEnum = value;  }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IBreakable.Status Process(KeyEnum keyEnum)
    {
        return keyEnum == m_BoundKeyEnum ? IBreakable.Status.Broken : IBreakable.Status.NotInteracted;
    }
}
