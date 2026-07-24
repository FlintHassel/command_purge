using System.Collections;
using UnityEngine;
using TMPro;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 0.5f; // Kecepatan kedip dalam detik
    private TMP_Text textComponent;
    private bool isBlinking = true;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        isBlinking = true;
        StartCoroutine(BlinkRoutine());
    }

    void OnDisable()
    {
        isBlinking = false;
        StopAllCoroutines();
    }

    private IEnumerator BlinkRoutine()
    {
        while (isBlinking && textComponent != null)
        {
            // Toggle alpha antara 1 (kelihatan) dan 0 (tidak kelihatan)
            Color color = textComponent.color;
            color.a = color.a > 0.5f ? 0f : 1f;
            textComponent.color = color;

            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
