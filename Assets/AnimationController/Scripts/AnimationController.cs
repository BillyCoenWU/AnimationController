#region Namespaces

using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

/// <summary>
/// Set This Enumeration According To Your Necessity
/// </summary>
public enum ANIMATION_TYPE
{
	EMPTY = -1,

	IDLE,
	WALK,
	DEATH,
	ATTACK,
	GESTURE
}

[RequireComponent (typeof (SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
	/// <summary>
	/// Set This Array According To Your Necessity
	/// </summary>
	public static readonly string[] ANIMATIONS_NAMES = new string[]
	{
		"Idle",
		"Walk",
		"Death",
		"Attack",
		"Gesture"
	};

	[Serializable]
	public class Data
	{
		public string name = "";

		[ContextMenuItem("Sort Sprites By Name", "DoSort")]
		public Sprite[] frames = null;

		public bool loop = false;

		[Range(1, 60)]
		public int fps = 24;

		public ANIMATION_TYPE type = ANIMATION_TYPE.EMPTY;

		public delegate void OnCompleteAnimation();
		public delegate void OnChangeAnimation();
		public delegate void OnStartAnimation();
		public delegate void OnLoopAnimation();

		public OnCompleteAnimation onCompleteAnimation = null;
		public OnChangeAnimation onChangeAnimation = null;
		public OnStartAnimation onStartAnimation = null;
		public OnLoopAnimation onLoopAnimation = null;

		public UnityEvent completeAnimationEvent = null;
		public UnityEvent changeAnimationEvent = null;
		public UnityEvent startAnimationEvent = null;
		public UnityEvent loopAnimationEvent = null;
	}

	#region Variables & Properties

	public List<Data> animations = new List<Data>();

	public UnityEvent changeAnimationEvent = null;

	public delegate void OnChangeAnimation();
	public OnChangeAnimation onChangeAnimation = null;

	private Data m_currentAnimation = null;
	public Data currentAnimation
	{
		get
		{
			return m_currentAnimation;
		}
	}

	private SpriteRenderer m_spriteRenderer = null;
	public SpriteRenderer spriteRenderer
	{
		get
		{
			if(m_spriteRenderer == null)
			{
				m_spriteRenderer = GetComponent<SpriteRenderer>();
			}

			return m_spriteRenderer;
		}
	}

	private float m_secondsPerFrame = 0.0f;
	private float m_nextFrameTime = 0.0f;

	private int m_currentFrame = 0;
	public int currentFrame
	{
		get
		{
			return m_currentFrame;
		}
	}

	private bool m_playing = false;
	public bool isPlaying
	{
		get
		{
			return m_playing;
		}
	}

	private bool m_done = false;
	public bool isDone
	{
		get
		{
			return m_done;
		}
	}

	public float duration
	{
		get
		{
			return m_currentAnimation.frames.Length * m_currentAnimation.fps;
		}
		
		set
		{
			m_currentAnimation.fps = (int)(value / m_currentAnimation.frames.Length);
		}
	}

	#endregion

	#region Editor Method

	private void DoSort ()
	{
		foreach (AnimationController.Data anim in animations)
		{
			System.Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
		}
	}

	#endregion

	#region Unity Methods

	private void Awake ()
	{
		if(m_spriteRenderer == null)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}

	private void Update ()
	{
		Animation();
	}

	#endregion

	#region Gets Methods

	public Data GetAnimationByType (ANIMATION_TYPE type)
	{
		for(int i = 0; i < animations.Count; i++)
		{
			if(animations[i].type == type)
			{
				return animations[i];
			}
		}

		return null;
	}

	public Data GetAnimationByName (string name)
	{
		for(int i = 0; i < animations.Count; i++)
		{
			if(animations[i].name == name)
			{
				return animations[i];
			}
		}

		return null;
	}

	#endregion
		
	#region Play Methods

	public void PlayByType (ANIMATION_TYPE type)
	{
		PlayByIndex(animations.FindIndex(a => a.type == type));
	}

	public void PlayByName (string name)
	{
		PlayByIndex(animations.FindIndex(a => a.name == name));
	}
	
	public void PlayByIndex (int index)
	{
		if (index < 0)
		{
			return;
		}

		if(m_currentAnimation != null)
		{
			if(m_currentAnimation.onChangeAnimation != null)
			{
				m_currentAnimation.onChangeAnimation();
			}

			if(m_currentAnimation.changeAnimationEvent.GetPersistentEventCount() > 0)
			{
				m_currentAnimation.changeAnimationEvent.Invoke();
			}

			if(onChangeAnimation != null)
			{
				onChangeAnimation();
			}

			if(changeAnimationEvent.GetPersistentEventCount() > 0)
			{
				changeAnimationEvent.Invoke();
			}
		}
		
		m_currentAnimation = animations[index];
		
		m_secondsPerFrame = 1.0f / m_currentAnimation.fps;
		m_nextFrameTime = Time.unscaledTime;
		m_currentFrame = -1;
		m_playing = true;
		m_done = false;

		if(m_currentAnimation.onStartAnimation != null)
		{
			m_currentAnimation.onStartAnimation();
		}

		if(m_currentAnimation.startAnimationEvent.GetPersistentEventCount() > 0)
		{
			m_currentAnimation.startAnimationEvent.Invoke();
		}
	}

	public void PlayByTypeWithDelay (ANIMATION_TYPE type, float delay)
	{
		InvokeDelay(PlayByType, delay, type);
	}

	public void PlayByNameWithDelay (string name, float delay)
	{
		InvokeDelay(PlayByName, delay, name);
	}

	public void PlayByIndexWithDelay (int index, float delay)
	{
		InvokeDelay(PlayByIndex, delay, index);
	}

	#endregion

	#region Animation Methods

	public void Animation ()
	{
		if (!m_playing || Time.unscaledTime < m_nextFrameTime)
		{
			return;
		}

		m_currentFrame++;

		if (m_currentFrame >= m_currentAnimation.frames.Length)
		{
			if (!m_currentAnimation.loop)
			{
				m_done = true;
				m_playing = false;

				if(m_currentAnimation.onCompleteAnimation != null)
				{
					m_currentAnimation.onCompleteAnimation();
				}

				if(m_currentAnimation.completeAnimationEvent.GetPersistentEventCount() > 0)
				{
					m_currentAnimation.completeAnimationEvent.Invoke();
				}

				return;
			}

			if(m_currentAnimation.onLoopAnimation != null)
			{
				m_currentAnimation.onLoopAnimation();
			}

			if(m_currentAnimation.loopAnimationEvent.GetPersistentEventCount() > 0)
			{
				m_currentAnimation.loopAnimationEvent.Invoke();
			}

			m_currentFrame = 0;
		}

		spriteRenderer.sprite = m_currentAnimation.frames[m_currentFrame];
		m_nextFrameTime += m_secondsPerFrame;
	}

	public void Stop ()
	{
		m_playing = false;
	}
	
	public void Resume ()
	{
		m_playing = true;
		m_nextFrameTime = Time.unscaledTime + m_secondsPerFrame;
	}

	#endregion

	#region Coroutines

	private Coroutine InvokeDelay (Action<int> action, float time, int index)
	{
		return StartCoroutine(InvokeImpl(action, time, index));
	}

	private Coroutine InvokeDelay (Action<ANIMATION_TYPE> action, float time, ANIMATION_TYPE type)
	{
		return StartCoroutine(InvokeImpl(action, time, type));
	}

	private Coroutine InvokeDelay (Action<string> action, float time, string name)
	{
		return StartCoroutine(InvokeImpl(action, time, name));
	}

	private IEnumerator InvokeImpl (Action<int> action, float time, int index)
	{
		yield return new WaitForSeconds(time);

		action(index);
	}

	private IEnumerator InvokeImpl (Action<ANIMATION_TYPE> action, float time, ANIMATION_TYPE type)
	{
		yield return new WaitForSeconds(time);

		action(type);
	}

	private IEnumerator InvokeImpl (Action<string> action, float time, string name)
	{
		yield return new WaitForSeconds(time);

		action(name);
	}

	#endregion
}
