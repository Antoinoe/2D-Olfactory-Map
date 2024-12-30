using UnityEngine;
using UnityEngine.Profiling;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace OlfactoryMap
{
    public class OlfactoryMapProfilerAnalyser : MonoBehaviour
    {
        [SerializeField] private bool multipleProfilingSessions = false;
        [SerializeField] private List<SessionSetup> sessions = new List<SessionSetup>();
        [SerializeField] private int repetition = 3;
        [SerializeField] private string fileName = "empty";
        [SerializeField] private bool movePlayerAtStart = true;
        [SerializeField] private FakeSmellPlayer player;
        [SerializeField] private float profilingDuration = 5.0f;
        [SerializeField] private bool isProfiling = false;
        [SerializeField] private List<string> profilingData;
        [SerializeField] private float startTime;

        private int currentSessionIndex = 0;

        private void Awake()
        {
            currentSessionIndex = 0;
        }

        private void Start()
        {
            player.MovementController.OnWalkComplete.AddListener(() => OnWalkComplete());
        }

        public void SetupSession()
        {
            try
            {
                print(sessions[currentSessionIndex]);
            }
            catch
            {
                Debug.LogError("END");
                return;
            }

            player.MovementController.WalkSpeed = sessions[currentSessionIndex].walkSpeed;

            for (int i = 0; i < player.SmellTexReader.AvailableSmells.Count; i++)
            {
                player.SmellTexReader.AvailableSmells[i].enabled = i < sessions[currentSessionIndex].nbOfTex;
            }

            fileName = $"{sessions[currentSessionIndex].nbOfTex}-tex_{sessions[currentSessionIndex].walkSpeed}-mov_";
            StartProfiling();
        }



        public void StartProfiling()
        {
            if (!isProfiling)
            {
                isProfiling = true;
                startTime = Time.time;
                profilingData = new List<string>();
                StartCoroutine(CollectProfilingData());

                if(sessions[currentSessionIndex].walkSpeed != 0)
                    player.MovementController.StartMovements();

                Debug.Log("Profiling started.");
            }
        }
        private IEnumerator CollectProfilingData()
        {
            while (isProfiling)
            {
                float cpuUsage = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024); // en Mo
                float gcMemory = Profiler.GetMonoUsedSizeLong() / (1024 * 1024); // en Mo
                float frameRate = 1.0f / Time.deltaTime; // FPS

                var pos = new Vector2(transform.position.x, transform.position.z);
                var data = $"{Time.time - startTime:F2};{cpuUsage:F2};{gcMemory:F2};{frameRate:F2};{pos.x:F2}:{pos.y:F2};";
                var availableSmells = player.SmellTexReader.AvailableSmells;
                for (int i = 0; i < availableSmells.Count; i++)
                {
                    if (!availableSmells[i].enabled)
                        continue;

                    var smellData = player.GetSmellValueById(i);
                    data += $"{smellData:F2};";
                }

                profilingData.Add(data);

                yield return new WaitForEndOfFrame();

                if (Time.time - startTime >= profilingDuration && player.MovementController.WalkSpeed == 0)
                {
                    StopProfiling();
                    currentSessionIndex++;
                    SetupSession();
                }
            }
        }

        public void StopProfiling()
        {
            if (isProfiling)
            {
                isProfiling = false;
                Debug.Log("Profiling stopped.");
                SaveProfilingData();
            }
        }

        private void SaveProfilingData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"profiling_data_{fileName}_{player.SmellTexReader.texDimension}_{DateTime.Now:hh_mm_ss-dd_mm}.csv");

            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Time (s);CPU Usage (MB);GC Memory (MB);Frame Rate (FPS);pos (Vec2);s0Val (Float);s1Val (Float);s2Val (Float);s3Val (Float);s4Val (Float);s5Val (Float);s6Val (Float);s7Val (Float);s8Val (Float);s9Val (Float)");

            foreach (string line in profilingData)
            {
                csvContent.AppendLine(line);
            }

            File.WriteAllText(filePath, csvContent.ToString());
            Debug.Log($"Profiling data saved to {filePath}");
        }

        public void CancelProfiling()
        {
            if (isProfiling)
            {
                isProfiling = false;
                Debug.Log("Profiling canceled.");
            }
        }

        private void OnWalkComplete()
        {
            if (isProfiling)
                StopProfiling();

            if (multipleProfilingSessions)
            {
                //todo : faire un systeme pour faire le bail automatiquement.
                //todo : rajouter le temps d'une session à la fin ou dans le titre
                currentSessionIndex++;
                SetupSession();
            }
        }
    }
}

