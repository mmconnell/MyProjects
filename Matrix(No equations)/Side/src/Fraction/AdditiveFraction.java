package Fraction;

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

   public void reduce()
   {
      Multiples gcd = this.num.allTermsGCD().gcd(this.den.allTermsGCD());
      this.num.divideAll(gcd);
      this.den.divideAll(gcd);
      //NEGATIVE
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
            retStr += "(" + num + ")";
         }
         retStr += "/";
         if(den.singleElement())
         {
            retStr += den;
         }
         else
         {
            retStr += "(" + den + ")";
         }
         return retStr;
      }
   }
}
