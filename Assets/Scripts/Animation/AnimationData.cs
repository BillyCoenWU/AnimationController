namespace RGSMS.Animation
{
    #region Namespaces
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;
    using System;
    #endregion
    
    #region Data

    [Serializable]
	public class Data : ISerializationCallbackReceiver
	{
		public string name = "";

        public ANIMATION type = ANIMATION.NULL;

        public int fps = 1;
        public float duration
        {
            get
            {
                return frames.Length * fps;
            }

            set
            {
                fps = Mathf.FloorToInt(value / frames.Length);
                if (fps <= 0)
                {
                    fps = 1;
                }
            }
        }
        
        public bool loop = false;

        [Tooltip("Use names like: Sprite_001 or Sprite_01")]
		public Sprite[] frames = null;
        
		/// <summary>
		/// Set This Enum By Code According Your Necessity.
		/// </summary>
		public Enum genericType;

        public delegate void AnimationEvent();

        /// <summary>
        /// Event Called When The Animation Start.
        /// </summary>
        public AnimationEvent OnStartAnimation = null;

		/// <summary>
		/// Event Called When The Animation Has Changed Without Complete.
		/// </summary>
		public AnimationEvent OnChangeAnimation = null;

		/// <summary>
		/// Event Called When The Animation Ended and Will Loop.
		/// </summary>
		public AnimationEvent OnLoopAnimation = null;

		/// <summary>
		/// Event Called When The Animation Ended and Will Not Loop.
		/// </summary>
		public AnimationEvent OnCompleteAnimation = null;

        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            if(fps <= 0)
            {
                fps = 1;
            }
        }
    }

	#endregion

	[CreateAssetMenu(fileName = "AnimationData", menuName = "RGSMS/Animation Data", order = -1)]
	public class AnimationData : ScriptableObject
	{
		[SerializeField]
		private Data[] m_animations = null;
		public Data[] animations { get { return m_animations; } }
		public List<Data> animationsList { get { return m_animations.ToList(); } }
	}
}
