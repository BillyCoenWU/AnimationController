namespace RGSMS
{
    namespace Animation
    {
        #region Namespaces
        using UnityEditor;
        using UnityEngine;
        using UnityEngine.UI;

        using System;
        using System.Collections;
        #endregion

        #region enums

        public enum COMPONENT
        {
            SPRITE_RENDERER = 0,
            IMAGE
        }

        /// <summary>
        /// Set This Enumeration According To Your Necessity
        /// </summary>
        public enum ANIMATION
        {
            NULL = -1,

            IDLE,
            MOVE,
            JUMP,

            // MUST BE THE FINAL VALUE
            TOTAL
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

            [HideInInspector]
            public Image image = null;

            [HideInInspector]
            public SpriteRenderer spriteRenderer = null;

            [SerializeField]
            private AnimationData m_animationData = null;
            public AnimationData animationData
            {
                get { return m_animationData; }
                set { m_animationData = value; }
            }

            [SerializeField]
            private COMPONENT m_componentType = COMPONENT.SPRITE_RENDERER;
            public COMPONENT componentType
            {
                get
                {
                    return m_componentType;
                }
            }

            private WaitForSeconds m_waitForSeconds;
            private WaitForSecondsRealtime m_waitForUnscaledSecond;
            
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

            private float m_secondsPerFrame = 0.0f;
            private float m_time
            {
                get
                {
                    return m_unscaledAnimation ? Time.unscaledTime : Time.time;
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

            private bool m_unscaledAnimation = false;
            public bool unscaledAnimation
            {
                get { return m_unscaledAnimation; }
                set { m_unscaledAnimation = value; }
            }

            private bool m_playOnStart = false;
            public bool playOnStart
            {
                get { return m_playOnStart; }
                set { m_playOnStart = value; }
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

            #endregion

            #region Editor Method

            private void DoSort()
            {
                foreach (Data anim in m_animationData.animations)
                {
                    Array.Sort(anim.frames, (a, b) => a.name.CompareTo(b.name));
                }
            }

            public void GetSpriteRenderer()
            {
                spriteRenderer = GetComponent<SpriteRenderer>();

                if (spriteRenderer == null)
                {
                    if (EditorUtility.DisplayDialog("Component Missing!", "The GameObject do not have the component \"SpriteRenderer\". Do you want to add it?", "Yes", "No"))
                    {
                        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    }
                }
            }

            public void GetImage()
            {
                image = GetComponent<Image>();

                if (image == null)
                {
                    if (EditorUtility.DisplayDialog("Component Missing!", "The GameObject do not have the component \"Image\". Do you want to add it?", "Yes", "No"))
                    {
                        image = gameObject.AddComponent<Image>();
                    }
                }
            }

            #endregion

            #region Unity Methods

            private void Start()
            {
                if (m_playOnStart)
                {
                    PlayByIndex(0);
                }
            }
            
            #endregion

            #region Gets Methods

            public Data GetAnimationByGenericType(Enum genericType)
            {
                return m_animationData.animationsList.Find(a => a.genericType.Equals(genericType));
            }

            public Data GetAnimationByType(ANIMATION type)
            {
                return m_animationData.animationsList.Find(a => a.type == type);
            }

            public Data GetAnimationByName(string name)
            {
                return m_animationData.animationsList.Find(a => (String.Compare(a.name, name, false) == 1));
            }

            #endregion

            #region Play Methods

            public void PlayByGenericType(Enum genericType)
            {
                PlayByIndex(m_animationData.animationsList.FindIndex(a => a.genericType.Equals(genericType)));
            }

            public void PlayByType(ANIMATION type)
            {
                PlayByIndex(m_animationData.animationsList.FindIndex(a => a.type == type));
            }

            public void PlayByName(string name)
            {
                PlayByIndex(m_animationData.animationsList.FindIndex(a => (String.Compare(a.name, name, false) == 1)));
            }

            public void PlayByIndex(int index)
            {
                if (index < 0)
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

                m_currentAnimation = m_animationData.animations[index];

                m_secondsPerFrame = 1.0f / m_currentAnimation.fps;
                m_currentFrame = -1;
                m_playing = true;
                m_done = false;

                if (m_currentAnimation.OnStartAnimation != null)
                {
                    m_currentAnimation.OnStartAnimation();
                }

                StopCoroutine(Animation());

                if(m_unscaledAnimation)
                {
                    m_waitForUnscaledSecond = new WaitForSecondsRealtime(m_secondsPerFrame);
                }
                else
                {
                    m_waitForSeconds = new WaitForSeconds(m_secondsPerFrame);
                }

                StartCoroutine(Animation());
            }

            public void PlayDelayedByType(Enum genericType, float delay)
            {
                StartCoroutine(PlayDelayed(PlayByGenericType, delay, genericType));
            }

            public void PlayDelayedByType(ANIMATION type, float delay)
            {
                StartCoroutine(PlayDelayed(PlayByType, delay, type));
            }

            public void PlayDelayedByName(string name, float delay)
            {
                StartCoroutine(PlayDelayed(PlayByName, delay, name));
            }

            public void PlayDelayedByIndex(int index, float delay)
            {
                StartCoroutine(PlayDelayed(PlayByIndex, delay, index));
            }

            #endregion

            #region Animation Methods

            public void Stop()
            {
                m_done = true;
                StopCoroutine(Animation());
            }

            public void Pause()
            {
                m_playing = false;
            }

            public void Resume()
            {
                m_playing = true;
            }

            #endregion

            #region Coroutines
            
            private IEnumerator Animation()
            {
                while (!m_playing)
                {
                    yield return null;
                }

                if (m_unscaledAnimation)
                {
                    yield return m_waitForUnscaledSecond;
                }
                else
                {
                    yield return m_waitForSeconds;
                }

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

                        if (m_componentType == COMPONENT.SPRITE_RENDERER)
                        {
                            spriteRenderer.sprite = currentAnimation.frames[m_currentFrame];
                        }
                        else
                        {
                            image.sprite = currentAnimation.frames[m_currentFrame];
                        }

                        StartCoroutine(Animation());
                    }
                    else
                    {
                        m_done = true;
                        m_playing = false;

                        if (m_currentAnimation.OnCompleteAnimation != null)
                        {
                            m_currentAnimation.OnCompleteAnimation();
                        }
                    }
                }
                else
                {
                    if (m_componentType == COMPONENT.SPRITE_RENDERER)
                    {
                        spriteRenderer.sprite = currentAnimation.frames[m_currentFrame];
                    }
                    else
                    {
                        image.sprite = currentAnimation.frames[m_currentFrame];
                    }

                    StartCoroutine(Animation());
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
