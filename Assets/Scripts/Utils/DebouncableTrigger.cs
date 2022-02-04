using System.Collections;
using System.Collections.Generic;

public class DebouncableTrigger {
  
  private bool state = false;
  private bool debounced = true;
  private float lowerTrigger;
  private float upperTrigger;

  public DebouncableTrigger(float lowerTrigger, float upperTrigger) {
    this.lowerTrigger = lowerTrigger;
    this.upperTrigger = upperTrigger;
  }  

  public bool update(float currentValue) {
    if (currentValue > lowerTrigger && debounced) {
      debounced = false;
      state = true;
      return true;
    } else if (currentValue < upperTrigger && !debounced) {
      debounced = true;
      state = false;
      return true;
    }
    return false;
  }

  public bool get() {
    return state;
  }
}
