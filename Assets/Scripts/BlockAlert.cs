using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;
public class BlockAlert : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animation anim = null;
    public Image image;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Alert()
    {
        StartCoroutine(PlayAnimationRoutine());
    }

    IEnumerator PlayAnimationRoutine()
    {
        image.color = Color.red;

        anim.Play();
        do
        {
            yield return new WaitForEndOfFrame();
        } while (anim.isPlaying);

        image.color = Color.white;
        yield return null;
    }
}
