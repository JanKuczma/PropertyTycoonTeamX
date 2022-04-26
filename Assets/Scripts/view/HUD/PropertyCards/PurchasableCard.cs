using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
public abstract class PurchasableCard : MonoBehaviour
{
    public Model.Space.Purchasable property;
    public Text propertyName;
    public Text price;
    public Text mortgage;
}
}
