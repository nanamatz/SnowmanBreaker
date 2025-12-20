using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public ParticleSystem leftHandParticle;
    public ParticleSystem rightHandParticle;
    public ParticleSystem leftFootParticle;

    public GameObject body;

    public int level = 1;

    // public float maxHp = 0;
    // public float hp = 100;

    public int qteMaxCount;
    public Queue<KeyEnum> qteQueue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // maxHp = level * 100.0f;
        // hp = maxHp;
        qteMaxCount = level * 10;
        qteQueue = new Queue<KeyEnum>();
        for (int i = 0; i < qteMaxCount; i++)
        {
            qteQueue.Enqueue((KeyEnum)Random.Range(0, 3));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnHit(Collider collision, KeyEnum lastPressedKey)
    {
        // hp -= 5;
        // body.transform.localScale = new Vector3(5.0f, 5.0f * Mathf.Max(0.0f, Mathf.Min(1.0f, (hp / maxHp))), 5.0f);
        if (qteQueue.Peek() != lastPressedKey)
            return;

        qteQueue.Dequeue();
        body.transform.localScale = new Vector3(5.0f, 5.0f * Mathf.Max(0.0f, Mathf.Min(1.0f, ((float)qteQueue.Count / (float)qteMaxCount))), 5.0f);
        int layer = collision.gameObject.layer;
        switch (layer)
        {
            case 6:
                {
                    PlayHitParticle(leftHandParticle);
                    break;
                }
            case 7:
                {
                    PlayHitParticle(rightHandParticle);

                    break;
                }
            case 8:
                {
                    PlayHitParticle(leftFootParticle);
                    break;
                }
            default:
                {
                    break;
                }
        }


        //ContactPoint contact = collision.contacts[0];
        //Vector3 hitPoint = contact.point;
        //Vector3 reflectionDir = -collision.relativeVelocity.normalized;

        //Quaternion particleRotation = Quaternion.LookRotation(reflectionDir);


    }

    public void PlayHitParticle(ParticleSystem particle)
    {
        if (false == particle.gameObject.activeSelf)
        {
            particle.gameObject.SetActive(true);
        }

        particle.Stop();
        particle.Play();
    }

    public void Respawn(int respawnLevel)
    {
        level = respawnLevel;
        // maxHp = level * 100.0f;
        // hp = maxHp;
        qteMaxCount = level * 10;
        qteQueue = new Queue<KeyEnum>();
        for (int i = 0; i < qteMaxCount; i++)
        {
            qteQueue.Enqueue((KeyEnum)Random.Range(0, 3));
        }
        body.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

    }
}
