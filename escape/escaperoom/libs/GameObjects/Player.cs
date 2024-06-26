namespace libs;

public class Player : GameObject
{

    private static Player instance = null;
    private GameObjectFactory gameObjectFactory;
    private int targetsLeft;

    public Map map = GameEngine.Instance.GetMap();

    private Player() : base()
    {
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;
        this.gameObjectFactory = GameEngine.Instance.gameObjectFactory as GameObjectFactory;
        this.targetsLeft = gameObjectFactory.AmountOfBoxes;

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


        GameObject? PotentialBox = map.Get(goToY, goToX);


        if (PotentialBox.Type == GameObjectType.Obstacle) return;

        GameObject? PotentialDoor = map.Get(goToY, goToX);

        if (gameObjectFactory.AmountOfBoxes == 11)
        {

//here target changes to green 
            GameEngine.Instance.UpdateTargetColors();


            if (PotentialDoor.Type == GameObjectType.Target && PotentialDoor.Code == "0" && PotentialDoor.Color == ConsoleColor.Green)
            {


                Console.WriteLine("Yo are a winner!!");



                Environment.Exit(0);
            }

        }

        if (PotentialBox.Type == GameObjectType.Box)
        {
            GameObject? NextObject = map.Get(goToY + dy, goToX + dx);

            //    for (int i = 0; i < 3000; i++)
            //         {
            //             Console.WriteLine(PotentialBox.Code);
            //         }



            if (NextObject.Type == GameObjectType.Obstacle || NextObject.Type == GameObjectType.Box) return;

            PotentialBox.Move(dx, dy);
            PotentialBox.Color = ConsoleColor.Red;
        }

        this.SetPrevPosY(this.PosY);
        this.SetPrevPosX(this.PosX);
        this.PosX += dx;
        this.PosY += dy;
    }
}