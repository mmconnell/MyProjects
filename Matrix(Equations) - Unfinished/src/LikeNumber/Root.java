package LikeNumber;

import java.math.BigInteger;

public class Root implements LikeNumber
{
   private String val;
   private AdditiveEquation num;
   private int power;
   private String type;

   public Root(AdditiveEquation num, int power)
   {
      this.num = num.clone();
      this.val = num + "";
      this.power = power;
      type = "Root";
   }

   public String getValue()
   {
      return val;
   }

   public int getPower()
   {
      return power;
   }

   public boolean addable(LikeNumber ln, String type)
   {
      return type != null && ln != null && type.equals(this.type) && val.equals(ln.getValue()) && power == ln.getPower();
   }

   public boolean canMultiply(LikeNumber ln, String type)
   {
      return type != null && ln != null && type.equals(this.type) && power == ln.getPower();
   }

   public void multiply(LikeNumber ln)
   {
      int value;
      if(canMultiply(ln, ln.getType()))
      {
         Root root = (Root)ln;
         this.num.multiply(root.getEquation());
         this.val = this.num + "";
      }
   }

   public AdditiveEquation getEquation()
   {
      return num.clone();
   }

   private void reducePersonal()
   {

   }

   public AdditiveEquation consolidateTerms()
   {
      if(power == 1)
      {
         this.num.consolidateTerms();
         return this.num;
      }
      AdditiveEquation ae = new AdditiveEquation();
      ae.toReduce = false;
      Multiples mult;
      if(this.num.singleElement())
      {
         mult = this.num.firstElement().rootify(power);
         ae.add(mult);
         this.power = 1;
      }
      else
      {
         mult = new Multiples();
         mult.multiply(new Root(this.num, power));
         ae.add(mult);
      }
      ae.toReduce = true;
      return ae;
   }

   public void setPower(int power)
   {
      this.power = power;
   }

   public Multiples reduce()
   {
      int value = power;
      Multiples mult = this.num.allTermsGCD();
      //reduceValue(power, mult);
      Multiples retMult = new Multiples();
      Multiples temp, divide;
      AdditiveEquation add;
      for(; value > 1; value--)
      {
         add = new AdditiveEquation();
         if(power % value == 0)
         {
            temp = mult.getMultiple(value);
            divide = temp.clone();
            divide.pow(value);
            this.num.divideAll(divide);
            mult.divideWhole(divide);
            add.add(temp);
            retMult.multiply(new Root(add, power/value));
           // retMult.multiply(new Root())
         }
      }
      return retMult;
   }

   private Multiples reduceValue(int power, Multiples gcd)
   {
      return null;
   }

   public void pow(int power)
   {
      LikeNumber ln = clone();
      if(this.power % power == 0)
      {
         this.power /= power;
      }
      else
      {
         for (int x = 1; x < power; x++)
         {
            multiply(ln);
         }
      }
   }

   public LikeNumber getMultiple(int power)
   {
      return new Root(this.num.clone(), this.power * power);
   }

   public String getType()
   {
      return type;
   }

   public boolean canRemove()
   {
      return num.isOne();
   }

   public boolean equals(Object obj)
   {
      if(obj == null || !(obj instanceof Root))
      {
         return false;
      }

      Root root = (Root)obj;

      return root.num.equals(num) && root.power == power;
   }

   public String toString()
   {
      return "t" + power + "(" + num + ")";
   }

   public LikeNumber clone()
   {
      return new Root(num, power);
   }

   @Override
   public void divide(LikeNumber ln)
   {
      if(canDivide(ln))
      {
         Root root = (Root)ln;
         BigInteger big = new BigInteger(power + "");
         int gcd = big.gcd(new BigInteger(root.power + "")).intValue();
         int lcm = (power*root.power)/gcd;
         this.num.pow(lcm/power);
         root.num.pow(lcm/root.power);
         Multiples multiples = root.num.allTermsGCD().gcd(this.num.allTermsGCD());
         multiples.multiply(new MyNumber(1));
         AdditiveEquation add = new AdditiveEquation();
         add.toReduce = false;
         add.add(multiples);
         add.toReduce = true;
         if(add.equals(root.num))
         {
            this.num.divideAll(multiples);
         }
      }
   }

   public boolean isZero()
   {
      return num.isZero();
   }

   @Override
   public LikeNumber gcd(LikeNumber ln)
   {
      if(isMultiple(ln))
      {
         Root root = (Root)ln.clone();
         BigInteger big = new BigInteger(power + "");
         int gcd = big.gcd(new BigInteger(root.power + "")).intValue();
         int lcm = (power*root.power)/gcd;
         Root temp = (Root)this.clone();
         root.pow(lcm/root.power);
         temp.pow(lcm/temp.power);
         Multiples multiples = root.num.allTermsGCD().gcd(temp.num.allTermsGCD());
         AdditiveEquation add = new AdditiveEquation();
         add.toReduce = false;
         add.add(multiples);
         add.toReduce = true;
         return new Root(add,lcm);
      }
      return new MyNumber(1);
   }

   @Override
   public boolean isMultiple(LikeNumber ln)
   {
      if(!ln.getType().equals("Root"))
      {
         return false;
      }
      Root root = (Root)ln;
      return !root.num.allTermsGCD().gcd(num.allTermsGCD()).equals(new Multiples());
   }

   @Override
   public boolean isNegative()
   {
      return false;
   }

   @Override
   public void negate()
   {

   }

   @Override
   public boolean canDivide(LikeNumber ln)
   {
      if(ln == null || !ln.getType().equals(type) || ln.getPower() % power != 0)
      {
         return false;
      }
      return !this.num.allTermsGCD().gcd(((Root)ln).num.allTermsGCD()).equals(new Multiples());
   }
}
