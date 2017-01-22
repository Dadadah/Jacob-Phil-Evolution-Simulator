using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LineGraphManager : MonoBehaviour {

	public GameObject linerenderer;
	public GameObject pointer;

	public GameObject pointerRed;
	public GameObject pointerBlue;

	public GameObject HolderPrefb;

	public GameObject holder;
	public GameObject xLineNumber;

	public Material bluemat;
	public Material greenmat;

	public Text topValue;

    public Evolution evolve;

    public List<float> maximum = new List<float>();
    public List<float> minimum = new List<float>();

	private float gd;
	private float highestValue = 56;

	public Transform origin;

	public TextMesh player1name;
	public TextMesh player2name;

	private float lrWidth = 0.1f;
	private int dataGap = 0;

	void Start(){

		// showing graph
		ShowGraph();
	}

    public void UpdateValues(float max, float min)
    {
        maximum.Add(max*7 /56);
        minimum.Add(min*7 /56);
    }
	
	public void ShowData(List<float> gdlist,int playerNum,float gap) {

        if (playerNum == 1) 
			StartCoroutine(BarGraphBlue(gdlist,gap));
		else if(playerNum == 2) 
			StartCoroutine(BarGraphGreen(gdlist,gap));
	}
    
	public void ShowGraph(){

		ClearGraph();

		if(maximum.Count >= 1 && minimum.Count >= 1){
			holder = Instantiate(HolderPrefb,Vector3.zero,Quaternion.identity) as GameObject;
			holder.name = "h2";

			dataGap = GetDataGap(minimum.Count);
            
			float gap = 1.0f;
            int datacount = minimum.Count;

            while (datacount > 13)
            {
                gap /= 2;
                datacount /= 2;
            }

			ShowData(maximum, 1, gap);
			ShowData(minimum, 2, gap);
		}
	}

	public void ClearGraph(){
		if(holder)
			Destroy(holder);
	}

	int GetDataGap(int dataCount){
		int value = 1;
		int num = 0;
		while((dataCount-(40+num)) >= 0){
			value+= 1;
			num+= 20;
		}
		
		return value;
	}


	IEnumerator BarGraphBlue(List<float> gd,float gap)
	{
		float xIncrement = gap;
		int dataCount = 0;
		bool flag = false;
		Vector3 startpoint = new Vector3((origin.position.x+xIncrement),(origin.position.y+gd[dataCount]),(origin.position.z));//origin.position;//

		while(dataCount < gd.Count)
		{
			
			Vector3 endpoint = new Vector3((origin.position.x+xIncrement),(origin.position.y+gd[dataCount]),(origin.position.z));
			startpoint = new Vector3(startpoint.x,startpoint.y,origin.position.z);
			// pointer is an empty gameObject, i made a prefab of it and attach it in the inspector
			GameObject p = Instantiate(pointer, new Vector3(startpoint.x, startpoint.y, origin.position.z),Quaternion.identity) as GameObject;
			p.transform.parent = holder.transform;


			GameObject lineNumber = Instantiate(xLineNumber, new Vector3(origin.position.x+xIncrement, origin.position.y-0.18f , origin.position.z),Quaternion.identity) as GameObject;
			lineNumber.transform.parent = holder.transform;
			lineNumber.GetComponent<TextMesh>().text = (dataCount+1).ToString();


			// linerenderer is an empty gameObject with Line Renderer Component Attach to it, 
			// i made a prefab of it and attach it in the inspector
			GameObject lineObj = Instantiate(linerenderer,startpoint,Quaternion.identity) as GameObject;
			lineObj.transform.parent = holder.transform;
			lineObj.name = dataCount.ToString();
			
			LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
			
			lineRenderer.material = bluemat;
			lineRenderer.SetWidth(lrWidth, lrWidth);
			lineRenderer.SetVertexCount(2);

			while(Vector3.Distance(p.transform.position,endpoint) > 0.2f)
			{
				float step = 5 * Time.deltaTime;
				p.transform.position = Vector3.MoveTowards(p.transform.position, endpoint, step);
				lineRenderer.SetPosition(0, startpoint);
				lineRenderer.SetPosition(1, p.transform.position);
				
				yield return null;
			}
			
			lineRenderer.SetPosition(0, startpoint);
			lineRenderer.SetPosition(1, endpoint);
			
			
			p.transform.position = endpoint;
			GameObject pointered = Instantiate(pointerRed,endpoint,pointerRed.transform.rotation) as GameObject ;
			pointered.transform.parent = holder.transform;
			startpoint = endpoint;

			if(dataGap > 1){
				if((dataCount+dataGap) == gd.Count)
                {
					dataCount+=dataGap-1;
					flag = true;
				}
				else if((dataCount+dataGap) > gd.Count && !flag){
					dataCount =	gd.Count - 1;
					flag = true;
				}
				else{
					dataCount+=dataGap;
					if(dataCount == (gd.Count - 1))
						flag = true;
				}
			}
			else
				dataCount+=dataGap;

			xIncrement+= gap;
			
			yield return null;
			
		}
	}

	IEnumerator BarGraphGreen(List<float> gd, float gap)
	{
		float xIncrement = gap;
		int dataCount = 0;
		bool flag = false;

		Vector3 startpoint = new Vector3((origin.position.x+xIncrement),(origin.position.y+gd[dataCount]),(origin.position.z));
		while(dataCount < gd.Count)
		{
			
			Vector3 endpoint = new Vector3((origin.position.x+xIncrement),(origin.position.y+gd[dataCount]),(origin.position.z));
			startpoint = new Vector3(startpoint.x,startpoint.y,origin.position.z);
			// pointer is an empty gameObject, i made a prefab of it and attach it in the inspector
			GameObject p = Instantiate(pointer, new Vector3(startpoint.x, startpoint.y, origin.position.z),Quaternion.identity) as GameObject;
			p.transform.parent = holder.transform;
			
			// linerenderer is an empty gameObject with Line Renderer Component Attach to it, 
			// i made a prefab of it and attach it in the inspector
			GameObject lineObj = Instantiate(linerenderer,startpoint,Quaternion.identity) as GameObject;
			lineObj.transform.parent = holder.transform;
			lineObj.name = dataCount.ToString();
			
			LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
			
			lineRenderer.material = greenmat;
			lineRenderer.SetWidth(lrWidth, lrWidth);
			lineRenderer.SetVertexCount(2);

			while(Vector3.Distance(p.transform.position,endpoint) > 0.2f)
			{
				float step = 5 * Time.deltaTime;
				p.transform.position = Vector3.MoveTowards(p.transform.position, endpoint, step);
				lineRenderer.SetPosition(0, startpoint);
				lineRenderer.SetPosition(1, p.transform.position);
				
				yield return null;
			}
			
			lineRenderer.SetPosition(0, startpoint);
			lineRenderer.SetPosition(1, endpoint);
			
			
			p.transform.position = endpoint;
			GameObject pointerblue = Instantiate(pointerBlue,endpoint,pointerBlue.transform.rotation) as GameObject; 
			pointerblue.transform.parent = holder.transform;
			startpoint = endpoint;

			if(dataGap > 1){
				if((dataCount+dataGap) == gd.Count)
                {
					dataCount+=dataGap-1;
					flag = true;
				}
				else if((dataCount+dataGap) > gd.Count && !flag){
					dataCount =	gd.Count - 1;
					flag = true;
				}
				else{
					dataCount+=dataGap;
					if(dataCount == (gd.Count - 1))
						flag = true;
				}
			}
			else
				dataCount+=dataGap;

			xIncrement+= gap;
			
			yield return null;
			
		}
	}
}
