using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace OlfactoryMap
{
    public class FakeMovements : MonoBehaviour
    {
        public float WalkSpeed = 1;
        public float walkSpeedMultiplier = 1;
        public UnityEvent OnWalkComplete;
        [SerializeField] private Transform startPosition;
        [SerializeField] private List<Transform> checkpoints = new List<Transform>();
        [SerializeField] private float yPosition = 0;

        Rigidbody rb;

        private Sequence seq;

        private void Awake()
        {
            //player = GetComponent<FakeSmellPlayer>();
            //if (!player)
            //    Debug.LogError("Could not find PlayerSmellData");
            rb = GetComponent<Rigidbody>();
            
        }

        public void StartMovements()
        {
            ResetPosition();
            seq = CreateSequence();
            seq?.Play();
        }

        public void PauseMovements()
        {
            seq.Pause();
            rb.velocity = Vector3.zero;
        }

        public void Restart()
        {
            ResetPosition();
            seq.Restart();
        }

        public void ResetPosition()
        {
            rb.velocity = Vector3.zero;
            transform.position = checkpoints[0].position;
        }

        private Sequence CreateSequence()
        {
            //rb.useGravity = true;
            var speed = WalkSpeed * walkSpeedMultiplier;
            if(speed == 0)
                return null;
            var seq = DOTween.Sequence();
            seq.Append(rb.DOMove(checkpoints[0].position,0));
            for (int i = 1; i < checkpoints.Count; i++)
            {
                Debug.LogError($"Going to {checkpoints[i].position}");
                seq.Append(rb.DOMove(checkpoints[i].position, GetDuration(checkpoints[i - 1].position, checkpoints[i].position,speed)).SetEase(Ease.Linear));
            }

            seq.OnComplete(() =>{
                rb.velocity = Vector3.zero;
                OnWalkComplete?.Invoke(); 
            });

            return seq;
        }

        private float GetDuration(Vector3 start, Vector3 end, float walkSpeed)
        {
            if (walkSpeed == 0)
                return 999;
            return Vector3.Distance(start, end) / walkSpeed;
        }
    }
}

