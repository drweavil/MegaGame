using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LevelGeneration : MonoBehaviour {
	public int height;
	public int width;
	private int[,] level;
	//private GridController grid;

	private const int left = 0, right = 1, down = 2, up = 3;


	// Use this for initialization
	void Start () {
		//grid = GameObject.Find ("GridController").GetComponent<GridController>();
		//int currentPoint;
		//currentPoint = level [0, Random.Range (0, 10)];
		//PartType (new Vector3 (0, 0, 0), 11);	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown (KeyCode.Space)) {
			//grid.Refresh ();
			CreateLevelPath ();
			//level [1, 2] = -1;
			//RefreshArray ();
			//SetValue (new int[] {1,10}, left);
			//CreateLevelPath();
			//PartType (new Vector3 (0, 0 * -1, 0), 15);
			//Test ();
			for (int i = 0; i < height+2; i++) {
				for (int j = 0; j < width+2; j++) {
					PartType (new Vector3 (j, i * -1, 0), level[i,j]);
				}
			}
			//Debug.Log (IsExit());

			int[] lol = new int[2];
			lol [0] = 1;
			lol [1] = 2;
		}
	*/
	}

	int PathDirectionLeftRightDown(int leftPercent, int rightPercent, int downPercent){
		int firstLine = leftPercent;
		int secondLine = leftPercent + rightPercent;
		int lastLine = leftPercent + rightPercent + downPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);
		if (random < firstLine) {
			pathDirection = left; 
		} 

		if (random < secondLine && random >= firstLine) {
			pathDirection = right; 
		}

		if (random <= lastLine && random >= secondLine) {
			pathDirection = down; 
		}
		return pathDirection;
	}

	int PathDirectionLeftUpDown(int leftPercent, int upPercent, int downPercent){
		int firstLine = leftPercent;
		int secondLine = leftPercent + upPercent;
		int lastLine = leftPercent + upPercent + downPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);
		if (random < firstLine) {
			pathDirection = left; 
		} 

		if (random < secondLine && random >= firstLine) {
			pathDirection = up; 
		}

		if (random <= lastLine && random >= secondLine) {
			pathDirection = down;
		}
		return pathDirection;
	}

	int PathDirectionRightUpDown(int rightPercent, int upPercent, int downPercent){
		int firstLine = rightPercent;
		int secondLine = rightPercent + upPercent;
		int lastLine = rightPercent + upPercent + downPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);
		if (random < firstLine) {
			pathDirection = right; 
		} 

		if (random < secondLine && random >= firstLine) {
			pathDirection = up; 
		}

		if (random <= lastLine && random >= secondLine) {
			pathDirection = down;
		}
		return pathDirection;
	}

	int PathDirectionRightDown(int rightPercent, int downPercent){
		int firstLine = rightPercent;
		int lastLine = rightPercent + downPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);

		if (random < firstLine) {
			pathDirection = right; 
		}

		if (random <= lastLine && random >= firstLine) {
			pathDirection = down; 
		}
		return pathDirection;
	}

	int PathDirectionLeftDown(int leftPercent, int downPercent){
		int firstLine = leftPercent;
		int lastLine = leftPercent + downPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);

		if (random < firstLine) {
			pathDirection = left; 
		}

		if (random <= lastLine && random >= firstLine) {
			pathDirection = down; 
		}
		return pathDirection;
	}

	int PathDirectionLeftUp(int leftPercent, int upPercent){
		int firstLine = leftPercent;
		int lastLine = leftPercent + upPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);

		if (random < firstLine) {
			pathDirection = left; 
		}

		if (random <= lastLine && random >= firstLine) {
			pathDirection = up; 
		}
		return pathDirection;
	}

	int PathDirectionRightUp(int rightPercent, int upPercent){
		int firstLine = rightPercent;
		int lastLine = rightPercent + upPercent;
		int pathDirection = -1;
		int random = Random.Range(0, lastLine);

		if (random < firstLine) {
			pathDirection = right; 
		}

		if (random <= lastLine && random >= firstLine) {
			pathDirection = up; 
		}
		return pathDirection;
	}

		

	int[] GetNextPoint(int [] currentPoint, int direction){
		int[] nextPoint = new int[] {currentPoint[0], currentPoint[1]};
		switch(direction){
		case left: //left
			nextPoint [1] = currentPoint [1] - 1;
			break;
		case right://right
			nextPoint [1] = currentPoint [1] + 1;
			break;			
		case down://down
			nextPoint [0] = currentPoint [0] + 1;
			break;
		case up://up
			nextPoint [0] = currentPoint [0] - 1;
			break;
		}
		return nextPoint;
	}
		

	void RefreshArray(){
		for (int i = 0; i < height+2; i++) {
			for (int j = 0; j < width+2; j++) {
				if(j == 0 || j == (width+1) || i == 0 || i == (height+1)){
					level [i, j] = -1;
				}else{
					level [i, j] = 0;
				}
			}
		}
	}

	int Randomize(int[] array){
		int rand = Random.Range (0, array.Length);
		return array [rand];
	}

	public int[,] CreateLevelPath(int setHeight, int setWidth){
		height = setHeight;
		width = setWidth;
		level = new int[height+2, width+2];

		RefreshArray ();
		int[] startPoint = new int[2];
		startPoint [0] = 1;
		startPoint [1] = Random.Range(1, width+1);
		//Debug.Log (startPoint [0].ToString () + "; " + startPoint [1].ToString ());
		SetValue (startPoint, up);
		ClearLevel ();
		if (CheckPortalsCount() < 4) {
		 	level = CreateLevelPath (setHeight, setWidth);
		}
		return level;
	}

	public int CheckPortalsCount(){
		int portalsCount = 0;
		for (int i = 1; i <= height; i++) {
			for (int j = 1; j <= width; j++) {
				if (level [i, j] == 12 || 
					level [i, j] == 13 ||
					level [i, j] == 14 ||
					level [i, j] == 15) {
					portalsCount++;
				}
			}
		}
		return portalsCount;
	}

	public void ClearLevel(){
		for (int i = 1; i <= height; i++) {
			for (int j = 1; j <= width; j++) {
				if (level [i, j] != 0 && level [i, j] != -1 && level [i, j] != -2) {
					CheckNeighbors (new int[2]{ i, j });
				}
			}
		}

		for (int i = 0; i < height+2; i++) {
			for (int j = 0; j < width+2; j++) {
				if (level [i, j] == 0 || level [i, j] == -1) {
					level [i, j] = -3;
				}

				if (level [i, j] == -2) {
					level [i, j] = 0;
				}
			}
		}
	}

	public void CheckNeighbors(int[] coord){
		//1
		CheckCoord(new int[]{coord[0]-1, coord[1]-1});
		//2
		CheckCoord(new int[]{coord[0]-1, coord[1]});
		//3
		CheckCoord(new int[]{coord[0]-1, coord[1]+1});
		//4
		CheckCoord(new int[]{coord[0], coord[1]+1});
		//5
		CheckCoord(new int[]{coord[0]+1, coord[1]+1});
		//6
		CheckCoord(new int[]{coord[0]+1, coord[1]});
		//7
		CheckCoord(new int[]{coord[0]+1, coord[1]-1});
		//8
		CheckCoord(new int[]{coord[0], coord[1]-1});
	}

	public void CheckCoord(int[] coord){
		if(level [coord [0], coord [1]] == 0 || 
			level [coord [0], coord [1]] == -1){
			level [coord [0], coord [1]] = -2;
		}
	}

	bool IsExit(){
		bool isExit = false;
		int rand = Random.Range (0, 2);
		switch(rand){
		case 0:
			isExit = false;
			break;
		case 1:
			isExit = true;
			break;
		}
		return isExit;
	}



	void SetValueLeft(int[] point, int fromDirection){
		//1
		if (level [point [0], point [1] - 1] == 0 &&
			//level [point [0], point [1] + 1] != 0 && level [point [0], point [1] + 1] != -1 &&
			level [point [0]+1, point [1]] == 0 &&
			level [point [0]-1, point [1]] == 0 ) {
			int direction = PathDirectionLeftUpDown (33, 33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 1; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(direction){
			case left:
				level [point [0], point [1]] = Randomize(new int[] {1,2,3,4});
				SetValueLeft (nextPoint, right);
				return;
				break;			
			case down:
				level [point [0], point [1]] = Randomize(new int[] {3,4,5,8});
				SetValueLeft (nextPoint, up);
				return;
				break;
			case up:
				level [point [0], point [1]] = Randomize(new int[] {2,4,5,10});
				SetValueLeft (nextPoint, down);
				return;
				break;
			}

		}

		//2
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0]+1, point [1]] == 0 &&
			level [point [0]-1, point [1]] != 0) {
			int direction = PathDirectionLeftDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 2; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(fromDirection){
			//if (fromDirection == up) {
			case up:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 6, 11 });
					SetValueLeft (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4, 5, 6, 7 });
					SetValueLeft (nextPoint, up);
					return;
					break;
				}
				break;
			//} 
			//if(fromDirection == right){
			case right:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
					SetValueLeft (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 5, 8 });
					SetValueLeft (nextPoint, up);
					return;
					break;
				}
				break;
			}

		}

		//3
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0]+1, point [1]] != 0 &&
			level [point [0]-1, point [1]] == 0 ) {
			int direction = PathDirectionLeftUp (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 3; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			//if (fromDirection == right) {
			switch(fromDirection){
			case right:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
					SetValueLeft (nextPoint, right);
					return;
					break;			
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 5, 10 });
					SetValueLeft (nextPoint, down);
					return;
					break;
				}
				break;
			//}

			//if (fromDirection == down) {
			case down:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 6, 9 });
					SetValueLeft (nextPoint, right);
					return;
					break;			
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 4, 5, 6, 7 });
					SetValueLeft (nextPoint, down);
					return;
					break;
				}
				break;
			}
		}

		//4
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0]+1, point [1]] != 0 &&
			level [point [0]-1, point [1]] != 0 ) {
			int[] nextPoint = GetNextPoint (point, left);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 4; Dir:" + left.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			//if (fromDirection == up) {
			switch(fromDirection){
			case up:
				level [point [0], point [1]] = Randomize (new int[] { 2, 4, 6, 11 });
				SetValueLeft (nextPoint, right);
				return;
				break;
			//}

			//if (fromDirection == right) {
			case right:
				level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
				SetValueLeft (nextPoint, right);
				return;
				break;
			//}

			//if (fromDirection == down) {
			case down:
				level [point [0], point [1]] = Randomize (new int[] { 3, 4, 6, 9 });
				SetValueLeft (nextPoint, right);
				return;
				break;
			}
		}

		//5
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0]+1, point [1]] == 0 &&
			level [point [0]-1, point [1]] != 0 ) {
			int direction = PathDirectionLeftDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 5; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch (fromDirection) {
			case up:
				switch (direction) {
				case left:
					//SetValueLeft (nextPoint, right);
					if (IsExit ()) {
						level [point [0], point [1]] = 15;
					} else {
						level [point [0], point [1]] = Randomize (new int[] { 2,4,5,6,7,10,11});	
					}
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
					SetValueLeft (nextPoint, up);
					return;
					break;
				}
				break;
			case right:
				switch (direction) {
				case left:
					if (IsExit ()) {
						level [point [0], point [1]] = 13;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,5,8,10});	
					}
					//SetValueLeft (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3,4,5,8 });
					SetValueLeft (nextPoint, up);
					return;
					break;
				}
				break;
			}

		}

		//6

		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0]+1, point [1]] != 0 &&
			level [point [0]-1, point [1]] == 0 ) {
			int direction = PathDirectionLeftUp (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 6; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(fromDirection){
			case down:
				switch (direction) {
				case left:
					if (IsExit ()) {
						level [point [0], point [1]] = 14;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 3,4,5,6,7,8,9});	
					}
					//SetValueLeft (nextPoint, right);
					return;
					break;			
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
					SetValueLeft (nextPoint, down);
					return;
					break;
				}
				break;
			case right:
				switch (direction) {
				case left:
					if (IsExit ()) {
						level [point [0], point [1]] = 13;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,5,8,10});	
					}
					//SetValueLeft (nextPoint, right);
					return;
					break;			
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 2,4,5,10 });
					SetValueLeft (nextPoint, down);
					return;
					break;
				}
				break;
			}

		}

		//7
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0]+1, point [1]] != 0 &&
			level [point [0]-1, point [1]] != 0 ) {
			int direction = PathDirectionLeftUpDown (33, 33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 7; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(fromDirection){
			case right:
				if (IsExit ()) {
					level [point [0], point [1]] = 13;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,5,8,10});	
				}
				//SetValueLeft (nextPoint, right);
				return;
				break;			
			case down:
				if (IsExit ()) {
					level [point [0], point [1]] = 14;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 3,4,5,6,7,8,9});	
				}
				//SetValueLeft (nextPoint, right);
				return;
				break;
			case up:
				if (IsExit ()) {
					level [point [0], point [1]] = 15;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 2,4,5,6,7,10,11});	
				}
				//SetValueLeft (nextPoint, right);
				return;
				break;
			}

		}

		//8
		if (level [point [0], point [1] - 1] != 0 &&
			//level [point [0], point [1] + 1] != 0 && level [point [0], point [1] + 1] != -1 &&
			level [point [0]+1, point [1]] == 0 &&
			level [point [0]-1, point [1]] == 0 ) {
			int direction = PathDirectionLeftUpDown (33, 33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 1; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(direction){
			case left:
				if (IsExit ()) {
					level [point [0], point [1]] = 13;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,5,8,10});	
				}
				//SetValueLeft (nextPoint, right);
				return;
				break;			
			case down:
				level [point [0], point [1]] = Randomize(new int[] {3,4,5,8});
				SetValueLeft (nextPoint, up);
				return;
				break;
			case up:
				level [point [0], point [1]] = Randomize(new int[] {2,4,5,10});
				SetValueLeft (nextPoint, down);
				return;
				break;
			}

		}

	}

	void SetValueRight(int[] point, int fromDirection){
		//1
		if (//level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0]+1, point [1]] == 0 &&
			level [point [0]-1, point [1]] == 0 ) {
			int direction = PathDirectionRightUpDown (33, 33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 1; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(direction){
			case right:
				level [point [0], point [1]] = Randomize(new int[] {1,2,3,4});
				SetValueRight (nextPoint, left);
				return;
				break;			
			case down:
				level [point [0], point [1]] = Randomize(new int[] {3,4,6,9});
				SetValueRight (nextPoint, up);
				return;
				break;
			case up:
				level [point [0], point [1]] = Randomize(new int[] {2,4,6,11});
				SetValueRight (nextPoint, down);
				return;
				break;
			}

		}

		//2
		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] == 0 &&
			level [point [0] - 1, point [1]] != 0) {
			int direction = PathDirectionRightDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
					SetValueRight (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 6, 9 });
					SetValueRight (nextPoint, up);
					return;
					break;
				}
				break;
			case up:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 5, 10 });
					SetValueRight (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4, 5, 6, 7 });
					SetValueRight (nextPoint, up);
					return;
					break;
				}
				break;
			}
		}


		//3
		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] != 0 &&
			level [point [0] - 1, point [1]] == 0) {
			int direction = PathDirectionRightUp (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 6, 11 });
					SetValueRight (nextPoint, down);
					return;
					break;			
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
					SetValueRight (nextPoint, left);
					return;
					break;
				}
				break;
			case down:
				switch (direction) {
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 4, 5, 6, 7 });
					SetValueRight (nextPoint, down);
					return;
					break;			
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 5, 8 });
					SetValueRight (nextPoint, left);
					return;
					break;
				}
				break;

			}
		}

		//4
		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] != 0 &&
			level [point [0] - 1, point [1]] != 0) {
			int direction = right;
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
				SetValueRight (nextPoint, left);
				return;
				break;
			case down:
				level [point [0], point [1]] = Randomize (new int[] { 3,4,5,8 });
				SetValueRight (nextPoint, left);
				return;
				break;
			case up:
				level [point [0], point [1]] = Randomize (new int[] { 2,4,5,10 });
				SetValueRight (nextPoint, left);
				return;
				break;

			}
		}

		//5

		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] == 0 &&
			level [point [0] - 1, point [1]] != 0) {
			int direction = PathDirectionRightDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					if (IsExit ()) {
						level [point [0], point [1]] = 12;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,6,9,11});	
					}
					//SetValueRight (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3,4,6,9 });
					SetValueRight (nextPoint, up);
					return;
					break;
				}
				break;
			case up:
				switch (direction) {
				case right:
					if (IsExit ()) {
						level [point [0], point [1]] = 15;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 2,4,5,6,7,10,11});	
					}
					//SetValueRight (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
					SetValueRight (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//6
		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] != 0 &&
			level [point [0] - 1, point [1]] == 0) {
			int direction = PathDirectionRightUp (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					if (IsExit ()) {
						level [point [0], point [1]] = 12;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,6,9,11});	
					}
					//SetValueRight (nextPoint, right);
					return;
					break;			
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 2,4,6,11 });
					SetValueRight (nextPoint, down);
					return;
					break;
				}
				break;
			case down:
				switch (direction) {
				case right:
					if (IsExit ()) {
						level [point [0], point [1]] = 14;
					}else {
						level [point [0], point [1]] = Randomize (new int[] { 3,4,5,6,7,8,9});	
					}
					//SetValueRight (nextPoint, right);
					return;
					break;			
				case up:
					level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
					SetValueRight (nextPoint, down);
					return;
					break;
				}
				break;

			}
		}

		//7
		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] != 0 &&
			level [point [0] - 1, point [1]] != 0) {
			int direction = right;
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				if (IsExit ()) {
					level [point [0], point [1]] = 12;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,6,9,11});	
				}
				//SetValueRight (nextPoint, right);
				return;
				break;
			case down:
				if (IsExit ()) {
					level [point [0], point [1]] = 14;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 3,4,5,6,7,8,9});	
				}
				//SetValueRight (nextPoint, up);
				return;
				break;
			case up:
				if (IsExit ()) {
					level [point [0], point [1]] = 15;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 2,4,5,6,7,10,11});	
				}
				//SetValueRight (nextPoint, down);
				return;
				break;

			}
		}

		//8
		if (//level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] == 0 &&
			level [point [0] - 1, point [1]] == 0) {
			int direction = PathDirectionRightUpDown(33,33,33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 1; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (direction) {
			case up:
				level [point [0], point [1]] = Randomize (new int[] { 2,4,6,11 });
				SetValueRight (nextPoint, down);
				return;
				break;
			case down:
				level [point [0], point [1]] =  Randomize (new int[] { 3,4,6,9 });
				SetValueRight (nextPoint, up);
				return;
				break;
			case right:
				if (IsExit ()) {
					level [point [0], point [1]] = 12;
				}else {
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4,6,9,11});	
				}
				//SetValueRight (nextPoint, down);
				return;
				break;

			}
		}
	}

	void SetValueDown(int[] point, int fromDirection){
		//1
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0]+1, point [1]] == 0 
			//level [point [0]-1, point [1]] == 0 
		) {
			int direction = PathDirectionLeftRightDown (33, 33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 1; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(direction){
			case right:
				level [point [0], point [1]] = Randomize(new int[] {2,4,5,10});
				SetValueDown (nextPoint, left);
				return;
				break;			
			case down:
				level [point [0], point [1]] = Randomize(new int[] {4,5,6,7});
				SetValueDown (nextPoint, up);
				return;
				break;
			case left:
				level [point [0], point [1]] = Randomize(new int[] {2,4,6,11});
				SetValueDown (nextPoint, right);
				return;
				break;
			}

		}

		//2
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] == 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = PathDirectionRightDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 2; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
					SetValueDown (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 6, 9 });
					SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;
			case up:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 5, 10 });
					SetValueDown (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4, 5, 6, 7 });
					SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;
			}
		}


		//3
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] == 0 
			//level [point [0] - 1, point [1]] == 0
		) {
			int direction = PathDirectionLeftDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 3; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case up:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 2,4,6,11 });
					SetValueDown (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
					SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;
			case right:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
					SetValueDown (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 5, 8 });
					SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//4
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] == 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = down;
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 4; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				level [point [0], point [1]] = Randomize (new int[] { 3,4,6,9 });
				SetValueDown (nextPoint, up);
				return;
				break;
			case right:
				level [point [0], point [1]] = Randomize (new int[] { 3,4,5,8 });
				SetValueDown (nextPoint, up);
				return;
				break;
			case up:
				level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
				SetValueDown (nextPoint, up);
				return;
				break;

			}
		}

		//5

		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = PathDirectionLeftDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 5; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case up:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 6, 11 });
					SetValueDown (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = -1;//Randomize (new int[] { 3,4,6,9 });
					//SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;
			case right:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
					SetValueDown (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = -1;//Randomize (new int[] { 4,5,6,7 });
					//SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//6
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] == 0
		) {
			int direction = PathDirectionRightDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 6; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
					SetValueDown (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = -1;//Randomize (new int[] { 2,4,6,11 });
					//SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;
			case up:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 2,4,5,10 });
					SetValueDown (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = -1;//Randomize (new int[] { 4,5,6,7 });
					//SetValueDown (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//7
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = down;
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 7; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				level [point [0], point [1]] = -1;//Randomize (new int[] { 1,2,3,4 });
				//SetValueDown (nextPoint, right);
				return;
				break;
			case down:
				level [point [0], point [1]] = -1;// Randomize (new int[] { 3,4,5,8 });
				//SetValueDown (nextPoint, up);
				return;
				break;
			case right:
				level [point [0], point [1]] = -1;//Randomize (new int[] { 2,4,5,10 });
				//SetValueDown (nextPoint, down);
				return;
				break;

			}
		}

		//8
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] == 0
		) {
			int direction = PathDirectionLeftRightDown(33,33,33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 8; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (direction) {
			case left:
				level [point [0], point [1]] = Randomize (new int[] { 2,4,6,11 });
				SetValueDown (nextPoint, right);
				return;
				break;
			case down:
				level [point [0], point [1]] = -1;// Randomize (new int[] { 3,4,6,9 });
				//SetValueDown (nextPoint, up);
				return;
				break;
			case right:
				level [point [0], point [1]] = Randomize (new int[] { 2,4,5,10 });
				SetValueDown (nextPoint, left);
				return;
				break;

			}
		}
	}


	void SetValue(int[] point, int fromDirection){
		//0
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0]+1, point [1]] == 0 &&
			level [point [0]-1, point [1]] == -1 
		) {
			int direction = PathDirectionLeftRightDown (33, 33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 1; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
			switch(direction){
			case right:
				level [point [0], point [1]] = 13;
				SetValue (nextPoint, left);
				return;
				break;			
			case down:
				level [point [0], point [1]] = 14;
				SetValue (nextPoint, up);
				return;
				break;
			case left:
				level [point [0], point [1]] = 12;
				SetValue (nextPoint, right);
				return;
				break;
			}
		}

		//1
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0]+1, point [1]] == 0 
			//level [point [0]-1, point [1]] == 0 
		) {
			int rand = Random.Range (1, 101);
			if(rand > 70){
				int leftOrRightOrBoth = Random.Range (0, 3);
				switch (leftOrRightOrBoth) {
				case 0: 
					SetValueLeft (GetNextPoint (point, left), right);
					level [point [0], point [1]] = Randomize (new int[] { 4,6 });
					SetValue (GetNextPoint (point, down), up);
					return;
					break;
				case 1: 
					SetValueRight (GetNextPoint (point, right), left);
					level [point [0], point [1]] = Randomize (new int[] { 4,5 });
					SetValue (GetNextPoint (point, down), up);
					return;
					break;
				case 2: 
					SetValueRight (GetNextPoint (point, right), left);
					SetValueLeft (GetNextPoint (point, left), right);
					level [point [0], point [1]] = Randomize (new int[] { 4 });
					SetValue (GetNextPoint (point, left), right);
					return;
					break;
				}
			}else{
				int direction = PathDirectionLeftRightDown (33, 33, 33);
				int[] nextPoint = GetNextPoint (point, direction);
				//Debug.Log ("Cur: ["+ point[0].ToString() + "," + point[1].ToString() + "]; Alg: 1; Dir:" + direction.ToString() + "; Next: [" + nextPoint[0].ToString() + "," + nextPoint[1].ToString() + "]");
				switch(direction){
				case right:
					level [point [0], point [1]] = Randomize(new int[] {2,4,5,10});
					SetValue (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize(new int[] {4,5,6,7});
					SetValue (nextPoint, up);
					return;
					break;
				case left:
					level [point [0], point [1]] = Randomize(new int[] {2,4,6,11});
					SetValue (nextPoint, right);
					return;
					break;
				}
			}

		}

		//2
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] == 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = PathDirectionRightDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 2; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 1, 2, 3, 4 });
					SetValue (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 6, 9 });
					SetValue (nextPoint, up);
					return;
					break;
				}
				break;
			case up:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 5, 10 });
					SetValue (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4, 5, 6, 7 });
					SetValue (nextPoint, up);
					return;
					break;
				}
				break;
			}
		}


		//3
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] == 0 
			//level [point [0] - 1, point [1]] == 0
		) {
			int direction = PathDirectionLeftDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 3; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case up:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 2,4,6,11 });
					SetValue (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
					SetValue (nextPoint, up);
					return;
					break;
				}
				break;
			case right:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
					SetValue (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = Randomize (new int[] { 3, 4, 5, 8 });
					SetValue (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//4
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] == 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = down;
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 4; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				level [point [0], point [1]] = Randomize (new int[] { 3,4,6,9 });
				SetValue (nextPoint, up);
				return;
				break;
			case right:
				level [point [0], point [1]] = Randomize (new int[] { 3,4,5,8 });
				SetValue (nextPoint, up);
				return;
				break;
			case up:
				level [point [0], point [1]] = Randomize (new int[] { 4,5,6,7 });
				SetValue (nextPoint, up);
				return;
				break;

			}
		}

		//5

		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = PathDirectionLeftDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 5; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case up:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 2, 4, 6, 11 });
					SetValue (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = 15;
					//SetValue (nextPoint, up);
					return;
					break;
				}
				break;
			case right:
				switch (direction) {
				case left:
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
					SetValue (nextPoint, right);
					return;
					break;			
				case down:
					level [point [0], point [1]] = 13;
					//SetValue (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//6
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] == 0
		) {
			int direction = PathDirectionRightDown (33, 33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 6; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 1,2,3,4 });
					SetValue (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = 12;
					//SetValue (nextPoint, up);
					return;
					break;
				}
				break;
			case up:
				switch (direction) {
				case right:
					level [point [0], point [1]] = Randomize (new int[] { 2,4,5,10 });
					SetValue (nextPoint, left);
					return;
					break;			
				case down:
					level [point [0], point [1]] = 15;
					//SetValue (nextPoint, up);
					return;
					break;
				}
				break;

			}
		}

		//7
		if (level [point [0], point [1] - 1] != 0 &&
			level [point [0], point [1] + 1] != 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] != 0
		) {
			int direction = down;
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 7; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (fromDirection) {
			case left:
				level [point [0], point [1]] = 12;
				//SetValue (nextPoint, right);
				return;
				break;
			case up:
				level [point [0], point [1]] = 15;
				//SetValue (nextPoint, up);
				return;
				break;
			case right:
				level [point [0], point [1]] = 13;
				//SetValue (nextPoint, down);
				return;
				break;

			}
		}

		//8
		if (level [point [0], point [1] - 1] == 0 &&
			level [point [0], point [1] + 1] == 0 &&
			level [point [0] + 1, point [1]] != 0 
			//level [point [0] - 1, point [1]] == 0
		) {
			int direction = PathDirectionLeftRightDown(33,33,33);
			int[] nextPoint = GetNextPoint (point, direction);
			//Debug.Log ("Cur: [" + point [0].ToString () + "," + point [1].ToString () + "]; Alg: 8; Dir:" + direction.ToString () + "; Next: [" + nextPoint [0].ToString () + "," + nextPoint [1].ToString () + "]");
			switch (direction) {
			case left:
				level [point [0], point [1]] = Randomize (new int[] { 2,4,6,11 });
				SetValue (nextPoint, right);
				return;
				break;
			case down:
				level [point [0], point [1]] = 15;
				//SetValue (nextPoint, up);
				return;
				break;
			case right:
				level [point [0], point [1]] = Randomize (new int[] { 2,4,5,10 });
				SetValue (nextPoint, left);
				return;
				break;

			}
		}

	}
		

	void Test(){
		int lol = 1;
		for (int i = 0; i < height+2; i++) {
			for (int j = 0; j < width+2; j++) {
				level [i, j] = lol;
				lol++;
			}
		}
	}
}
