using UnityEngine;
using UnityEditor;
using Ashen.DeliverySystem;
using UnityEngine.UI;

namespace Manager
{
    public class SliderHandler : I_ThresholdListener
    {
        private Slider slider;

        public SliderHandler(Slider slider)
        {
            this.slider = slider;
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            //slider.value = ((float)value.currentValue) / value.maxValue;
        }
    }
}