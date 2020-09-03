using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour {

	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	public float m_Drift = 2f;

	private void LateUpdate () {
		InitializeIfNeeded ();

		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles (m_Particles);

		if (numParticlesAlive <= 0) {
			Destroy (this.gameObject);
		}

		// Change only the particles that are alive
		for (int i = 0; i < numParticlesAlive; i++) {
			Vector3 nextPos = m_Particles[i].position + (Vector3.down * m_Drift * Time.deltaTime);

			if (ValidTile (World.Instance.Map.WorldToCell (nextPos))) {
				m_Particles[i].position = nextPos;
			}
		}

		// Apply the particle changes to the Particle System
		m_System.SetParticles (m_Particles, numParticlesAlive);
	}

	void InitializeIfNeeded () {
		if (m_System == null)
			m_System = GetComponent<ParticleSystem> ();

		if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
			m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
	}

	private bool ValidTile (Vector3Int tilePosInCells) {
		return World.Instance.Map.GetTile (tilePosInCells) == World.Instance.ClearTile && World.Instance.Colliders.GetTile (tilePosInCells) == null;
	}
}
