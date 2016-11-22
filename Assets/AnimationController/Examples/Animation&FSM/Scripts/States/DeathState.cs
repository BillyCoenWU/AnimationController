using UnityEngine;

namespace CharacterState
{
	public class Death : ICharacterState
	{
		private Character m_character = null;

		public void EnterState (Character character)
		{
			m_character = character;

			m_character.animationController.PlayByType(ANIMATION_TYPE.DEATH);
		}

		public void FixedUpdate () {}
		public void Update () {}
	}
}
