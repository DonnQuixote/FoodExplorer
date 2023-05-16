using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{

	public static Spawner Instance { get; private set; }
	public Wave[] waves;
	public Enemy enemy;

	Wave currentWave;
	int currentWaveNumber;

	int enemiesRemainingToSpawn;
	int enemiesRemainingAlive;
	float nextSpawnTime;

	public bool isSpawn = true;


	//限定区间随机生成位置
	private float radius = 23.0f;
	private float leftWidthStart = -16.0f;
	private float rightWidth = 16.0f;
	private float topHeight = 0f;
	private float bottomHeight = -18f;

	private float speedMax = 5.0f;

    private void Awake()
    {
		Instance = this;

	}
    void Start()
	{
		NextWave();
		enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0.5f;
	}

	void Update()
	{

		if (isSpawn &&enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
		{
			enemiesRemainingToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

			Enemy spawnedEnemy = Instantiate(enemy, randomPosition(), Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;
		}
	}

	private Vector3 randomPosition()
    {
		Vector3 result = Vector3.zero;
		result.y = enemy.transform.position.y;
		float temp;
		List<float> listFourDirections = new List<float>();
		temp = Random.Range(-radius, leftWidthStart);
		listFourDirections.Add(temp);

		temp = Random.Range(rightWidth, radius);
		listFourDirections.Add(temp);

		temp = Random.Range(-radius, bottomHeight);
		listFourDirections.Add(temp);

		temp = Random.Range(topHeight, radius);
		listFourDirections.Add(temp);

		result.x = listFourDirections[Random.Range(0, 2)];
		result.z = listFourDirections[Random.Range(2, 4)];

		//Debug.Log(result);
		return result;
    }

	void OnEnemyDeath()
	{
		enemiesRemainingAlive--;

		if (enemiesRemainingAlive == 0)
		{
			NextWave();
		}
	}

	void NextWave()
	{
		currentWaveNumber++;

		float tempSpeed = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed + 1.0f;

		enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = tempSpeed < speedMax ? tempSpeed : speedMax;

		if (currentWaveNumber - 1 < waves.Length)
		{
			currentWave = waves[currentWaveNumber - 1];

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemainingAlive = enemiesRemainingToSpawn;
		}
	}

	[System.Serializable]
	public class Wave
	{
		public int enemyCount;
		public float timeBetweenSpawns;
	}


}