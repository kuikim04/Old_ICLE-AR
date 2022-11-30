using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("Function")]
    public class ResultUIAction : ComponentAction<ResultUI>
    {
        [RequiredField]
        [CheckForComponent(typeof(ResultUI))]
        public FsmOwnerDefault gameObject;

        public enum Action
        {
            CheckHighScore, SaveHighScore, DeleteSong
        }
        public Action action;

        private ResultUI script;

        // Code that runs on entering the state.
        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (UpdateCache(go))
                script = cachedComponent;

            if (action == Action.CheckHighScore)
                script.CheckHighScore();
            else if (action == Action.SaveHighScore)
                script.SaveHighScore();
            else if (action == Action.DeleteSong)
                script.DeleteSong();

            Finish();
        }


    }

}
