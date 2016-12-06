using UnityEngine;
using System.Collections;
using XBOXParty;

namespace Board
{
    public class Pawn : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1.0f;

        private Node _currentNode;
        public Node CurrentNode
        {
            get { return _currentNode; }
        }

        private VoidDelegate _callback;
        private int _nodesLeft = 0;
        private Vector3 _offset;

        public void SetCurrentNode(Node node)
        {
            _currentNode = node;
            transform.position = _currentNode.transform.position + _offset;
        }

        public void SetOrderInLayer(int order)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = order;
        }

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
        }

        public void SetColor(Color color)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.color = color;
        }

        public void Move(int numNodes, VoidDelegate callback)
        {
            _nodesLeft = numNodes;
            _callback = callback;

            StartMoving();
        }

        private void StartMoving()
        {
            _currentNode = _currentNode.NextNode;

            //If there is no follow up node, we've reached the end!
            if (_currentNode == null)
            {
                Debug.Log(gameObject.name + " is already onb the finish line!");
                return;
            }

            StartCoroutine(MoveRoutine(_currentNode.transform.position + _offset));
        }

        private void MoveComplete()
        {
            _nodesLeft -= 1;

            if (_currentNode.NextNode == null)
            {
                Debug.Log(gameObject.name + " has reached the finish line!");
                _nodesLeft = 0;
            }

            if (_nodesLeft > 0)
            {
                StartMoving();
            }
            else
            {
                if (_callback != null)
                    _callback();
            }
        }

        private IEnumerator MoveRoutine(Vector3 targetPosition)
        {
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            float timer = 0.0f;
            while (timer < 1.0f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, timer);

                timer = Mathf.Clamp01(timer + (Time.deltaTime * _speed));
                yield return new WaitForEndOfFrame();
            }

            MoveComplete();
            yield return null;
        }

        public bool IsOnLastNode()
        {
            return (_currentNode.NextNode == null);
        }
    }
}
