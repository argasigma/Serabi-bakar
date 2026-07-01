using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Taruh script ini di GameObject "FadeCanvas" yang berisi:
/// Canvas (Screen Space - Overlay, sort order paling atas)
///   > Image "FadeImage" - full screen, warna hitam, alpha 0 di awal
///
/// Cara pakai dari script lain:
///   screenFader.FadeOutThenIn(() => { /* pindahkan player di sini */ });
/// </summary>
public class ScreenFader : MonoBehaviour
{
    [Header("Referensi UI")]
    [Tooltip("Image hitam full-screen yang menutupi layar")]
    public Image fadeImage;

    [Header("Pengaturan Durasi")]
    public float fadeOutDuration = 0.6f;
    public float holdBlackDuration = 0.15f; // jeda gelap total sebelum fade in
    public float fadeInDuration = 0.6f;

    private void Awake()
    {
        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.raycastTarget = false; // biar tidak blok input pas transparan
        }
    }

    /// <summary>
    /// Fade ke hitam, jalankan aksi di tengah (mis. pindah posisi player),
    /// lalu fade kembali terang.
    /// </summary>
    public void FadeOutThenIn(Action onBlackScreen)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(onBlackScreen));
    }

    private IEnumerator FadeRoutine(Action onBlackScreen)
    {
        if (fadeImage != null)
            fadeImage.raycastTarget = true; // blok klik/tap saat transisi

        yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration));

        onBlackScreen?.Invoke();

        if (holdBlackDuration > 0f)
            yield return new WaitForSeconds(holdBlackDuration);

        yield return StartCoroutine(Fade(1f, 0f, fadeInDuration));

        if (fadeImage != null)
            fadeImage.raycastTarget = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeImage == null) yield break;

        float t = 0f;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, to);
    }
}