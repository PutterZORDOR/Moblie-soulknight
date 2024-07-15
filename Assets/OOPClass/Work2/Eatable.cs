using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Eatable : MonoBehaviour
{
    public int heal;
    public string Cname;
    public int amount;
    public int fillHunger;   
    public virtual void Eat()
    {
        if(amount > 0)
        {
            amount--;
            Debug.Log($"You eat {Cname} have {amount} left");
        }
        else
        {
            Debug.Log("You dont have this item");
        }
    }
    public virtual void Regenaration()
    {
        if(amount > 0)
        {
            amount--;
            Debug.Log($"Heal with '{Cname}' have {amount} left");
        }
    }
}
