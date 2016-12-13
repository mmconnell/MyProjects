package LikeNumber;

import java.math.BigInteger;

public interface LikeNumber extends Cloneable
{
   String getValue();
   int getPower();
   boolean addable(LikeNumber ln, String type);
   boolean canMultiply(LikeNumber ln, String type);
   boolean canRemove();
   boolean isZero();
   boolean isMultiple(LikeNumber ln);
   boolean isNegative();
   void negate();
   Multiples reduce();
   void pow(int power);
   LikeNumber getMultiple(int power);
   LikeNumber gcd(LikeNumber ln);
   void multiply(LikeNumber ln);
   String getType();
   LikeNumber clone();
   boolean canDivide(LikeNumber ln);
   void divide(LikeNumber ln);
}
