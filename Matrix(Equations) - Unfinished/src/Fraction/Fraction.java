package Fraction;

import java.math.BigInteger;

public class Fraction
{
   private BigInteger num;
   private BigInteger den;

   public Fraction(int num, int den)
   {
      if(den == 0 || num == 0)
      {
         den = 1;
      }

      this.num = new BigInteger(num + "");
      this.den = new BigInteger(den + "");

      this.reduce();
   }

   public Fraction(BigInteger num, BigInteger den)
   {
      this.num = new BigInteger(num.toString());
      this.den = new BigInteger(den.toString());
      if(this.den.toString().equals("0") || this.num.toString().equals("0"))
      {
         this.den = new BigInteger("1");
      }
      this.reduce();
   }

   public void reduce()
   {
      BigInteger gcd = this.num.gcd(this.den);
      this.num = this.num.divide(gcd);
      this.den = this.den.divide(gcd);
      if(!this.den.abs().equals(this.den))
      {
         this.num = this.num.multiply(new BigInteger("-1"));
         this.den = this.den.multiply(new BigInteger("-1"));
      }
   }

   public void multiply(Fraction fraction)
   {
      Fraction fraction1 = new Fraction(this.num, fraction.den);
      Fraction fraction2 = new Fraction(fraction.num, this.den);
      this.num = fraction1.num.multiply(fraction2.num);
      this.den = fraction1.den.multiply(fraction2.den);
      this.reduce();
   }

   public void divide(Fraction fraction)
   {
      Fraction newFraction = new Fraction(fraction.den, fraction.num);
      this.multiply(newFraction);
   }

   public void add(Fraction fraction)
   {
      this.num = this.num.multiply(fraction.den);
      BigInteger temp = fraction.num.multiply(this.den);
      this.num = this.num.add(temp);
      this.den = this.den.multiply(fraction.den);
      this.reduce();
   }

   public void subtract(Fraction fraction)
   {
      Fraction temp = new Fraction(fraction.num, fraction.den);
      temp.multiply(new Fraction(-1, 1));
      this.add(temp);
   }

   public BigInteger getNum()
   {
      return new BigInteger(num.toString());
   }

   public BigInteger getDen()
   {
      return new BigInteger(den.toString());
   }

   public static Fraction reciprocal(Fraction fraction)
   {
      return new Fraction(fraction.getDen(), fraction.getNum());
   }

   public static Fraction negate(Fraction fraction)
   {
      BigInteger num = fraction.getNum().multiply(new BigInteger("-1"));
      return new Fraction(num, fraction.getDen());
   }

   public void setFraction(Fraction fraction)
   {
      this.num = fraction.getNum();
      this.den = fraction.getDen();
   }

   public boolean isWhole()
   {
      return den.equals(new BigInteger("1"));
   }

   public boolean isZero()
   {
      return num.equals(new BigInteger("0"));
   }

   public String toString()
   {
      if(isWhole())
      {
         return num + "";
      }
      else
      {
         return num + "/" + den;
      }
   }
}