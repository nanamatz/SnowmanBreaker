using UnityEngine;

public class SnowmanCollisionChecker : MonoBehaviour
{
    public Snowman parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Player")
        {
            return;
        }

        PlayerController controller = collider.GetComponentInParent<PlayerController>();
        parent.OnHit(collider, controller.lastPressedKey);
    }
}
