using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
//--------------------------Environment Variables-----------------------------------------------------------------//

    private Slider slider;

//---------------------------Awake--------------------------------------------------------------------------------//

    private void Awake()
    {
        slider = GetComponent<Slider>();
    } 

//----------------------------Methods-----------------------------------------------------------------------------//

    public void changeMaximum(float maximum)
    {
        slider.maxValue = maximum;
    }
    
    public void changeCurrent(float amountOf)
    {
        slider.value = amountOf;
    }
    
    public void initializeBar(float amount, float maxAmount)
    {
        slider = GetComponent<Slider>();
        changeMaximum(maxAmount);
        changeCurrent(amount);
    }

}
