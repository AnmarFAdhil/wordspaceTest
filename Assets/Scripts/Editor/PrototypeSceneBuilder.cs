using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;

public class PrototypeSceneBuilder : MonoBehaviour
{
    [MenuItem("Tools/Create Prototype Scene")]
    public static void CreatePrototypeScene()
    {
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        newScene.name = "Prototype";

        SetupLighting();
        CreateBridgeArena();
        CreatePlayer();
        CreateProjectilePrefab();
        CreateEnemyPrefab();
        CreateBossPrefab();
        CreateSpawner();
        SetupCamera();
        CreateCanvas();

        EditorSceneManager.SaveScene(newScene, "Assets/Scenes/Prototype.unity");
        Debug.Log("✓ Prototype scene created successfully!");
    }

    private static void SetupLighting()
    {
        GameObject lightObj = new GameObject("MainLight");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    private static void CreateBridgeArena()
    {
        GameObject bridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bridge.name = "Bridge";
        bridge.transform.position = new Vector3(0, 0, 25);
        bridge.transform.localScale = new Vector3(10, 1, 50);
        bridge.GetComponent<Collider>().isTrigger = false;
        Renderer bridgeRenderer = bridge.GetComponent<Renderer>();
        bridgeRenderer.material.color = new Color(0.5f, 0.5f, 0.5f);
        
        GameObject leftRail = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftRail.name = "LeftRail";
        leftRail.transform.position = new Vector3(-5.5f, 0.5f, 25);
        leftRail.transform.localScale = new Vector3(0.5f, 1, 50);
        leftRail.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f);
        
        GameObject rightRail = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightRail.name = "RightRail";
        rightRail.transform.position = new Vector3(5.5f, 0.5f, 25);
        rightRail.transform.localScale = new Vector3(0.5f, 1, 50);
        rightRail.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f);
    }

    private static void CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = new Vector3(0, 1, 0);

        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Body";
        body.transform.parent = player.transform;
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(1, 2, 1);
        body.GetComponent<Collider>().isTrigger = true;
        Renderer bodyRenderer = body.GetComponent<Renderer>();
        bodyRenderer.material.color = Color.blue;

        GameObject muzzleObj = new GameObject("Muzzle");
        muzzleObj.transform.parent = player.transform;
        muzzleObj.transform.localPosition = new Vector3(0, 0.5f, 0.6f);

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        player.AddComponent<Health>();
        
        Gun gun = player.AddComponent<Gun>();
        gun.SetMuzzlePoint(muzzleObj.transform);
        
        player.AddComponent<PlayerController>();

        foreach (Transform child in body.transform)
        {
            if (child.GetComponent<Collider>() != null)
                DestroyImmediate(child.GetComponent<Collider>());
        }
    }

    private static void CreateProjectilePrefab()
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.name = "Projectile";
        projectile.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        Collider col = projectile.GetComponent<Collider>();
        col.isTrigger = true;
        
        Renderer renderer = projectile.GetComponent<Renderer>();
        renderer.material.color = Color.yellow;
        
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        
        projectile.AddComponent<Projectile>();
        
        string prefabPath = "Assets/Prefabs/Projectile.prefab";
        PrefabUtility.SaveAsPrefabAsset(projectile, prefabPath);
        GameObject.DestroyImmediate(projectile);
        
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            Gun gun = playerObj.GetComponent<Gun>();
            if (gun != null)
                gun.SetProjectilePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
        }
    }

    private static void CreateEnemyPrefab()
    {
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cube);
        enemy.name = "Enemy";
        enemy.transform.localScale = new Vector3(1, 2, 1);
        
        Collider col = enemy.GetComponent<Collider>();
        col.isTrigger = true;
        
        Renderer renderer = enemy.GetComponent<Renderer>();
        renderer.material.color = Color.red;
        
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        enemy.AddComponent<EnemyAI>();
        
        string prefabPath = "Assets/Prefabs/Enemy.prefab";
        PrefabUtility.SaveAsPrefabAsset(enemy, prefabPath);
        GameObject.DestroyImmediate(enemy);
        
        GameObject spawnerObj = GameObject.Find("WaveSpawner");
        if (spawnerObj != null)
        {
            WaveSpawner spawner = spawnerObj.GetComponent<WaveSpawner>();
            if (spawner != null)
                spawner.SetEnemyPrefab(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
        }
    }

    private static void CreateBossPrefab()
    {
        GameObject boss = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boss.name = "Boss";
        boss.transform.localScale = new Vector3(3, 3, 3);
        
        Collider col = boss.GetComponent<Collider>();
        col.isTrigger = true;
        
        Renderer renderer = boss.GetComponent<Renderer>();
        renderer.material.color = new Color(1, 0.5f, 0);
        
        Rigidbody rb = boss.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        boss.AddComponent<FreezeableBoss>();
        
        string prefabPath = "Assets/Prefabs/Boss.prefab";
        PrefabUtility.SaveAsPrefabAsset(boss, prefabPath);
        GameObject.DestroyImmediate(boss);
        
        GameObject spawnerObj = GameObject.Find("WaveSpawner");
        if (spawnerObj != null)
        {
            WaveSpawner spawner = spawnerObj.GetComponent<WaveSpawner>();
            if (spawner != null)
                spawner.SetBossPrefab(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
        }
    }

    private static void CreateSpawner()
    {
        GameObject spawner = new GameObject("WaveSpawner");
        spawner.transform.position = new Vector3(0, 1, 50);
        
        WaveSpawner waveSpawnerComponent = spawner.AddComponent<WaveSpawner>();
        spawner.AddComponent<BoxCollider>();
    }

    private static void SetupCamera()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.transform.position = new Vector3(0, 8, -10);
            mainCam.transform.LookAt(new Vector3(0, 2, 25));
        }
    }

    private static void CreateCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.anchorMin = Vector2.zero;
        canvasRect.anchorMax = Vector2.one;
        canvasRect.offsetMin = Vector2.zero;
        canvasRect.offsetMax = Vector2.zero;

        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.parent = canvasObj.transform;
        RectTransform healthRect = healthTextObj.AddComponent<RectTransform>();
        healthRect.anchoredPosition = new Vector2(10, -10);
        healthRect.sizeDelta = new Vector2(200, 50);
        TextMeshProUGUI healthText = healthTextObj.AddComponent<TextMeshProUGUI>();
        healthText.text = "Health: 100";
        healthText.fontSize = 36;

        GameObject waveTextObj = new GameObject("WaveText");
        waveTextObj.transform.parent = canvasObj.transform;
        RectTransform waveRect = waveTextObj.AddComponent<RectTransform>();
        waveRect.anchoredPosition = new Vector2(10, -60);
        waveRect.sizeDelta = new Vector2(200, 50);
        TextMeshProUGUI waveText = waveTextObj.AddComponent<TextMeshProUGUI>();
        waveText.text = "Wave: 0";
        waveText.fontSize = 36;

        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.parent = canvasObj.transform;
        RectTransform scoreRect = scoreTextObj.AddComponent<RectTransform>();
        scoreRect.anchoredPosition = new Vector2(10, -110);
        scoreRect.sizeDelta = new Vector2(200, 50);
        TextMeshProUGUI scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "Score: 0";
        scoreText.fontSize = 36;

        SimpleGameUI gameUI = canvasObj.AddComponent<SimpleGameUI>();
    }
}
