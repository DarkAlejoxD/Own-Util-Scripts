using System;

namespace DialogueSystem
{
    public class DialogueDecoder
    {
        private readonly DialogueObject _dialogue;
        private readonly Branch _branch;
        private readonly string _playerName;
        private DialogueNode _currentNode;
        private bool _delayedEnded;
        private Action _onEndDialogue;

        public DialogueDecoder(DialogueObject dialogueObject)
        {
            _dialogue = dialogueObject;
            _branch = _dialogue.GetBranch();
            _branch.ResetBranch();
            _currentNode = _branch.GetNextNode();
            _playerName = "Player";
        }

        public DialogueDecoder(DialogueObject dialogueObject, string playerName)
        {
            _dialogue = dialogueObject;
            _branch = _dialogue.GetBranch();
            _branch.ResetBranch();
            _currentNode = _branch.GetNextNode();
            _playerName = playerName;
        }

        /// <summary> Gets the Next Text </summary>
        /// <param name="text"> the string text reference </param>
        /// <param name="entityName"> the entityName </param>
        /// <param name="nodeID"> nodeID, it should only have relevance in DEBUG </param>
        /// <param name="ended"> if the Dialogue Ended </param>
        public void GetNextText(out string text, out string entityName, out string nodeID, out bool ended)
        {
            text = "";
            entityName = "";
            nodeID = "";
            ended = true;

            if (_currentNode == null)
                return;

            text = _currentNode.Text;
            if (_currentNode.WhoTalks == EntityEnum.Player)
                entityName = _playerName;
            else
                entityName = _currentNode.EntityName;

            nodeID = _currentNode.NodeID;
            ended = _delayedEnded;
            _delayedEnded = _branch.EndNodeFound || _branch.BranchEnded;

            _currentNode = _branch.GetNextNode();

            if (_currentNode is ISubBranchDialogueNode)
            {
                _currentNode = _branch.GetNextNode();
            }

            if (_currentNode is EndNodeAction endnode)
            {
                _onEndDialogue = endnode.DialogueEndReference.OnDialogueEnd;
            }

        }

        public void DialogueEnded()
        {
            _onEndDialogue?.Invoke();
        }
    }
}