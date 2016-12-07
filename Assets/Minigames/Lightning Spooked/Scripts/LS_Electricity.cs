using UnityEngine;
using System.Collections;

namespace LightningSpooked
{
    public class LS_Electricity : MonoBehaviour
    {
        [SerializeField]
        private float LS_moveSpeed, LS_maxSpeed;

        [SerializeField]
        private LayerMask LS_LayerMask;

        [SerializeField]
        private LS_PlayerController[] players = new LS_PlayerController[4];

        [SerializeField]
        private LS_MinigameController LS_miniGameController;

        [SerializeField]
        private GameObject bolt;

        [SerializeField]
        private GameObject breakingGlassParticle;

        //Set start rotation.
        private void Start()
        {
            transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }


        //Calculate the bounce and turn the ball into that direction.
        private void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Time.deltaTime * LS_moveSpeed + 0.1f, LS_LayerMask))
            {
                //Show the normal of the plane
                Debug.DrawRay(transform.position, transform.forward);

                //Reflect the electricity
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);

                //Calculate the turn of the electricity
                float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, rot, 0);

                //Add some moveSpeed
                LS_moveSpeed += 0.5f;
            }

            //Move the player and set the movespeed.
            transform.Translate(Vector3.forward * Time.deltaTime * LS_moveSpeed);
            LS_moveSpeed = Mathf.Clamp(LS_moveSpeed, 3, LS_maxSpeed);
        }


        //Make sure to give the players damage.
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "LS_Player1")
            {
                players[0].getSetLS_playerHP -= 1;

                if (players[0].getSetLS_playerHP <= 0)
                {
                    LS_miniGameController.SetPlayerDeathsInListAndSubmitScore(players[0].getSetLS_playerID);
                    Destroy(players[0].gameObject);
                }
            }
            else if (other.gameObject.name == "LS_Player2")
            {
                players[1].getSetLS_playerHP -= 1;

                if (players[1].getSetLS_playerHP <= 0)
                {
                    LS_miniGameController.SetPlayerDeathsInListAndSubmitScore(players[1].getSetLS_playerID);
                    Destroy(players[1].gameObject);
                }
            }
            else if (other.gameObject.name == "LS_Player3")
            {
                players[2].getSetLS_playerHP -= 1;

                if (players[2].getSetLS_playerHP <= 0)
                {
                    LS_miniGameController.SetPlayerDeathsInListAndSubmitScore(players[2].getSetLS_playerID);
                    Destroy(players[2].gameObject);
                }
            }
            else if (other.gameObject.name == "LS_Player4")
            {
                players[3].getSetLS_playerHP -= 1;

                if (players[3].getSetLS_playerHP <= 0)
                {
                    LS_miniGameController.SetPlayerDeathsInListAndSubmitScore(players[3].getSetLS_playerID);
                    Destroy(players[3].gameObject);
                }
            }
        }
    }
}
