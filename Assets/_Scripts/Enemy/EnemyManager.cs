using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager main;

    public Transform[] checkpoints;
    public Transform spawnpoint;

    [SerializeField] private GameObject zombie;
    [SerializeField] private GameObject fastZombie;
    [SerializeField] private GameObject tankZombie;

    [SerializeField] private int wave = 1;
    [SerializeField] private int enemyCount = 6;
    [SerializeField] private float enemyCountRate = 0.2f;
    [SerializeField] private float spawnDelayMax = 1f;
    [SerializeField] private float spawnDelayMin = 0.75f;

    [SerializeField] private float zombieRate = 0.5f;
    [SerializeField] private float fastZombieRate = 0.4f;
    [SerializeField] private float tankZombieRate = 0.1f;

    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TextMeshProUGUI waveCounterGUI; 

    private bool waveDone = false;
    private bool waveOver = false;
    private List<GameObject> waveset = new List<GameObject>();
    private int enemyLeft;

    private int zombieCount;
    private int fastZombieCount;
    private int tankZombieCount;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        UpdateWaveCounter();
        setWave();
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (!waveOver && waveDone && enemies.Length == 0)
        {
            Player.main.money += 10 * wave;
            waveOver = true;
            wavePanel.SetActive(true);
        }
    }

    private void setWave()
    {
        zombieCount = Mathf.RoundToInt(enemyCount * (zombieRate + tankZombieCount));
        fastZombieCount = Mathf.RoundToInt(enemyCount * fastZombieRate);
        tankZombieCount = 0;

        if (wave % 5 == 0){
            zombieCount = Mathf.RoundToInt(enemyCount * zombieRate);
            tankZombieCount = Mathf.RoundToInt(enemyCount * tankZombieRate);
        }

        enemyLeft = zombieCount + fastZombieCount + tankZombieCount;
        enemyCount = enemyLeft;

        waveset = new List<GameObject>();

        for (int i = 0; i < zombieCount; i++)
        {
            waveset.Add(zombie);
        }
        for (int i = 0; i < fastZombieCount; i++)
        {
            waveset.Add(fastZombie);
        }
        for (int i = 0; i < tankZombieCount; i++)
        {
            waveset.Add(tankZombie);
        }

        waveset = Shuffle(waveset);

        StartCoroutine(spawn());
    }

    public List<GameObject> Shuffle(List<GameObject> waveSet)
    {
        List<GameObject> temp = new List<GameObject>();
        List<GameObject> result = new List<GameObject>();
        temp.AddRange(waveSet);

        for (int i = 0; i < waveSet.Count; i++)
        {
            int index = Random.Range(0, temp.Count - 1);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }

    public void NextWave() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        wavePanel.SetActive(false);

        if (waveDone && enemies.Length == 0)
        {
            wave++;
            UpdateWaveCounter(); 
            waveDone = false;
            waveOver = false;
            enemyCount += Mathf.RoundToInt(enemyCount * enemyCountRate);
            setWave();
        }
    }

    private void UpdateWaveCounter()
    {
        if (waveCounterGUI != null)
        {
            waveCounterGUI.text = "Horda Atual: " + wave.ToString();
        }
    }

    IEnumerator spawn()
    {
        for (int i = 0; i < waveset.Count; i++)
        {
            Instantiate(waveset[i], spawnpoint.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
        }
        waveDone = true;
    }
}