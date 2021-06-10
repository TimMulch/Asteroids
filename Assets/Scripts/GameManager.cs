using UnityEngine;
using UnityEngine.UI;

// GameManager zorgt voor de staat van de game zoals scoreboard, levens etc.
public class GameManager : MonoBehaviour{
    /// Speler
    public Player player;

    /// Explosie van astroid
    public ParticleSystem explosionEffect;

    /// Game Over Schem
    public GameObject gameOverUI;

    // Puntentellen
    public int score {
        get;
        private set;
    }
    public Text scoreText;

    // Aantal levens
    public int lives {
        get;
        private set;
        }
    public Text livesText;

    // Maakt een nieuwe gamesessie aan. 
    private void Start(){
        NewGame();
    }

    private void Update() {
        // Reset game en start nieuwe sessie.
        if (this.lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            NewGame();
        }
    }

    public void NewGame() {
        // Verwijderd alle astroid objecten (als deze nog bestaan) in het begin van de game. Anders krijg je gelijk astroids in je scherm en dat moeten we nie hebben.
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        for (int i = 0; i < asteroids.Length; i++) {
            Destroy(asteroids[i].gameObject);
        }

        // Zet overlay UI op niet zichtbaar.
        this.gameOverUI.SetActive(false);

        // Reset score en aantal levens.
        SetScore(0);
        SetLives(3);

        // Spawned de speler. Lekker asteroids schieten :)
        Respawn();
    }

    public void Respawn() {
        // Zet speler in het midden neer.
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }

    public void AsteroidDestroyed(Asteroid asteroid) {
        // Speel de ParticleSystem effect (explosionEffect) af op de de asteroid.
        this.explosionEffect.transform.position = asteroid.transform.position;
        this.explosionEffect.Play();

        // Geeft de speler score op basis van asteroid groote. Score is zelfde als orgineel.
        if (asteroid.size < 0.7f) {
            SetScore(this.score + 100); // Klein
        } else if (asteroid.size < 1.4f) {
            SetScore(this.score + 50); // Middel
        } else {
            SetScore(this.score + 25); // Groot
        }
    }

    public void PlayerDeath(Player player) {
        // Speel de ParticleSystem effect (explosionEffect) af op de de speler.
        this.explosionEffect.transform.position = player.transform.position;
        this.explosionEffect.Play();

        // Haalt er 1 leven vanaf
        SetLives(this.lives - 1);

        // Kijkt of de speler nog levens heeft.
        if (this.lives <= 0) {
            GameOver();
        } else {
            // Als je levens hebt mag je gewoon weer lekker asteroids schieten...
            Invoke(nameof(Respawn), player.respawnDelay);
        }
    }

    public void GameOver(){
        // zet de overlay UI weer op zichtbaar
        this.gameOverUI.SetActive(true);
    }

    private void SetScore(int score) {
        // Zet de highscore neer en update de score text linksboven
        this.score = score;
        this.scoreText.text = score.ToString();
    }

    private void SetLives(int lives){
        // Zet het aantal levens neer en update de score text linksboven
        this.lives = lives;
        this.livesText.text = lives.ToString();
    }

}
