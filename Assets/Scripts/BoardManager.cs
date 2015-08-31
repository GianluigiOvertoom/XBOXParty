using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> _logoTextures;
    
    [SerializeField]
    private List<Texture2D> _boardTextures;

    [SerializeField]
    private List<Node> _nodes;

	// Use this for initialization
	private void Awake()
    {
        //Shuffle my lists again with the global seed
        //_logoTextures.Shuffle();
        //_boardTextures.Shuffle();

        //Set the textures to the first one in the list
	}

    public Node GetNode(int id)
    {
        if (id >= _nodes.Count)
            return null;

        return _nodes[id];
    }
}
