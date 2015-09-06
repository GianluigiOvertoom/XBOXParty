using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private Image _logoImage;

        [SerializeField]
        private SpriteRenderer _boardImage;

        [SerializeField]
        private List<Sprite> _logoSprites;

        [SerializeField]
        private List<Sprite> _boardSprites;

        [SerializeField]
        private List<Node> _nodes;

        // Use this for initialization
        private void Awake()
        {
            int rand = Random.Range(0, _logoSprites.Count);
            _logoImage.sprite = _logoSprites[rand];

            rand = Random.Range(0, _boardSprites.Count);
            _boardImage.sprite = _boardSprites[rand];
        }

        public Node GetNode(int id)
        {
            if (id >= _nodes.Count)
                return null;

            return _nodes[id];
        }

        public int GetNodeId(Node node)
        {
            for (int i = 0; i < _nodes.Count; ++i)
            {
                if (_nodes[i] == node)
                    return i;
            }

            return -1;
        }
    }
}
