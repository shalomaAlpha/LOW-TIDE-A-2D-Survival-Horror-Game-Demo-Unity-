using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayNightLightController : MonoBehaviour
{
    public static DayNightLightController Instance;
    public Light2D globalLight; 
    public Gradient lightColorOverTime; 
    public AnimationCurve lightIntensityCurve;
    float maxIntensity = 1f;
    float minIntensity = 0f;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        globalLight = GetComponent<Light2D>();
    }
    private void Update()
    {
        if(TimeSystem.Instance!=null)
        {
            UpdateLighting(TimeSystem.Instance.hour, TimeSystem.Instance.minute);
        }
    }

    public void UpdateLighting(int hour, int minute)
    {
        float timePercent = (hour * 60f + minute) / 1440f;

        globalLight.color = lightColorOverTime.Evaluate(timePercent);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, lightIntensityCurve.Evaluate(timePercent));
        globalLight.intensity = intensity;
    }
}
