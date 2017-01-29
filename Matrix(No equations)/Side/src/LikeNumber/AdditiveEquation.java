package LikeNumber;

import java.util.*;

public class AdditiveEquation
{
   private List<Multiples> values;
   boolean toReduce;

   public AdditiveEquation()
   {
      values = new ArrayList<>();
      toReduce = true;
   }

   public void multiply(AdditiveEquation equation)
   {
      List<Multiples> tempValues = new ArrayList<>();
      AdditiveEquation temp2 = new AdditiveEquation();
      List<Multiples> newValues = new ArrayList<>();

      for(Multiples originalMult : values)
      {
         tempValues.add(originalMult.clone());
         for(Multiples newMult : equation.values)
         {
            Multiples temp = originalMult.clone();
            temp.multiply(newMult);
            temp2.toReduce = false;
            temp2.add(temp);
            temp2.toReduce = true;
         }
      }

      values = temp2.values;

      reduce();
   }

   public void add(AdditiveEquation equation)
   {
      boolean added;
      for(Multiples mult : equation.values)
      {
         add(mult);
      }
      reduce();
   }

   public void add(Multiples multiples)
   {
      boolean added;
      added = false;
      for (Multiples mul : values)
      {
         if (!added && mul.canAddTo(multiples))
         {
            mul.add(multiples);
            added = true;
         }
      }
      if (!added)
      {
         values.add(multiples.clone());
      }
      reduce();
   }

   public boolean singleElement()
   {
      return values.size() == 1;
   }

   public Multiples firstElement()
   {
      return values.get(0);
   }

   public boolean isZero()
   {
      return (values.size() == 0) || (values.size() == 1 && values.get(0).isZero());
   }

   public void reduce()
   {
      if(toReduce)
      {
         for (int x = 0; x < values.size(); )
         {
            if (values.get(x).isZero())
            {
               values.remove(x);
            } else
            {
               values.get(x).reduceElements();
               x++;
            }
         }
         consolidateTerms();
      }
   }

   public void consolidateTerms()
   {
      Multiples temp;
      AdditiveEquation ae = new AdditiveEquation();
      for(int x = 0; x < values.size();)
      {
         temp = values.remove(x);
         ae.toReduce = false;
         ae.add(temp.consolidateTerms());
         ae.toReduce = true;
      }
      values = ae.values;
   }

   public Multiples allTermsGCD()
   {
      Multiples gcd = null;
      if(values.size() == 0)
      {
         return new Multiples();
      }

      gcd = values.get(0).clone();

      for(int x = 1; x < values.size(); x++)
      {
         gcd = gcd.gcd(values.get(x).clone()).clone();
      }

      return gcd;
   }

   public void pow(int value)
   {
      AdditiveEquation add = clone();
      for(int x = 1; x < value; x++)
      {
         this.multiply(add);
      }
   }

   public boolean divideAll(Multiples value)
   {
      for(Multiples mult : values)
      {
         if(!mult.divideWhole(value))
         {
            return false;
         }
      }
      reduce();
      return true;
   }

   public boolean isOne()
   {
      return values.size() == 1 && values.get(0).isOne();
   }

   public static int gcd(int x, int y)
   {
      x = Math.abs(x);
      y = Math.abs(y);
      int temp;
      while(y != 0)
      {
         temp = y;
         y = x % y;
         x = temp;
      }
      return x;
   }

   public String toString()
   {
      String ret = "";
      if(values.size() == 0)
      {
         ret += "0";
      }

      for(Multiples mult : values)
      {
         if(!ret.isEmpty())
         {
            ret = ret + (mult.isNegative() ? "-" : "+") + mult;
         }
         else
         {
            ret = (mult.isNegative() ? "-" : "") + mult + "";
         }
      }
      return ret;
   }

   public boolean equals(Object obj)
   {
      if(obj == null || !(obj instanceof AdditiveEquation))
      {
         return false;
      }
      AdditiveEquation ae = (AdditiveEquation)obj;
      AdditiveEquation temp = ae.clone();
      boolean found;
      for(Multiples mult : values)
      {
         found = false;
         for(int x = 0; x < temp.values.size()&&!found;)
         {
            if(mult.equals(temp.values.get(x)))
            {
               found = true;
               temp.values.remove(x);
            }
            else
            {
               x++;
            }
         }
         if(!found)
         {
            return false;
         }
      }
      return temp.values.size() == 0;
   }

   public void negate()
   {
      Multiples neg = new Multiples(new MyNumber(-1));
      for(Multiples mult : values)
      {
         mult.multiply(neg);
      }
   }

   public AdditiveEquation clone()
   {
      AdditiveEquation temp = new AdditiveEquation();
      for(Multiples mult : values)
      {
         temp.add(mult.clone());
      }
      return temp;
   }
}
