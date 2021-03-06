package SpecialAbilities;

import Characters.A_Character;
import PartyManagement.Party;
import Utilities.Display;

/**
 * Created by Michael on 5/25/2016.
 */
public class BarkSkin extends SpecialAbility
{
    public boolean executeAbility(A_Character character, Party allies, Party enemies)
    {
        abilityExecution(character);
        return false;
    }

    public boolean executeAbilityRandom(A_Character character, Party allies, Party enemies)
    {
        abilityExecution(character);
        return false;
    }

    public void abilityExecution(A_Character character)
    {
        Display.displayMessage(character.getName() + " used bark skin!");
        double calculateBuff = 1.0 + .015*((double)character.getPower());
        character.getConditions().giveDamageReductionBuff(calculateBuff, calculateRounds(character), "Bark Skin");
    }

    public String toString()
    {
        return "Bark Skin";
    }

    public static String description()
    {
        return "     - Bark Skin: reduces incoming damage";
    }
}
