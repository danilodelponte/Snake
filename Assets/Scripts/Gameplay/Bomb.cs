using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float timer;
    private float maxTime = 5f;
    private float ShootForce = 10f;

    public void Shoot(Vector3 direction) {
        if(direction == Vector3.zero) return;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * ShootForce, ForceMode.Impulse);
    }

    public void FixedUpdate() {
        timer += Time.deltaTime;
        if(timer > maxTime) Explode();
    }

    public void Explode(){
        Debug.Log("BOOOOM");
        Destroy(gameObject);
    }
    
    public void OnTriggerEnter(Collider other) {
        // if bomb hits a snake, it is killed
        SnakeSegment segment = other.gameObject.GetComponent<SnakeSegment>();
        if(segment != null) {
            GameplayController.Singleton.KillSnake(segment.Snake);
        }
        Debug.Log("BOOOOM");
        Destroy(gameObject);
    }
}
