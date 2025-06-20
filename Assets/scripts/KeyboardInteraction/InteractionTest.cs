using TMPro;
using UnityEngine;

public class InteractionTest : MonoBehaviour, IInteractable
{
    public GameObject UIHoverObject;
    public TextMeshProUGUI UIHoverText;
    private string TooltipText = "[E] Use";
    public bool HasShownUI { get; set; }
    public float floatSpeed = 1f;  // Szybkość pływania
    public float floatDistanceX = 100f; // Maksymalna wysokość pływania
    public float floatDistanceY = 30f;
    private Vector3 originalPosition;
    public Camera mainCamera;
    public BasicGlitchShaderController objectWithInteractiveMaterial;

    public void Start()
    {
        UIHoverObject = GameObject.Find("UI Hover Text");
        UIHoverText = UIHoverObject.GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        if (HasShownUI)
        {
            AnimateUI();
        }
    }

    public void Interact()
    {
        Debug.Log("InteractionNotImplemented");
    }

    public void AnimateUI()
    {
        // Przekształcenie pozycji obiektu na ekran
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        // Zyskujemy szerokość tekstu (można także użyć preferredWidth z TextMeshProUGUI)
        float textWidth = UIHoverText.preferredWidth;
        Vector3 targetPosition = Vector3.zero;

        targetPosition = screenPos + new Vector3((floatDistanceX  * -1), floatDistanceY, 0);

        UIHoverText.rectTransform.position = Vector3.Lerp(UIHoverText.rectTransform.position, targetPosition, Time.deltaTime * 5f);
    }

    public void ShowUI()
    {
        Debug.Log("Show UI");
        UIHoverObject.SetActive(true);
        UIHoverText.text = TooltipText;
        HasShownUI = true;
        if(objectWithInteractiveMaterial != null) objectWithInteractiveMaterial.setGlitchActivationBool(true);
    }

    public void HideUI()
    {
        Debug.Log("Hide UI");
        UIHoverObject.SetActive(false);
        HasShownUI = false;
        if(objectWithInteractiveMaterial != null) objectWithInteractiveMaterial.setGlitchActivationBool(false);
    }

}
