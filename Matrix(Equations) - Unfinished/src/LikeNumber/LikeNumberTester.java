package LikeNumber;

import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class LikeNumberTester
{
   public static String root = "(t[1-9][0-9]*\\([1-9][0-9]*\\))";
   public static String variable = "((([a-s,u-z])|([a-s,u-z]\\^[1-9][0-9]*))+)";
   public static String number = "([1-9][0-9]*)";
   public static String equation = "^([1-9][0-9]*)?((" + number + "|" + root + "|" + variable + ")+\\+)*" + "((" + number + "|" + root + "|" + variable + ")+)$";
   public static void main(String[] args)
   {
      Scanner kb = new Scanner(System.in), kb2;
      System.out.println("Enter the first equation: \nWhere t2(3) is the square root of 3\nAnd variables are in the form x^power where x can be any letter except for t");
      String input = kb.nextLine();
      Pattern pattern = Pattern.compile(equation);
      Matcher matcher = pattern.matcher(input);
      LikeNumber ln;
      Multiples mult;
      AdditiveEquation ae1, ae2;
      String temp;
      boolean keepGoing;
      char val;
      int num;
      int index;
      int[] indices;
      int constant;
      if(matcher.find())
      {
         ae1 = readInput(input);
         System.out.println("Enter the second equation: ");
         input = kb.nextLine();
         matcher = pattern.matcher(input);
         if(matcher.find())
         {
            ae2 = readInput(input);
            System.out.println("1) Multiply\n2) Add");
            int choice = kb.nextInt();
            switch(choice)
            {
               case 1:
                  ae1.multiply(ae2);
                  break;
               case 2:
                  ae1.add(ae2);
                  break;
            }
            System.out.println(ae1);
         }
      }
   }

   public static AdditiveEquation readInput(String input )
   {
      AdditiveEquation ae;
      String temp;
      Multiples mult;
      char val;
      int num, index, constant;
      int[] indices;
      LikeNumber ln;
      Scanner kb = new Scanner(input);
      kb.useDelimiter("\\+");
      ae = new AdditiveEquation();
      while(kb.hasNext())
      {
         temp = kb.next();
         mult = new Multiples(new MyNumber(1));
         while(temp.length() > 0)
         {
            val = temp.charAt(0);
            num = readInput(temp, val);
            switch(num)
            {
               case 0:
                  index = readNum(temp);
                  constant = Integer.parseInt(temp.substring(0,index));
                  //mult.multiplyConstant(constant);
                  temp = temp.substring(index);
                  break;
               case 1:
                  indices = readRoot(temp);
                  //ln = new Root(Integer.parseInt(temp.substring(indices[0] + 1,indices[1])),Integer.parseInt(temp.substring(1,indices[0])));
                  //mult.multiply(ln);
                  temp = temp.substring(indices[1] + 1);
                  break;
               case 2:
                  index = readVar(temp);
                  if(index == -1)
                  {
                     ln = new Variable(temp.substring(0,1),1);
                     temp = temp.substring(1);
                  }
                  else
                  {
                     ln = new Variable(temp.substring(0, 1), Integer.parseInt(temp.substring(2, index)));
                     temp = temp.substring(index);
                  }
                  mult.multiply(ln);
                  break;
            }
         }
         ae.add(mult);
      }
      return ae;
   }

   public static int readNum(String temp)
   {
      int count = 0;
      while(count < temp.length() && temp.charAt(count) >= '0' && temp.charAt(count) <= '9')
      {
         count++;
      }
      return count;
   }

   public static int[] readRoot(String temp)
   {
      int count = 1;
      int[] array = new int[2];
      while(temp.charAt(count) >= '0' && temp.charAt(count) <= '9')
      {
         count++;
      }
      array[0] = count;

      count += 1;
      while(temp.charAt(count) >= '0' && temp.charAt(count) <= '9')
      {
         count++;
      }
      array[1] = count;
      return array;
   }

   public static int readVar(String temp)
   {
      int count = 2;
      if(temp.length() < 2 || temp.charAt(1) != '^')
      {
         return -1;
      }
      while(count < temp.length() && temp.charAt(count) >= '0' && temp.charAt(count) <= '9')
      {
         count++;
      }
      return count;
   }

   public static int readInput(String temp, char val)
   {
      if(val > '0' && val <= '9')
      {
         return 0;
      }
      else if(val == 't')
      {
         return 1;
      }
      return 2;
   }
}
