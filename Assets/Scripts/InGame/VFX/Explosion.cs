using UnityEditor;
using UnityEngine;

namespace InGame.VFX
{
    public class Explosion: ParticleController
    {
        public void SetExplosionMultiplier(float multiplier = 1)
        {
            var shape = particleSystem.shape;
            shape.radius = shape.radius * multiplier;

            var emission = particleSystem.emission;
            var bursts = new ParticleSystem.Burst[emission.burstCount];
            emission.GetBursts(bursts);

            if (bursts.Length >= 1)
            {
                bursts[0].count = new ParticleSystem.MinMaxCurve(bursts[0].count.constant * (multiplier * multiplier));
                
            }
            emission.SetBursts(bursts);
        }
    }
}