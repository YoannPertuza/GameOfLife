using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class BinaryTree
    {
    }

    public interface INode
    {
        INode Node(int valueToFind);
    }

    public class NullNode : INode
    {
        public INode Node(int valueToFind)
        {
            throw new Exception(string.Format("Value {0} is not present in the tree", valueToFind));
        }
    }

    public class TwoBranchNode : INode
    {
        public TwoBranchNode(int value, INode leftNode, INode rightNode)
        {
            this.value = value;
            this.leftNode = leftNode;
            this.rightNode = rightNode;
        }

        private int value;
        private INode leftNode;
        private INode rightNode;

        public INode Node(int valueToFind)
        {
            if (valueToFind == this.value)
            {
                return this;
            }
            else if (valueToFind < value)
            {
                return leftNode.Node(valueToFind);
            }
            else
            {
                return rightNode.Node(valueToFind);
            }
        }
    }
}
