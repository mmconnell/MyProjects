package Input;

/**
 * Created by Michael on 10/11/2016.
 */
public class StaticStrings
{
   static final String scale = "(([-]?([0-9]+/([-]?[1-9][0-9]*))|[-]?([0-9]+))?)";
   static final String row = "r[1-9]+";
   static final String matrix = "m[1-9]+";
   static final String newMatrix = "^m\\[((([-]?[0-9]+)|([-]?[0-9]+/([-]?[1-9][0-9]*)))(:|,))*(([-]?[0-9]+)|([-]?[0-9]+/([-]?[1-9][0-9]*)))\\]$";
}