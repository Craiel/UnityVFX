namespace Assets.Scripts.VFX
{
    using Craiel.GDX.AI.Sharp.Contracts;
    using DG.Tweening;
    using UnityEngine;

    public class VFXController : MonoBehaviour, IPoolable
    {
        private bool isFading;
        private float fadeTimeRemaining;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public bool FadeParticleSystems;

        public VFXProperties Properties { get; private set; }

        public bool IsActive { get; private set; }

        public void Awake()
        {
            this.Properties = this.GetComponent<VFXProperties>();

            this.gameObject.SetActive(false);
        }
        
        public void Reset()
        {
            this.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (this.isFading)
            {
                this.fadeTimeRemaining -= Time.deltaTime;
                if (this.fadeTimeRemaining <= 0)
                {
                    this.IsActive = false;
                }
            }
        }

        public void Begin()
        {
            this.isFading = false;
            this.IsActive = true;
            this.gameObject.SetActive(true);

            foreach (ParticleSystem system in this.Properties.ParticleSystems)
            {
                system.Play();
            }
        }

        public void End()
        {
            this.IsActive = false;
        }

        public void BeginFade(float time)
        {
            this.isFading = true;
            this.fadeTimeRemaining = time;

            if (this.FadeParticleSystems)
            {
                foreach (ParticleSystem system in this.Properties.ParticleSystems)
                {
                    system.Stop(true);
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        public void LerpTo(Vector3 targetPosition, float time)
        {
            this.transform.DOMove(targetPosition, time);
        }
    }
}
