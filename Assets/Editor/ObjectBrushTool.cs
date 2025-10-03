using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Editor
{
    public class ObjectBrushTool : EditorWindow
    {
        [MenuItem("Tools/Object Brush")]
        public static void ShowWindow()
        {
            GetWindow<ObjectBrushTool>("Object Brush");
        }

        [SerializeField]
        private List<GameObject> _brushObjects = new();

        public float BrushSize = 2f;
        public float Density = 0.5f;
        public float MinScale = 0.8f;
        public float MaxScale = 1.2f;
        public float Spacing = 1f;
        public LayerMask PaintLayer = 1;

        private bool _isPainting = false;
        private bool _isErasing = false;
        private bool _useEnglish = false;
        private List<GameObject> _paintedObjects = new();
        private SerializedObject _serializedObject;
        private SerializedProperty _brushObjectsProperty;

        private Dictionary<string, string> _englishTexts = new()
    {
        {"Settings", "Object Brush Settings"},
        {"BrushObjects", "Brush Objects"},
        {"DragDrop", "Drag & Drop GameObjects here"},
        {"BrushObjectsList", "Brush Objects List"},
        {"ClearList", "Clear List"},
        {"RemoveNulls", "Remove Nulls"},
        {"BrushSettings", "Brush Settings"},
        {"BrushSize", "Brush Size"},
        {"Density", "Density"},
        {"MinScale", "Min Scale"},
        {"MaxScale", "Max Scale"},
        {"Spacing", "Spacing"},
        {"PaintLayer", "Paint Layer"},
        {"BrushModes", "Brush Modes"},
        {"PaintMode", "Paint Mode"},
        {"EraseMode", "Erase Mode"},
        {"ClearAll", "Clear All Painted"},
        {"SelectAll", "Select All Painted"},
        {"PaintedObjects", "Painted Objects"},
        {"BrushObjectsCount", "Brush Objects"},
        {"Warning", "Add at least one GameObject to Brush Objects list!"},
        {"Language", "Language"},
        {"Russian", "Russian"},
        {"English", "English"}
    };

        private Dictionary<string, string> _russianTexts = new()
    {
        {"Settings", "Настройки кисти"},
        {"BrushObjects", "Объекты для рисования"},
        {"DragDrop", "Перетащите GameObjects сюда"},
        {"BrushObjectsList", "Список объектов кисти"},
        {"ClearList", "Очистить список"},
        {"RemoveNulls", "Удалить пустые"},
        {"BrushSettings", "Настройки кисти"},
        {"BrushSize", "Размер кисти"},
        {"Density", "Плотность"},
        {"MinScale", "Мин. масштаб"},
        {"MaxScale", "Макс. масштаб"},
        {"Spacing", "Расстояние"},
        {"PaintLayer", "Слой рисования"},
        {"BrushModes", "Режимы кисти"},
        {"PaintMode", "Режим рисования"},
        {"EraseMode", "Режим стирания"},
        {"ClearAll", "Очистить все"},
        {"SelectAll", "Выделить все"},
        {"PaintedObjects", "Нарисовано объектов"},
        {"BrushObjectsCount", "Объектов в кисти"},
        {"Warning", "Добавьте хотя бы один GameObject в список кисти!"},
        {"Language", "Язык"},
        {"Russian", "Русский"},
        {"English", "Английский"}
    };

        private Dictionary<string, string> CurrentTexts => _useEnglish ? _englishTexts : _russianTexts;

        private string T(string key)
        {
            return CurrentTexts.ContainsKey(key) ? CurrentTexts[key] : key;
        }

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            _brushObjectsProperty = _serializedObject.FindProperty("_brushObjects");
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            _serializedObject?.Dispose();
        }

        private void OnGUI()
        {
            _serializedObject.Update();

            GUILayout.Label(T("Settings"), EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(T("Language"));

            if (GUILayout.Button(T("Russian")))
            {
                _useEnglish = false;
            }

            if (GUILayout.Button(T("English")))
            {
                _useEnglish = true;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(T("BrushObjects"), EditorStyles.boldLabel);

            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, T("DragDrop"));

            HandleDragAndDrop(dropArea);

            EditorGUILayout.PropertyField(_brushObjectsProperty, new GUIContent(T("BrushObjectsList")), true);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(T("ClearList")))
            {
                _brushObjects.Clear();
            }

            if (GUILayout.Button(T("RemoveNulls")))
            {
                _brushObjects.RemoveAll(obj => obj == null);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(T("BrushSettings"), EditorStyles.boldLabel);
            BrushSize = EditorGUILayout.Slider(T("BrushSize"), BrushSize, 0.1f, 40f);
            Density = EditorGUILayout.Slider(T("Density"), Density, 0.01f, 1f);
            MinScale = EditorGUILayout.FloatField(T("MinScale"), MinScale);
            MaxScale = EditorGUILayout.FloatField(T("MaxScale"), MaxScale);
            Spacing = EditorGUILayout.Slider(T("Spacing"), Spacing, 0.1f, 10f);
            PaintLayer = EditorGUILayout.LayerField(T("PaintLayer"), PaintLayer);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(T("BrushModes"), EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            GUI.color = _isPainting ? Color.green : Color.white;

            if (GUILayout.Button(T("PaintMode")))
            {
                _isPainting = !_isPainting;
                _isErasing = false;
            }

            GUI.color = _isErasing ? Color.red : Color.white;

            if (GUILayout.Button(T("EraseMode")))
            {
                _isPainting = false;
                _isErasing = !_isErasing;
            }

            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(T("ClearAll")))
            {
                ClearAllObjects();
            }

            if (GUILayout.Button(T("SelectAll")))
            {
                SelectAllPaintedObjects();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField($"{T("PaintedObjects")}: {_paintedObjects.Count}");
            EditorGUILayout.LabelField($"{T("BrushObjectsCount")}: {_brushObjects.Count}");

            if (_brushObjects.Count == 0)
            {
                EditorGUILayout.HelpBox(T("Warning"), MessageType.Warning);
            }

            _serializedObject.ApplyModifiedProperties();
        }

        private void HandleDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                    {
                        return;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        HandleDragPerform();
                        evt.Use();
                    }
                    break;
            }
        }

        private void HandleDragPerform()
        {
            DragAndDrop.AcceptDrag();

            foreach (Object draggedObject in DragAndDrop.objectReferences)
            {
                if (draggedObject is GameObject gameObject)
                {
                    AddBrushObject(gameObject);
                }
                else if (draggedObject is Component component)
                {
                    AddBrushObject(component.gameObject);
                }
            }

            _brushObjects = _brushObjects.OrderBy(obj => obj.name).ToList();
        }

        private void AddBrushObject(GameObject gameObject)
        {
            if (!_brushObjects.Contains(gameObject))
            {
                _brushObjects.Add(gameObject);
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            sceneView.Repaint();

            if (!_isPainting && !_isErasing)
            {
                return;
            }

            if (_brushObjects.Count == 0 && _isPainting)
            {
                return;
            }

            Event e = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                DrawBrush(hit.point, hit.normal);

                if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
                {
                    if (e.button == 0)
                    {
                        HandleMouseDrag(hit);
                        e.Use();
                    }
                }
            }

            if (e.type == EventType.MouseUp && e.button == 0)
            {
                GUIUtility.hotControl = 0;
                e.Use();
            }
        }

        private void HandleMouseDrag(RaycastHit hit)
        {
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

            if (Event.current.type == EventType.MouseDrag)
            {
                if (_isPainting)
                {
                    PaintObjects(hit.point, hit.normal);
                }
                else if (_isErasing)
                {
                    EraseObjects(hit.point);
                }
            }
        }

        private void DrawBrush(Vector3 position, Vector3 normal)
        {
            Handles.color = _isPainting ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f);
            Handles.DrawSolidDisc(position, normal, BrushSize);

            Handles.color = _isPainting ? Color.green : Color.red;

            for (int i = 0; i < 3; i++)
            {
                Handles.DrawWireDisc(position, normal, BrushSize - i * 0.02f);
            }

            Handles.color = _isPainting ? new Color(0, 0.8f, 0, 0.6f) : new Color(0.8f, 0, 0, 0.6f);
            Handles.DrawWireDisc(position, normal, BrushSize * 0.5f);

            string modeText = _isPainting ? "PAINT" : "ERASE";
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = _isPainting ? Color.green : Color.red;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fontSize = 12;

            Handles.Label(position + Vector3.up * 0.3f, modeText, labelStyle);
            Handles.Label(position + Vector3.up * 0.5f, $"Size: {BrushSize:F1}", labelStyle);
        }

        private void PaintObjects(Vector3 position, Vector3 normal)
        {
            if (_brushObjects.Count == 0)
            {
                return;
            }

            int objectsToPlace = Mathf.RoundToInt(BrushSize * BrushSize * Density);

            for (int i = 0; i < objectsToPlace; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * BrushSize;
                Vector3 spawnPos = position + new Vector3(randomCircle.x, 0, randomCircle.y);

                if (Physics.Raycast(spawnPos + Vector3.up * 10f, Vector3.down, out RaycastHit groundHit, 20f, PaintLayer))
                {
                    spawnPos = groundHit.point;
                }

                if (!IsTooCloseToOtherObjects(spawnPos, Spacing))
                {
                    CreatePaintedObject(spawnPos);
                }
            }
        }

        private void CreatePaintedObject(Vector3 spawnPos)
        {
            GameObject objToSpawn = _brushObjects[Random.Range(0, _brushObjects.Count)];
            GameObject newObj;

            if (PrefabUtility.IsPartOfAnyPrefab(objToSpawn))
            {
                newObj = PrefabUtility.InstantiatePrefab(objToSpawn) as GameObject;
            }
            else
            {
                newObj = Instantiate(objToSpawn);
            }

            if (newObj != null)
            {
                newObj.transform.position = spawnPos;
                newObj.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                newObj.transform.localScale = Vector3.one * Random.Range(MinScale, MaxScale);

                GameObject parent = GameObject.Find("PaintedObjects");

                if (parent == null)
                {
                    parent = new GameObject("PaintedObjects");
                }

                newObj.transform.SetParent(parent.transform);
                _paintedObjects.Add(newObj);
                Undo.RegisterCreatedObjectUndo(newObj, "Paint Object");
            }
        }

        private bool IsTooCloseToOtherObjects(Vector3 position, float minDistance)
        {
            foreach (GameObject obj in _paintedObjects)
            {
                if (obj != null && Vector3.Distance(obj.transform.position, position) < minDistance)
                {
                    return true;
                }
            }

            return false;
        }

        private void EraseObjects(Vector3 position)
        {
            List<GameObject> toRemove = new List<GameObject>();

            foreach (GameObject obj in _paintedObjects)
            {
                if (obj != null && Vector3.Distance(obj.transform.position, position) <= BrushSize)
                {
                    toRemove.Add(obj);
                    Undo.DestroyObjectImmediate(obj);
                }
            }

            foreach (GameObject obj in toRemove)
            {
                _paintedObjects.Remove(obj);
            }
        }

        private void ClearAllObjects()
        {
            if (EditorUtility.DisplayDialog("Clear All Objects", "Are you sure you want to delete all painted objects?", "Yes", "No"))
            {
                foreach (GameObject obj in _paintedObjects)
                {
                    if (obj != null)
                    {
                        DestroyImmediate(obj);
                    }
                }

                _paintedObjects.Clear();
            }
        }

        private void SelectAllPaintedObjects()
        {
            List<GameObject> validObjects = new List<GameObject>();

            foreach (GameObject obj in _paintedObjects)
            {
                if (obj != null)
                {
                    validObjects.Add(obj);
                }
            }

            if (validObjects.Count > 0)
            {
                Selection.objects = validObjects.ToArray();
            }
        }
    }
}