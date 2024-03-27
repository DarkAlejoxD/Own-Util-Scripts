using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class Branch
    {
        [SerializeReference] public List<DialogueNode> DialogueNodes;
        private DialogueNode _currentNode;
        private DialogueNode _nextNode;
        private int index = 0;
        public bool FoldOut;
        public bool BranchEnded { get; private set; }
        public bool EndNodeFound { get; private set; }
        public bool AlreadyEvaluating { get; private set; }

        public Branch()
        {
            DialogueNodes ??= new List<DialogueNode>();
            AlreadyEvaluating = false;
        }

        public void ResetBranch()
        {
            index = 0;
            BranchEnded = false;
            EndNodeFound = false;
            AlreadyEvaluating = false;

            _currentNode = DialogueNodes.First();
            _nextNode = _currentNode;

            foreach (var item in DialogueNodes)
            {
                if (item is ISubBranchDialogueNode subBranch)
                    subBranch.Reset();
            }

            SetNextNode();
        }

        public DialogueNode GetNextNode()
        {
            DialogueNode sendedDialogueNode = _currentNode;

            #region Old
            /*
            if (!AlreadyEvaluating)
            {
                AlreadyEvaluating = true;

                if (_currentNode is ISubBranchDialogueNode subBranch)
                {
                    var node = subBranch.GetNextNode();
                    if (node == null)
                    {
                        if (subBranch.FoundEndNode && !subBranch.IgnoreEndNode)
                        {
                            SetNextNode();
                            return null;
                        }
                        else if (subBranch.BranchEnded)
                        {
                            SetNextNode();
                            return GetNextNode();
                        }
                    }
                    else
                    {
                        sendedDialogueNode = node;
                    }
                }
                else if (_currentNode is EndNode)
                {
                    EndNodeFound = true;
                    sendedDialogueNode = _currentNode;
                }
            }
            else
            {
                if (_currentNode is ISubBranchDialogueNode subBranch)
                {
                    var node = subBranch.GetNextNode();
                    if (node == null)
                    {
                        if (subBranch.FoundEndNode && !subBranch.IgnoreEndNode)
                        {
                            SetNextNode();
                            return null;
                        }
                        else if (subBranch.BranchEnded)
                        {
                            SetNextNode();
                            return GetNextNode();
                        }
                    }
                    else
                    {
                        sendedDialogueNode = node;
                    }
                }
                else if (_currentNode is EndNode)
                {
                    EndNodeFound = true;
                    sendedDialogueNode = _currentNode;
                }
            }
            */
            #endregion Technically works, but i think it can be simplified

            #region New

            if (_currentNode is ISubBranchDialogueNode subBranch)
            {
                var node = subBranch.GetNextNode();
                if (node == null)
                {
                    if (subBranch.FoundEndNode && !subBranch.IgnoreEndNode)
                    {
                        return null;
                    }
                    else if (subBranch.BranchEnded)
                    {
                        if (_nextNode != null)
                        {
                            SetNextNode();
                            return GetNextNode();
                        }
                    }
                    else
                    {
                        Debug.Log(String.Format("Unkwon Case\n BranchEndedState: {0}\n BranchFoundNodeState: {1}\n BranchIgnoreNode: {2}",
                                                subBranch.BranchEnded, subBranch.FoundEndNode, subBranch.IgnoreEndNode));
                    }
                }
                else
                {
                    sendedDialogueNode = node;
                }
            }
            else if (_currentNode is EndNode)
            {
                EndNodeFound = true;
                sendedDialogueNode = _currentNode;
            }

            #endregion //Simplified, not tested

            //if (sendedDialogueNode == null)
            //    return GetNextNode();
            SetNextNode();
            return sendedDialogueNode;

        }

        private void SetNextNode()
        {
            if (_currentNode.CanPassNextNode())
            {
                if (_nextNode == null)
                {
                    BranchEnded = true;
                    return;
                }
                index = DialogueNodes.IndexOf(_nextNode);
                index++;
                DialogueNode next = _nextNode;
                //Debug.Log(String.Format("Current Node: {0} \nNext Node: {1}", _currentNode.Text, _nextNode.Text));
                if (DialogueNodes.Count > 0 && index <= DialogueNodes.Count)
                {
                    _currentNode = next;
                    if (index < DialogueNodes.Count)
                        _nextNode = DialogueNodes.ElementAt(index);
                    else
                        _nextNode = null;
                }
                //else if (index > DialogueNodes.Count)
                //{

                //    _currentNode = null;
                //}
            }
            else if (_currentNode is EndNode)
            {
                _nextNode = null;
                EndNodeFound = true;
                BranchEnded = EndNodeFound;
            }
            else if (_currentNode is ISubBranchDialogueNode subBranch)
            {
                if (subBranch.FoundEndNode && !subBranch.IgnoreEndNode)
                {
                    _nextNode = null;
                    EndNodeFound = true;
                    BranchEnded = EndNodeFound;
                }
                else if (subBranch.BranchEnded)
                {
                    _nextNode = null;
                    BranchEnded = true;
                }
            }
        }

        public void CreateNewDialogueNode(DialogueNode node, int index)
        {
            DialogueNodes ??= new List<DialogueNode>();

            if (index < 0 || index > DialogueNodes.Count - 1)
                DialogueNodes.Add(node);

            else
                DialogueNodes.Insert(index, node);

            PrintTable(DialogueNodes);
        }

        public bool CanPassNextNode()
        {
            if (EndNodeFound)
                return false;

            return BranchEnded;
        }

        public void PrintTable(List<DialogueNode> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string text = "Item of type {0} and {1}";

                DialogueNode node = list.ElementAt(i);

                if (node is EndNode node1)
                {
                    text = string.Format(text, node.GetType(), node1.GetType());
                }
                else
                {
                    text = string.Format(text, node.GetType(), "_");
                }

                Debug.Log(text);
            }
        }
    }
}