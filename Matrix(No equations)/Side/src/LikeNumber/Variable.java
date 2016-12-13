package LikeNumber;

public class Variable implements LikeNumber
{
   private String val;
   private int power;
   private String type;

   public Variable(String val, int power)
   {
      if(val == null || val.isEmpty() || (val.charAt(0) >= '0' && val.charAt(0) <= 9))
      {
         throw new IllegalArgumentException("Invalid inputs");
      }
      this.val = val;
      this.power = power;
      this.type = "Variable";
   }

   @Override
   public String getValue()
   {
      return val;
   }

   @Override
   public int getPower()
   {
      return power;
   }

   @Override
   public boolean canRemove()
   {
      return power == 0;
   }

   @Override
   public String getType()
   {
      return type;
   }

   @Override
   public LikeNumber clone()
   {
      return new Variable(val, power);
   }

   @Override
   public void divide(LikeNumber ln)
   {
      if(canDivide(ln))
      {
         power -= ln.getPower();
      }
   }

   @Override
   public boolean canDivide(LikeNumber ln)
   {
      return ln != null && ln.getType().equals(getType()) && val.equals(ln.getValue()) && power >= ln.getPower();
   }

   @Override
   public void multiply(LikeNumber ln)
   {
      if(canMultiply(ln, ln.getType()))
      {
         this.power = this.power + ln.getPower();
      }
   }

   @Override
   public boolean canMultiply(LikeNumber ln, String type)
   {
      return ln != null && type != null && type.equals(this.type) && val.equals(ln.getValue());
   }

   @Override
   public boolean addable(LikeNumber ln, String type)
   {
      return ln != null && type != null && type.equals(this.type) && val.equals(ln.getValue()) && power == ln.getPower();
   }

   public boolean equals(Object obj)
   {
      if(obj == null || !(obj instanceof LikeNumber))
      {
         return false;
      }

      LikeNumber ln = (LikeNumber)obj;

      return type.equals(ln.getType()) && val.equals(ln.getValue()) && power == ln.getPower();
   }

   public boolean isZero()
   {
      return false;
   }

   @Override
   public LikeNumber gcd(LikeNumber ln)
   {
      if(isMultiple(ln))
      {
         return new Variable(getValue(), Math.min(power, ln.getPower()));
      }
      return new MyNumber(1);
   }

   @Override
   public boolean isMultiple(LikeNumber ln)
   {
      if(ln.getValue().equals(getValue()))
      {
         return true;
      }
      return false;
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
   public Multiples reduce()
   {
      return new Multiples();
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
   public LikeNumber getMultiple(int power)
   {
      if(this.power < power)
      {
         return new MyNumber(1);
      }
      else
      {
         return new Variable(getValue(), this.power/power);
      }
   }

   public String toString()
   {
      return val + (power != 1 ?"^" + power : "");
   }
}
