using System;

using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

namespace LeonLaci.UI
{
    public class AnimatedPanel : MonoBehaviour
    {
        public bool Locked;

        public AnimationMode Mode;

        public Vector2 StartPosition;
        public Vector2 EndPosition;

        public Offset StartOffset;
        public Offset EndOffset;

        public OnStartMoveToPosition OnStartMoveTo;

        public Ease StartEase = Ease.InQuad;
        public Ease EndEase = Ease.OutQuad;

        public float TransitionDuration = 1;

        public bool HasEnded { get; protected set; }

        [Space (20)]

        public UnityEvent OnStart;
        public UnityEvent OnEnd;

        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform> ();

                return _rectTransform;
            }
        }

        private void Start ()
        {
            if (OnStartMoveTo == OnStartMoveToPosition.End)
                GoToEnd ();
            else
                GoToStart ();
        }

        /// <summary>
        /// Do not use this. Only for internal use.
        /// </summary>
        public void INTERNAL_JumpToStart ()
        {
            switch (Mode)
            {
                case AnimationMode.AnchorPosition:
                    RectTransform.anchoredPosition = StartPosition;
                    break;

                case AnimationMode.OffsetAnimation:
                    StartOffset.ApplyToRectTransform (RectTransform);
                    break;
            }
        }

        /// <summary>
        /// Do not use this. Only for internal use.
        /// </summary>
        public void INTERNAL_JumpToEnd ()
        {
            switch (Mode)
            {
                case AnimationMode.AnchorPosition:
                    RectTransform.anchoredPosition = EndPosition;
                    break;
                case AnimationMode.OffsetAnimation:
                    EndOffset.ApplyToRectTransform (RectTransform);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Forcefully animates to start.
        /// </summary>
        public void ForceStart ()
        {
            GoToStart ();
        }

        /// <summary>
        /// Forcefully animates to end.
        /// </summary>
        public void ForceEnd ()
        {
            GoToEnd ();
        }

        /// <summary>
        /// Animates to start.
        /// </summary>
        private void GoToStart ()
        {
            if (Locked)
                return;

            HasEnded = false;

            switch (Mode)
            {
                case AnimationMode.AnchorPosition:
                    RectTransform.DOAnchorPos (StartPosition, TransitionDuration).SetEase (StartEase);
                    break;

                case AnimationMode.OffsetAnimation:
                    DOTween.To (() => RectTransform.offsetMax, x => RectTransform.offsetMax = x, StartOffset.Max, TransitionDuration);
                    DOTween.To (() => RectTransform.offsetMin, x => RectTransform.offsetMin = x, StartOffset.Min, TransitionDuration);
                    break;
            }

            OnStart?.Invoke ();
        }

        /// <summary>
        /// Animates to end.
        /// </summary>
        private void GoToEnd ()
        {
            if (Locked)
                return;

            HasEnded = true;

            switch (Mode)
            {
                case AnimationMode.AnchorPosition:
                    RectTransform.DOAnchorPos (EndPosition, TransitionDuration).SetEase (EndEase);
                    break;

                case AnimationMode.OffsetAnimation:
                    DOTween.To (() => RectTransform.offsetMax, x => RectTransform.offsetMax = x, EndOffset.Max, TransitionDuration);
                    DOTween.To (() => RectTransform.offsetMin, x => RectTransform.offsetMin = x, EndOffset.Min, TransitionDuration);
                    break;
            }

            OnEnd?.Invoke ();
        }

        /// <summary>
        /// Toggles the animation.
        /// </summary>
        public void Toggle ()
        {
            if (Locked)
                return;

            if (HasEnded)
                GoToStart ();
            else
                GoToEnd ();
        }

        public enum AnimationMode
        {
            AnchorPosition,
            OffsetAnimation
        }

        public enum OnStartMoveToPosition
        {
            Start,
            End
        }

        [Serializable]
        public struct Offset
        {
            public float Top, Bottom, Left, Right;

            public Vector2 Max => new Vector2 (Right, Top);
            public Vector2 Min => new Vector2 (Left, Bottom);

            public Offset (float top, float bottom, float left, float right)
            {
                Top = top;
                Bottom = bottom;
                Left = left;
                Right = right;
            }

            public Offset (RectTransform rectTransform)
            {
                Top = rectTransform.offsetMax.y;
                Bottom = rectTransform.offsetMin.y;
                Right = rectTransform.offsetMax.x;
                Left = rectTransform.offsetMin.x;
            }

            public void ApplyToRectTransform (RectTransform rectTransform)
            {
                rectTransform.offsetMax = new Vector2 (Right, Top);
                rectTransform.offsetMin = new Vector2 (Left, Bottom);
            }
        }
    }
}
