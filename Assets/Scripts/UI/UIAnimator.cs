using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Wildflare.Sounds;


namespace Wildflare.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class UIAnimator : MonoBehaviour {
        [SerializeField] private float multiplier;
        [SerializeField] private float time;
        private EventTrigger trigger;

        private StartState startState;

        private void Awake() {
            trigger = GetComponent<EventTrigger>();
            var startTransform = transform;
            startState = new StartState(startTransform.position, startTransform.rotation, startTransform.localScale);
        }

        public void ScaleUp() =>
            transform.DOScale(startState.localScale * multiplier, time).SetEase(Ease.OutCubic);

        public void ScaleDown() => 
            transform.DOScale(startState.localScale, time).SetEase(Ease.InCubic);

        
    }

    struct StartState {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;

        public StartState(Vector3 _position, Quaternion _rotation, Vector3 _localScale) {
            position = _position;
            rotation = _rotation;
            localScale = _localScale;
        }
    }
}