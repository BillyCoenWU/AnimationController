using UnityEngine;

public class LifeManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] m_lifes = null;

	[SerializeField]
	private Character m_character = null;

	private Vector3 m_position = Vector3.zero;

	private Transform m_transform = null;
	public new Transform transform
	{
		get
		{
			if(m_transform == null)
			{
				m_transform = base.transform;
			}

			return m_transform;
		}
	}

	private int m_life = 5;
	public int life
	{
		get
		{
			return m_life;
		}

		set
		{
			m_life = value;
		}
	}

	private void FixedUpdate ()
	{
		m_position = transform.position;
		m_position.x = m_character.transform.position.x;

		transform.position = m_position;
	}

	public void LostLife ()
	{
		m_life--;
		m_lifes[m_life].SetActive(false);
	}

	public void Revive ()
	{
		m_life = 5;

		for(int i = 0; i < m_lifes.Length; i++)
		{
			m_lifes[i].SetActive(true);
		}

		m_character.SetState(new CharacterState.Idle());
	}
}
