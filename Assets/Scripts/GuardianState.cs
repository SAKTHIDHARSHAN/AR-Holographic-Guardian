using TMPro;
using UnityEngine;

public class GuardianState : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI statusText;

    private Material hologramMaterial;
    private Camera mainCamera;

    private readonly int BaseColor = Shader.PropertyToID("_Base_Color");
    private readonly int EmissionColor = Shader.PropertyToID("_Emission_Color");
    private readonly int EmissionStrength = Shader.PropertyToID("_Emission_Strength");

    void Start()
    {
        mainCamera = Camera.main;

        Renderer renderer = GetComponent<Renderer>();
        hologramMaterial = renderer.material;
    }

    void Update()
    {
        if (mainCamera == null)
            return;

        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);

        
        if (distance > 0.5f)
        {
            hologramMaterial.SetColor(BaseColor, Color.cyan);
            hologramMaterial.SetColor(EmissionColor, Color.cyan);
            hologramMaterial.SetFloat(EmissionStrength, 2f);

            statusText.text = "SYSTEM ARMED";
        }

        
        else if (distance > 0.2f)
        {
            float pulse = Mathf.Lerp(2f, 5f,
                (Mathf.Sin(Time.time * 5f) + 1f) / 2f);

            hologramMaterial.SetColor(BaseColor, Color.yellow);
            hologramMaterial.SetColor(EmissionColor, Color.yellow);
            hologramMaterial.SetFloat(EmissionStrength, pulse);

            statusText.text = "WARNING:\nRESTRICTED AREA";
        }

       
        else
        {
            float flash = Mathf.Lerp(2f, 8f,
                (Mathf.Sin(Time.time * 10f) + 1f) / 2f);

            hologramMaterial.SetColor(BaseColor, Color.red);
            hologramMaterial.SetColor(EmissionColor, Color.red);
            hologramMaterial.SetFloat(EmissionStrength, flash);

            statusText.text = "CRITICAL HALT\nBACK AWAY";
        }
    }
}