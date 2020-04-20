using DG.Tweening;
using UnityEngine;

public class VehicleMovement : MonoBehaviour {
    public Transform leftPoint;
    public Transform rightPoint;
    
    public float minSpeed = 200;
    public float maxSpeed = 500;
    public bool goingLeft;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float speed;
    
    public void Awake() {
        Vector3 rightPosition = rightPoint.position;
        Vector3 leftPosition = leftPoint.position;

        startPosition = goingLeft ? rightPosition : leftPosition;
        endPosition = goingLeft ? leftPosition : rightPosition;

        speed = Random.Range(minSpeed, maxSpeed);
        
        transform.position = startPosition;
    }

    public void FixedUpdate() {
        Vector3 pos = transform.position;

        if (Vector3.Distance(pos, endPosition) < 1) {
            transform.DOKill();
            transform.position = startPosition;
            speed = Random.Range(minSpeed, maxSpeed);
        }

        pos = transform.position;

        float move = Time.fixedDeltaTime * speed;
        move = goingLeft ? -move : move;

        pos = new Vector3(
            Mathf.Clamp(pos.x + move, leftPoint.position.x, rightPoint.position.x),
            pos.y,
            pos.z);
        
        transform.DOMove(pos, 1f);
    }
}