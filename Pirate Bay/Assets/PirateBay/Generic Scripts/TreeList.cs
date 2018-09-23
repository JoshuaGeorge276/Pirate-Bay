using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeList<T> {

    [SerializeField]
    public TreeNode<T> root;

    private int nodeCount;

    // Default constructor
    public TreeList()
    {

    }

    public TreeList(TreeNode<T> root)
    {
        this.root = root;
    }

    public void Insert(TreeNode<T> node)
    {
        // If we do not yet have a root node.
        if(root == null)
        {
            root = node;
            return;
        }

        if(node != null)
        {
            bool isInserted = false;
            Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
            queue.Enqueue(root);
            while(!isInserted)
            {
                TreeNode<T> tmp = queue.Dequeue();
                if (tmp.NeedsChild())
                {
                    tmp.AddChild(node);
                    isInserted = true;
                }
                else
                {
                    for(int i = 0; i < tmp.children.Count; i++)
                    {
                        queue.Enqueue(tmp.children[i]);
                    }
                }
            }
        }
    }
	
}

[System.Serializable]
public class TreeNode<S>
{

    public S node;
    public List<TreeNode<S>> children;

    public bool IsLeafNode { get { return children.Count <= 0; } }

    private int childCount;

    public TreeNode(S parent, int childCount)
    {
        node = parent;
        this.childCount = childCount;
        children = new List<TreeNode<S>>(childCount);
    }

    public TreeNode(S parent, List<TreeNode<S>> children, int childCount)
    {
        node = parent;
        this.children = children;
        this.childCount = childCount;
    }

    public bool NeedsChild()
    {
        return children.Count < childCount;
    }

    public void AddChild(TreeNode<S> node)
    {
        children.Add(node);
    }

    public TreeNode<S> GetChild(int index)
    {
        index = (index >= childCount) ? childCount-1 : index;
        return children[index];
    }

}


