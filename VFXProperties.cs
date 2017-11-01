﻿namespace Assets.Scripts.VFX
{
    using UnityEngine;

    public class VFXProperties : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        [HideInInspector]
        public Renderer[] Renderers;
        
        [SerializeField]
        [HideInInspector]
        public ParticleSystem[] ParticleSystems;
    }
}
