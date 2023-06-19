using System.Collections.Generic;
using UnityEngine;


class Enemies : MonoBehaviour
{

    public Bot botPrefab;
    public Human humanPrefab;
    public Infected infectedPrefab;

    private List<Transform> usedSpawnPointsBots = new List<Transform>();

    public Transform[] spawnPointsBots;

    public Transform[] patrolPointsBots;


    private List<Transform> usedSpawnPointsHumans = new List<Transform>();
    public Transform[] spawnPointsHumans;


    private List<Transform> usedSpawnPointsInfected = new List<Transform>();

    public Transform[] spawnPointsInfected;


    private void Start(){
        
        SpawnBots();
        SpawnHumans();
        SpawnInfected();
    }

    void SpawnBots(){

        List<Transform[]> patrolPointsBotsList = new List<Transform[]>();
        for (int i = 0; i < patrolPointsBots.Length; i += 2){
            Transform[] patrolPoints = new Transform[2];
            patrolPoints[0] = patrolPointsBots[i];
            patrolPoints[1] = patrolPointsBots[i + 1];
            patrolPointsBotsList.Add(patrolPoints);

        }

        for (int i = 0; i < spawnPointsBots.Length; i++){
            int randomIndex = Random.Range(0, spawnPointsBots.Length);
            Transform spawnPoint = spawnPointsBots[randomIndex];
            while (usedSpawnPointsBots.Contains(spawnPoint)){
                randomIndex = Random.Range(0, spawnPointsBots.Length);
                spawnPoint = spawnPointsBots[randomIndex];
            }
            usedSpawnPointsBots.Add(spawnPoint);
            Bot bot = Instantiate(botPrefab, spawnPoint.position, spawnPoint.rotation);
            bot.SetPatrolPoints(patrolPointsBotsList[i]);
        }
    }

    void SpawnHumans(){
        for (int i = 0; i < spawnPointsHumans.Length; i++){
            int randomIndex = Random.Range(0, spawnPointsHumans.Length);
            Transform spawnPoint = spawnPointsHumans[randomIndex];
            while (usedSpawnPointsHumans.Contains(spawnPoint)){
                randomIndex = Random.Range(0, spawnPointsHumans.Length);
                spawnPoint = spawnPointsHumans[randomIndex];
            }
            usedSpawnPointsHumans.Add(spawnPoint);
            Human human = Instantiate(humanPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    void SpawnInfected(){
        for (int i = 0; i < spawnPointsInfected.Length; i++){
            int randomIndex = Random.Range(0, spawnPointsInfected.Length);
            Transform spawnPoint = spawnPointsInfected[randomIndex];
            while (usedSpawnPointsInfected.Contains(spawnPoint)){
                randomIndex = Random.Range(0, spawnPointsInfected.Length);
                spawnPoint = spawnPointsInfected[randomIndex];
            }
            usedSpawnPointsInfected.Add(spawnPoint);
            Infected infected = Instantiate(infectedPrefab, spawnPoint.position, spawnPoint.rotation);
            infected.killed += OnInfectedKilled;
        }
    }

    private void OnInfectedKilled(){
        int randomIndex = Random.Range(0, usedSpawnPointsInfected.Count);
        Transform spawnPoint = usedSpawnPointsInfected[randomIndex];
        Infected infected = Instantiate(infectedPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}