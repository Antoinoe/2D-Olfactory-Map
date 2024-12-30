using UnityEditor;
using UnityEngine;

namespace OlfactoryMap
{
    [CustomEditor(typeof(OlfactoryMapProfilerAnalyser))]
    public class CustomProfilerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Afficher les propriétés par défaut de l'inspecteur
            DrawDefaultInspector();

            // Récupérer la référence au script CustomProfiler
            OlfactoryMapProfilerAnalyser profiler = (OlfactoryMapProfilerAnalyser)target;

            // Ajouter un espace pour séparer les boutons des autres propriétés
            GUILayout.Space(10);

            // Bouton pour arrêter le profilage
            if (GUILayout.Button("Start Multiple Profiling"))
            {
                profiler.SetupSession();
            }
            // Bouton pour démarrer le profilage
            if (GUILayout.Button("Start Profiling"))
            {
                profiler.StartProfiling();
            }

            // Bouton pour arrêter le profilage
            if (GUILayout.Button("Stop Profiling"))
            {
                profiler.StopProfiling();
            }
        }
    }
}
