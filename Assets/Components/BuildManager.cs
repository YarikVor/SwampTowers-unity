using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    public static GameObject BuildProcessPrefab { get; private set; }
    public static BuildManager Instance {get; private set;}
    public GameObject buildProcessPrefab;

    public Grid grid;
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;
    public GameObject tower;
    public ShopItem selectShopItem;


    public float buildTime = 1f;

    public Dictionary<Vector3Int, GameObject> poligon = new Dictionary<Vector3Int, GameObject>();

    public TowerInfoCollection towerInfoCollection;

    private TowerInfoProvider _towerInfoProvider;

    public Transform pointer;
    public Transform group;

    public Vector3Int towerPosition;
    public Transform RadiusObject;

    private void Awake()
    {
        BuildProcessPrefab = buildProcessPrefab;
        Instance = this;
    }

    void Start()
    {
        ImportCells();
        _towerInfoProvider = new TowerInfoProvider(towerInfoCollection);

        ShopManager.Instance.OnSelectShopItem += OnSelectShopItem;
    }

    void Destroy()
    {
        ShopManager.Instance.OnSelectShopItem -= OnSelectShopItem;
    }

    private void OnSelectShopItem(ShopItem shopItem)
    {
        selectShopItem = shopItem;
        tilemapRenderer.enabled = true;
    }

    private void ImportCells()
    {
        var collections = tilemap.cellBounds.allPositionsWithin;

        foreach (var vec3Int in collections)
        {
            if(tilemap.HasTile(vec3Int))
                poligon.Add(vec3Int, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = grid.WorldToCell(pos);
        var offsetPosition = ConvertGridPositionToGlobalPosition(cellPos);

        pointer.position = offsetPosition;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            if(IsTower(cellPos))
                towerPosition = cellPos;

            if (!BuyTower(cellPos))
                SetInfoTower(cellPos);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            Debug.Log(SellTower(cellPos));
        }

        if (Input.GetMouseButtonDown((int)MouseButton.Middle))
        {
            Debug.Log(UpdateTower(cellPos));
        }

        tower = ShopManager.Instance?.SelectedShopItem?.tower;
    }

    private Vector3 ConvertGridPositionToGlobalPosition(Vector3Int cellPosition)
    {
        var vectorCellPos = grid.CellToWorld(cellPosition);
        var offsetPosition = new Vector3(
            vectorCellPos.x,
            vectorCellPos.y + grid.cellSize.y / 2
        );

        return offsetPosition;
    }

    public bool SetInfoTower(Vector3Int cellPos)
    {
        //TowerInfoUI.Instance.gameObject.SetActive(false);

        if (!TryGetTower(cellPos, out GameObject gameTower))
            return false;

        Tower tower = GetTower(gameTower);

        var model = _towerInfoProvider.GetTowerModelViewer(tower);

        if (model == null)
            return false;

        TowerInfoUI.Instance.gameObject.SetActive(true);

        TowerInfoUI.Instance.SetModel(model);

        UpdateRadius(tower);

        return true;
    }

    private void UpdateRadius(Tower tower)
    {
        var radius = tower.Radius;

        RadiusObject.localScale = new Vector3(radius, radius / 2);

        RadiusObject.position = tower.transform.position;

        RadiusObject.gameObject.SetActive(true);
    }

    private static Tower GetTower(GameObject gameTower)
    {
        var tower = gameTower.GetComponentInChildren<Tower>();
        if (!tower)
            throw new Exception($"{gameTower.name} is not Tower");
        return tower;
    }

    public bool AddTower(Vector3Int cellPos)
    {
        return AddTower(cellPos, tower);
    }

    public bool AddTower(Vector3Int cellPos, GameObject prefab)
    {
        if (!poligon.TryGetValue(cellPos, out GameObject poligonGameObject) || poligonGameObject)
            return false;

        var offsetPosition = ConvertGridPositionToGlobalPosition(cellPos);

        var instantiate = InstantiateTower(prefab, offsetPosition);
        poligon[cellPos] = instantiate;

        return true;
    }

    private GameObject InstantiateTower(GameObject prefab, Vector3 offsetPosition)
    {
        var gameObject = Instantiate(prefab, offsetPosition, Quaternion.identity);
        gameObject.GetComponentInChildren<Tower>().prefab = prefab;

        var buildProcess = Instantiate(BuildProcessPrefab, offsetPosition, Quaternion.identity, group);

        buildProcess
            .GetComponent<BuildProcessComponent>()
            .Init(buildTime, gameObject);

        return gameObject;
    }

    public void SellTower() => SellTower(towerPosition);

    public bool SellTower(Vector3Int cellPos)
    {
        if (!TryGetTower(cellPos, out GameObject currentTower))
            return false;

        if (!currentTower.activeSelf)
            return false;

        Destroy(currentTower);

        Debug.Log("Sell tower");

        poligon[cellPos] = null;

        var info = _towerInfoProvider.GetTowerLevelInfoResult(currentTower);

        AddPrice(info.sellPrice);

        return true;
    }

    public bool BuyTower(Vector3Int cellPos)
    {
        tilemapRenderer.enabled = false;

        if (selectShopItem == null)
            return false;

        var price = selectShopItem.price;

        if (!HasPrice(price))
            return false;

        Debug.Log("Buy here - 1");

        buildTime = selectShopItem.buildTime;

        var tower = selectShopItem.tower;

        selectShopItem = null;

        if (!AddTower(cellPos, tower))
            return false;

        Debug.Log("Buy here - 2");

        SubPrice(price);

        return true;
    }

    public void UpdateTower() => UpdateTower(towerPosition);

    public bool UpdateTower(Vector3Int cellPos)
    {
        Debug.Log("Here");


        if (!poligon.TryGetValue(cellPos, out GameObject gameObject) || gameObject == null)
            return false;

        var result = _towerInfoProvider.GetTowerLevelInfoResult(gameObject);

        if (result == null || !result.HasNextLevel)
            return false;

        if (!TrySubPrice(result.updatePrice))
            return false;

        if (!SellTower(cellPos))
            return false;

        buildTime = result.buildTime;

        if (!AddTower(cellPos, result.nextLevelTower))
            return false;

        return true;
    }


    public bool TrySubPrice(int price)
    {
        var hasMoney = HasPrice(price);

        if (hasMoney)
            ShopManager.Instance.money -= price;

        return hasMoney;
    }

    public bool HasPrice(int price)
    {
        return ShopManager.Instance.money >= price;
    }

    public void SubPrice(int price)
    {
        ShopManager.Instance.money -= price;
    }    

    public void AddPrice(int price)
    {
        ShopManager.Instance.money += price;
    }

    public bool IsPlace(Vector3Int cellPos)
    {
        return poligon.ContainsKey(cellPos);
    }

    public bool IsFreePlace(Vector3Int cellPos)
    {
        return poligon.TryGetValue(cellPos, out GameObject tower) && tower == null;
    }

    public bool IsTower(Vector3Int cellPos)
    {
        return poligon.TryGetValue(cellPos, out GameObject tower) && tower != null;
    }

    public bool TryGetTower(Vector3Int cellPos, out GameObject tower)
    {
        return poligon.TryGetValue(cellPos, out tower) && tower != null;
    }

    public bool RemoveTower(Vector3Int cellPos)
    {
        if(!poligon.TryGetValue(cellPos, out GameObject tower))
            return false;

        poligon[cellPos] = null;

        Destroy(tower);

        return true;
    }


}