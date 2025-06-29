using UnityEngine;

public class MarkerScript : EntityBase
{
    [SerializeField] Transform target;
    [SerializeField] Renderer renderer;

    public bool isActive => gameObject.activeSelf;

    void Start()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();

        EntityManager.Instance.Register<MarkerScript>(this);
        UpdateEntityDisplayName();

        Deactivate();
    }

    void Update()
    {
        // billboard behaviour
        if (target != null)
        {
            transform.LookAt(target.transform);
            transform.Rotate(0, 180, 0);
        }
    }

    public override void UpdateEntityDisplayName()
    {
        if (string.IsNullOrEmpty(entityName))
            entityName = "Marker";
        base.UpdateEntityDisplayName();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Activate(Vector3 pos)
    {
        this.transform.position = pos;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetColor(Color color)
    {
        if (renderer == null)
        {
            Debug.LogError($"{gameObject.name} - renderer not found");
            return;
        }

        renderer.material.SetColor("_MainColor", color);
    }
}