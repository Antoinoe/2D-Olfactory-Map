using System.Collections;
using UnityEngine;

namespace OlfactoryMap
{
    public class FakeSmellPlayer : MonoBehaviour
    {
        public SmellTexReader SmellTexReader;
        public FakeMovements MovementController;

        [SerializeField] private float delayBetweenRequests = 1f;

        private bool canRequestSmellValues = false;

        private void Update()
        {
            if (canRequestSmellValues)
                StartCoroutine(GetSmellValueRequest());
        }

        private IEnumerator GetSmellValueRequest()
        {
            canRequestSmellValues = false;
            Debug.Log("Get Smells");
            GetSmell(SmellType.Forest);

            yield return new WaitForSeconds(delayBetweenRequests);
            canRequestSmellValues = true;
        }



        private float GetSmell(SmellType smell)
        {
            var value = SmellTexReader.GetSmellValue(smell, new Vector2(transform.position.x, transform.position.z));
            Debug.LogError($"Smell for {smell} is {value}");
            return value;
        }

        public float GetSmellValueById(int id)
        {
            Debug.LogError($"getting value for id : {id}");
            return SmellTexReader.GetSmellValueById(id, new Vector2(transform.position.x, transform.position.z));
        }


    }
}

