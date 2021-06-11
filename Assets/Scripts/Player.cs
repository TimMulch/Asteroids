using UnityEngine;

// Beheert de beweging (movement) en het schieten van de speler.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class Player : MonoBehaviour {
    // Snelheid naar voren
    public float thrustSpeed = 2.0f;
    // Draai snelheid
    public float rotationSpeed = 0.1f;
    // Respawn timer. Standaard is 3 seconden.
    public float respawnDelay = 3.0f;
    // Aantal seconden godmode na het respawnen. Standaard is 3 seconden.
    public float respawnInvulnerability = 3.0f;
    // Het object dat gecloned wordt als er een nieuwe bullet wordt gemaakt.
    public Bullet bulletPrefab;
    // De huidige directie waar de speler naar draait. 1=left, -1=right, 0=none
    private float turnDirection = 0.0f;
    /// Boolean als de speler naar voren gaat. if thrusting.
    private bool thrusting = false;
    // Maakt een object van de speler.
    private Rigidbody2D _rigidbody;

    private void Awake(){
        // Store references to the player's components
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable(){
        // Geeft de speler tijdelijk godmode als in geen collisions. Geeft de speler tijd om weg te komen na een respawn.
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        Invoke(nameof(TurnOnCollisions), this.respawnInvulnerability);
    }

    private void Update(){
        // Als de speler naar voren gaat met [W] of [🠕]: Beweeg dan de speler naar voren
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        
        // Duwt de speler in de juiste richting op basis van de movement keys [A][S] of [🠔][➞]
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            turnDirection = 1.0f;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            turnDirection = -1.0f;
        } else {
            turnDirection = 0.0f;
        }

        // Als de speler op spatie drukt of klikt met linkermuisknop. Schiet dan bullet.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    private void FixedUpdate() {
        // Geef gas / thrust
        if (thrusting) {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }

        // Geef torque om beetje te vliegen nadat je gas "thrust" heb gegeven
        if (turnDirection != 0.0f) {
            _rigidbody.AddTorque(this.rotationSpeed * turnDirection);
        }
    }

    private void Shoot() {
        // Schiet een bullet in de juiste directie. (zelfde als speler)
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
        GetComponent<AudioSource>().Play();
    }

    private void TurnOnCollisions() {
        // Speler met collisions
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Kijkt of de speler is gecrashed tegen een asteroid.
        if (collision.gameObject.tag == "Asteroid") {
            // Stopt alle movement van de speler.
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            // Stop alle controls en stop het renderen van de speler.
            this.gameObject.SetActive(false);

            // Update scoreboard met aantal levens.
            FindObjectOfType<GameManager>().PlayerDeath(this);
        }
    }

}
