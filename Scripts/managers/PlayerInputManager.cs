namespace managers {
  using generic;
  using hud;
  using UnityEngine;

  /** Responsible for gating and routing player input
   */
  public class PlayerInputManager : SingletonComponent<PlayerInputManager> {
  
    EventLog eventLog;

    private PlayerInputMode inputMode = Disabled;

    public void setInputMode(PlayerInputMode newMode) {
      inputMode = newMode;
    }

    public PlayerInputMode getInputMode() {
      return inputMode;
    }
  }
}
