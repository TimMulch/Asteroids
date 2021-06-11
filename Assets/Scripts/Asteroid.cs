using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class Asteroid : MonoBehaviour {
    // Array met verschillende asteroids textures.
    public Sprite[] sprites;
    // Standaard grote van de asteroid.
    public float size = 1.0f;
    // De minimale groote van de asteroids
    public float minSize = 0.35f;
    // De maximale groote van de asteroids
    public float maxSize = 1.65f;
    // Snelheid van de asteroids
    public float movementSpeed = 50.0f;
    // Max tijd voordat de asteroid despawned (performance boost)
    public float maxLifetime = 30.0f;
    // SpriteRenderer
    private SpriteRenderer spriteRenderer;
    // Maakt een object aan voor de asteroid.
    private Rigidbody2D _rigidbody;

    private void Awake() {
        // Maakt een object aan voor de asteroid.
        _rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        // Kiest een random sprite als asteroid, anders lijkt alles op elkaar.
        spriteRenderer.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
        // Random draai van asteroid, anders lijkt alles op elkaar.
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        // Maakt een asteroid aan op basis van de standaard gedefineerde instellingen
        this.transform.localScale = Vector3.one * this.size;
        _rigidbody.mass = this.size;
        // Verwijderd asteroid als object maxlifetime is.
        Destroy(this.gameObject, this.maxLifetime);
    }

    public void SetTrajectory(Vector2 direction) {
        // Beweegt de asteroid in richting van de aangemaakte directie met vaste snelheid.
        _rigidbody.AddForce(direction * this.movementSpeed);
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bullet") { //Als de asteroid geraakt is door een bullet.
            // Bevat geluideffect voor explosie.
            GameObject obj = GameObject.Find("AudioHolder");
            AudioSource aud = obj.GetComponent<AudioSource>();
            // Speel explosie geluidje af
            aud.Play();
            if ((this.size * 0.5f) >= this.minSize) { // Als de asteroid groot genoeg is om in 2 te splitten.
                // Maakt 2 nieuwe asteroids aan. Als een geraakt is split deze in 2 net zo lang tot dat deze klein genoeg is.
                CreateSplit();
                CreateSplit();
            }
            // update de asteroid als destroyed zodat score aangepast wordt
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject, 3f); // Verwijder het object
        }
    }

    private Asteroid CreateSplit() {
        // Spawned een nieuwe asteroid op de zelfde positie als de oude, wel met andere richting om botsing te verkomen
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        // Maakt de nieuwe asteroid de helft kleiner dan de vorige.
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        // Maakt een nieuwe baan aan voor de asteroid.
        half.SetTrajectory(Random.insideUnitCircle.normalized);
        return half;
    }
}
