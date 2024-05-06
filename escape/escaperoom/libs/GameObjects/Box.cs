namespace libs;

public class Box : GameObject
{

    private GameObjectFactory gameObjectFactory;
    private int targetsLeft;

    public Map map = GameEngine.Instance.GetMap();

    public Box() : base()
    {
        this.gameObjectFactory = GameEngine.Instance.gameObjectFactory as GameObjectFactory;
        this.targetsLeft = gameObjectFactory.AmountOfBoxes;
        Type = GameObjectType.Box; 

        this.Color = ConsoleColor.DarkCyan;
   
        
    }

    public override void Move(int dx, int dy)
    {
        int goToX = PosX + dx;
        int goToY = PosY + dy;

        GameObject? potentialTarget = map.Get(goToY, goToX);


        
        if (potentialTarget != null && potentialTarget.Type == GameObjectType.Target && potentialTarget.Code==Code)
        {

            //    for (int i = 0; i < 3000; i++)
            //     {
            //         Console.WriteLine( Code);
            //    }

            if (map.Get(PosY, PosX).Type != GameObjectType.Target)
            {
                gameObjectFactory.IncrementAmountOfBoxes();
       
      
     

//i need to keep rendering obje..
            }
        }
        else
        {

            if (map.Get(PosY, PosX).Type == GameObjectType.Target)
            {
                gameObjectFactory.DecrementAmountOfBoxes();


            }
        }


        base.Move(dx, dy);
    }
}
