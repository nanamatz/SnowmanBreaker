using UnityEngine;

public class Block : MonoBehaviour
{
    public enum Status
    {
        Broken = 0,
        Interacted = 1,
        NotInteracted = 2,
    }
   
    [SerializeField] protected KeyEnum m_BoundKeyEnum;
    public KeyEnum BoundKeyEnum { get => m_BoundKeyEnum; set => m_BoundKeyEnum = value;  }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual Status Process(KeyEnum keyEnum)
    {

        return keyEnum == m_BoundKeyEnum ? Status.Broken : Status.NotInteracted;
    }
}
