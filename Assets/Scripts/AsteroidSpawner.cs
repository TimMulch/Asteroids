using UnityEngine;

/// Zorgt ervoor dat er asteroids blijven spawnen
public class AsteroidSpawner : MonoBehaviour {
    /// Hoofdobject (Clones)
    public Asteroid asteroidPrefab;

    /// Hoe ver de asteroids kunnen spawnen vanaf speler.
    public float spawnDistance = 12.0f;

    /// Om de hoeveel seconden er een asteroid spawned.
    public float spawnRate = 1.0f;

    /// Hoeveel asteroids er elke keer spawnen. Standaard is 1 net als orgineel.
    public int amountPerSpawn = 1;

    /// Radius waarin de asteroids kunnen afwijken van orginele spawnpoint.
    public float trajectoryVariance = 15.0f;

    private void Start() {
        // Maak een asteroid met vaste snelheid
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    public void Spawn() {
        for (int i = 0; i < this.amountPerSpawn; i++) {
            // Kiest een random richting vanaf middenpunt speler en spawned een asteroid ver weg.
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = spawnDirection * this.spawnDistance;

            // Maakt de spawnpoint (middenpunt)
            spawnPoint += this.transform.position;

            // Zorgt ervoor dat de asteroid beetje van pad afwijkt.
            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            //Maak een nieuwe asteroid met een random groote.
            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            // Zorgt ervoor dat de asteroid naar de speler beweegt.
            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }

}
