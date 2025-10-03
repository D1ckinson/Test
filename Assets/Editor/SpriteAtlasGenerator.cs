using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Assets.Editor
{
    public class SpriteAtlasGenerator : EditorWindow
    {
        [MenuItem("Tools/Sprite Atlas Generator")]
        public static void ShowWindow()
        {
            GetWindow<SpriteAtlasGenerator>("Sprite Atlas Generator");
        }

        public string AtlasName = "NewSpriteAtlas";
        public int AtlasSize = 1024;
        public int Padding = 4;
        public List<Texture2D> SourceTextures = new List<Texture2D>();
        public string OutputFolder = "Assets/GeneratedAtlases";
        public bool GenerateSprites = true;

        private Vector2 scrollPosition;
        private SerializedObject serializedObject;
        private SerializedProperty sourceTexturesProperty;

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            sourceTexturesProperty = serializedObject.FindProperty("SourceTextures");
        }

        private void OnGUI()
        {
            serializedObject.Update();

            GUILayout.Label("Sprite Atlas Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Настройки
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            AtlasName = EditorGUILayout.TextField("Atlas Name", AtlasName);
            AtlasSize = EditorGUILayout.IntField("Atlas Size", AtlasSize);
            Padding = EditorGUILayout.IntField("Padding", Padding);
            OutputFolder = EditorGUILayout.TextField("Output Folder", OutputFolder);
            GenerateSprites = EditorGUILayout.Toggle("Generate Sprites", GenerateSprites);

            EditorGUILayout.Space();

            // Список текстур
            EditorGUILayout.LabelField("Source Textures", EditorStyles.boldLabel);

            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag & Drop Textures here");
            HandleDragAndDrop(dropArea);

            EditorGUILayout.PropertyField(sourceTexturesProperty, true);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear List"))
            {
                SourceTextures.Clear();
            }
            if (GUILayout.Button("Remove Nulls"))
            {
                SourceTextures.RemoveAll(texture => texture == null);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            // Кнопка генерации
            GUI.enabled = SourceTextures.Count > 0 && !string.IsNullOrEmpty(AtlasName);
            if (GUILayout.Button("Generate Atlas", GUILayout.Height(30)))
            {
                GenerateAtlas();
            }
            GUI.enabled = true;

            // Информация
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Textures: {SourceTextures.Count}", EditorStyles.helpBox);

            serializedObject.ApplyModifiedProperties();
        }

        private void HandleDragAndDrop(Rect dropArea)
        {
            Event evt = Event.current;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is Texture2D texture && !SourceTextures.Contains(texture))
                            {
                                SourceTextures.Add(texture);
                            }
                        }
                        evt.Use();
                    }
                    break;
            }
        }

        private void GenerateAtlas()
        {
            try
            {
                // Создаем папку если не существует
                if (!Directory.Exists(OutputFolder))
                {
                    Directory.CreateDirectory(OutputFolder);
                }

                // Подготавливаем текстуры
                List<Texture2D> processedTextures = new List<Texture2D>();

                foreach (Texture2D texture in SourceTextures)
                {
                    if (texture != null)
                    {
                        // Создаем копию текстуры для обработки
                        Texture2D processedTexture = ProcessTextureForAtlas(texture);
                        if (processedTexture != null)
                        {
                            processedTextures.Add(processedTexture);
                        }
                    }
                }

                if (processedTextures.Count == 0)
                {
                    EditorUtility.DisplayDialog("Error", "No valid textures to process!", "OK");
                    return;
                }

                // Создаем атлас
                Texture2D atlas = new Texture2D(AtlasSize, AtlasSize, TextureFormat.RGBA32, false);
                Rect[] uvRects = atlas.PackTextures(processedTextures.ToArray(), Padding, AtlasSize);

                if (uvRects == null || uvRects.Length == 0)
                {
                    EditorUtility.DisplayDialog("Error", "Failed to pack textures into atlas! Try increasing atlas size.", "OK");
                    return;
                }

                // Сохраняем атлас
                byte[] pngData = atlas.EncodeToPNG();
                string atlasPath = Path.Combine(OutputFolder, AtlasName + ".png");
                File.WriteAllBytes(atlasPath, pngData);

                // Импортируем текстуру в Unity
                AssetDatabase.Refresh();

                // Настраиваем импорт текстуры
                TextureImporter atlasImporter = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
                if (atlasImporter != null)
                {
                    atlasImporter.textureType = TextureImporterType.Sprite;

                    if (GenerateSprites)
                    {
                        atlasImporter.spriteImportMode = SpriteImportMode.Multiple;

                        // Создаем метаданные для спрайтов
                        List<SpriteMetaData> spriteMetaData = new List<SpriteMetaData>();
                        for (int i = 0; i < uvRects.Length && i < processedTextures.Count; i++)
                        {
                            SpriteMetaData metaData = new SpriteMetaData();
                            metaData.name = SourceTextures[i].name;

                            // Конвертируем UV координаты в пиксельные координаты
                            Rect uvRect = uvRects[i];
                            metaData.rect = new Rect(
                                uvRect.x * AtlasSize,
                                uvRect.y * AtlasSize,
                                uvRect.width * AtlasSize,
                                uvRect.height * AtlasSize
                            );

                            metaData.pivot = new Vector2(0.5f, 0.5f);
                            metaData.alignment = (int)SpriteAlignment.Center;

                            spriteMetaData.Add(metaData);
                        }

                        atlasImporter.spritesheet = spriteMetaData.ToArray();
                    }
                    else
                    {
                        atlasImporter.spriteImportMode = SpriteImportMode.Single;
                    }

                    atlasImporter.mipmapEnabled = false;
                    atlasImporter.filterMode = FilterMode.Bilinear;
                    atlasImporter.wrapMode = TextureWrapMode.Clamp;
                    atlasImporter.maxTextureSize = AtlasSize;
                    atlasImporter.textureCompression = TextureImporterCompression.Uncompressed;

                    atlasImporter.SaveAndReimport();
                }

                // Очищаем память
                foreach (Texture2D tex in processedTextures)
                {
                    DestroyImmediate(tex);
                }
                DestroyImmediate(atlas);

                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success",
                    $"Atlas generated successfully!\nPath: {atlasPath}\nSprites: {uvRects.Length}", "OK");

                // Выделяем созданный атлас в проекте
                Texture2D generatedAtlas = AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPath);
                if (generatedAtlas != null)
                {
                    Selection.activeObject = generatedAtlas;
                    EditorGUIUtility.PingObject(generatedAtlas);
                }
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", $"Failed to generate atlas: {e.Message}", "OK");
                Debug.LogError($"Atlas generation failed: {e}");
            }
        }

        private Texture2D ProcessTextureForAtlas(Texture2D sourceTexture)
        {
            try
            {
                string path = AssetDatabase.GetAssetPath(sourceTexture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer == null)
                    return null;

                // Временно меняем настройки текстуры для корректной упаковки
                bool wasReadable = importer.isReadable;

                if (!importer.isReadable)
                {
                    importer.isReadable = true;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.SaveAndReimport();
                }

                // Создаем копию текстуры
                Texture2D processedTexture = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
                processedTexture.SetPixels(sourceTexture.GetPixels());
                processedTexture.Apply();

                // Восстанавливаем оригинальные настройки
                if (!wasReadable)
                {
                    importer.isReadable = wasReadable;
                    importer.textureCompression = TextureImporterCompression.Compressed;
                    importer.SaveAndReimport();
                }

                return processedTexture;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to process texture {sourceTexture.name}: {e}");
                return null;
            }
        }
    }
}