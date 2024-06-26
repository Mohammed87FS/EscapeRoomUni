namespace libs;

public class Enemy : GameObject {

    public Enemy () : base(){
        Type = GameObjectType.Enemy;
        CharRepresentation = '=';
    }
}