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

    public float maxHp = 0;
    public float hp = 100;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHp = level * 100.0f;
        hp = maxHp;
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
        hp -= 5;
        body.transform.localScale = new Vector3(5.0f, 5.0f * Mathf.Max(0.0f, Mathf.Min(1.0f, (hp / maxHp))), 5.0f);
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
        maxHp = level * 100.0f;
        hp = maxHp;
        body.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);

    }
}
