using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WiggleButtons : MonoBehaviour
{
    [Header("Buttons to Wiggle")]
    public List<RectTransform> buttonsToWiggle;

    [Header("Wiggle Settings")]
    public float angleAmplitude = 10f;  // Max rotation in degrees
    public float frequency = 2f;        // Speed of the wiggle

    private Dictionary<RectTransform, float> offsetTimes = new Dictionary<RectTransform, float>();

    void Start()
    {
        // Give each button a different wiggle offset for variation
        foreach (var button in buttonsToWiggle)
        {
            offsetTimes[button] = Random.Range(0f, 100f);
        }
    }

    void Update()
    {
        float time = Time.time;

        foreach (var button in buttonsToWiggle)
        {
            if (button == null) continue;

            float offset = offsetTimes[button];
            float angle = Mathf.Sin((time + offset) * frequency) * angleAmplitude;

            button.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
