namespace libs;
using Newtonsoft.Json;
using System.Collections.Generic; 

public class Map {
    private char[,] RepresentationalLayer;
    private GameObject?[,] GameObjectLayer;

    private int _mapWidth;
    private int _mapHeight;

    public Map() {
        _mapWidth = 15;
        _mapHeight = 15;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    public Map(int width, int height) {
        _mapWidth = width;
        _mapHeight = height;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    public void Initialize() {
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];

        for (int i = 0; i < GameObjectLayer.GetLength(0); i++) {
            for (int j = 0; j < GameObjectLayer.GetLength(1); j++) {
                GameObjectLayer[i, j] = new Floor(); 
            }
        }
    }

    public int MapWidth {
        get { return _mapWidth; }
        set { _mapWidth = value; Initialize(); }
    }

    public int MapHeight {
        get { return _mapHeight; }
        set { _mapHeight = value; Initialize(); }
    }

    public GameObject Get(int x, int y) {
        return GameObjectLayer[x, y];
    }

    public void Set(GameObject gameObject) {
        GameObjectType type = gameObject.Type;
        int posY = gameObject.PosY;
        int posX = gameObject.PosX;
        int prevPosY = gameObject.GetPrevPosY();
        int prevPosX = gameObject.GetPrevPosX();

        if (prevPosX >= 0 && prevPosX < _mapWidth &&
            prevPosY >= 0 && prevPosY < _mapHeight) {
            GameObjectLayer[prevPosY, prevPosX] = new Floor();
        }

        if (posX >= 0 && posX < _mapWidth &&
            posY >= 0 && posY < _mapHeight) {
            GameObjectLayer[posY, posX] = gameObject;
            RepresentationalLayer[posY, posX] = gameObject.CharRepresentation;
        }
    }

    public List<GameObject> GetAllObjects() {
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < _mapHeight; i++) {
            for (int j = 0; j < _mapWidth; j++) {
                if (GameObjectLayer[i, j] != null) {
                    objects.Add(GameObjectLayer[i, j]);
                }
            }
        }
        return objects;
    }
}
