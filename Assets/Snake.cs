using System.Linq;
using UnityEngine;
public class Snake : MonoBehaviour {
    private GameObject[]snake,apple;
    private byte direction=0,tmr=2,effectiveDirection=0; // Direction.LEFT == 0, Direction.RIGHT ==1, Direction.UP == 2, Direction.DOWN == 3
    void Update() {
        if (snake==null) snake=new []{Instantiate(GameObject.Find("HeadSquare"),new Vector3(-1.5F,2.5F,1F),Quaternion.identity),Instantiate(GameObject.Find("BodySquare"),new Vector3(-0.5F,2.5F,1F),Quaternion.identity),Instantiate(GameObject.Find("BodySquare"),new Vector3(0.5F,2.5F,1F),Quaternion.identity) };
        if (apple==null) apple=new []{Instantiate(GameObject.Find("FoodSquare"),new Vector3(-2.5F,-1.5F,1F),Quaternion.identity)};
        direction=(byte)(Input.GetKeyDown(KeyCode.RightArrow)?(effectiveDirection!=0?1:direction):Input.GetKeyDown(KeyCode.LeftArrow)?(effectiveDirection!=1?0:direction):Input.GetKeyDown(KeyCode.UpArrow)?(effectiveDirection!=3?2:direction):Input.GetKeyDown(KeyCode.DownArrow)?(effectiveDirection!=2?3:direction):direction);
        if (--tmr!=1) return;
        effectiveDirection=direction; // fixes a very minor glitch that doesn't even break the game and is hard to do. Only realised it because it made sense logically, not because I came across it while playing
        for (Vector3 pPos=snake.First().transform.position,cPos;tmr!=snake.Length;++tmr) {
            cPos=pPos;
            pPos=snake[tmr].transform.position;
            snake[tmr].transform.position=cPos;
        }
        snake.First().transform.Translate((direction==0)?(snake.First().transform.position.x==-5.5F?9:-1):(direction==1)?(snake.First().transform.position.x==3.5F?-9:1):0,(direction==2)?(snake.First().transform.position.y==4.5F?-9:1):(direction==3)?(snake.First().transform.position.y==-4.5F?9:-1):0,0);
        if (snake.Where(x=>x.name=="BodySquare(Clone)").Select(x=>x.transform.position).Where(x=>x.Equals(snake.First().transform.position)).Count()!=0)
            snake.Concat(apple).ToList().ForEach(x=>Destroy(x));
        if (snake.First().transform.position==apple.First().transform.position) {
            while (snake.Select(x=>x.transform.position).Where(x=>x.Equals(apple.First().transform.position)).Count()!=0)
                apple.First().transform.position=new Vector3(((int)(Random.value*8-5)+0.5F),((int)(Random.value*9F-4F)+0.5F),1);
            snake=snake.Concat(new []{Instantiate(GameObject.Find("BodySquare"),Vector3.one*10,Quaternion.identity) }).ToArray();
        }
        tmr=31;
    }
}