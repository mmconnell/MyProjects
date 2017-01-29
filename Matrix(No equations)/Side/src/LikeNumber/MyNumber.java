package LikeNumber;

public class MyNumber implements LikeNumber
{
   private int value;

   public MyNumber(int value)
   {
      this.value = value;
   }

   public void add(MyNumber num)
   {
      this.value += num.value;
   }

   public void subtract(MyNumber num)
   {
      this.value -= num.value;
   }

   public void multiply(MyNumber num)
   {
      this.value *= num.value;
   }

   public void divide(int value)
   {
      this.value /= value;
   }

   public void reduce(MyNumber num)
   {
      int toDivide = GCD(num.value, value);
      divide(toDivide);
      num.divide(toDivide);
   }

   public void negate()
   {
      this.value = 0-value;
   }

   private int GCD(int val1, int val2)
   {
      int min = Math.min(val1, val2);

      for(; min > 1; min--)
      {
         if(val1 % min == 0 && val2 % min == 0)
         {
            return min;
         }
      }
      return 1;
   }

   public boolean isZero()
   {
      return value == 0;
   }

   @Override
   public LikeNumber gcd(LikeNumber ln)
   {
      return new MyNumber(GCD(Integer.parseInt(getValue()), Integer.parseInt(ln.getValue())));
   }

   @Override
   public boolean isMultiple(LikeNumber ln)
   {
      return getType().equals(ln.getType());
   }

   @Override
   public boolean isNegative()
   {
      return value < 0;
   }

   @Override
   public Multiples reduce()
   {
      return new Multiples();
   }

   @Override
   public LikeNumber getMultiple(int power)
   {
      for(int x = value; x > 0; x--)
      {
         if((int)Math.pow(x, power) <= value && value % (int)Math.pow(x, power)== 0)
         {
            return new MyNumber(x);
         }
      }
      return new MyNumber(1);
   }

   public String getValue()
   {
      return this.value + "";
   }

   public void pow(int power)
   {
      LikeNumber ln = clone();
      for(int x = 1; x < power; x++)
      {
         multiply(ln);
      }
   }

   @Override
   public int getPower()
   {
      return 1;
   }

   @Override
   public boolean canRemove()
   {
      return value == 1;
   }

   @Override
   public String getType()
   {
      return "Number";
   }

   @Override
   public LikeNumber clone()
   {
      return new MyNumber(value);
   }

   @Override
   public void divide(LikeNumber ln)
   {
      if(canDivide(ln))
      {
         value /= Integer.parseInt(ln.getValue());
      }
   }

   @Override
   public boolean canDivide(LikeNumber ln)
   {
      return ln != null && ln.getType().equals(getType()) && Integer.parseInt(ln.getValue()) <= value && value % Integer.parseInt(ln.getValue()) == 0;
   }

   @Override
   public void multiply(LikeNumber ln)
   {
      if(canMultiply(ln, ln.getType()))
      {
         this.value *= Integer.parseInt(ln.getValue());
      }
   }

   @Override
   public boolean canMultiply(LikeNumber ln, String type)
   {
      return ln != null && type != null && getType().equals(type);
   }

   @Override
   public boolean addable(LikeNumber ln, String type)
   {
      return canMultiply(ln, type);
   }

   public String toString()
   {
      return "" + this.value;
   }

   public boolean equals(Object obj)
   {
      if(obj == null || !(obj instanceof MyNumber))
      {
         return false;
      }
      MyNumber num = (MyNumber)obj;

      return value == num.value;
   }
}
