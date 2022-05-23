using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackToLife
{
    public class ParticleManager : MonoBehaviour
    {
        private static List<Particle> _particles;
        public List<Particle> previewParticles;
        private static List<ParticleOwner> _particleOwners;

        private void Awake()
        {
            _particleOwners = new List<ParticleOwner>();
            _particles = previewParticles;
            print("Particles loaded");
        }

        private void Update()
        {
            foreach (var particleOwner in _particleOwners.Where(particleOwner => !particleOwner.particleSystem.isPlaying).ToList())
            {
                _particleOwners.Remove(particleOwner);
                Destroy(particleOwner.particleSystem.gameObject);
            }
        }

        public static void PlayParticle(Transform transform, Particle.ParticleType type, Vector2 dir)
        {
            dir = dir.normalized;
            var particle = GetParticle(type);
            var rotation = new Vector3(0,90,0)
            {
                x = dir.x switch
                {
                    0 when Math.Abs(dir.y - 1) < 0.00001 => 90,
                    0 when Math.Abs(dir.y - (-1)) < 0.00001 => 270,
                    1 => 180,
                    -1 => 0,
                    _ => 0
                }
            };
            var particleSystem = Instantiate(particle.particleSystem, transform.position, Quaternion.Euler(rotation), transform);
            particleSystem.Play();
            _particleOwners.Add(new ParticleOwner(transform,particleSystem));
            
        }

        public static void StopParticle(Transform transform)
        {
            foreach (var particleOwner in _particleOwners.Where(particleOwner => particleOwner.owner == transform))
            {
                particleOwner.particleSystem.Stop();
            }
        }

        private static Particle GetParticle(Particle.ParticleType type)
        {
            var result = new Particle();
            foreach (var particle in _particles)
            {
                if (particle.type == Particle.ParticleType.Default)
                    result = particle;
                if (particle.type == type)
                    result = particle;
            }

            return result;
        }

        private class ParticleOwner
        {
            public readonly ParticleSystem particleSystem;
            public readonly Transform owner;

            public ParticleOwner(Transform own, ParticleSystem system)
            {
                particleSystem = system;
                owner = own;
            }
        }
        
        [Serializable]
        public class Particle
        {
            public ParticleSystem particleSystem;
            public ParticleType type = ParticleType.Default;
            public enum ParticleType
            {
                Default,
                Ice,
                Magic
            }
        }
    }
}