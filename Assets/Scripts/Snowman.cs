using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Snowman : MonoBehaviour
{
    public ParticleSystem leftHandParticle;
    public ParticleSystem rightHandParticle;
    public ParticleSystem leftFootParticle;

    public GameObject body;

    public int level = 1;
    public int maxBlockCount = 0;
    public int remainingBlockCount = 0;

    void Awake()
    {
        maxBlockCount = GetMaxBlockCount(level);
        remainingBlockCount = maxBlockCount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxBlockCount = GetMaxBlockCount(level);
        remainingBlockCount = maxBlockCount;
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
        remainingBlockCount--;
        Debug.Log("Remaining Block Count : " + remainingBlockCount.ToString());
        float newScaleY = (float)remainingBlockCount / maxBlockCount;
        body.transform.localScale = new Vector3(5.0f, 5.0f * Mathf.Max(0.0f, Mathf.Min(1.0f, newScaleY)), 5.0f);

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
        remainingBlockCount = maxBlockCount;
        body.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
    }

    int GetMaxBlockCount(int level)
    {
        return level * 4 + Mathf.Max(0, level - 2);
    }
}
