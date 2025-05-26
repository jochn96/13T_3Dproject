using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float starAlpha = 0.3f;
        float a = starAlpha;

        while (a > 0)
        {
            a -= (starAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        image.enabled = false;
    }
}
