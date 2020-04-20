using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour {
    public List<GameObject> vehicles;
    
    public Vector3 bottomRange;
    public Vector3 topRange;

    public int vehicleCount;

    private void Awake() {
        
        for (int i = 0; i <= vehicleCount; i++) {
            float y = Random.Range(bottomRange.y, topRange.y);
            float numStep = Mathf.Floor(y / 4);
            float adjustedY = numStep * 4;
            
            Instantiate(vehicles[Random.Range(0, vehicles.Count - 1)], 
                new Vector3(0, adjustedY, 0), Quaternion.identity, transform);
        }
    }
}
