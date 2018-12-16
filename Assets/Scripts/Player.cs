using UnityEngine;

namespace Assets.Scripts
{
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Rigidbody))]
	public sealed class Player : MonoBehaviour
	{
		#region Inspector

		[SerializeField]
		private float m_ThrottleSpeed;

		[SerializeField]
		private float m_Acceleration;

		[SerializeField]
		private float m_Deceleration;

		[SerializeField]
		private float m_MaxSpeed;

		[SerializeField]
		private float m_MaxReverseSpeed;

		#endregion

		private float m_Throttle;
		private float m_CurrentlAcceleration;
		private Rigidbody m_CachedRigidbody;

		private Rigidbody Rigidbody { get { return m_CachedRigidbody ?? (m_CachedRigidbody = GetComponent<Rigidbody>()); } }

		/// <summary>
		/// Update is called once per frame.
		/// </summary>
		private void Update()
		{
			// Update throttle position
			m_Throttle += Input.GetAxis("Vertical") * m_ThrottleSpeed * Time.deltaTime;
			m_Throttle = Mathf.Clamp(m_Throttle, -1.0f, 1.0f);

			// Calculate current acceleration
			m_CurrentlAcceleration = m_Throttle > 0 ? m_Acceleration : m_Deceleration;
			m_CurrentlAcceleration *= Mathf.Abs(m_Throttle);

			// Update velocity
			Vector3 velocity = Rigidbody.velocity;
			float forwardVelocity = velocity.z;
			forwardVelocity += m_CurrentlAcceleration * Time.deltaTime;
			forwardVelocity = Mathf.Clamp(forwardVelocity, m_MaxReverseSpeed, m_MaxSpeed);

			Rigidbody.velocity =
				new Vector3(velocity.x,
				            velocity.y,
				            forwardVelocity);
		}

		private void OnGUI()
		{
			GUILayout.Label(string.Format("Throttle: {0}", m_Throttle));
			GUILayout.Label(string.Format("Acceleration: {0}", m_CurrentlAcceleration));
			GUILayout.Label(string.Format("Speed: {0}", Rigidbody.velocity.z));
		}
	}
}
