using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [CustomEditor(typeof(DialogueObject))]
    public class DialogueObjectEditor : Editor
    {
        private const int SPACES = 10;
        private const int DESIRED_MAX_SUBBRANCHES = 5;
        private const int SUBBRANCH_PADDING = 8;
        private const float TEMPORAL_WARNINGTIME = 5;
        private Gradient BOX_COLOR;
        private Texture2D BOX_TEXTURE;
        private float _temporalWarningControl = 0;
        private bool _isTemporalWarning;

        private bool _nodeFoldOut = true;

        private bool _showNodeType;
        private bool _showWarnings;
        List<string> _warnings;

        private static readonly GUILayoutOption miniButtonWidth = GUILayout.Width(50);

        private void OnEnable()
        {
            _showNodeType = true;
            _showWarnings = true;
            _isTemporalWarning = false;
            _temporalWarningControl = 0;
            _warnings = new();
            BOX_COLOR = new Gradient();

            #region White Gradient
            //Color startColor = new(200f / 255f,
            //                       200f / 255f,
            //                       200f / 255f, 1f);
            //Color endColor = new(100f / 255f,
            //                     100f / 255f,
            //                     100f / 255f, 1f);
            #endregion
            #region Cyan Gradient
            Color startColor = new(142f / 255f,
                                   185f / 255f,
                                   217f / 255f, 1f);
            Color endColor = new(16f / 255f,
                                 60f / 255f,
                                 94f / 255f, 1f);
            #endregion

            GradientColorKey[] keys = new GradientColorKey[]
            {
                new GradientColorKey(startColor, 0),
                new GradientColorKey(endColor, 1)
            };

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(0.5f, 0),
                new GradientAlphaKey(1, 1)
            };

            BOX_COLOR.colorKeys = keys;
            BOX_COLOR.alphaKeys = alphaKeys;

            BOX_COLOR.mode = GradientMode.Blend;
            BOX_COLOR.colorSpace = ColorSpace.Linear;

            BOX_TEXTURE = Resources.Load<Texture2D>("Test/DialogueBoxEditor");
            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            Color lastColor = GUI.backgroundColor;
            GUI.backgroundColor = BOX_COLOR.Evaluate(0);

            serializedObject.Update();
            //base.OnInspectorGUI();

            #region DialogueEditor
            GUILayout.Space(SPACES * 2);

            CreateDialogueEditor();

            GUILayout.Space(SPACES);

            CreateNodeButton(((DialogueObject)target).GetBranch());

            GUILayout.Space(SPACES);

            if (_isTemporalWarning)
            {
                _temporalWarningControl += Time.deltaTime;
                if (_temporalWarningControl > TEMPORAL_WARNINGTIME)
                {
                    _temporalWarningControl = 0;
                    _isTemporalWarning = false;
                }
            }
            #endregion

            serializedObject.ApplyModifiedProperties();

            GUI.backgroundColor = lastColor;
        }

        private void CreateDialogueEditor()
        {
            int oldIndentedLevel = EditorGUI.indentLevel;

            GUIStyle style = new(StyleFramework.foldout)
            {
                fontStyle = FontStyle.Bold
            };

            EditorGUILayout.BeginHorizontal();
            _nodeFoldOut = EditorGUILayout.Foldout(_nodeFoldOut, "Node List", true, style: style);
            style = new(StyleFramework.button);
            if (GUILayout.Button("Show Type", style, miniButtonWidth, GUILayout.Width(100)))
                ChangeTypeVisibility();
            if (GUILayout.Button("Show Warnings", style, miniButtonWidth, GUILayout.Width(100)))
                ChangeWarningsVisibility();
            EditorGUILayout.EndHorizontal();

            if (_nodeFoldOut)
            {
                var branch = ((DialogueObject)target).GetBranch();
                DrawNodeList(branch, 1, "");
            }

            EditorGUI.indentLevel = oldIndentedLevel;
        }

        private void DrawNodeList(Branch branch, int subBranchCounter, string parentNode)
        {
            if (branch == null || branch.Equals(null))
                return;

            List<DialogueNode> list = branch.DialogueNodes;

            if (list == null || list.Equals(null))
                return;

            int oldIndentedLevel = EditorGUI.indentLevel;
            if (list.Count > 0)
            {

                Action<int, List<DialogueNode>> onListChangedAction = null;
                int index = -1;
                bool hasEndNode = false;
                _warnings.Clear();
                GUILayout.Space(SPACES);

                for (int i = 0; i < list.Count; i++)
                {
                    #region Init & Not Null
                    Color lastColor = GUI.backgroundColor;
                    GUI.backgroundColor = BOX_COLOR.Evaluate((float)subBranchCounter / DESIRED_MAX_SUBBRANCHES);

                    GUIStyle newBoxStyle = new(GUI.skin.box);

                    newBoxStyle.padding = new RectOffset(newBoxStyle.padding.left + SUBBRANCH_PADDING * subBranchCounter,
                                                         newBoxStyle.padding.right,
                                                         newBoxStyle.padding.top,
                                                         newBoxStyle.padding.bottom);

                    EditorGUILayout.BeginVertical(style: newBoxStyle);
                    GUILayout.Space(SPACES);
                    EditorGUI.indentLevel++;
                    bool isNull = false;
                    DialogueNode node = list.ElementAt(i);
                    if (node == null || node.Equals(null))
                        isNull = true;
                    #endregion

                    #region Header
                    EditorGUILayout.BeginHorizontal();
                    node.NodeID = parentNode + i;
                    string text = "Node: " + node.NodeID;

                    if (_showNodeType && !isNull) text = text + " | " + "Type: " + node.GetType().Name;
                    else if (isNull) text = text + " | " + "Type: NULL";
                    EditorGUILayout.LabelField(text, TextStyles.HeaderStyle);

                    #region Buttons
                    FuncionalButtons(() =>
                    {
                        if (i > 0)
                        {
                            index = i;
                            onListChangedAction = (int i, List<DialogueNode> list) =>
                            {
                                var node = list.ElementAt(i);
                                list.Remove(node);
                                list.Insert(i - 1, node);
                            };
                        }
                    }, () =>
                    {
                        if (i < list.Count - 1)
                        {
                            index = i;
                            onListChangedAction = (int i, List<DialogueNode> list) =>
                            {
                                var node = list.ElementAt(i);
                                list.Remove(node);
                                list.Insert(i + 1, node);
                            };
                        }
                    }, () =>
                    {
                        index = i;
                        onListChangedAction = (int i, List<DialogueNode> list) =>
                        {
                            var node = list.ElementAt(i);
                            list.Remove(node);
                        };
                    }
                    );
                    CreateNodeButton(branch, i + 1);
                    #endregion

                    EditorGUILayout.EndHorizontal();

                    #endregion

                    #region NodeContent
                    GUILayout.Space(SPACES);

                    if (!isNull)
                        DrawNode(node, subBranchCounter, parentNode + i + ".");

                    GUILayout.Space(SPACES);
                    EditorGUILayout.EndVertical();
                    GUILayout.Space(SPACES);
                    #endregion

                    #region Warnings
                    if (node is EndNode && _showWarnings)
                    {
                        if (i < list.Count - 1)
                        {
                            EditorGUILayout.HelpBox("All DialogueNodes after an EndNode will be ignored",
                                                    MessageType.Warning);
                        }

                        if (!hasEndNode)
                            hasEndNode = true;
                        else
                            _warnings.Add("There are more than 1 EndNodes in the Current Branch b Dialogue");
                    }
                    else if (isNull)
                    {
                        EditorGUILayout.HelpBox("Delete this Node due to Null Reference", MessageType.Error);
                    }
                    #endregion

                    GUI.backgroundColor = lastColor;
                    EditorGUI.indentLevel = oldIndentedLevel;

                }
                #region General Warnings && ModificationInTheList
                onListChangedAction?.Invoke(index, list);
                if (_warnings.Count > 0)
                {
                    foreach (string item in _warnings)
                    {
                        EditorGUILayout.HelpBox(item, MessageType.Warning);
                    }
                }
                #endregion
            }
            else
            {
                EditorGUILayout.HelpBox("Empty Branch", MessageType.Error);
            }
            EditorGUI.indentLevel = oldIndentedLevel;
        }

        private void FuncionalButtons(Action OnUpButton, Action OnDownButton, Action OnRemoveButton)
        {
            if (GUILayout.Button("Up", EditorStyles.miniButtonLeft, miniButtonWidth))
            {
                OnUpButton.Invoke();
            }
            else if (GUILayout.Button("Down", EditorStyles.miniButtonMid, miniButtonWidth))
            {
                OnDownButton.Invoke();
            }
            else if (GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonWidth))
            {
                OnRemoveButton.Invoke();
            }
        }

        private void DrawNode(DialogueNode node, int subBranch, string parentNode)
        {

            if (node is ConditionedDialogueNode newNode)
            {
                DrawConditionedDialogueNode(newNode, subBranch, parentNode);
            }
            else if (node is OtherDialogueBranchNode other)
            {
                DrawOtherDialogueNode(other);
            }
            else
            {
                DrawDialogueNode(node);
            }

        }

        private void DrawDialogueNode(DialogueNode node)
        {
            if (node == null)
                return;

            int oldIndentedLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            GUILayout.BeginHorizontal();
            {
                if (node.WhoTalks == EntityEnum.Player)
                {
                    GUILayout.Space(SPACES * 15);//
                }

                GUILayout.BeginVertical();
                node.WhoTalks = (EntityEnum)EditorGUILayout.EnumPopup("Who Talks: ", node.WhoTalks);
                if (node.WhoTalks == EntityEnum.NPC)
                    node.EntityName = EditorGUILayout.TextField("NPC Name: ", node.EntityName);

                EditorGUILayout.LabelField("Text");
                GUILayout.Space(SPACES);
                node.Text = EditorGUILayout.TextArea(node.Text);

                if (node is EndNodeAction nodeAction)
                {
                    nodeAction.DialogueEndReference = (DialogueEndActionObject)
                        EditorGUILayout.ObjectField("EndActionObject", nodeAction.DialogueEndReference,
                        typeof(DialogueEndActionObject), false);

                    if (nodeAction.DialogueEndReference == null)
                    {
                        EditorGUILayout.HelpBox("Dialogue End Action null Reference \n" +
                            "Assign an action reference or use basic EndNode instead", MessageType.Error);
                    }
                }

                GUILayout.EndVertical();

                if (node.WhoTalks == EntityEnum.NPC)
                {
                    GUILayout.Space(SPACES * 15);
                }
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel = oldIndentedLevel;
        }

        private void DrawConditionedDialogueNode(ConditionedDialogueNode node, int subBranch, string parentNode)
        {
            if (node == null)
                return;

            if (node.CanAddCondition())
            {
                GUIStyle gUIStyle = new(StyleFramework.button);
                GUIStyle style = gUIStyle;
                if (GUILayout.Button("Add Condition", style, miniButtonWidth, GUILayout.Width(100)))
                {
                    node.AddCondition();
                }
            }

            var list = node.ConditionedBranch;
            if (list != null || !list.Equals(null))
            {
                int index = -1;
                Action<int, List<ConditionAndDialoguePair>> onListChangedAction = null;

                _warnings.Clear();
                subBranch++;
                for (int i = 0; i <= list.Count; i++)
                {
                    GUIStyle newBoxStyle = new(StyleFramework.boxChild);
                    newBoxStyle.padding = new RectOffset(newBoxStyle.padding.left + SUBBRANCH_PADDING * (subBranch),
                                                         newBoxStyle.padding.right,
                                                         newBoxStyle.padding.top,
                                                         newBoxStyle.padding.bottom);

                    EditorGUILayout.BeginVertical(style: newBoxStyle);

                    if (i != list.Count)
                    {
                        #region Header
                        var item = list[i];
                        EditorGUILayout.BeginHorizontal();

                        string text = "IF";
                        if (i > 0)
                            text = "ELSE " + text;

                        item.Condition = (DialogueConditionObject)EditorGUILayout.
                            ObjectField(text, item.Condition, typeof(DialogueConditionObject), false);

                        #region Buttons
                        FuncionalButtons(() =>
                        {
                            if (i > 0)
                            {
                                index = i;
                                onListChangedAction = (int i, List<ConditionAndDialoguePair> list) =>
                                {
                                    var node = list.ElementAt(i);
                                    list.Remove(node);
                                    list.Insert(i - 1, node);
                                };
                            }
                        }, () =>
                        {
                            if (i < list.Count - 1)
                            {
                                index = i;
                                onListChangedAction = (int i, List<ConditionAndDialoguePair> list) =>
                                {
                                    var node = list.ElementAt(i);
                                    list.Remove(node);
                                    list.Insert(i + 1, node);
                                };
                            }
                        }, () =>
                        {
                            index = i;
                            onListChangedAction = (int i, List<ConditionAndDialoguePair> list) =>
                            {
                                var node = list.ElementAt(i);
                                list.Remove(node);
                            };
                        }
                        );
                        #endregion

                        EditorGUILayout.EndHorizontal();
                        #endregion
                    }
                    else
                    {
                        EditorGUILayout.LabelField("ELSE/DEFAULT");
                    }

                    #region Content
                    Branch branch = i == list.Count ? node.DefaultBranch : list[i].ConditionedBranch;
                    GUIStyle style = new(StyleFramework.foldout)
                    {
                        fontStyle = FontStyle.Bold
                    };

                    EditorGUILayout.BeginHorizontal();
                    branch.FoldOut = EditorGUILayout.Foldout(branch.FoldOut, "Conditioned Node List: ", true, style);
                    CreateNodeButton(branch);
                    EditorGUILayout.EndHorizontal();

                    if (branch.FoldOut)
                        DrawNodeList(branch, subBranch, parentNode);
                    #endregion
                    GUILayout.Space(SPACES);
                    EditorGUILayout.EndVertical();
                    GUILayout.Space(SPACES);

                }
                onListChangedAction?.Invoke(index, list);
            }

        }

        private void DrawOtherDialogueNode(OtherDialogueBranchNode node)
        {
            if (node == null)
                return;

            int oldIndentedLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            //int ignoreInt = node.IgnoreEndNode ? 1 : 0;
            //ignoreInt = EditorGUILayout.IntSlider("Ignore END_NODE (0:False, 1:True)", ignoreInt, 0, 1);
            node.IgnoreEndNode = false;//EditorGUILayout.Toggle("Ignore END_NODE", node.IgnoreEndNode);

            node.OtherBranch = (DialogueObject)EditorGUILayout.ObjectField("Dialogue Object Reference: ",
                                                                           node.OtherBranch,
                                                                           typeof(DialogueObject), false);

            bool sameObject = node.OtherBranch == (DialogueObject)target;

            if (sameObject)
            {
                node.OtherBranch = null;
                _isTemporalWarning = true;
                _temporalWarningControl = 0;
            }

            if (_showWarnings && _isTemporalWarning)
            {
                EditorGUILayout.HelpBox("You Can't put a reference of the same Dialogue Object", MessageType.Error);
            }

            EditorGUI.indentLevel = oldIndentedLevel;
        }

        private void CreateNodeButton(Branch branch, int index = -1)
        {
            Type t = typeof(DialogueNode);
            GUIContent label = new GUIContent
            {
                text = "+"
            };

            if (EditorGUILayout.DropdownButton(label, FocusType.Passive,
                                               EditorStyles.miniButtonRight, miniButtonWidth))
            {
                GenericMenu menu = new();

                // inherited types
                foreach (Type type in SomeEditorUtilities.GetClasses(t))
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        //var a = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                        branch.CreateNewDialogueNode(Activator.CreateInstance(type) as DialogueNode, index);
                    });
                }
                menu.ShowAsContext();
            }
        }

        private void ChangeTypeVisibility()
        {
            _showNodeType = !_showNodeType;
        }
        private void ChangeWarningsVisibility()
        {
            _showWarnings = !_showWarnings;
        }
    }
}
