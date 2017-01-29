package LikeNumber;

import java.util.*;

public class Multiples implements Cloneable
{
   private List<LikeNumber> values;
   private boolean negative;

   public Multiples(LikeNumber ln)
   {
      values = new ArrayList<>();
      values.add(new MyNumber(1));
      values.add(ln.clone());
      negative = false;
      if(ln.isNegative())
      {
         negative = true;
         ln.negate();
      }
      reduce();
   }

   public Multiples()
   {
      values = new ArrayList<>();
      values.add(new MyNumber(1));
      negative = false;
      reduce();
   }

   public void multiply(Multiples mult)
   {
      boolean multiply;

      if(mult.isNegative())
      {
         negative = !negative;
      }
      if(mult.isZero())
      {
         values.clear();
      }

      for(LikeNumber input : mult.values)
      {
         multiply(input);
      }
      reduce();
   }

   public boolean isNegative()
   {
      return negative;
   }

   public void multiply(LikeNumber ln)
   {
      boolean multiply = false;
      if(ln.isNegative())
      {
         negative = !negative;
         ln.negate();
      }
      for(LikeNumber orig : values)
      {
         if(!multiply && orig.canMultiply(ln, ln.getType()))
         {
            orig.multiply(ln.clone());
            multiply = true;
         }
      }
      if(!multiply)
      {
         if(!(ln.getType().equals("Number") && ((MyNumber)ln).equals(new MyNumber(1))))
         {
            values.add(ln.clone());
         }
      }
      reduce();
   }

   public boolean isOne()
   {
      return values.size() == 1 && values.get(0).getType().equals("Number") && ((MyNumber)values.get(0)).getValue().equals("1");
   }

   public boolean divideWhole(Multiples multiples)
   {
      boolean divided = false;
      for(LikeNumber ln : multiples.values)
      {
         divided = false;
         for(LikeNumber ln2 : values)
         {
            if(ln2.canDivide(ln) && !divided)
            {
               ln2.divide(ln);
               divided = true;
            }
         }
         if(!divided)
         {
            return false;
         }
      }
      return true;
   }

   public void pow(int power)
   {
      Multiples mult = clone();
      for(int x = 1; x < power; x++)
      {
         multiply(mult);
      }
   }

   public void add(Multiples mult)
   {
      int first = -1, second = -1;
      int total = 0;
      for(int x = 0; x < values.size(); x++)
      {
         if(values.get(x).getType().equals("Number"))
         {
            first = x;
         }
      }

      for(int x = 0; x < mult.values.size(); x++)
      {
         if(mult.values.get(x).getType().equals("Number"))
         {
            second = x;
         }
      }

      total += first == -1 ? 1 : Integer.parseInt(values.get(first).getValue());
      total += second == -1 ? 1 : Integer.parseInt(mult.values.get(second).getValue());

      if(first != -1)
      {
         values.remove(first);
      }
      values.add(new MyNumber(total));
      reduce();
   }

   public Multiples rootify(int pow)
   {
      Multiples mult = new Multiples();
      Multiples temp;
      AdditiveEquation ae;
      for(LikeNumber ln : values)
      {
         if(ln.getType().equals("Root"))
         {
            Root root = (Root)ln;
            root.setPower(root.getPower()*pow);
            mult.multiply(root);
         }
         else
         {
            temp = new Multiples();
            ae = new AdditiveEquation();
            temp.multiply(ln);
            ae.add(temp);
            mult.multiply(new Root(ae, pow));
         }
      }
      return mult;
   }

   public boolean singleElement()
   {
      int count = 0;
      for(LikeNumber ln : values)
      {
         if(!ln.equals(new MyNumber(1)))
         {
            count++;
         }
      }
      if(count < 1)
      {
         count = 1;
      }
      return count == 1;
   }

   public LikeNumber getFirst()
   {
      for(LikeNumber ln : values)
      {
         if(!ln.equals(new MyNumber(1)))
         {
            return ln;
         }
      }
      return values.get(0);
   }

   public void reduce()
   {
      boolean deleted = false;
      for (int x = 0; x < values.size(); )
      {
         if (values.get(x).isZero())
         {
            values.clear();
            deleted = true;
         } else if (values.get(x).canRemove())
         {
            LikeNumber ln = values.remove(x);
         } else
         {
            x++;
         }
      }
      boolean added = false;
      for (LikeNumber ln : values)
      {
         if (ln.getType().equals("Number"))
         {
            added = true;
         }
      }
      if (!added && !deleted)
      {
         values.add(0, new MyNumber(1));
      }
   }

   public void reduceElements()
   {
      Multiples temp = new Multiples();
      Multiples toMultiply = new Multiples();
      for(int x = 0; x < values.size();)
      {
         if(values.get(x).isZero())
         {
            values.clear();
         }
         else if(values.get(x).canRemove() && values.size() > 1)
         {
            LikeNumber ln = values.remove(x);
         }
         else
         {
            toMultiply = values.get(x).reduce();
            if(!toMultiply.isOne())
            {
               temp.multiply(toMultiply);
            }
            x++;
         }
      }
      multiply(temp);
   }

   public AdditiveEquation consolidateTerms()
   {
      Multiples temp = new Multiples();
      temp.negative = negative;
      AdditiveEquation ae = new AdditiveEquation();
      ae.toReduce = false;
      ae.add(new Multiples(new MyNumber(1)));
      for(LikeNumber ln : values)
      {
         if(ln.getType().equals("Root"))
         {
            ae.multiply(((Root)ln).consolidateTerms());
         }
         else
         {
            temp.multiply(ln);
         }
      }
      if(!temp.equals(new Multiples()))
      {
         AdditiveEquation ae2 = new AdditiveEquation();
         ae2.toReduce = false;
         ae2.add(temp);
         ae2.toReduce = true;
         ae.multiply(ae2);
      }
      ae.toReduce = true;
      return ae;
   }

   private void multiplyReduced(Multiples mult)
   {
      for(LikeNumber likeNumber : mult.values)
      {
         values.add(likeNumber);
      }
   }

   public Multiples gcd(Multiples mult)
   {
      Multiples gcd = new Multiples();
      LikeNumber temp;

      for(LikeNumber ln : values)
      {
         for(int x = 0; x < mult.values.size();)
         {
            temp = mult.values.get(x);
            if(ln.isMultiple(temp))
            {
               gcd.multiply(ln.gcd(temp));
               mult.values.remove(x);
            }
            else
            {
               x++;
            }
         }
      }

      return gcd;
   }

   public Multiples getMultiple(int root)
   {
      //NEEDS WORK
      Multiples mult = clone();
      Multiples retMult = new Multiples();
      for(LikeNumber ln : values)
      {
         retMult.multiply(ln.getMultiple(root));
      }
      return retMult.clone();
   }

   public boolean isZero()
   {
      return values.size() == 0;
   }

   public boolean isConstant()
   {
      return values.size() == 0;
   }

   public boolean canAddTo(Multiples mult)
   {
      boolean canAdd = false;
      if(mult.isZero() || values.size() != mult.values.size())
      {
         return false;
      }
      for(LikeNumber num : values)
      {
         canAdd = false;
         if(num.getType().equals("Number"))
         {
            canAdd = true;
         }
         for(LikeNumber toCheck : mult.values)
         {
            canAdd = canAdd || num.equals(toCheck);
         }
         if(!canAdd)
         {
            return false;
         }
      }
      return canAdd;
   }

   @Override
   public Multiples clone()
   {
      Multiples temp = null;

      for(LikeNumber ln : values)
      {
         if(temp == null)
         {
            temp = new Multiples(ln);
         }
         else
         {
            temp.values.add(ln.clone());
         }
      }
      temp.negative = negative;
      return temp;
   }

   public String toString()
   {
      String ret = "";

      if(isZero())
      {
         return "0";
      }
      for(LikeNumber ln : values)
      {
         if(ln.getType().equals("Number") && ln.getValue().equals("1"))
         {
            if(values.size() == 1)
            {
               ret += ln;
            }
         }
         else if(ln.getType().equals("Number"))
         {
            ret = ln + ret;
         }
         else
         {
            ret = ret + ln;
         }
      }

      return ret;
   }

   public boolean equals(Object obj)
   {
      if(obj == null || !(obj instanceof Multiples))
      {
         return false;
      }
      Multiples mult = (Multiples)obj;
      Multiples temp = mult.clone();
      boolean found;
      for(LikeNumber ln : values)
      {
         found = false;
         for(int x = 0; x < temp.values.size()&&!found;)
         {
            if(ln.equals(temp.values.get(x)))
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
      return temp.values.size() == 0 && negative == mult.negative;
   }
}
