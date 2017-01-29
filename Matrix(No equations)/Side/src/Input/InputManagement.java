package Input;

import Fraction.*;
import LikeNumber.*;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.*;

public class InputManagement
{
   private RegexManagement[] options;

   public InputManagement()
   {
      options = RegexManagement.values();
   }

   public RegexManagement InterpretCommand(String command)
   {
      Pattern pattern;
      Matcher matcher;

      for(RegexManagement management: options)
      {
         pattern = Pattern.compile(management.getRegex());
         matcher = pattern.matcher(command);
         if(matcher.find())
         {
            return management;
         }
      }
      return RegexManagement.Nothing;
   }

   public AdditiveFraction interpretAdditiveFraction(String str)
   {
      boolean fraction = str.contains("/");
      AdditiveEquation addEquationNum, addEquationDen;
      if(fraction)
      {
         addEquationNum = interpretAdditiveEquation(str.substring(0, str.indexOf('/')));
         addEquationDen = interpretAdditiveEquation(str.substring(str.indexOf('/') + 1));
      }
      else
      {
         addEquationNum = interpretAdditiveEquation(str);
         addEquationDen = new AdditiveEquation();
         addEquationDen.add(new Multiples());
      }
      if(addEquationNum == null || addEquationDen == null)
      {
         return null;
      }
      return new AdditiveFraction(addEquationNum, addEquationDen);
   }

   public AdditiveEquation interpretAdditiveEquation(String str)
   {
      int firstIndex = 0, secondIndex = 0;
      boolean looking = false, start = false;
      int pCount = 0;
      AdditiveEquation finalEq = new AdditiveEquation();
      List<String> list = new ArrayList<>();
      Multiples mult;
      for(int x = 0; x < str.length(); x++)
      {
         if(!looking && str.charAt(x) == 't')
         {
            firstIndex = x;
            pCount = 0;
            looking = true;
         }
         else if(looking)
         {
            if(str.charAt(x) == '(')
            {
               start = true;
               pCount++;
            }
            else if(str.charAt(x) == ')')
            {
               pCount--;
            }
            if(pCount == 0 && start)
            {
               looking = false;
               start = false;
               list.add(str.substring(firstIndex, x + 1));
               str = str.substring(0, firstIndex) + "ROOT" + str.substring(x + 1);
               x = firstIndex + 2;
            }
         }
      }
      mult = new Multiples();
      for(int x = 0; x < str.length(); x++)
      {
         char val = str.charAt(x);
         if(val >= '0' && val <= '9')
         {
            x = grabNumber(x, str, mult);
         }
         else if(val <= 'z' && val >= 'a' && val != 't')
         {
            x = grabVariable(x, str, mult);
         }
         else if(val == 'R')
         {
            mult.multiply(interpretRoot(list.remove(0)));
            x = x + 3;
         }
         else if(val == '+' || val == '-')
         {
            finalEq.add(mult);
            mult = new Multiples();
            if(val == '-')
            {
               mult.multiply(new MyNumber(-1));
            }
         }
         else
         {
            x = -1;
         }
         if(x == -1)
         {
            return null;
         }
      }
      finalEq.add(mult);
      return finalEq;
   }

   public int grabVariable(int start, String str, Multiples mult)
   {
      String var = str.charAt(start) + "";
      String num = "";
      if(start + 2 < str.length() && str.charAt(start+1) == '^' && (str.charAt(start + 2) >= '0' && str.charAt(start + 2) <= '9'))
      {
         start = start + 2;
         for(int x = start; x < str.length(); x++)
         {
            if(str.charAt(x) >= '0' && str.charAt(x) <= '9')
            {
               num += str.charAt(x);
            }
            else
            {
               mult.multiply(new Variable(var, Integer.parseInt(num)));
               return x - 1;
            }
         }
         return -1;
      }
      else
      {
         mult.multiply(new Variable(var, 1));
         return start;
      }
   }

   public int grabNumber(int start, String str, Multiples mult)
   {
      String num = "";
      for(int x = start; x < str.length(); x++)
      {
         if(str.charAt(x) >= '0' && str.charAt(x) <= '9')
         {
            num += str.charAt(x);
         }
         else
         {
            mult.multiply(new MyNumber(Integer.parseInt(num)));
            return x - 1;
         }
      }
      if(num.isEmpty())
      {
         return -1;
      }
      mult.multiply(new MyNumber(Integer.parseInt(num)));
      return start;
   }

   public LikeNumber interpretRoot(String str)
   {
      String num = "";
      int add = 0;
      boolean found = false;
      for(int x = 1; x < str.length() && !found; x++)
      {
         if(str.charAt(x) >= '0' && str.charAt(x) <= '9')
         {
            num += str.charAt(x);
         }
         else
         {
            add = x + 1;
            found = true;
         }
      }
      if(num.isEmpty())
      {
         return null;
      }
      return new Root(interpretAdditiveEquation(str.substring(add, str.length() - 1)), Integer.parseInt(num));
   }

   public Fraction findFraction(String str)
   {
      if(str.isEmpty())
      {
         return new Fraction(1,1);
      }

      Pattern pattern = Pattern.compile("^" + StaticStrings.scale + "$");
      Matcher matcher = pattern.matcher(str);

      if(!matcher.find())
      {
         return null;
      }

      if(str.contains("/"))
      {
         return new Fraction(Integer.parseInt(str.substring(0,str.indexOf("/"))), Integer.parseInt(str.substring(str.indexOf("/") + 1)));
      }
      else
      {
         return new Fraction(Integer.parseInt(str), 1);
      }
   }
}