using UnityEditor;
using UnityEngine;

namespace OlfactoryMap
{

    [CustomEditor(typeof(FakeMovements))]
    public class FakeMovementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Afficher les propriétés par défaut de l'inspecteur
            DrawDefaultInspector();

            // Récupérer la référence au script CustomProfiler
            FakeMovements movementController = (FakeMovements)target;

            // Ajouter un espace pour séparer les boutons des autres propriétés
            GUILayout.Space(10);

            // Bouton pour démarrer le profilage
            if (GUILayout.Button("Start Movements"))
            {
                movementController.StartMovements();
            }

            // Bouton pour arrêter le profilage
            if (GUILayout.Button("Pause Movements"))
            {
                movementController.PauseMovements();
            }

            if (GUILayout.Button("Restart"))
            {
                movementController.Restart();
            }
        }
    }

}
