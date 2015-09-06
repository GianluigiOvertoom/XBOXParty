using UnityEngine;
using System.Collections;

namespace Board
{
    public class Node : MonoBehaviour
    {
        [SerializeField]
        private Node _nextNode;
        public Node NextNode
        {
            get { return _nextNode; }
        }
    }
}
