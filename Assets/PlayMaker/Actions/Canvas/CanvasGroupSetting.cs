using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("Canvas")]
    public class CanvasGroupSetting : ComponentAction<CanvasGroup>
    {
        [RequiredField]
        [CheckForComponent(typeof(CanvasGroup))]
        public FsmOwnerDefault gameObject;

        public float Alpha;
        public bool Interactable;
        public bool BlocksRaycasts;
        public bool IgnoreParentGroups;

        private CanvasGroup script;
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (UpdateCache(go))
                script = cachedComponent;

            script.alpha = Alpha;
            script.interactable = Interactable;
            script.blocksRaycasts = BlocksRaycasts;
            script.ignoreParentGroups = IgnoreParentGroups;

            Finish();
        }
    }
}
