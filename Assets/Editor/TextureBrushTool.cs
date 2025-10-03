using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Editor
{
    public class TextureBrushTool : EditorWindow
    {
        [MenuItem("Tools/Texture Brush")]
        public static void ShowWindow()
        {
            GetWindow<TextureBrushTool>("Texture Brush");
        }

        [SerializeField]
        private List<Sprite> _brushSprites = new();

        public float BrushSize = 2f;
        public float Density = 0.5f;
        public float MinScale = 0.8f;
        public float MaxScale = 1.2f;
        public float Spacing = 1f;
        public LayerMask PaintLayer = 1;
        public Material GrassMaterial;
        public Texture2D AtlasTexture;
        public bool RandomRotation = true;
        public float HeightOffset = 0f;

        private bool _isPainting = false;
        private bool _isErasing = false;
        private bool _useEnglish = false;
        private List<GameObject> _paintedObjects = new();
        private SerializedObject _serializedObject;
        private SerializedProperty _brushSpritesProperty;
        private List<Rect> _atlasRegions = new();

        // Кэш материалов для каждого региона атласа
        private Dictionary<Rect, Material> _regionMaterials = new();
        private Material _fallbackMaterial;

        private Dictionary<string, string> _englishTexts = new()
        {
            {"Settings", "Texture Brush Settings"},
            {"BrushSprites", "Brush Sprites"},
            {"DragDrop", "Drag & Drop Sprites or Textures here"},
            {"BrushSpritesList", "Brush Sprites List"},
            {"ClearList", "Clear List"},
            {"RemoveNulls", "Remove Nulls"},
            {"BrushSettings", "Brush Settings"},
            {"BrushSize", "Brush Size"},
            {"Density", "Density"},
            {"MinScale", "Min Scale"},
            {"MaxScale", "Max Scale"},
            {"Spacing", "Spacing"},
            {"PaintLayer", "Paint Layer"},
            {"HeightOffset", "Height Offset"},
            {"GrassMaterial", "Grass Material"},
            {"AtlasTexture", "Atlas Texture"},
            {"RandomRotation", "Random Rotation"},
            {"BrushModes", "Brush Modes"},
            {"PaintMode", "Paint Mode"},
            {"EraseMode", "Erase Mode"},
            {"ClearAll", "Clear All Painted"},
            {"SelectAll", "Select All Painted"},
            {"PaintedObjects", "Painted Objects"},
            {"BrushSpritesCount", "Brush Sprites"},
            {"AtlasRegionsCount", "Atlas Regions"},
            {"Warning", "Add Atlas Texture or Sprites to Brush!"},
            {"Language", "Language"},
            {"Russian", "Russian"},
            {"English", "English"}
        };

        private Dictionary<string, string> _russianTexts = new()
        {
            {"Settings", "Настройки кисти текстур"},
            {"BrushSprites", "Спрайты для рисования"},
            {"DragDrop", "Перетащите Sprites или Textures сюда"},
            {"BrushSpritesList", "Список спрайтов кисти"},
            {"ClearList", "Очистить список"},
            {"RemoveNulls", "Удалить пустые"},
            {"BrushSettings", "Настройки кисти"},
            {"BrushSize", "Размер кисти"},
            {"Density", "Плотность"},
            {"MinScale", "Мин. масштаб"},
            {"MaxScale", "Макс. масштаб"},
            {"Spacing", "Расстояние"},
            {"PaintLayer", "Слой рисования"},
            {"HeightOffset", "Смещение по высоте"},
            {"GrassMaterial", "Материал травы"},
            {"AtlasTexture", "Атлас текстур"},
            {"RandomRotation", "Случайный поворот"},
            {"BrushModes", "Режимы кисти"},
            {"PaintMode", "Режим рисования"},
            {"EraseMode", "Режим стирания"},
            {"ClearAll", "Очистить все"},
            {"SelectAll", "Выделить все"},
            {"PaintedObjects", "Нарисовано объектов"},
            {"BrushSpritesCount", "Спрайтов в кисти"},
            {"AtlasRegionsCount", "Регионов в атласе"},
            {"Warning", "Добавьте атлас или спрайты в кисть!"},
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
            _brushSpritesProperty = _serializedObject.FindProperty("_brushSprites");
            SceneView.duringSceneGui += OnSceneGUI;
            UpdateAtlasRegions();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            _serializedObject?.Dispose();
            ClearMaterialCache();
        }

        private void ClearMaterialCache()
        {
            foreach (var material in _regionMaterials.Values)
            {
                if (material != null)
                    DestroyImmediate(material);
            }
            _regionMaterials.Clear();

            if (_fallbackMaterial != null)
                DestroyImmediate(_fallbackMaterial);
        }

        private void UpdateAtlasRegions()
        {
            _atlasRegions.Clear();
            ClearMaterialCache();

            if (AtlasTexture != null)
            {
                string atlasPath = AssetDatabase.GetAssetPath(AtlasTexture);
                if (!string.IsNullOrEmpty(atlasPath))
                {
                    Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(atlasPath).OfType<Sprite>().ToArray();
                    foreach (Sprite sprite in sprites)
                    {
                        if (sprite != null)
                        {
                            Rect uvRect = new Rect(
                                sprite.textureRect.x / (float)AtlasTexture.width,
                                sprite.textureRect.y / (float)AtlasTexture.height,
                                sprite.textureRect.width / (float)AtlasTexture.width,
                                sprite.textureRect.height / (float)AtlasTexture.height
                            );
                            _atlasRegions.Add(uvRect);
                        }
                    }
                }

                if (_atlasRegions.Count == 0)
                {
                    string[] allSpritePaths = AssetDatabase.FindAssets("t:Sprite");
                    foreach (string guid in allSpritePaths)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                        if (sprite != null && sprite.texture == AtlasTexture)
                        {
                            Rect uvRect = new Rect(
                                sprite.textureRect.x / (float)AtlasTexture.width,
                                sprite.textureRect.y / (float)AtlasTexture.height,
                                sprite.textureRect.width / (float)AtlasTexture.width,
                                sprite.textureRect.height / (float)AtlasTexture.height
                            );
                            _atlasRegions.Add(uvRect);
                        }
                    }
                }
            }
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

            EditorGUILayout.LabelField(T("BrushSprites"), EditorStyles.boldLabel);

            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, T("DragDrop"));

            HandleDragAndDrop(dropArea);

            EditorGUILayout.PropertyField(_brushSpritesProperty, new GUIContent(T("BrushSpritesList")), true);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(T("ClearList")))
            {
                _brushSprites.Clear();
            }

            if (GUILayout.Button(T("RemoveNulls")))
            {
                _brushSprites.RemoveAll(obj => obj == null);
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
            HeightOffset = EditorGUILayout.Slider(T("HeightOffset"), HeightOffset, -1f, 1f);
            RandomRotation = EditorGUILayout.Toggle(T("RandomRotation"), RandomRotation);
            GrassMaterial = (Material)EditorGUILayout.ObjectField(T("GrassMaterial"), GrassMaterial, typeof(Material), false);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Atlas Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            AtlasTexture = (Texture2D)EditorGUILayout.ObjectField(T("AtlasTexture"), AtlasTexture, typeof(Texture2D), false);
            if (EditorGUI.EndChangeCheck())
            {
                UpdateAtlasRegions();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Detect Regions"))
            {
                UpdateAtlasRegions();
            }
            EditorGUILayout.EndHorizontal();

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
            EditorGUILayout.LabelField($"{T("BrushSpritesCount")}: {_brushSprites.Count}");
            EditorGUILayout.LabelField($"{T("AtlasRegionsCount")}: {_atlasRegions.Count}");

            if (_brushSprites.Count == 0 && AtlasTexture == null)
            {
                EditorGUILayout.HelpBox(T("Warning"), MessageType.Warning);
            }
            else if (AtlasTexture != null && _atlasRegions.Count == 0)
            {
                EditorGUILayout.HelpBox("No regions detected in atlas. Click 'Detect Regions' or add sprites that use this atlas.", MessageType.Info);
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
                if (draggedObject is Sprite sprite)
                {
                    AddBrushSprite(sprite);
                }
                else if (draggedObject is Texture2D texture)
                {
                    Sprite newSprite = Sprite.Create(texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f);
                    newSprite.name = texture.name + "_AutoSprite";
                    AddBrushSprite(newSprite);
                }
            }

            _brushSprites = _brushSprites.OrderBy(obj => obj.name).ToList();
        }

        private void AddBrushSprite(Sprite sprite)
        {
            if (!_brushSprites.Contains(sprite))
            {
                _brushSprites.Add(sprite);
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            sceneView.Repaint();

            if (!_isPainting && !_isErasing)
            {
                return;
            }

            if (_brushSprites.Count == 0 && AtlasTexture == null && _isPainting)
            {
                return;
            }

            Event e = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, PaintLayer))
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
                    PaintTextures(hit.point, hit.normal);
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

            string modeText = _isPainting ? "TEXTURE PAINT" : "ERASE";
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = _isPainting ? Color.green : Color.red;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.fontSize = 12;

            Handles.Label(position + normal * 0.3f, modeText, labelStyle);
            Handles.Label(position + normal * 0.5f, $"Size: {BrushSize:F1}", labelStyle);
        }

        private void PaintTextures(Vector3 position, Vector3 normal)
        {
            if (_brushSprites.Count == 0 && AtlasTexture == null)
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
                    spawnPos = groundHit.point + groundHit.normal * HeightOffset;
                    if (!IsTooCloseToOtherObjects(spawnPos, Spacing))
                    {
                        CreateTextureObject(spawnPos, groundHit.normal);
                    }
                }
            }
        }

        private void CreateTextureObject(Vector3 spawnPos, Vector3 surfaceNormal)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            DestroyImmediate(quad.GetComponent<Collider>());

            // Используем материал с правильным регионом атласа
            Material material = GetMaterialForRegion();

            quad.GetComponent<Renderer>().material = material;

            // Настройка трансформации
            quad.transform.position = spawnPos;
            quad.transform.rotation = Quaternion.LookRotation(Vector3.Cross(surfaceNormal, Vector3.forward), surfaceNormal);

            if (quad.transform.forward == Vector3.zero)
            {
                quad.transform.up = surfaceNormal;
            }

            if (RandomRotation)
            {
                float randomAngle = Random.Range(0f, 360f);
                quad.transform.Rotate(surfaceNormal, randomAngle, Space.World);
            }

            quad.transform.localScale = Vector3.one * Random.Range(MinScale, MaxScale);

            GameObject parent = GameObject.Find("PaintedTextures");
            if (parent == null)
            {
                parent = new GameObject("PaintedTextures");
            }

            quad.transform.SetParent(parent.transform);
            _paintedObjects.Add(quad);
            Undo.RegisterCreatedObjectUndo(quad, "Paint Texture Object");
        }

        private Material GetMaterialForRegion()
        {
            // Если используем атлас - выбираем случайный регион
            if (AtlasTexture != null && _atlasRegions.Count > 0)
            {
                Rect region = _atlasRegions[Random.Range(0, _atlasRegions.Count)];

                // Используем кэшированный материал для этого региона
                if (!_regionMaterials.ContainsKey(region))
                {
                    _regionMaterials[region] = CreateRegionMaterial(region);
                }

                return _regionMaterials[region];
            }

            // Если используем отдельные спрайты
            if (_brushSprites.Count > 0)
            {
                Sprite selectedSprite = _brushSprites[Random.Range(0, _brushSprites.Count)];
                if (selectedSprite != null && selectedSprite.texture != null)
                {
                    Rect region = new Rect(
                        selectedSprite.textureRect.x / (float)selectedSprite.texture.width,
                        selectedSprite.textureRect.y / (float)selectedSprite.texture.height,
                        selectedSprite.textureRect.width / (float)selectedSprite.texture.width,
                        selectedSprite.textureRect.height / (float)selectedSprite.texture.height
                    );

                    if (!_regionMaterials.ContainsKey(region))
                    {
                        _regionMaterials[region] = CreateRegionMaterial(region, selectedSprite.texture);
                    }

                    return _regionMaterials[region];
                }
            }

            // Fallback материал
            if (_fallbackMaterial == null)
            {
                _fallbackMaterial = CreateBaseMaterial();
            }
            return _fallbackMaterial;
        }

        private Material CreateRegionMaterial(Rect region, Texture2D texture = null)
        {
            Material material = CreateBaseMaterial();

            if (texture != null)
            {
                material.mainTexture = texture;
            }
            else if (AtlasTexture != null)
            {
                material.mainTexture = AtlasTexture;
            }

            // Устанавливаем UV координаты для региона
            material.mainTextureOffset = new Vector2(region.x, region.y);
            material.mainTextureScale = new Vector2(region.width, region.height);

            return material;
        }

        private Material CreateBaseMaterial()
        {
            Material material;
            if (GrassMaterial != null)
            {
                material = new Material(GrassMaterial);
            }
            else
            {
                material = new Material(Shader.Find("Standard"));
                material.SetFloat("_Mode", 2);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }

            // Включаем GPU Instancing для лучшей производительности
            material.enableInstancing = true;

            return material;
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
            if (EditorUtility.DisplayDialog("Clear All Objects", "Are you sure you want to delete all painted texture objects?", "Yes", "No"))
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
