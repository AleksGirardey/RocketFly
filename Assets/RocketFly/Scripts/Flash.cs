using UnityEngine;

public class Flash : MonoBehaviour {
    [Range(0f, 1f)] public float minAlpha;
    [Range(0f, 1f)] public float maxAlpha;

    [Range(2f, 100f)] public float speed;
    
    private SpriteRenderer _renderer;
    private Color _color;
    private bool _up = true;
    
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = new Color(255, 255, 255, minAlpha);
    }
    
    private void Update() {
        float alpha = _renderer.color.a;
        
        if (_up) {
            alpha = Mathf.Clamp(alpha + Time.deltaTime, minAlpha, maxAlpha);
            if (alpha >= maxAlpha) _up = false;
        } else {
            alpha = Mathf.Clamp(alpha - Time.deltaTime, minAlpha, maxAlpha);
            if (alpha <= minAlpha) _up = true;
        }

        _renderer.color = new Color(255,255,255, alpha);
        Debug.Log($"{_renderer.color}");
    }
}
