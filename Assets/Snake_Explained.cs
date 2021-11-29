using System.Linq;
using UnityEngine;
public class Snake_Explained : MonoBehaviour {

    /* Loose rules
     *  
     *  - Only 1 class
     *  - Roughly 1 statement = 1 line
     *  - Performance least important    
     * 
     */

    // Define the snake and apple which are stored in GameObject arrays
    // The apple only ever has 1 item, but it is written as an array to 
    // save a line. Additionally, it makes it easy to implement multiple
    // apples at once if I wanted to.
    private GameObject[]snake,apple;

    // Create the timer, which later has 2 uses, primarily to define the FPS of the game
    private byte tmr=2;

    // The direction the snake should move in, and the direction the snake is moving in
    // (The direction the snake is moving, LEFT/RIGHT/UP/DOWN)
    private Direction direction,effectiveDirection;

    void Update() {

        /*
         * Check if the game is not started, and if so start the game
         * (by creating the snake, and the first apple).
         * This was initially done in the Start() method as would be expected,
         * but was moved here because it would save lines. It does
         * reduce performance, but not noticeably. Performance is sacrificed often
         * and not cared about, so I will not mention it again from this point on
         */ 
        if (snake==null) snake=new []{Instantiate(GameObject.Find("HeadSquare"),new Vector3(-1.5F,2.5F,1F),Quaternion.identity),Instantiate(GameObject.Find("BodySquare"),new Vector3(-0.5F,2.5F,1F),Quaternion.identity),Instantiate(GameObject.Find("BodySquare"),new Vector3(0.5F,2.5F,1F),Quaternion.identity) };
        if (apple==null) apple=new []{Instantiate(GameObject.Find("FoodSquare"),new Vector3(-2.5F,-1.5F,1F),Quaternion.identity)};

        // Take input from the user to decide which direction the snake goes in (up,down,left,right arrow keys)
        // It is actually pretty simple, basically, if the user presses the left arrow key, the snake goes left. Same for all other arrow keys
        // The only other thing the line does, is ensure the snake does not move inside itself (i.e it is moving right, and the user presses left arrow key)
        direction=Input.GetKeyDown(KeyCode.RightArrow)?
            (effectiveDirection!=Direction.LEFT?Direction.RIGHT:direction):
            Input.GetKeyDown(KeyCode.LeftArrow)?
            (effectiveDirection!=Direction.RIGHT?Direction.LEFT:direction):
            Input.GetKeyDown(KeyCode.UpArrow)?
            (effectiveDirection!=Direction.DOWN?Direction.UP:direction):
            Input.GetKeyDown(KeyCode.DownArrow)?
            (effectiveDirection!=Direction.UP?Direction.DOWN:direction):
            direction;

        // Reduce timer to 1 from 31 to slow down game.
        // It would be unplayable because the game runs too quick without this
        if (--tmr!=1) return;

        // Fixes an unlikely glitch where the snake can move inside itself if the user does a quick set of specific key presses
        effectiveDirection=direction; 

        // This code block simply moves the snake forward by one block (1 unit in unity) in whichever direction it is going
        // Notice it uses the previous variable "tmr" which was used for defining the FPS of the game. This is to save a line
        // as I now use the variable declaration statement to instead create some variables that help move the snake's body forward.
        for (Vector3 pPos=snake.First().transform.position,cPos;tmr!=snake.Length;++tmr) {
            // Move the current body square of the snake into where the next body square previously was
            // (Moves the body forward)
            cPos=pPos;
            pPos=snake[tmr].transform.position;
            snake[tmr].transform.position=cPos;
        }
        // Move the head forward 1 block in whichever direction the snake is facing
        // (Moves the head forward)
        snake.First().transform.Translate((direction==Direction.LEFT)?(snake.First().transform.position.x==-5.5F?9:-1):(direction==Direction.RIGHT)?(snake.First().transform.position.x==3.5F?-9:1):0,(direction==Direction.UP)?(snake.First().transform.position.y==4.5F?-9:1):(direction==Direction.DOWN)?(snake.First().transform.position.y==-4.5F?9:-1):0,0);

        // Check all body squares. If one body square is in the same position as the head of the snake (the snake ran into itself), then remove the snake and apple (end the game).
        if (snake.Where(x=>x.name=="BodySquare(Clone)").Select(x=>x.transform.position).Where(x=>x.Equals(snake.First().transform.position)).Count()!=0)
            snake.Concat(apple).ToList().ForEach(x=>Destroy(x));

        // If the snake's head is touching the apple
        // (If the player collected the apple)
        if (snake.First().transform.position==apple.First().transform.position) {

            // While the apple is not on on top of the snake
            // (This is written so the apple doesn't spawn ontop of the snake)
            while (snake.Select(x=>x.transform.position).Where(x=>x.Equals(apple.First().transform.position)).Count()!=0)
                // Spawn an apple randomly on the playing area
                apple.First().transform.position=new Vector3(((int)(Random.value*8-5)+0.5F),((int)(Random.value*9F-4F)+0.5F),1);

            // Add a new body square which will be attatched to the snake on the next frame (well, next loop)
            snake=snake.Concat(new []{Instantiate(GameObject.Find("BodySquare"),Vector3.one*10,Quaternion.identity) }).ToArray();

        }

        // Set the timer to slow down the game / set the FPS (as mentioned earlier)
        tmr=31;

    }
}

// Direction.LEFT == 0, Direction.RIGHT ==1, Direction.UP == 2, Direction.DOWN == 3
public enum Direction : byte { LEFT,RIGHT,UP,DOWN }