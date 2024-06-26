using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace libs
{
    public sealed class GameEngine
    {
        private static GameEngine? _instance;
        private System.Timers.Timer gameTimer;
        private DateTime startTime;
        private const int GameDuration = 80000;
        private int lastLineCursor = 0;
        public GameObjectFactory gameObjectFactory;

        public GameState CurrentGameState { get; private set; } = GameState.Running;

        public static GameEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEngine();
                }
                return _instance;
            }
        }

        private GameEngine()
        {
            gameObjectFactory = new GameObjectFactory();
        }

        private GameObject? _focusedObject;
        private List<GameObject> gameObjects = new List<GameObject>();
        private Map map = new Map();

        private int currentLevel = 1;
        public Map GetMap()
        {
            return map;
        }

        public GameObject GetFocusedObject()
        {
            return _focusedObject;
        }

        private void SetupTimer()
        {
            gameTimer = new System.Timers.Timer(1000);
            gameTimer.Elapsed += OnTimerElapsed;
            gameTimer.AutoReset = true;
            gameTimer.Start();
            startTime = DateTime.Now;

            Console.WriteLine("Timer: 00:00");
            lastLineCursor = Console.CursorTop - 1;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan timeElapsed = DateTime.Now - startTime;
            int timeLeft = GameDuration - (int)timeElapsed.TotalMilliseconds;
            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                Console.SetCursorPosition(0, lastLineCursor);
                Console.WriteLine("Time's up! Game over!        ");
                Environment.Exit(0);
            }
            else
            {
                Console.SetCursorPosition(0, lastLineCursor);
                Console.WriteLine($"Time left: {timeLeft / 1000:D2}");
            }
        }

        public void Setup(bool hardMode)
        {
            SetupTimer();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            dynamic gameData = FileHandler.ReadJson();
            map.MapWidth = gameData.map.width;
            map.MapHeight = gameData.map.height;
            gameObjects.Clear();

            dynamic objectsToLoad = hardMode ? gameData.Hard.gameObjects : gameData.Easy.gameObjects;

            foreach (var gameObject in objectsToLoad)
            {
                AddGameObject(CreateGameObject(gameObject));
            }

            _focusedObject = gameObjects.OfType<Player>().First();

            Render();
        }

        public void Render()
        {
            Console.Clear();

            map.Initialize();
            PlaceGameObjects();

            for (int i = 0; i < map.MapHeight; i++)
            {
                for (int j = 0; j < map.MapWidth; j++)
                {
                    DrawObject(map.Get(i, j));
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('.', map.MapWidth));
            lastLineCursor = Console.CursorTop;
        }

        public GameObject CreateGameObject(dynamic obj)
        {
            return gameObjectFactory.CreateGameObject(obj);
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            if (gameObject.Type == GameObjectType.Box)
            {
                gameObjectFactory.IncrementAmountOfBoxes();
            }
        }

        private void PlaceGameObjects()
        {
            foreach (var obj in gameObjects)
            {
                map.Set(obj);
                if (obj is Box && map.Get(obj.PosY, obj.PosX).Type == GameObjectType.Target)
                {
                    gameObjectFactory.DecrementAmountOfBoxes();
                }
            }
        }

        public void UpdateTargetColors()
        {
            var targets = map.GetAllObjects()
                             .Where(obj => obj.Type == GameObjectType.Target && obj.Color == ConsoleColor.Black)
                             .ToList();

            targets.ForEach(target => target.Color = ConsoleColor.Green);
        }

        public void SaveGameState()
        {
            var gameState = new
            {
                CurrentLevel = currentLevel,
                PlayerPosition = new { _focusedObject.PosX, _focusedObject.PosY },
                Boxes = gameObjects.Where(go => go.Type == GameObjectType.Box).Select(box => new { box.PosX, box.PosY }),
                GameState = CurrentGameState.ToString()
            };

            FileHandler.SaveJson(gameState);
            Console.WriteLine("Game state saved!");
        }

        private void DrawObject(GameObject gameObject)
        {
            Console.ResetColor();
            if (gameObject != null)
            {
                Console.ForegroundColor = gameObject.Color;
                Console.Write(gameObject.CharRepresentation);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(' ');
            }
        }
    }
}
