using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullet physics. Piew Piew Piew
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Bullet : MonoBehaviour {
    // Defineer hier de snelheid van de bullet. Standaard is even snel als orgineel.
    public float speed = 500.0f;
    // Defineer hoelang de bullet blijft vliegen. Standaard is even lang als orgineel.
    public float maxLifetime = 10.0f;

    // Maak een object aan voor de bullet.
    private Rigidbody2D _rigidbody;

    private void Awake() {
        // Store references to the bullet's components
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction) {
        // Schiet de bullet in de juiste directie met de gedefineerde snelheid.
        _rigidbody.AddForce(direction * this.speed);
        // Verwijderd het object "bullet" als de waarde maxLifetime is bereikt.
        Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Verwijdert het object als er een collision is met ander object (astroid).
        Destroy(this.gameObject);
    }
}