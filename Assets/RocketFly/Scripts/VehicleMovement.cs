using DG.Tweening;
using UnityEngine;

public class VehicleMovement : MonoBehaviour {
    public Transform leftPoint;
    public Transform rightPoint;
    
    public float minSpeed = 200;
    public float maxSpeed = 500;
    public bool goingLeft;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    public void Awake() {
        Vector3 rightPosition = rightPoint.position;
        Vector3 leftPosition = leftPoint.position;

        _startPosition = goingLeft ? rightPosition : leftPosition;
        _endPosition = goingLeft ? leftPosition : rightPosition;

        transform.position = _startPosition;
    }

    public void FixedUpdate() {
        Vector3 pos = transform.position;

        if (pos.x >= _endPosition.x - 1 && pos.x <= _endPosition.x + 1) {
            transform.position = _startPosition;
        }

        float move = Time.fixedDeltaTime * Random.Range(minSpeed, maxSpeed);
        move = goingLeft ? -move : move;

        pos = new Vector3(
            Mathf.Clamp(pos.x + move, leftPoint.position.x, rightPoint.position.x),
            pos.y,
            pos.z);
        
        transform.DOMove(pos, 1f);
    }
}