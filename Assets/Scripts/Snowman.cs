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
    public int maxBlockCount = 0;
    public int remainBlockCount = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxBlockCount = GetMaxBlockCount(level);
        remainBlockCount = maxBlockCount;
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

        OnHit(collider);
    }

    public void OnHit(Collider collision)
    {
        body.transform.localScale = new Vector3(5.0f, 5.0f * Mathf.Max(0.0f, Mathf.Min(1.0f, remainBlockCount / (float)(level * 4)), 5.0f));
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
        maxBlockCount = GetMaxBlockCount(respawnLevel);
        remainBlockCount = maxBlockCount;
        body.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

    }

    int GetMaxBlockCount(int level)
    {
        return level * 4 + Mathf.Max(0, level - 2);
    }
}
