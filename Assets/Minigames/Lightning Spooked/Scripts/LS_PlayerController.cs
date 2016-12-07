using UnityEngine;
using System.Collections;
using XBOXParty;

namespace LightningSpooked
{
    public class LS_PlayerController : MonoBehaviour
    {
        //Public enums
        #region enums
        public enum LS_PlayerState
        {
            idle = 0,
            move = 1
        };
        #endregion

        //private class variables
        #region class variables
        //Class variables.
        private float LS_moveSpeed = 3;
        private float LS_rotation = 0.01f;
        private int LS_controllerIndex;

        [SerializeField]
        private int LS_playerHP;

        private LS_PlayerState LS_playerState;
        private Animator LS_animController;
        private LS_MinigameController LS_miniGameController;
        private Material LS_defaultMat;
        private Rigidbody rb;
        #endregion

        //public gets and sets
        #region publicGetSets
        public int getSetLS_playerID
        {
            get {return LS_controllerIndex; }
            set { LS_controllerIndex = value; }
        }
        public int getSetLS_playerHP
        {
            get { return LS_playerHP; }
            set { LS_playerHP = value; }
        }
        #endregion


        //Gets called when game is started after the awake.
        private void Start()
        {

            //Get the miniGameController.
            LS_miniGameController = FindObjectOfType<LS_MinigameController>();

            //Set the animator
            LS_animController = GetComponent<Animator>();

            rb = GetComponent<Rigidbody>();

            //Set player input
            InputManager.Instance.BindAxis("LS_HorizontalAxis"+ LS_controllerIndex, LS_controllerIndex, ControllerAxisCode.LeftStickX);
            InputManager.Instance.BindAxis("LS_VerticalAxis" + LS_controllerIndex, LS_controllerIndex, ControllerAxisCode.LeftStickY);
            InputManager.Instance.BindAxis("LS_HorizontalMirAxis" + LS_controllerIndex, LS_controllerIndex, ControllerAxisCode.RightStickX);
            InputManager.Instance.BindAxis("LS_VerticalMirAxis" + LS_controllerIndex, LS_controllerIndex, ControllerAxisCode.RightStickY);


            //Get the default material and set it to the playerColor of the game.
            LS_defaultMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
            LS_defaultMat.color = GlobalGameManager.Instance.GetPlayerColor(LS_controllerIndex);
             

            //Set a default state.
            LS_playerState = LS_PlayerState.idle;
        }


        //Gets called every frame.
        private void Update()
        {
            //Player configuration.
            float LS_horMove = InputManager.Instance.GetAxis("LS_HorizontalAxis" + LS_controllerIndex);
            float LS_verMove = InputManager.Instance.GetAxis("LS_VerticalAxis" + LS_controllerIndex);
            float LS_horMirMove = InputManager.Instance.GetAxis("LS_HorizontalMirAxis" + LS_controllerIndex);
            float LS_verMirMove = InputManager.Instance.GetAxis("LS_VerticalMirAxis" + LS_controllerIndex);

            //Make the player move according to the left thumbstick.
            MovePlayer(LS_horMove, LS_verMove);

            
            //Update the player animations.
            SetPlayerAnimations();


            //Change player rotation.
            float currentAngle = transform.rotation.eulerAngles.y;
            float desired = Mathf.Atan2(LS_verMirMove, LS_horMirMove) * Mathf.Rad2Deg;

            if (LS_horMirMove == 0.0f && LS_verMirMove == 0.0f && desired == 0.0f) return;

            float tweenAngle = Mathf.LerpAngle(currentAngle, desired, LS_rotation * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, -desired - 90, 0));
        }


        //Update the player animations.
        private void SetPlayerAnimations()
        {
            LS_animController.SetFloat("PlayerState", (float)LS_playerState);
        }


        //Move the player according to the left thumbstick.
        private void MovePlayer(float LS_horMove, float LS_verMove)
        {

            if ((LS_horMove == 0) && (LS_verMove == 0))
            {
                LS_playerState = LS_PlayerState.idle;
            }
            else
            {
                LS_playerState = LS_PlayerState.move;

                transform.position = new Vector3(transform.position.x + (LS_horMove * LS_moveSpeed * Time.deltaTime),
                transform.position.y, transform.position.z + (LS_verMove * LS_moveSpeed * Time.deltaTime));
            }
        }
    }
}
