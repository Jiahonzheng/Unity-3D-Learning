using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace Smoke
{
    public class SmokeParticle : MonoBehaviour
    {
        // 车辆句柄。
        private GameObject car;
        private CarController carController;
        // 粒子系统。
        private ParticleSystem exhaust;

        void Start()
        {
            car = transform.parent.gameObject;
            carController = car.GetComponent<CarController>();
            exhaust = GetComponent<ParticleSystem>();
        }

        [System.Obsolete]
        void Update()
        {
            // 设置粒子释放速率。
            SetEmissionRate();
            // 设置粒子颜色。
            SetColor();
        }

        // 根据引擎转速设置粒子释放速率。
        private void SetEmissionRate()
        {
            // 比例系数。
            var K = 5000;
            // 注意：若使用 exhaust.emission.rateOverTime = K * carController.Revs + 60; 会返回语法错误。
            var emission = exhaust.emission;
            emission.rateOverTime = K * carController.Revs;
        }

        // 根据车辆损坏程度设置粒子颜色。
        private void SetColor()
        {
            // 获取粒子颜色句柄。
            var color = exhaust.colorOverLifetime;
            // 获取车辆损坏情况。
            var damage = car.GetComponent<CarCollider>().GetDamage();
            // 根据损坏情况设置颜色深浅，损坏越严重，颜色越深。
            var gradient = new Gradient();
            var colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(214, 189, 151), 0.079f), new GradientColorKey(Color.white, 1.0f) };
            var alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(damage / 255f + 10f / 255f, 0.061f), new GradientAlphaKey(0.0f, 1.0f) };
            gradient.SetKeys(colorKeys, alphaKeys);
            color.color = gradient;
        }
    }
}