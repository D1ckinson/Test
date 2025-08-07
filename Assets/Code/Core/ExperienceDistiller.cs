using Assets.Code.Tools;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    public class ExperienceDistiller : MonoBehaviour
    {
        private const float Cooldown = 0.15f;

        private HeroLevel _heroExperience;
        private float _time;

        public void Initialize(HeroLevel heroExperience)
        {
            _heroExperience = heroExperience.ThrowIfNull();
        }

        private void Update()
        {
            if (_heroExperience.IsNull())
            {
                return;
            }

            _time += Time.deltaTime;

            if (_time > Cooldown)
            {
                _heroExperience.Transfer();
                _time = 0;
            }
        }
    }
}
