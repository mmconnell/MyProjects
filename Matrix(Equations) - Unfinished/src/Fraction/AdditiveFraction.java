package Fraction;

import Input.InputManagement;
import LikeNumber.*;

public class AdditiveFraction
{
   private AdditiveEquation num;
   private AdditiveEquation den;

   public AdditiveFraction(AdditiveEquation num, AdditiveEquation den)
   {
      if(den.isZero() || num.isZero())
      {
         den = new AdditiveEquation();
         den.add(new Multiples());
      }
      this.num = num.clone();
      this.den = den.clone();
      reduce();
   }

   public AdditiveFraction(int num, int den)
   {
      if(num == 0 || den == 0)
      {
         this.num = new AdditiveEquation();
         this.num.add(new Multiples(new MyNumber(0)));
         this.den = new AdditiveEquation();
         this.den.add(new Multiples());
      }
      else
      {
         this.num = (new InputManagement()).interpretAdditiveEquation(num+"");
         this.den = (new InputManagement()).interpretAdditiveEquation(den+"");
      }
   }

   public void reduce()
   {
      Multiples gcd = this.num.allTermsGCD().gcd(this.den.allTermsGCD());
      this.num.divideAll(gcd);
      this.den.divideAll(gcd);

      AdditiveEquation temp1 = num.clone();
      AdditiveEquation temp2 = den.clone();
      AdditiveEquation temp3;
      Multiples mult1 = num.allTermsGCD();
      Multiples mult2 = den.allTermsGCD();

      AdditiveEquation ae2 = new AdditiveEquation();
      ae2.add(new Multiples(new MyNumber(-1)));

      temp1.divideAll(mult1);
      temp2.divideAll(mult2);

      temp3 = temp2.clone();
      temp3.multiply(ae2);

      if(temp1.equals(temp2))
      {
         temp1 = new AdditiveEquation();
         temp2 = new AdditiveEquation();
         temp1.add(new Multiples());
         temp2.add(new Multiples());
      }
      else if(temp1.equals(temp3))
      {
         temp1 = new AdditiveEquation();
         temp2 = new AdditiveEquation();
         temp1.add(new Multiples(new MyNumber(-1)));
         temp2.add(new Multiples());
      }

      AdditiveEquation temp = new AdditiveEquation();
      temp.add(mult1);
      temp1.multiply(temp);
      temp = new AdditiveEquation();
      temp.add(mult2);
      temp2.multiply(temp);

      this.num = temp1.clone();
      this.den = temp2.clone();

      if(this.den.isMoreNegative())
      {
         AdditiveEquation ae = new AdditiveEquation();
         ae.add(new Multiples(new MyNumber(-1)));
         this.den.multiply(ae);
         this.num.multiply(ae);
      }

      if(num.isZero())
      {
         den = new AdditiveEquation();
         den.add(new Multiples());
      }
   }

   public void multiply(AdditiveFraction fraction)
   {
      AdditiveFraction fraction1 = new AdditiveFraction(this.num, fraction.den);
      AdditiveFraction fraction2 = new AdditiveFraction(fraction.num, this.den);
      fraction1.num.multiply(fraction2.num);
      fraction1.den.multiply(fraction2.den);
      this.num = fraction1.num;
      this.den = fraction1.den;
      this.reduce();
   }

   public void divide(AdditiveFraction fraction)
   {
      AdditiveFraction newFraction = new AdditiveFraction(fraction.den, fraction.num);
      this.multiply(newFraction);
   }

   public void add(AdditiveFraction fraction)
   {
      this.num.multiply(fraction.den);
      AdditiveEquation temp = fraction.num.clone();
      temp.multiply(this.den);
      this.num.add(temp);
      this.den.multiply(fraction.den);
      this.reduce();
   }

   public void subtract(AdditiveFraction fraction)
   {
      this.add(negate(fraction));
   }

   public AdditiveEquation getNum()
   {
      return this.num.clone();
   }

   public AdditiveEquation getDen()
   {
      return this.den.clone();
   }

   public static AdditiveFraction reciprocal(AdditiveFraction fraction)
   {
      return new AdditiveFraction(fraction.getDen(), fraction.getNum());
   }

   public static AdditiveFraction negate(AdditiveFraction fraction)
   {
      AdditiveFraction temp = new AdditiveFraction(fraction.getNum(), fraction.getDen());
      temp.num.negate();
      return temp;
   }

   public boolean isWhole()
   {
      return den.isOne();
   }

   public boolean isZero()
   {
      return num.isZero();
   }

   public String toString()
   {
      String retStr = "";
      if(isWhole())
      {
         return num + "";
      }
      else
      {
         if(num.singleElement())
         {
            retStr += num;
         }
         else
         {
            retStr += num;//"(" + num + ")";
         }
         retStr += "/";
         if(den.singleElement())
         {
            retStr += den;
         }
         else
         {
            retStr += den;//"(" + den + ")";
         }
         return retStr;
      }
   }
}
