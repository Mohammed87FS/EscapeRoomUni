namespace libs;

public class Player : GameObject
{
    private static Player instance = null;
    private GameObjectFactory gameObjectFactory;
    private int targetsLeft;
    private int health;
    private List<string> inventory;

    public int Health => health;

    public Map map = GameEngine.Instance.GetMap();

    private Player() : base()
    {
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;
        this.gameObjectFactory = GameEngine.Instance.gameObjectFactory as GameObjectFactory;
        this.targetsLeft = gameObjectFactory.AmountOfBoxes;
        this.health = 100;
        this.inventory = new List<string>();
    }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }

        set { }
    }

    public override void Move(int dx, int dy)
    {
        int goToX = PosX + dx;
        int goToY = PosY + dy;

        GameObject? PotentialObject = map.Get(goToY, goToX);

        if (PotentialObject.Type == GameObjectType.Obstacle) return;

        if (PotentialObject.Type == GameObjectType.Enemy)
        {
            TakeDamage(50);
            return;
        }


        if (gameObjectFactory.AmountOfBoxes == 11)
        {
            GameEngine.Instance.UpdateTargetColors();

            if (PotentialObject.Type == GameObjectType.Target && PotentialObject.Code == "0" && PotentialObject.Color == ConsoleColor.Green)
            {
                Console.WriteLine("You are a winner!!");
                Environment.Exit(0);
            }
        }

        if (PotentialObject.Type == GameObjectType.Box)
        {
            GameObject? NextObject = map.Get(goToY + dy, goToX + dx);

            if (NextObject.Type == GameObjectType.Obstacle || NextObject.Type == GameObjectType.Box) return;

            PotentialObject.Move(dx, dy);
            PotentialObject.Color = ConsoleColor.Red;
        }

        this.SetPrevPosY(this.PosY);
        this.SetPrevPosX(this.PosX);
        this.PosX += dx;
        this.PosY += dy;
    }

    private void TakeDamage(int amount)
    {
        health -= amount;
        Console.WriteLine($"Player health: {health}");
        if (health <= 0)
        {
            Console.WriteLine("You died!");
            Environment.Exit(0);
        }
    }


   
}
