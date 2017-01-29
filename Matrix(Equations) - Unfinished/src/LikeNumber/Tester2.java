package LikeNumber;

import Fraction.AdditiveFraction;
import Input.InputManagement;

public class Tester2
{
   public static void main(String[] args)
   {
      Multiples myMult = new Multiples(new Variable("a",1));
      AdditiveEquation myAdd = new AdditiveEquation();
      myAdd.add(myMult);
      AdditiveEquation myAdd2 = new AdditiveEquation();
      myAdd2.add(myMult);
      myMult = new Multiples(new Variable("c",1));
      myAdd.add(myMult);
      myAdd2.add(myMult);
      myAdd.equals(myAdd2);
      AdditiveEquation test1 = new AdditiveEquation();
      Multiples test1Mult = new Multiples(new MyNumber(2));
      test1.add(test1Mult);
      Root root1 = new Root(test1, 2);
      test1Mult = new Multiples(new MyNumber(6));
      test1 = new AdditiveEquation();
      test1.add(test1Mult);
      Root root2 = new Root(test1, 3);
      if(root1.isMultiple(root2))
      {
         System.out.println("Yay");
      }
      else
      {
         System.out.println("Boo");
      }

      test1 = new AdditiveEquation();
      test1Mult = new Multiples();
      test1Mult.multiply(new MyNumber(2));
      test1.add(test1Mult);
      root1 = new Root(test1, 2);
      test1Mult = new Multiples(root1.clone());
      test1Mult.multiply(new Variable("x", 1));
      test1Mult.multiply(new MyNumber(3));
      test1 = new AdditiveEquation();
      test1.add(test1Mult);
      test1Mult = new Multiples(root1);
      test1Mult.multiply(new Variable("y", 1));
      test1Mult.multiply(new MyNumber(3));
      test1.add(test1Mult);
      root1 = new Root(test1, 2);
      test1 = new AdditiveEquation();
      test1.add(new Multiples(root1));
      System.out.println(root1 + " to " + test1);

//      test1 = new AdditiveEquation();
//      test1Mult = new Multiples();
//      test1Mult.multiply(new MyNumber(2));
//      test1.add(test1Mult);
//      test1.add(new Multiples(new Variable("x", 1)));
//      root1 = new Root(test1, 2);
//      root2 = new Root(test1, 2);
//      test1 = new AdditiveEquation();
//      test1.add(new Multiples(root1));
//      test1.add()

      Multiples mult = new Multiples();
      mult.multiply(new MyNumber(2));
      AdditiveEquation add = new AdditiveEquation();
      add.add(mult);
      LikeNumber ln = new Root(add, 2);
      add = new AdditiveEquation();
      mult = new Multiples();
      mult.multiply(ln);
      add.add(mult);
      ln = new Root(add, 3);
      mult = new Multiples();
      mult.multiply(ln);
      add = new AdditiveEquation();
      add.add(mult);
      add.consolidateTerms();
      mult = new Multiples();
      ln = new Variable("x", 2);
      mult.multiply(new MyNumber(3));
      mult.multiply(ln);
      add.add(mult);
      ln = new Root(add,5);
      mult = new Multiples();
      mult.multiply(ln);
      add = new AdditiveEquation();
      add.add(mult);
      mult = new Multiples();
      mult.multiply(new MyNumber(3));
      add.add(mult);
      AdditiveEquation add2 = new AdditiveEquation();
      mult = new Multiples();
      mult.multiply(new MyNumber(3));
      add2.add(mult);
      ln = new Root(add2, 5);
      add2 = new AdditiveEquation();
      mult = new Multiples();
      mult.multiply(ln);
      add2.add(mult);
      add.multiply(add2);
      add2 = new AdditiveEquation();
      mult = new Multiples();
      mult.multiply(new MyNumber(3));
      add2.add(mult);
      ln = new Root(add2, 5);
      mult = new Multiples();
      mult.multiply(ln);
      mult.multiply(new MyNumber(-3));
      add2 = new AdditiveEquation();
      add2.add(mult);
      add.add(add2);
      System.out.println(add);

      add = new AdditiveEquation();
      add2 = new AdditiveEquation();
      mult = new Multiples();
      ln = new Variable("x", 4);
      mult.multiply(ln);
      mult.multiply(new MyNumber(3));
      mult.multiply(new Variable("y", 10));
      add.add(mult);
      mult = new Multiples();
      mult.multiply(new Variable("x", 2));
      mult.multiply(new MyNumber(6));
      mult.multiply(new Variable("y", 3));
      AdditiveEquation tempAdd = new AdditiveEquation();
      tempAdd.add(mult);
      System.out.println(add + " over " + tempAdd);
      AdditiveFraction aFraction = new AdditiveFraction(add,tempAdd);
      System.out.println(aFraction);
      add.add(mult);
      System.out.print("GCD of " + add + " is ");
      System.out.println(add.allTermsGCD());
      mult = add.allTermsGCD();
      System.out.print(add + " divided by " + mult + " is ");
      add.divideAll(mult);
      System.out.println(add);

      InputManagement input = new InputManagement();
      System.out.println(input.interpretAdditiveFraction("3t2(x^2-a)+xyt3(x^4+y)/4"));

      add = new AdditiveEquation();
      mult = new Multiples(new MyNumber(216));
      add.add(mult);
      ln = new Root(add, 6);
      mult = new Multiples();
      mult.multiply(ln);
      add = new AdditiveEquation();
      add.add(mult);
      System.out.print(mult + " to ");
      System.out.println(add);

      add = new AdditiveEquation();
      mult = new Multiples();
      mult.multiply(new MyNumber(128));
      add.add(mult);
      ln = new Root(add, 3);
      mult = new Multiples();
      mult.multiply(ln);
      add = new AdditiveEquation();
      add.add(mult);
      System.out.print(mult + " to ");
      System.out.println(add);

      add = new AdditiveEquation();
      mult = new Multiples();
      ln = new Variable("x", 3);
      mult.multiply(ln);
      mult.multiply(new MyNumber(27));
      add.add(mult);
      mult = new Multiples();
      mult.multiply(new Variable("x", 4));
      mult.multiply(new MyNumber(9));
      mult.multiply(new Variable("y", 2));
      add.add(mult);
      Root root = new Root(add, 2);
      add = new AdditiveEquation();
      System.out.print(root + " to ");
      mult = new Multiples();
      mult.multiply(root);
      add.add(mult);
      System.out.println(add);


   }
}
