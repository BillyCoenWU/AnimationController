namespace RGSMS
{
    namespace Animation
    {
        #region Namespaces
        #if UNITY_EDITOR
        using UnityEditor;
        #endif
        using UnityEngine;
        using UnityEngine.UI;

        using System;
        using System.Collections;
        #endregion

        #region Enums
        
        /// <summary>
        /// Set This Enumeration According To Your Necessity
        /// </summary>
        public enum ANIMATION
        {
            NULL = -1,

            IDLE,
            MOVE,
            JUMP
        }

        #endregion

        [DisallowMultipleComponent]
        public class AnimationController : MonoBehaviour
        {
            /// <summary>
            /// Set This Array According The Names That You Gave To Animations
            /// </summary>
            public static readonly string[] ANIMATIONS_NAMES = new string[]
            {
                "Idle",
                "Move",
                "Jump",
            };

            #region Variables & Properties
            
            [SerializeField]
            private AnimationData m_animationData = null;

            [SerializeField, HideInInspector]
            private SpriteRenderer m_spriteRenderer = null;

            [SerializeField, HideInInspector]
            private Image m_image = null;

            private IEnumerator m_coroutine = null;
            private WaitForSeconds m_waitForSeconds = null;

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
            
            private int m_currentFrame = 0;
            public int currentFrame
            {
                get
                {
                    return m_currentFrame;
                }
            }

            [SerializeField]
            private bool m_playOnStart = false;

            [SerializeField, HideInInspector]
            private bool m_isUIObject = false;

            private bool m_isPlaying = false;
            public bool isPlaying
            {
                get
                {
                    return m_isPlaying;
                }
            }

            private bool m_isDone = false;
            public bool isDone
            {
                get
                {
                    return m_isDone;
                }
            }

            #endregion

            #region Editor Method

            #if UNITY_EDITOR
            [ContextMenu("Sort Sprites By Name")]
            private void DoSort()
            {
                foreach (Data anim in m_animationData.animations)
                {
                    Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
                }
            }

            [ContextMenu("Set Sprite Renderer")]
            private void SetSpriteRenderer()
            {
                m_spriteRenderer = GetComponent<SpriteRenderer>();

                if (m_spriteRenderer == null)
                {
                    if (EditorUtility.DisplayDialog("Component Missing!", "The GameObject do not have the component \"SpriteRenderer\". Do you want to add it?", "Yes", "No"))
                    {
                        m_spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    }
                }

                m_isUIObject = false;
            }

            [ContextMenu("Set Image")]
            private void SetImage()
            {
                m_image = GetComponent<Image>();

                if (m_image == null)
                {
                    if (EditorUtility.DisplayDialog("Component Missing!", "The GameObject do not have the component \"SpriteRenderer\". Do you want to add it?", "Yes", "No"))
                    {
                        m_image = gameObject.AddComponent<Image>();
                    }
                }

                m_isUIObject = true;
            }
            #endif

            #endregion

            #region Unity Methods

            private void Start()
            {
                if (m_playOnStart)
                {
                    Play(0);
                }
            }
            
            #endregion

            #region Get Methods

            public Data GetAnimation(Enum genericType)
            {
                return m_animationData.animationsList.Find(a => a.genericType.Equals(genericType));
            }

            public Data GetAnimation(ANIMATION type)
            {
                return m_animationData.animationsList.Find(a => a.type == type);
            }

            public Data GetAnimation(string name)
            {
                return m_animationData.animationsList.Find(a => (String.Compare(a.name, name, false) == 1));
            }

            public Data GetAnimation(int index)
            {
                return m_animationData.animations[index];
            }

            #endregion

            #region Play Methods

            public void PlayDelayed(Enum genericType, float delay)
            {
                StartCoroutine(PlayDelayed(Play, delay, genericType));
            }

            public void PlayDelayed(ANIMATION type, float delay)
            {
                StartCoroutine(PlayDelayed(Play, delay, type));
            }

            public void PlayDelayed(string name, float delay)
            {
                StartCoroutine(PlayDelayed(Play, delay, name));
            }

            public void PlayDelayed(int index, float delay)
            {
                if(index < 0)
                {
                    return;
                }

                StartCoroutine(PlayDelayed(Play, delay, index));
            }

            public void Play(Enum genericType)
            {
                Play(GetAnimation(genericType));
            }

            public void Play(ANIMATION type)
            {
                Play(GetAnimation(type));
            }

            public void Play(string name)
            {
                Play(GetAnimation(name));
            }

            public void Play(int index)
            {
                if (index < 0)
                {
                    return;
                }

                Play(m_animationData.animations[index]);
            }

            private void Play(Data newAnimation)
            {
                if(newAnimation == null)
                {
                    return;
                }

                if (m_currentAnimation != null)
                {
                    if (!isDone)
                    {
                        if (m_currentAnimation.OnChangeAnimation != null)
                        {
                            m_currentAnimation.OnChangeAnimation();
                        }
                    }

                    if (onChangeAnimation != null)
                    {
                        onChangeAnimation();
                    }
                }

                m_currentAnimation = newAnimation;
                
                m_currentFrame = -1;
                m_isPlaying = true;
                m_isDone = false;

                if (m_currentAnimation.OnStartAnimation != null)
                {
                    m_currentAnimation.OnStartAnimation();
                }

                if(m_coroutine != null)
                {
                    StopCoroutine(m_coroutine);
                }

                m_waitForSeconds = new WaitForSeconds(1.0f / m_currentAnimation.fps);
                m_coroutine = Animation();

                StartCoroutine(m_coroutine);
            }

            #endregion

            #region Animation Methods

            public void Stop()
            {
                m_isDone = true;
                StopCoroutine(Animation());
            }

            public void Pause()
            {
                m_isPlaying = false;
            }

            public void Resume()
            {
                m_isPlaying = true;
            }

            public void ChangeAnimationDuration(Enum genericType, float duration)
            {
                Data d = GetAnimation(genericType);

                d.duration = duration;

                if (d == m_currentAnimation)
                {
                    m_waitForSeconds = new WaitForSeconds(1.0f / m_currentAnimation.fps);
                }
            }
            
            public void ChangeAnimationDuration(ANIMATION type, float duration)
            {
                Data d = GetAnimation(type);

                d.duration = duration;

                if (d == m_currentAnimation)
                {
                    m_waitForSeconds = new WaitForSeconds(1.0f / m_currentAnimation.fps);
                }
            }

            public void ChangeAnimationDuration(string name, float duration)
            {
                Data d = GetAnimation(name);

                d.duration = duration;

                if (d == m_currentAnimation)
                {
                    m_waitForSeconds = new WaitForSeconds(1.0f / m_currentAnimation.fps);
                }
            }

            public void ChangeAnimationDuration(int index, float duration)
            {
                if(index < 0)
                {
                    return;
                }

                Data d = m_animationData.animations[index];

                d.duration = duration;

                if (d == m_currentAnimation)
                {
                    m_waitForSeconds = new WaitForSeconds(1.0f / m_currentAnimation.fps);
                }
            }

            #endregion

            #region Coroutines

            private IEnumerator Animation()
            {
                while (!m_isPlaying)
                {
                    yield return null;
                }

                yield return m_waitForSeconds;

                m_currentFrame++;

                if (m_currentFrame >= m_currentAnimation.frames.Length)
                {
                    if (m_currentAnimation.loop)
                    {
                        if (m_currentAnimation.OnLoopAnimation != null)
                        {
                            m_currentAnimation.OnLoopAnimation();
                        }

                        m_currentFrame = 0;

                        if(m_isUIObject)
                        {
                            m_image.sprite = m_currentAnimation.frames[m_currentFrame];
                        }
                        else
                        {
                            m_spriteRenderer.sprite = m_currentAnimation.frames[m_currentFrame];
                        }

                        m_coroutine = Animation();

                        StartCoroutine(m_coroutine);
                    }
                    else
                    {
                        m_isDone = true;
                        m_isPlaying = false;

                        if (m_currentAnimation.OnCompleteAnimation != null)
                        {
                            m_currentAnimation.OnCompleteAnimation();
                        }
                    }
                }
                else
                {
                    if (m_isUIObject)
                    {
                        m_image.sprite = m_currentAnimation.frames[m_currentFrame];
                    }
                    else
                    {
                        m_spriteRenderer.sprite = m_currentAnimation.frames[m_currentFrame];
                    }
                    
                    m_coroutine = Animation();

                    StartCoroutine(m_coroutine);
                }
            }

            private IEnumerator PlayDelayed(Action<int> action, float time, int index)
            {
                yield return new WaitForSeconds(time);

                action(index);
            }

            private IEnumerator PlayDelayed(Action<ANIMATION> action, float time, ANIMATION type)
            {
                yield return new WaitForSeconds(time);

                action(type);
            }

            private IEnumerator PlayDelayed(Action<Enum> action, float time, Enum genericType)
            {
                yield return new WaitForSeconds(time);

                action(genericType);
            }

            private IEnumerator PlayDelayed(Action<string> action, float time, string name)
            {
                yield return new WaitForSeconds(time);

                action(name);
            }

            #endregion
        }
    }
}
