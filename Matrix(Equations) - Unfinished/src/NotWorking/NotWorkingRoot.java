package NotWorking;

import LikeNumber.*;

public class NotWorkingRoot
{
   private String val;
   private int wholeRoot;
   private AdditiveEquation num;
   private int power;
   private String type;

   public NotWorkingRoot(AdditiveEquation num, int power)
   {
      this.num = num.clone();
      this.val = num + "";
      this.power = power;
      type = "NestedRoot";
   }

   public NotWorkingRoot(int num, int power)
   {
      this.wholeRoot = num;
      this.val = num + "";
      this.power = power;
      type = "Root";
   }

   public String getValue()
   {
      return val;
   }

   public AdditiveEquation getNum()
   {
      return this.num;
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

   public int multiply(LikeNumber ln)
   {
      AdditiveEquation value = null;
      int num;
      if(canMultiply(ln, ln.getType()))
      {
         switch(ln.getType())
         {
            case "Root":
               
               break;
            case "NestedRoot":
               break;
         }
         //value = ln.getNum();
         this.num.multiply(value);
         this.val = this.num + "";
         return reduce();
      }
      return 1;
   }

   private int reduce()
   {
      int ret = 1;
 //     int num = getNum().allTermsGCD();
      int num = 1;
      for(int x = 1; x < num; x++)
      {
         if(num % Math.pow(x, power) == 0)
         {
    //        getNum().divideAll((int)Math.pow(x, power));
            ret *= x;
         }
      }
      return ret;
   }

   public String getType()
   {
      return type;
   }

   public boolean canRemove()
   {
      return num == null ? wholeRoot == 1 : false;//num.equalsOne();
   }

   public boolean equals(Object obj)
   {
      if(obj == null || !(obj instanceof NotWorkingRoot))
      {
         return false;
      }

      NotWorkingRoot notWorkingRoot = (NotWorkingRoot)obj;

      if(notWorkingRoot.num == num && notWorkingRoot.power == power)
      {
         return true;
      }

      return false;
   }

   public String toString()
   {
      return "t" + power + "(" + (num == null ? wholeRoot : num) + ")";
   }

   public LikeNumber clone()
   {
      if(num == null)
      {
    //     return new NotWorkingRoot(wholeRoot, power);
      }
      return null;
   }
}

