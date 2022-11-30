using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory(ActionCategory.AnimateVariables)]
    public class AnimateCanvasGroup : EaseFsmAction
    {
        [RequiredField]
        [CheckForComponent(typeof(CanvasGroup))]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        public FsmFloat fromValue;
        [RequiredField]
        public FsmFloat toValue;

        private bool finishInNextStep = false;

        private CanvasGroup script;

        public override void Reset()
        {
            base.Reset();
            fromValue = null;
            toValue = null;
            finishInNextStep = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            fromFloats = new float[1];
            fromFloats[0] = fromValue.Value;
            toFloats = new float[1];
            toFloats[0] = toValue.Value;
            resultFloats = new float[1];
            finishInNextStep = false;

            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            script = go.GetComponent<CanvasGroup>();
            script.alpha = fromValue.Value;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            script.alpha = resultFloats[0];

            if (finishInNextStep)
            {
                Finish();
                if (finishEvent != null) Fsm.Event(finishEvent);
            }

            if (finishAction && !finishInNextStep)
            {
                script.alpha = reverse.IsNone ? toValue.Value : reverse.Value ? fromValue.Value : toValue.Value;
                finishInNextStep = true;
            }
        }
    }

}
