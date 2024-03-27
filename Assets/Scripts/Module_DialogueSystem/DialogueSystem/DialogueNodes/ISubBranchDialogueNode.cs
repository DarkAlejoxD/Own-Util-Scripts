namespace DialogueSystem
{
    public interface ISubBranchDialogueNode
    {
        DialogueNode CurrentNode { get; }
        bool BranchEnded { get; }
        bool AlreadyEvaluating { get; }
        bool FoundEndNode { get; }
        bool IgnoreEndNode { get; }
        DialogueNode GetNextNode();
        bool CanPassNextNode();
        void Reset();
    }
}