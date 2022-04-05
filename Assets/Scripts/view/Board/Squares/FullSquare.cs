using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace View{
public class FullSquare : Square
{
    public static FullSquare Create(SqType type, Transform parent, int position, string name, string variant)
    {
        FullSquare square = Instantiate(Asset.Board(type,variant),parent).GetComponent<FullSquare>();
        square.transform.localScale = new Vector3(1,1,1);
        square.transform.localPosition = generateCoordinates(position);
        square.transform.localRotation = getRotation(position);
        square._position = position;
        square.setName(name);
        square.assignSpots();
        return square;
    }
    override protected void assignSpots()
    {
        float offsetSmall = offsetS*transform.localScale.x;
        float offsetBig = offsetB*transform.localScale.x;

        spots[0] = transform.position + transform.right*offsetSmall + transform.forward*(offsetSmall/5);
        spots[1] = transform.position + transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2) + transform.forward*(offsetSmall/5);
        spots[2] = transform.position + transform.right*offsetSmall - transform.forward*(offsetBig + offsetSmall/2) + transform.forward*(offsetSmall/5);
        spots[3] = transform.position - transform.right*offsetSmall - transform.forward*(offsetSmall/5);
        spots[4] = transform.position - transform.right*offsetSmall + transform.forward*(offsetBig + offsetSmall/2) - transform.forward*(offsetSmall/5);
        spots[5] = transform.position - transform.right*offsetSmall - transform.forward*(offsetBig + offsetSmall/2) - transform.forward*(offsetSmall/5);
    }
}
}