using Assets.Code.Loot;
using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class LootCollector : MonoBehaviour
    {
        private const float CollectDistance = 1f;
        private const float ExperienceTransferDelay = 0.15f;

        private readonly List<Loot> _toCollect = new();

        private float _time;
        private float _pullSpeed;
        private SphereCollider _collectArea;
        private float _attractionRadius;
        private Wallet _wallet;
        private HeroLevel _heroLevel;
        private bool _isRunning;

        public float CollectedGold { get; private set; }

        private void Awake()
        {
            _collectArea = GetComponent<SphereCollider>();
            _collectArea.isTrigger = true;
        }

        private void FixedUpdate()
        {
            if (_isRunning == false)
            {
                return;
            }

            for (int i = _toCollect.LastIndex(); i >= Constants.Zero; i--)
            {
                Loot loot = _toCollect[i];

                Vector3 distance = transform.position - loot.transform.position;
                Vector3 rawDirection = distance / (distance.magnitude + Constants.One);

                loot.Rigidbody.velocity = rawDirection * _pullSpeed;
                if (distance.sqrMagnitude <= CollectDistance)
                {
                    int collectValue = loot.Collect();
                    CollectValue(loot.Type, collectValue);
                    _toCollect.Remove(loot);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Loot loot) == false || _toCollect.Contains(loot))
            {
                return;
            }

            _toCollect.Add(loot);
        }

        private void OnDisable()
        {
            foreach (Loot loot in _toCollect)
            {
                if (loot.NotNull())
                {
                    loot.Rigidbody.velocity = Vector3.zero;
                }
            }

            _toCollect.Clear();
        }

        public void Initialize(float attractionRadius, float pullSpeed, Wallet wallet, HeroLevel heroLevel)
        {
            _pullSpeed = pullSpeed.ThrowIfZeroOrLess();
            _attractionRadius = attractionRadius.ThrowIfNegative();
            _wallet = wallet.ThrowIfNull();
            _heroLevel = heroLevel.ThrowIfNull();

            _collectArea.radius = _attractionRadius;
        }

        public void Run()
        {
            _isRunning = true;
            UpdateService.RegisterUpdate(TransferExperience);
        }

        public void Stop()
        {
            _isRunning = false;
            UpdateService.UnregisterUpdate(TransferExperience);
        }

        public void TransferGold()
        {
            if (CollectedGold > Constants.Zero)
            {
                _wallet.Add((int)CollectedGold);
            }
        }

        public void AddAttractionRadius(int value)
        {
            _collectArea.radius = _attractionRadius + value.ThrowIfNegative();
        }

        private void TransferExperience()
        {
            if (_heroLevel.IsNull())
            {
                return;
            }

            _time += Time.deltaTime;

            if (_time > ExperienceTransferDelay)
            {
                _heroLevel.Transfer();
                _time = 0;
            }
        }

        private void CollectValue(LootType type, int collectValue)
        {
            switch (type)
            {
                case LootType.LowExperience:
                    _heroLevel.Add(collectValue);
                    break;

                case LootType.MediumExperience:
                    _heroLevel.Add(collectValue);
                    break;

                case LootType.HighExperience:
                    _heroLevel.Add(collectValue);
                    break;

                case LootType.Coin:
                    CollectedGold += collectValue;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
