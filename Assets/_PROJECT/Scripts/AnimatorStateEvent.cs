using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorStateEvent : MonoBehaviour
{

    Animator anim;
    [System.Serializable]
    public class MyEvent
    {
        public string state;
        private int hash;
        private int prev_hash;
        public UnityEvent OnEnter;

        public void Init()
        {
            hash = Animator.StringToHash(state);
        }

        public void Update(AnimatorStateInfo state)
        {
            var current = state.shortNameHash;

            if (current == hash && current != prev_hash)
                OnEnter.Invoke();

            prev_hash = current;
        }
    }

    public List<MyEvent> events;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        foreach (var item in events)
            item.Init();
    }

    // Update is called once per frame
    void Update()
    {
        var state = anim.GetNextAnimatorStateInfo(0);
        var is_transition = anim.IsInTransition(0);
        if (is_transition)
        {
            for (int i = 0; i < events.Count; i++)
            {
                events[i].Update(state);
            }
        }
    }

    public void Test()
    {
        Debug.Log(Time.time.ToString("N2") + " On Enter");
    }
}
